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
using System.Text.RegularExpressions;
using System.IO;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for MissionViewPage.xaml
    /// </summary>
    public partial class MissionViewPage : Page
    {
        //filepath strings
        private string bwmfFilePath;
        private string pathToParentFolder;

        //regex for finding elements in description.ext
        string authorNameRegexExpression = "(?<=author = \")(.*?)(?=\";\\r\\nloadScreen)";
        string missionNameRegexExpression = "(?<=onLoadName = \")(.*?)(?=\";)";
        string missionTypeRegexExpression = "(?<=gameType = )(.*?)(?=;)";//TDM for tvt and Coop for COOP

        //string for holding the contents of description.ext
        private string descriptionExtTxt;

        public MissionViewPage(string bwmfFilePath)
        {
            //store the file path for the BWMF
            this.bwmfFilePath = bwmfFilePath;
            InitializeComponent();
            LoadMissionElementsFromDirectory();
        }

        /// <summary>
        /// takes user to the page for editing mission units
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomisePlayerUnitsButton_Click(object sender, RoutedEventArgs e) //TODO: make this page do something
        {
            NavigationService.Navigate(new UnitViewerPage());
        }

        /// <summary>
        /// Takes user to the page for editing mission briefing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomiseBriefingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Briefing(bwmfFilePath));
        }

        /// <summary>
        /// Populates the fields in the MissionViewPage with information taken from the passed directory
        /// </summary>
        private void LoadMissionElementsFromDirectory()
        {
            LoadElementsFromDirectoryName();
            LoadElementsFromDescriptionExt();
        }

        /// <summary>
        /// Populates Mission file name and Map name fields using information taken from the directory
        /// </summary>
        private void LoadElementsFromDirectoryName()
        {
            //get bwmf folder name
            string bwmfFolderName = GetBWMFFolderName();

            //mission file extension
            string extension = "";

            //mission name
            string missionFileNameString = bwmfFolderName;

            //find file extension
            //work backwards through the string until period found
            for (int i = bwmfFolderName.Length - 1; i > 0; --i)
            {
                if (bwmfFolderName[i] == '.')
                {
                    //Set the extension and mission name
                    extension = bwmfFolderName.Substring(i + 1, bwmfFolderName.Length - i - 1);
                    missionFileNameString = bwmfFolderName.Substring(0, i - 1);
                }
            }

            //set mission file name to substring
            missionFileName.Text = missionFileNameString;

            //set map based on extension or to BWMF if its empty
            switch (extension)
            {
                case "anim_helvantis_v2":
                    mapNameComboBox.SelectedItem = HelvantisMap;
                    break;
                case "Altis":
                    mapNameComboBox.SelectedItem = altisMap;
                    break;
                case "Stratis":
                    mapNameComboBox.SelectedItem = stratisMap;
                    break;
                default:
                    mapNameComboBox.SelectedItem = bwmfTemplate;
                    break;
            }
        }

        //TODO: make this function follow some kind of logic (or at least be less shit)
        /// <summary>
        /// Returns the name of the BWMF folder. Not the file path to the folder. Sets the path to the BWMF parent Folder.
        /// </summary>
        /// <returns></returns>
        private string GetBWMFFolderName()
        {
            //work backwards through the string until first slash found
            for (int i = bwmfFilePath.Length - 1; i > 0; --i)
            {
                if (bwmfFilePath[i] == '\\')
                {
                    //set path to BWMF folder
                    pathToParentFolder = bwmfFilePath.Substring(0, i);
                    //return the substring containing letters after the first slash
                    return bwmfFilePath.Substring(i + 1, bwmfFilePath.Length - i - 1);
                }
            }

            //if no slash found return an empty string
            throw new System.IO.FileNotFoundException();
            //return "";
  
        }

        /// <summary>
        /// Populates Author Name, Mission Type, and Mission Name using information from description.ext in the BWMF
        /// </summary>
        private void LoadElementsFromDescriptionExt()
        {
            string descriptionExtPath = bwmfFilePath + "\\description.ext";

            //read in the description.ext if it exists
            try
            {
                if (File.Exists(descriptionExtPath))
                {
                    using(StreamReader sr = new StreamReader(descriptionExtPath))
                    {
                        descriptionExtTxt = sr.ReadToEnd();
                    }
                }
                else
                {
                    throw (new System.IO.IOException("File Not Found"));
                }

                //Display the Author Name, Mission Name, and mission Type
                DisplayAuthorName(ref descriptionExtTxt);
                DisplayMission(ref descriptionExtTxt);
                DisplayMissionType(ref descriptionExtTxt);
            }
            catch
            {
                MessageBox.Show("Error reading description.ext file", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            

        }


        /// <summary>
        /// Uses regex to find the Author Name from a given description.ext
        /// </summary>
        /// <param name="description">text from a BWMF descreption.ext</param>
        /// <returns></returns>
        private void DisplayAuthorName(ref string description)
        {
            var matches = Regex.Matches(description, authorNameRegexExpression, RegexOptions.Singleline);
            foreach (Match nameMatch in matches)
            {
                authorName.Text = nameMatch.ToString();
            }     
        }

        /// <summary>
        /// Uses regex to find the Mission Name from a given description.ext
        /// </summary>
        /// <param name="description">text as a string from a BWMF description.ext</param>
        private void DisplayMission(ref string description)
        {
            var matches = Regex.Matches(description, missionNameRegexExpression, RegexOptions.Singleline);
            foreach(Match missionMatch in matches)
            {
                missionDisplayName.Text = missionMatch.ToString();
            }
        }

        /// <summary>
        /// Uses regex to determine if the mission is a coop or tvt using information from a given description.ext
        /// </summary>
        /// <param name="description">text as a string from a BWMF description.ext</param>
        private void  DisplayMissionType(ref string description)
        {
            var matches = Regex.Matches(description, missionTypeRegexExpression, RegexOptions.Singleline);
            foreach (Match missionTypeMatch in matches)
            {
                if (missionTypeMatch.ToString() == "Coop")
                {
                    missionTypeSelector.SelectedItem = missionTypeCOOP;
                }
                else
                {
                    missionTypeSelector.SelectedItem = missionTypeTVT;
                }
            }
        }

        private void missionTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Replaces elements in the description.ext with their counterparts from MissionViewPage and saves
        /// the changes to description.ext
        /// </summary>
        private void SaveElementsToDescriptionExt()
        {
            //replace author name
            Regex authorReplaceRegex = new Regex(authorNameRegexExpression);
            descriptionExtTxt = authorReplaceRegex.Replace(descriptionExtTxt, authorName.Text);

            //replace Mission Name
            Regex missionReplaceRegex = new Regex(missionNameRegexExpression);
            descriptionExtTxt = missionReplaceRegex.Replace(descriptionExtTxt, missionDisplayName.Text);

            //replace Mission type
            string missionTypeReplacement="TDM";
            if(missionTypeSelector.SelectedItem.Equals(missionTypeCOOP)) //change replacement if mission is a coop
            {
                missionTypeReplacement = "Coop";
            }

            Regex missionTypeReplaceRegex = new Regex(missionTypeRegexExpression);
            descriptionExtTxt = missionTypeReplaceRegex.Replace(descriptionExtTxt, missionTypeReplacement);

            //write file
            string descriptionExtPath = bwmfFilePath + "\\description.ext";
            try
            {
                System.IO.File.WriteAllText(descriptionExtPath, descriptionExtTxt);
            }

            catch
            {
                MessageBox.Show("Error writing description.ext", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// Renames the mission directory with values taken from MissionViewPage
        /// </summary>
        private void RenameMissionDirectory()
        {
            //Determine what file extension to add based on the selected map
            string fileType;
            switch (((ComboBoxItem)mapNameComboBox.SelectedItem).Content.ToString())
            {
                case "Helvantis":
                    fileType = ".anim_helvantis_v2";
                    break;
                case "Altis":
                    fileType = ".Altis";
                    break;
                case "Stratis":
                    fileType = ".Stratis";
                    break;
                case "BWMF Template":
                    fileType = "";
                    break;
                default:
                    fileType = "";
                    break;
            }

            //rename directory based on MissionViewPage values
            string newFilePath = pathToParentFolder + "\\" +missionFileName.Text + fileType;

            if (newFilePath != bwmfFilePath)
            {
                try
                {
                    Directory.Move(bwmfFilePath, newFilePath);
                }
                catch
                {
                    MessageBox.Show("Could not save new directory", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
           
            bwmfFilePath = newFilePath;
            
            
            GetBWMFFolderName();
        }

        /// <summary>
        /// Saves values input for Mission type, Author Name, Mission Name, and Mission File Name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveElementsToDescriptionExt();
            }
            catch
            {
                MessageBox.Show("Error writing description.ext have you set all fields?", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            RenameMissionDirectory();
        }
    }
}
