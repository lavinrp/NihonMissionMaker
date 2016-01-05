using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for Briefing.xaml
    /// </summary>
    public partial class Briefing : Page
    {

        #region Basic class setup
        //The file path of the folder that contains the briefings
        string briefingFolderFilePath;

        //faction enum
        private enum factions : byte {BLUF,IND,OPF,CIV};

        //briefing strings
        private string bluBriefing;
        private string redBriefing;
        private string indBriefing;
        private string civBriefing;

        //briefing file paths
        private string bluBriefingFilePath;
        private string redBriefingFilePath;
        private string indBriefingFilePath;
        private string civBriefingFilePath;

        //regexs for briefing segments
        private string administrationRegexExpression = "(?<=_adm = player createDiaryRecord \\[\"diary\", \\[\"Administration\",\"(\\r\\n|\\n)<br/>)(.*?)(?=\"\\]\\];)";
        private string missionRegexExpression = "(?<=_mis = player createDiaryRecord \\[\"diary\", \\[\"Mission\",\"(\\r\\n|\\n)<br/>)(.*?)(?=\"\\]\\])";
        private string situationRegexExpression = "(?<=_sit = player createDiaryRecord \\[\"diary\", \\[\"Situation\",\"(\\r\\n|\\n)<br/>(\\r\\n|\\n))(.*?)(?=<br/><br/>(\\r\\n|\\n)ENEMY FORCES)";
        private string enemyForcesRegexExpression = "(?<=ENEMY FORCES(\\r\\n|\\n)<br/>)(.*?)(?=<br/><br/>(\\r\\n|\\n)(\\r\\n|\\n)?\"\\]\\];)";

        //Bool to determine if change should be saved
        bool saveableChangeMade;

        //colors for user feedback
        SolidColorBrush enabledColor = new SolidColorBrush(Colors.Green);
        SolidColorBrush normalColor = new SolidColorBrush(Colors.Gray);
        SolidColorBrush errorColor = new SolidColorBrush(Colors.Red);

        /// <summary>
        /// initialize all variables and the GUI
        /// </summary>
        public Briefing(string passedBwmfFilePath)
        {
            InitializeComponent();

            //hardcoded path to testing BWMF
            //string BWMFFilePath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf";
            string bwmfFilePath = passedBwmfFilePath;


            //file paths to briefings
            briefingFolderFilePath = bwmfFilePath + "\\f\\briefing";
            bluBriefingFilePath = briefingFolderFilePath + "\\briefing_west.sqf";
            indBriefingFilePath = briefingFolderFilePath + "\\briefing_independent.sqf";
            redBriefingFilePath = briefingFolderFilePath + "\\briefing_east.sqf";
            civBriefingFilePath = briefingFolderFilePath + "\\briefing_civilian.sqf";

            //read in the briefings for each faction
            loadFactionBriefing(out bluBriefing, bluBriefingFilePath);
            loadFactionBriefing(out indBriefing, indBriefingFilePath);
            loadFactionBriefing(out redBriefing, redBriefingFilePath);
            loadFactionBriefing(out civBriefing, civBriefingFilePath);

            //initialize savable change to false
            saveableChangeMade = false;
        }
        #endregion

        #region save / load
        /// <summary>
        /// Saves a factions briefing file to a string given a file path to the briefing
        /// </summary>
        /// <param name="factionBriefing">string that will hold the briefing</param>
        /// <param name="factionBriefingFilePath">file path to the faction's briefing</param>
        private void loadFactionBriefing(out string factionBriefing, string factionBriefingFilePath)
        {
            //set default value for factionBriefing
            factionBriefing = "";

            //Read in briefing and save it in string
            try
            {
                if (File.Exists(factionBriefingFilePath))
                {
                    using (StreamReader sr = new StreamReader(factionBriefingFilePath))
                    {
                        factionBriefing = sr.ReadToEnd();
                    }
                }

                else
                {
                    throw (new System.IO.IOException("File Not Found"));
                }
            }

            //display message box with error if issue loading file
            catch
            {
                MessageBox.Show("Error reading faction briefing file", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }        
        
        /// <summary>
        /// Writes given briefing to given briefing file
        /// </summary>
        /// <param name="factionBriefing">briefing to be saved</param>
        /// <param name="factionBriefingFilePath">briefing path to save to</param>
        private void SaveFactionBriefing(ref string factionBriefing, string factionBriefingFilePath)
        {
            //write to the faction file
            try
            {
                System.IO.File.WriteAllText(factionBriefingFilePath, factionBriefing);
            }

            catch
            {
                MessageBox.Show("Error writing faction briefing file", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Overwrite faction briefing file with content of briefing strings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void briefingSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //overwrite faction briefing file with content of stored briefing strings
            SaveFactionBriefing(ref bluBriefing, bluBriefingFilePath);
            SaveFactionBriefing(ref redBriefing, redBriefingFilePath);
            SaveFactionBriefing(ref indBriefing, indBriefingFilePath);
            SaveFactionBriefing(ref civBriefing, civBriefingFilePath);

            //reset color to gray to show that the briefing has been saved
            saveableChangeMade = false;
            briefingSaveButton.Background = normalColor;
        }

        #endregion

        #region change text fields

        /// <summary>
        /// Converts text formatted for display in NMM to text formatted for display in arma
        /// </summary>
        /// <param name="rawText">Text formatted to display in NMM</param>
        /// <returns>Text formated to display in arma briefings</returns>
        private string PrepTextForArma(string rawText)
        {
            return rawText.Replace(Environment.NewLine, "<br/>");
        }

        /// <summary>
        /// replace whats in the administration section of the briefing with whats in the administration text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeAdmin(ref string briefing)
        {
            Regex adminRegex = new Regex(administrationRegexExpression, RegexOptions.Singleline);
            string formatedText = PrepTextForArma(administrationTextBox.Text);
            briefing = adminRegex.Replace(briefing, formatedText);
        }

        /// <summary>
        /// replace whats in the mission section of the briefing with whats in the mission text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeMission(ref string briefing)
        {
            Regex missionRegex = new Regex(missionRegexExpression, RegexOptions.Singleline);
            string formatedText = PrepTextForArma(missionTextBox.Text);
            briefing = missionRegex.Replace(briefing, formatedText);
        }

        /// <summary>
        /// Replace whats in the situation section of the briefing with whats in the situation text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeSituation(ref string briefing)
        {
            Regex situationRegex = new Regex(situationRegexExpression, RegexOptions.Singleline);
            string formatedText = PrepTextForArma(situationTextBox.Text);
            briefing = situationRegex.Replace(briefing, formatedText);
        }

        /// <summary>
        /// replaces whats in the enemy forces section of the briefing with whats in the enemy forces text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeEnemyForces(ref string briefing)
        {
            Regex enemyForcesRegex = new Regex(enemyForcesRegexExpression, RegexOptions.Singleline);
            string formatedText = PrepTextForArma(enemyForcesTextBox.Text);
            briefing = enemyForcesRegex.Replace(briefing, formatedText);
        }

        /// <summary>
        /// Loads correct factions briefing when user selects a faction from the faction selection combo box
        /// </summary>
        /// <param name="sender">faction selection combo box</param>
        /// <param name="e"></param>
        private void SideSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Store if a savable change has been made or not
            bool storeChage = saveableChangeMade;

            //Display selected faction briefing
            switch (side)
            {
                case Sides.BLUF:
                    DisplayAdmin(ref bluBriefing);
                    DisplayMission(ref bluBriefing);
                    DisplaySituation(ref bluBriefing);
                    DisplayEnemyForces(ref bluBriefing);
                    break;
                case Sides.OPF:
                    DisplayAdmin(ref redBriefing);
                    DisplayMission(ref redBriefing);
                    DisplaySituation(ref redBriefing);
                    DisplayEnemyForces(ref redBriefing);
                    break;
                case Sides.IND:
                    DisplayAdmin(ref indBriefing);
                    DisplayMission(ref indBriefing);
                    DisplaySituation(ref indBriefing);
                    DisplayEnemyForces(ref indBriefing);
                    break;
                case Sides.CIV:
                    DisplayAdmin(ref civBriefing);
                    DisplayMission(ref civBriefing);
                    DisplaySituation(ref civBriefing);
                    DisplayEnemyForces(ref civBriefing);
                    break;
            }

            //return saveableChangeMade to its former state
            //Ensure save button is colored correctly
            saveableChangeMade = storeChage;
            if (!storeChage)
            {
                briefingSaveButton.Background = normalColor;
            }
        }

        /// <summary>
        /// Detects change in the administration text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void administrationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Notify the briefing that the element has been changed
            ElementChanged();
        }

        /// <summary>
        /// detects change in the mission text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missionTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Notify the briefing that the element has been changed
            ElementChanged();
        }

        /// <summary>
        /// Detects change in the situation text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void situationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Notify the briefing that the element has been changed
            ElementChanged();
        }

        /// <summary>
        /// Detects change in the enemyForces text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enemyForcesTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Notify the briefing that the element has been changed
            ElementChanged();
        }

        /// <summary>
        /// Call whenever a briefing element is changed. Changes Save button to Red
        /// </summary>
        private void ElementChanged()
        {
            //can only consider element changed if a faction is selected
            if(SideSelectionBox.SelectedIndex != -1)
            {
                saveableChangeMade = true;

                //change the color of the element to show it can be saved
                briefingSaveButton.Background = enabledColor;
            }
        }

        /// <summary>
        /// Detects when the user exits the administrationTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void administrationTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            switch (side)
            {
                //make changes to string for selected factions briefing
                case Sides.BLUF:
                    ChangeAdmin(ref bluBriefing);
                    break;
                case Sides.OPF:
                    ChangeAdmin(ref redBriefing);
                    break;
                case Sides.IND:
                    ChangeAdmin(ref indBriefing);
                    break;
                case Sides.CIV:
                    ChangeAdmin(ref civBriefing);
                    break;
            }
        }

        /// <summary>
        /// Detects when the user exits the missioinTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            switch (side)
            {
                //make changes to string for selected factions briefing
                case Sides.BLUF:
                    ChangeMission(ref bluBriefing);
                    break;
                case Sides.OPF:
                    ChangeMission(ref redBriefing);
                    break;
                case Sides.IND:
                    ChangeMission(ref indBriefing);
                    break;
                case Sides.CIV:
                    ChangeMission(ref civBriefing);
                    break;
            }
        }

        /// <summary>
        /// Detects when the user exits the situationTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void situationTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            switch (side)
            {
                //make changes to string for selected factions briefing
                case Sides.BLUF:
                    ChangeSituation(ref bluBriefing);
                    break;
                case Sides.OPF:
                    ChangeSituation(ref redBriefing);
                    break;
                case Sides.IND:
                    ChangeSituation(ref indBriefing);
                    break;
                case Sides.CIV:
                    ChangeSituation(ref civBriefing);
                    break;
            }
        }

        /// <summary>
        /// Detects when the user exits the enemyForcesTextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void enemyForcesTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            switch (side)
            {
                //make changes to string for selected factions briefing
                case Sides.BLUF:
                    ChangeEnemyForces(ref bluBriefing);
                    break;
                case Sides.OPF:
                    ChangeEnemyForces(ref redBriefing);
                    break;
                case Sides.IND:
                    ChangeEnemyForces(ref indBriefing);
                    break;
                case Sides.CIV:
                    ChangeEnemyForces(ref civBriefing);
                    break;
            }
        }

        #endregion

        #region display text fields
        
        /// <summary>
        /// Preform all operations necessary to convert from arma formating of briefing to NMM formating of briefing.
        /// </summary>
        /// <param name="rawString">Raw string imported from briefing sqf file. formatted to be displayed in ArmA</param>
        /// <returns>String with correct formating to display in briefing sections of NMM.</returns>
        private string PrepStringForDisplay(ref string rawString)
        {
            return rawString.Replace("<br/>", Environment.NewLine);
        }

        /// <summary>
        /// fills the administration box with the administration information from the given factions briefing
        /// </summary>
        /// <param name="briefing">faction specific briefing string</param>
        private void DisplayAdmin(ref string briefing)
        {
            //read in all the matches for administration (should only be one)
            var admin = Regex.Matches(briefing, administrationRegexExpression, RegexOptions.Singleline);

            //write last match to GUI
            foreach (Match adminMatch in admin)
            {
                string rawString = adminMatch.ToString();
                administrationTextBox.Text = PrepStringForDisplay(ref rawString);
            }
        }

        /// <summary>
        /// Fills the Mission box with the mission information from the given factions briefing
        /// </summary>
        /// <param name="briefing">faction specific briefing string</param>
        private void DisplayMission(ref string briefing)
        {
            //read in all the matches for Mission
            var mission = Regex.Matches(briefing, missionRegexExpression, RegexOptions.Singleline);

            //write last match to GUI
            foreach (Match missionMatch in mission)
            {
                string rawString = missionMatch.ToString();
                missionTextBox.Text = PrepStringForDisplay(ref rawString);
            }
        }

        /// <summary>
        /// fills the situation box with information from the given factions briefing
        /// </summary>
        /// <param name="briefing">faction specific briefing string</param>
        private void DisplaySituation(ref string briefing)
        {
            //read in all matches to situation
            var situation = Regex.Matches(briefing, situationRegexExpression, RegexOptions.Singleline);

            //write last match to GUI
            foreach (Match situationMatch in situation)
            {
                string rawString = situationMatch.ToString();
                situationTextBox.Text = PrepStringForDisplay(ref rawString);
            }
        }

        /// <summary>
        /// fills the enemy forces box with information from the given factions briefing
        /// </summary>
        /// <param name="briefing">faction specific briefing string</param>
        private void DisplayEnemyForces(ref string briefing)
        {
            //read in all matches to situation
            var enemyForces = Regex.Matches(briefing, enemyForcesRegexExpression, RegexOptions.Singleline);

            //write last match to GUI
            foreach (Match enemyForcesMatch in enemyForces)
            {
                string rawString = enemyForcesMatch.ToString();
                enemyForcesTextBox.Text = PrepStringForDisplay(ref rawString);
            }
        }

        #endregion


        /// <summary>
        /// The selected side of the briefing page
        /// </summary>
        public Sides side
        {
            get
            {
                //get selection of the side selection combo box
                ComboBoxItem selection = (ComboBoxItem)(SideSelectionBox.SelectedValue);
                string selectionString;

                try
                {
                    //convert selection to string
                    selectionString = (string)selection.Content;
                }
                catch
                {
                     return Sides.EMPTY;
                }

                switch (selectionString)
                {
                    //make changes to string for selected factions briefing and overwrite faction briefing file with content of string
                    case "Blufor":
                        return Sides.BLUF;
                    case "Opfor":
                        return Sides.OPF;
                    case "Indfor":
                        return Sides.IND;
                    case "Civ":
                        return Sides.CIV;
                    default:
                        return Sides.EMPTY;
                }
            }
            set
            {
                //Sets the side selection combo box to the passed side
                switch (value)
                {
                    case Sides.BLUF:
                        SideSelectionBox.SelectedItem = briefSideBlue;
                        break;
                    case Sides.IND:
                        SideSelectionBox.SelectedItem = briefSideInd;
                        break;
                    case Sides.OPF:
                        SideSelectionBox.SelectedItem = briefSideRed;
                        break;
                    case Sides.CIV:
                        SideSelectionBox.SelectedItem = briefSideCiv;
                        break;
                    default:
                        SideSelectionBox.SelectedIndex = -1;
                        break;
                }
            }
        }
    }
}
