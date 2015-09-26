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
using Microsoft.Win32;



namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for EditMissionPage.xaml
    /// </summary>
    public partial class EditMissionPage : Page
    {
        /// <summary>
        /// File path to bwmf folder
        /// </summary>
        private string filePath;

        public EditMissionPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Opens a dialog to select the BWMF folder
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openFileDialogueButton_Click(object sender, RoutedEventArgs e)
        {

            //make user select BWMF folder and store the success status of the operation
            System.Windows.Forms.FolderBrowserDialog folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult folderSelectresult = folderDialog.ShowDialog();

            //String that will hold file path
            filePath = "";
            
            //if the operation was successful store the selected file path
            if (folderSelectresult == System.Windows.Forms.DialogResult.OK)
            {
                filePath = folderDialog.SelectedPath;
            }
            else
            {
                MessageBox.Show("Error selecting folder", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            
        }

        /// <summary>
        /// Checks to see if a BWMF folder has been selected then moves on to Mission Viewer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            //if filePath has been set continue
            if(Directory.Exists(filePath))
            {
                NavigationService.Navigate(new MissionViewPage(filePath));
            }
            else
            {
                MessageBox.Show("A BWMF folder must be selected to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            }
        }

    }
}
