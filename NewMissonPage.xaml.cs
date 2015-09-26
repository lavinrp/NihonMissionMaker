using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Microsoft.VisualBasic.FileIO;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for NewMissonPage.xaml
    /// </summary>
    public partial class NewMissonPage : Page
    {
        /// <summary>
        /// path to the destination folder for the new mission
        /// </summary>
        private string newDirfilePath;

        /// <summary>
        /// Initializes values for the new mission page
        /// </summary>
        public NewMissonPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Allows user to select the directory to place the new mission
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectOutputDir_Click(object sender, RoutedEventArgs e)
        {
            //make user select BWMF folder and store the success status of the operation
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult folderSelectresult = folderDialog.ShowDialog();

            //String that will hold file path
            newDirfilePath = "";

            //if the operation was successful store the selected file path
            if (folderSelectresult == System.Windows.Forms.DialogResult.OK)
            {
                newDirfilePath = folderDialog.SelectedPath;
            }
            else
            {
                MessageBox.Show("Error selecting folder", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        /// <summary>
        /// creates BWMF folder in chosen location and moves user to mission view page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            //if filePath has been set continue
            if (Directory.Exists(newDirfilePath))
            {
                //create BWMF folder at location
                //only continue if creation of folder was successful
                if (CopyBWMFToSelectedDir())
                {
                    //navigate to new page
                    NavigationService.Navigate(new MissionViewPage(newDirfilePath));
                }
                else
                {
                    //set the directory path to an invalid one so the user is forced to select a new path
                    newDirfilePath = "";
                }
            }
            else
            {
                MessageBox.Show("A destination folder must be selected to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

        /// <summary>
        /// Copies the BWMF file in the same directory as the exe to the given location
        /// </summary>
        private bool CopyBWMFToSelectedDir()
        {
            //gets the path to the master bwmf file located with the exe
            string masterBWMFLocation = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Master";

            newDirfilePath = newDirfilePath + @"\BWMF";

            try
            {
                Directory.CreateDirectory(newDirfilePath);

                //copy the directory to the new path
                FileSystem.CopyDirectory(masterBWMFLocation, newDirfilePath);
                return true;
            }
            catch
            {
                MessageBox.Show("Could not create BWMF folder at selected location", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
           
        }
    }
}
