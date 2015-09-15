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
        private string bwmfFilePath;
        private string pathToParentFolder;

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

            //set map based on extension or to helvantis if its empty
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
                    mapNameComboBox.SelectedItem = HelvantisMap;
                    break;
            }
        }

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
                    MessageBox.Show(pathToParentFolder);
                    //return the substring containing letters after the first slash
                    return bwmfFilePath.Substring(i + 1, bwmfFilePath.Length - i - 1);
                }
            }

            //if no slash found return an empty string
            return "";
            throw new System.IO.FileNotFoundException();
        }

        /// <summary>
        /// Populates Author Name, Mission Type, and Mission Name using information from description.ext in the BWMF
        /// </summary>
        private void LoadElementsFromDescriptionExt()
        {

        }
    }
}
