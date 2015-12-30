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
using System.Collections.ObjectModel;
using System.Reflection;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for MissionViewPage.xaml
    /// </summary>
    public partial class MissionViewPage : Page
    {

        #region basic class setup
        //filepath strings
        private string bwmfFilePath;
        private string pathToParentFolder;

        //regex for finding elements in description.ext
        string authorNameRegexExpression = "(?<=author = \")(.*?)(?=\";(\\r\\n|\\n)loadScreen)";
        string missionNameRegexExpression = "(?<=onLoadName = \")(.*?)(?=\";)";
        string missionTypeRegexExpression = "(?<=gameType = )(.*?)(?=;)";//TDM for tvt and Coop for COOP

        //string for holding the contents of description.ext
        private string descriptionExtTxt;

        //colors for user feedback
        SolidColorBrush enabledColor = new SolidColorBrush(Colors.Green);
        SolidColorBrush normalColor = new SolidColorBrush(Colors.LightGray);
        SolidColorBrush errorColor = new SolidColorBrush(Colors.Red);

        //bool to prevent save button from being enabled from components loading
        bool trackChanges = false;

        //Stores the known maps and their extensions
        ArmaMaps mapsToDisplay;

        /// <summary>
        /// Basic setup for the mission view page
        /// </summary>
        /// <param name="bwmfFilePath"></param>
        public MissionViewPage(string bwmfFilePath)
        {
            //store the file path for the BWMF
            this.bwmfFilePath = bwmfFilePath;

            InitializeComponent();

            //initialize map storage
            mapsToDisplay = new ArmaMaps();
            LoadKnowMapsFromFile();

            LoadMissionElementsFromDirectory();

            //keep track of changes from here on
            trackChanges = true;
        }
        #endregion

        #region page movement
        /// <summary>
        /// takes user to the page for editing mission units
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomisePlayerUnitsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UnitViewerPage(bwmfFilePath));
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
        #endregion

        #region save / load elements
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

            //set the combo box to the index of the given extension
            mapNameComboBox.SelectedIndex = mapsToDisplay.extensions.IndexOf(extension);

            //TODO: read the map name and extension from
            //set map based on extension or to BWMF if its empty
            /*switch (extension)
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
            }*/
        }

        /// <summary>
        /// Reads in maps found from the formated maps.txt file, associates the map names with their extensions through the
        /// ArmaMaps Class and add the map names to the mapNamesComboBox
        /// </summary>
        private void LoadKnowMapsFromFile()
        {
            //load in the maps file as a string. Trow error if this cannot be completed
            try
            {
                //stores the path to the text file containing the maps
                //string mapFileLocation = @"C:\Users\R Lavin\Desktop\Programing\C#\Nihon Mission Maker\Nihon Mission Maker\Maps.txt";
                string mapFileLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Maps.txt";//@".\Maps.txt";
                if (File.Exists(mapFileLocation))
                {
                    using (StreamReader sr = new StreamReader(mapFileLocation))
                    {
                        //string to hold unaltered line containing the formated map display name
                        string rawMapAndExtension;

                        //read in maps and add them to the GUI until the end of the stream is reached
                        while (!sr.EndOfStream)
                        {
                            //read in the line
                            rawMapAndExtension = sr.ReadLine();

                            //Whitespace should not be present. Break if white space is found
                            if (String.IsNullOrWhiteSpace(rawMapAndExtension))
                            {
                                break;
                            }

                            //store the map
                            mapsToDisplay.AddMap(rawMapAndExtension);
                        }
                    }
                }
                else
                {
                    throw (new System.IO.IOException("File Not Found"));
                }
            }

            catch
            {
                MessageBox.Show("Error reading map file", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            //set mapNameComboBox items to the maps found
            mapNameComboBox.ItemsSource = mapsToDisplay.mapNames;
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
                    using (StreamReader sr = new StreamReader(descriptionExtPath))
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
                //display error
                SaveButton.Background = errorColor;
                MessageBox.Show("Error writing description.ext have you set all fields?", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            RenameMissionDirectory();

            //reset color to show save sucessful
            SaveButton.Background = normalColor;
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
            string missionTypeReplacement = "TDM";
            if (missionTypeSelector.SelectedItem.Equals(missionTypeCOOP)) //change replacement if mission is a coop
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
            //Determine what file extension to add based on the selected map. Add nothing if the file extension is empty
            string selectedMapExtension = mapsToDisplay.extensions[mapNameComboBox.SelectedIndex];
            string fileType= "";
            if (!String.IsNullOrWhiteSpace(selectedMapExtension))
            {
                fileType = "." + mapsToDisplay.extensions[mapNameComboBox.SelectedIndex];
            }

            //validate directory name
            if (!Validation.FileNameValidation(missionFileName.Text))
            {
                MessageBox.Show("Invalid mission file name. Filename not changed.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            //rename directory based on MissionViewPage values
            string newFilePath = pathToParentFolder + "\\" + missionFileName.Text + fileType;

            if (newFilePath != bwmfFilePath)
            {
                try
                {
                    Directory.Move(bwmfFilePath, newFilePath);
                }
                catch
                {
                    //Display error with save
                    SaveButton.Background = errorColor;
                    MessageBox.Show("Could not save new directory", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            bwmfFilePath = newFilePath;


            GetBWMFFolderName();
        }
        #endregion

        #region detect changes

        /// <summary>
        /// Call whenever a MissionViewPage element is changed. Changes color of save button to enabled
        /// Use for input validation across multiple input sources
        /// </summary>
        private void ElementChanged()
        {
            //only change colors if changes are being tracked
            if (trackChanges)
            {
                //change the color of the element to show it can be saved
                SaveButton.Background = enabledColor;
            }

        }

        /// <summary>
        /// Detects changes in the mission type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missionTypeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ElementChanged();
        }

        /// <summary>
        /// detects changes in the author name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authorName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElementChanged();
        }

        /// <summary>
        /// detects changes in the display name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missionDisplayName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElementChanged();
        }

        /// <summary>
        /// detects changes in the mission file name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void missionFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElementChanged();
        }

        /// <summary>
        /// detects changes in the map name ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ElementChanged();
        }

        #endregion

        #region display elements
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
            foreach (Match missionMatch in matches)
            {
                missionDisplayName.Text = missionMatch.ToString();
            }
        }

        /// <summary>
        /// Uses regex to determine if the mission is a coop or tvt using information from a given description.ext
        /// </summary>
        /// <param name="description">text as a string from a BWMF description.ext</param>
        private void DisplayMissionType(ref string description)
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




        #endregion

    }

    /// <summary>
    /// Class that associates map names with their Arma map extensions
    /// </summary>
    public class ArmaMaps
    {
        /// <summary>
        /// Collection of map names to be displayed.
        /// used with combo box
        /// Index corresponds directly to those of extensions
        /// </summary>
        public ObservableCollection<string> mapNames;

        /// <summary>
        /// List containing the extensions for the maps in mapNames
        /// Index corresponds directly to those of mapNames
        /// </summary>
        public List<string> extensions;

        /// <summary>
        /// Constructor for ArmaMap. Takes a string and splits it across a tab to get the display name and the extension
        /// </summary>
        /// <param name="mapLine">Single line string with the map display name and map extension separated by a tab</param>
        public ArmaMaps()
        {
            //collection to display with the map name combo box
            mapNames = new ObservableCollection<string>();
            extensions = new List<string>();
            mapNames.Add("BWMF template");
            extensions.Add("");
        }

        /// <summary>
        /// Splits properly formated string into two parts and stores them into the mapNames and extensions
        /// </summary>
        /// <param name="mapLine">formated string with map display name and map extension separated by a single tab</param>
        public void AddMap(string mapLine)
        {
            //split input
            string[] mapAndextensionArray = mapLine.Split('\t');
            //add map and extension to correct array
            mapNames.Add(mapAndextensionArray[0]);

            if (Validation.FileNameValidation(mapAndextensionArray[1]))
            {
                extensions.Add(mapAndextensionArray[1]);
            }
            else
            {
                MessageBox.Show("Map extension contains invalid characters. Using default extension.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                extensions.Add("");
            }
        }

        /// <summary>
        /// returns the extension text associated with the given map name
        /// if the map name is not found returns the default extension
        /// </summary>
        /// <param name="mapName">map name associated with desired extension</param>
        /// <returns>extension associated with given map name</returns>
        public string NameToExtension(string mapName)
        {
            int index = mapNames.IndexOf(mapName);

            //if the index is valid return the associated extension
            if (index >= 0 && index < mapNames.Count)
            {
                return extensions[index];
            }
            //if the index is not valid return the default
            else
            {
                return extensions[0];
            }
        }

        /// <summary>
        /// returns the map name text associated with the given extension
        /// </summary>
        /// <param name="extension">extension associated with desired map name</param>
        /// <returns>map name associated with given extension</returns>
        public string ExtensionToName(string extension)
        {
            int index = extensions.IndexOf(extension);

            //if the index is valid return the associated map name
            if (index >= 0 && index < extensions.Count)
            {
                return mapNames[index];
            }

            //if the index is not valid return the default
            else
            {
                return mapNames[0];
            }
        }

    }
}
