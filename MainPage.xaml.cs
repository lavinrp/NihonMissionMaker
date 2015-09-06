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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void newMissionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MissionViewPage());
        }


        private void editMissionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MissionViewPage());
        }
    }
}
