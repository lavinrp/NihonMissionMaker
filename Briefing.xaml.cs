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

        //The file path of the folder that contains the briefings
        //TODO: set this path based on user input path to BWMF
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
        private string creditsRegexExpression = "(?<=_cre = player createDiaryRecord \\[\"diary\", \\[\"Credits\",\"\\r\\n<br/>)(.*?)(?=<br/><br/>\\r\\nMade with F3)";
        private string administrationRegexExpression = "(?<=_adm = player createDiaryRecord \\[\"diary\", \\[\"Administration\",\"\\r\\n<br/>)(.*?)(?=\"\\]\\];)";
        private string missionRegexExpression = "(?<=_mis = player createDiaryRecord \\[\"diary\", \\[\"Mission\",\"\\r\\n<br/>)(.*?)(?=\"\\]\\])";
        private string situationRegexExpression = "(?<=_sit = player createDiaryRecord \\[\"diary\", \\[\"Situation\",\"\\r\\n<br/>\\r\\n)(.*?)(?=<br/><br/>\\r\\nENEMY FORCES)";
        private string enemyForcesRegexExpression = "(?<=ENEMY FORCES\\r\\n<br/>)(.*?)(?=<br/><br/>\\r\\n(\\r\\n)?\"\\]\\];)";

        /// <summary>
        /// initialize all variables and the GUI
        /// </summary>
        public Briefing()
        {
            InitializeComponent();

            //TODO: take this path in from user and pass it to this function
            string BWMFFilePath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf";

            //file paths to briefings
            briefingFolderFilePath = BWMFFilePath + "\\f\\briefing";
            bluBriefingFilePath = briefingFolderFilePath + "\\f_briefing_nato.sqf";
            indBriefingFilePath = briefingFolderFilePath + "\\f_briefing_aaf.sqf";
            redBriefingFilePath = briefingFolderFilePath + "\\f_briefing_csat.sqf";
            civBriefingFilePath = briefingFolderFilePath + "\\f_briefing_civ.sqf";

            //read in the briefings for each faction
            loadFactionBriefing(out bluBriefing, bluBriefingFilePath);
            loadFactionBriefing(out indBriefing, indBriefingFilePath);
            loadFactionBriefing(out redBriefing, redBriefingFilePath);
            loadFactionBriefing(out civBriefing, civBriefingFilePath);
            
        }

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
        ///make changes to string for selected factions briefing and overwrite faction briefing file with content of string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void briefingSaveButton_Click(object sender, RoutedEventArgs e)
        {

            //get selection
            ComboBoxItem selection = (ComboBoxItem)(SideSelectionBox .SelectedValue);
            string selectionString;

            try
            {
                //convert selection to string
                selectionString = (string)selection.Content;
            }
            catch
            {
                MessageBox.Show("Error saving briefing. Have you selected a faction?", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            switch (selectionString)
            {
                //make changes to string for selected factions briefing and overwrite faction briefing file with content of string
                case "Blufor":
                    ChangeCredits(ref bluBriefing);
                    ChangeAdmin(ref bluBriefing);
                    ChangeMission(ref bluBriefing);
                    ChangeSituation(ref bluBriefing);
                    ChangeEnemyForces(ref bluBriefing);
                    SaveFactionBriefing(ref bluBriefing, bluBriefingFilePath);
                    break;
                case "Opfor":
                    ChangeCredits(ref redBriefing);
                    ChangeAdmin(ref redBriefing);
                    ChangeMission(ref redBriefing);
                    ChangeSituation(ref redBriefing);
                    ChangeEnemyForces(ref redBriefing);
                    SaveFactionBriefing(ref redBriefing, redBriefingFilePath);
                    break;
                case "Indfor":
                    ChangeCredits(ref indBriefing);
                    ChangeAdmin(ref indBriefing);
                    ChangeMission(ref indBriefing);
                    ChangeSituation(ref indBriefing);
                    ChangeEnemyForces(ref indBriefing);
                    SaveFactionBriefing(ref indBriefing, indBriefingFilePath);
                    break;
                case "Civ":
                    ChangeCredits(ref civBriefing);
                    ChangeAdmin(ref civBriefing);
                    ChangeMission(ref civBriefing);
                    ChangeSituation(ref civBriefing);
                    ChangeEnemyForces(ref civBriefing);
                    SaveFactionBriefing(ref civBriefing, civBriefingFilePath);
                    break;
            }

        }

        /// <summary>
        /// replace whats in the credits section of the briefing with whats in the credits text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeCredits(ref string briefing)
        {
            Regex creditsRgx = new Regex(creditsRegexExpression, RegexOptions.Singleline);
            briefing = creditsRgx.Replace(briefing, creditsTextBox.Text);
        }

        /// <summary>
        /// replace whats in the administration section of the briefing with whats in the administration text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeAdmin(ref string briefing)
        {
            Regex adminRegex = new Regex(administrationRegexExpression, RegexOptions.Singleline);
            briefing = adminRegex.Replace(briefing, administrationTextBox.Text);
        }

        /// <summary>
        /// replace whats in the mission section of the briefing with whats in the mission text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeMission(ref string briefing)
        {
            Regex missionRegex = new Regex(missionRegexExpression, RegexOptions.Singleline);
            briefing = missionRegex.Replace(briefing, missionTextBox.Text);
        }

        /// <summary>
        /// Replace whats in the situation section of the briefing with whats in the situation text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeSituation(ref string briefing)
        {
            Regex situationRegex = new Regex(situationRegexExpression, RegexOptions.Singleline);
            briefing = situationRegex.Replace(briefing, situationTextBox.Text);
        }

        /// <summary>
        /// replaces whats in the enemy forces section of the briefing with whats in the enemy forces text box
        /// </summary>
        /// <param name="briefing"></param>
        private void ChangeEnemyForces(ref string briefing)
        {
            Regex enemyForcesRegex = new Regex(enemyForcesRegexExpression, RegexOptions.Singleline);
            briefing = enemyForcesRegex.Replace(briefing, enemyForcesTextBox.Text);
        }

        /// <summary>
        /// Loads correct factions briefing when user selects a faction from the faction selection combo box
        /// </summary>
        /// <param name="sender">faction selection combo box</param>
        /// <param name="e"></param>
        private void SideSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get selection
            ComboBoxItem selection = (ComboBoxItem)((ComboBox)sender).SelectedValue;

            //convert selection to string
            string selectionString = (string)selection.Content;

            //Display selected faction briefing
            switch (selectionString)
            {
                case "Blufor":
                    DisplayCredits(ref bluBriefing);
                    DisplayAdmin(ref bluBriefing);
                    DisplayMission(ref bluBriefing);
                    DisplaySituation(ref bluBriefing);
                    DisplayEnemyForces(ref bluBriefing);
                    break;
                case "Opfor":
                    DisplayCredits(ref redBriefing);
                    DisplayAdmin(ref redBriefing);
                    DisplayMission(ref redBriefing);
                    DisplaySituation(ref redBriefing);
                    DisplayEnemyForces(ref redBriefing);
                    break;
                case "Indfor":
                    DisplayCredits(ref indBriefing);
                    DisplayAdmin(ref indBriefing);
                    DisplayMission(ref indBriefing);
                    DisplaySituation(ref indBriefing);
                    DisplayEnemyForces(ref indBriefing);
                    break;
                case "Civ":
                    DisplayCredits(ref civBriefing);
                    DisplayAdmin(ref civBriefing);
                    DisplayMission(ref civBriefing);
                    DisplaySituation(ref civBriefing);
                    DisplayEnemyForces(ref civBriefing);
                    break;
            }

        }

        /// <summary>
        /// Fills the credits box with the credits from the given factions briefing
        /// </summary>
        /// <param name="briefing">faction specific briefing string</param>
        private void DisplayCredits(ref string briefing)
        {
            //read in all the matches for credits (should only be one)
            var credits = Regex.Matches(briefing, creditsRegexExpression, RegexOptions.Singleline);

            //write last match to GUI
            foreach (Match creditsMatch in credits)
            {
                creditsTextBox.Text = creditsMatch.ToString();
            }
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
                administrationTextBox.Text = adminMatch.ToString();
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
                missionTextBox.Text = missionMatch.ToString();
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
            foreach(Match situationMatch in situation)
            {
                situationTextBox.Text = situationMatch.ToString();
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
                enemyForcesTextBox.Text = enemyForcesMatch.ToString();
            }
        }
    }
}
