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

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for MissionViewPage.xaml
    /// </summary>
    public partial class MissionViewPage : Page
    {
        private string bwmfFilePath;

        public MissionViewPage(string bwmfFilePath)
        {
            //store the file path for the BWMF
            this.bwmfFilePath = bwmfFilePath;
            InitializeComponent();
        }

        //TODO: make these buttons lead to the correct place
        private void CustomisePlayerUnitsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new UnitViewerPage());
        }

        private void CustomiseBriefingButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Briefing(bwmfFilePath));
        }
    }
}
