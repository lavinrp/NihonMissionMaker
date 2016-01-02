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

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for Unit.xaml
    /// </summary>
    public partial class Unit : UserControl
    {
        public Unit()
        {
            InitializeComponent();
        }

        public Unit(string unitString)
        {
            InitializeComponent();
            ImportDisplayName(ref unitString);
        }

        /// <summary>
        /// Takes the unit description from the mission.sqm and 
        /// </summary>
        /// <param name="unitString"></param>
        public void ImportDisplayName(ref string unitString)
        {
            try
            {
                //Expression for finding the display name
                const string displayNameRegexExpression = "(?<=description=\")(.*)(?=\")";
                var matches = Regex.Matches(unitString, displayNameRegexExpression, RegexOptions.Singleline);
                unitDisplayNameTextBox.Text = matches[0].ToString();
            }
            catch
            {
                MessageBox.Show("Error reading display name of unit", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
