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
    /// Interaction logic for GroupGUI.xaml
    /// </summary>
    public partial class GroupGUI : UserControl
    {
        public GroupGUI()
        {
            InitializeComponent();
        }

        public GroupGUI(string groupContents)
        {
            InitializeComponent();

            //Initialize Group member variables
            side = GetSideFromString(ref groupContents);
            string unitsSubstring = GetUnitsSubString(ref groupContents);
            units = new List<Unit>();

            //create units
            List<string> unitStrings = GetIndivisualUnitStrings(ref unitsSubstring);
            foreach (string unitString in unitStrings)
            {
                Unit newUnit = new Unit(unitString);
                units.Add(newUnit);
                DisplayUnit(newUnit);
            }

            //Display group name of first group
            if (units.Count() > 0)
            {
                groupNameTextBox.Text = units[0].GroupName;
            }
        }

        #region Importing from SQM

        /// <summary>
        /// returns the Sides enum value that corresponds to the side stored in the passed string
        /// The string must be correctly formated.
        /// </summary>
        /// <param name="groupContents">Formated string containing the side of the group
        /// Example:
        /// class Item1
        /// {
        ///     side="EAST";
        ///     class Vehicles
        ///     {
        ///         items=1;
        ///         class Item0
        ///         {
        ///             position[]={1.4001919,5,-0.013671875};
        ///             id=2;
        ///             side="EAST";
        ///             vehicle="O_Soldier_F";
        ///             leader=1;
        ///             skill=0.60000002;
        ///         };
        ///     };
        /// };
        /// 
        /// </param>
        /// <returns></returns>
        private Sides GetSideFromString(ref string groupContents)
        {
            //regular expression for finding the side of the group
            const string sideRegexExpression = "(?<=side=\")(.*?)(?=\")";

            try
            {
                //Find the first match and use it to assign the group to a side. All other matches are for individual units
                var sideMatches = Regex.Matches(groupContents, sideRegexExpression, RegexOptions.Singleline);
                switch (sideMatches[0].ToString())
                {
                    case "WEST":
                        return Sides.BLUF;
                    case "EAST":
                        return Sides.OPF;
                    case "GUER":
                        return Sides.IND;
                    case "CIV":
                        return Sides.CIV;
                    case "LOGIC":
                        return Sides.LOGIC;
                    case "EMPTY":
                        return Sides.EMPTY;
                    default:
                        MessageBox.Show("Error side: " + sideMatches[0].ToString() + " is not a valid side. Defaulting to BLUF.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return Sides.BLUF;
                }
            }
            catch
            {
                MessageBox.Show("Error finding group side. Defaulting to BLUF.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return Sides.BLUF;
            }
            
        }

        /// <summary>
        /// Returns string containing the units found from the passed group string.
        /// the substring will contain only the internals of the Vehicles class within each group
        /// Does not include the "class Vehicles" or any of the curly braces for the class.
        /// </summary>
        /// <param name="groupContents">Formated string containing the side of the group. 
        ///  Example:
        /// class Item1
        /// {
        ///     side="EAST";
        ///     class Vehicles
        ///     {
        ///         items=1;
        ///         class Item0
        ///         {
        ///             position[]={1.4001919,5,-0.013671875};
        ///             id=2;
        ///             side="EAST";
        ///             vehicle="O_Soldier_F";
        ///             leader=1;
        ///             skill=0.60000002;
        ///         };
        ///     };
        /// };
        /// 
        /// </param>
        /// <returns>String containing Units from mission.sqf</returns>
        private string GetUnitsSubString(ref string groupContents)
        {
            const string vehicleSubstringRegexExpression = @"(?<=class Vehicles\n\t\t\t\{)(.*?)(?=\t\t\t\};\n\t\t\};)";

            var vehicleSubstringMatches = Regex.Matches(groupContents, vehicleSubstringRegexExpression, RegexOptions.Singleline);
            try
            {
                return vehicleSubstringMatches[0].ToString();
            }
            catch
            {
                MessageBox.Show("Error finding vehicle substring in mission.sqm", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
        }

        /// <summary>
        /// Separates each individual unit from the passed string and returns them as separate strings.
        /// returned strings include the "class Item" and curly braces.
        /// </summary>
        /// <param name="unitsSubstring"></param>
        /// <returns></returns>
        private List<string> GetIndivisualUnitStrings(ref string unitsSubstring)
        {
            //initialize return value and regex
            List<string> unitStrings = new List<string>();
            const string individualUnitRegexExpression = @"(class Item\d*)(.*?)(\n\t\t\t\t\};)";

            //find all matches and add them to the return list
            var unitMatches = Regex.Matches(unitsSubstring, individualUnitRegexExpression, RegexOptions.Singleline);
            foreach (var match in unitMatches)
            {
                unitStrings.Add(match.ToString());
            }

            return unitStrings;

        }

        #endregion

        #region units

        /// <summary>
        /// Adds a unit to be displayed in the groups units scroll viewer
        /// </summary>
        /// <param name="unit">Unit to add to the groups scroll viewer.</param>
        private void DisplayUnit(Unit unit)
        {
            //Add unit element to the bottom of the grid
            unitGrid.RowDefinitions.Add(new RowDefinition());
            int gridRow = unitGrid.RowDefinitions.Count() - 1;
            unitGrid.Children.Add(unit);
            unit.SetValue(Grid.RowProperty, gridRow);
        }
        #endregion

        #region public Member variables
        public Sides side;
        public List<Unit> units;
        #endregion

        /// <summary>
        /// Creates and displays a new unit in this group.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addUnitButton_Click(object sender, RoutedEventArgs e)
        {
            Unit newUnit = new Unit(side);
            DisplayUnit(newUnit);
        }
    }
}
