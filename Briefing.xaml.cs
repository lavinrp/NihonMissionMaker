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
        //briefing strings
        private string bluBriefing;
        private string redBriefing;
        private string indBriefing;

        //regexs for briefing segments
        private string creditsRegexExpression = "(?<=_cre = player createDiaryRecord \\[\"diary\", \\[\"Credits\",\"\n<br/>)(.*?)(?=<br/><br/>\nMade with F3)";
        private string administrationRegexExpression = "(?<=_adm = player createDiaryRecord \\[\"diary\", \\[\"Administration\",\"\n<br/>)(.*?)(?=\"\\]\\];)";
        private string misionRegexExpression = "(?<=_mis = player createDiaryRecord \\[\"diary\", \\[\"Mission\",\"\n<br/>)(.*?)(?=\"\\]\\])";
        private string situationRegexExpression = "(?<=_sit = player createDiaryRecord \\[\"diary\", \\[\"Situation\",\"\n<br/>\n)(.*?)(<br/><br/>\nENEMY FORCES)";
        private string enemyForcesRegexExpression = "(?<=ENEMY FORCES\n<br/>)(.*?)(<br/><br/>\n\n\"\\]\\];)";

        public Briefing()
        {
            InitializeComponent();
            
        }

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


        

        private void briefingSaveButton_Click(object sender, RoutedEventArgs e)
        {
            

            //TODO: If no bluInput skip blu briefing file stuffs
            //TODO: allow user to change the file path
            string bluBriefingPath;
            bluBriefingPath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf\\f\\briefing\\f_briefing_nato.sqf";
            //newBluBriefingPath = "C:\\Users\\R Lavin\\Documents\\Arma 3 - Other Profiles\\NihonNukite\\TempMissionStore\\Nih Mission Maker\\bwmf\\f\\briefing\\f_briefing_nato.sqf";


            using (StreamWriter sw = new StreamWriter(bluBriefingPath))
            {
                

            }

            //Make changes

        }
    }
}
