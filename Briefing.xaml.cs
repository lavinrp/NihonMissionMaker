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
        private string enemyForcesRegexExpression = "(?<=ENEMY FORCES\\r\\n<br/>)(.*?)(<br/><br/>\\r\\n\\r\\n\"\\]\\];)";

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


        //TODO: remove this
        /// <summary>
        /// Copies the briefing file into memory and inserts user given string in the correct place.
        /// Returns list containing each line of the new briefing
        /// DOES NOT OUTPUT FILE
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>returnList: list of every line in the produced briefing</returns>
        private List<string> CreateBriefingArray(string filePath)
        {
            //create list
            List<String> returnList = new List<string>();


            //exit if the file does not exist
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File path is wrong", "Nihon Mission Maker", MessageBoxButton.OKCancel);
                return returnList;
            }

            //array to store old file
            //Open file and read in its lines
            using (StreamReader sr = new StreamReader(filePath))
            {
                string stringToAdd = "";

                //read in lines untill first key found or end of file
                while (stringToAdd != "*** Insert mission credits here. ***" && !sr.EndOfStream)
                {
                    returnList.Add(stringToAdd);
                    stringToAdd = sr.ReadLine();
                }

                //add user input field
                returnList.Add(creditsTextBox.Text);


                //read in lines untill second key found or end of file
                stringToAdd = sr.ReadLine();
                while (stringToAdd != "*** Insert information on administration and logistics here. ***" && !sr.EndOfStream)
                {
                    returnList.Add(stringToAdd);
                    stringToAdd = sr.ReadLine();
                }


                //add user input field
                returnList.Add(administrationTextBox.Text);

                //read in lines untill third key found or end of file
                stringToAdd = sr.ReadLine();
                while (stringToAdd != "*** Insert the mission here. ***" && !sr.EndOfStream)
                {
                    returnList.Add(stringToAdd);
                    stringToAdd = sr.ReadLine();
                }

                //add user input field
                returnList.Add(missionTextBox.Text);

                //read in lines untill third key found or end of file
                stringToAdd = sr.ReadLine();
                while (stringToAdd != "*** Insert general information about the situation here.***" && !sr.EndOfStream)
                {
                    returnList.Add(stringToAdd);
                    stringToAdd = sr.ReadLine();
                }

                //add user input field
                returnList.Add(situationTextBox.Text);

                //finish reading file
                stringToAdd = sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    returnList.Add(stringToAdd);
                    stringToAdd = sr.ReadLine();
                }


            }


            return returnList;
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
        

        private void briefingSaveButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: If no bluInput skip blu briefing file stuffs
            //TODO: allow user to change the file path
            string bluBriefingPath;
            bluBriefingPath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf\\f\\briefing\\f_briefing_nato.sqf";
            //newBluBriefingPath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf\\f\\briefing\\f_briefing_nato.sqf";

            /*
            using (StreamWriter sw = new StreamWriter(bluBriefingPath))
            {
                
            }
            */
            //Make changes

        }

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

            //TODO: finish method
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
            //TODO: make gui element and display logic for enemy forces
        }
    }
}
