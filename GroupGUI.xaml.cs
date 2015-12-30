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

            SetMembersFromString(ref groupContents);

            //TODO: create units from string here
        }

        #region private methods

        /// <summary>
        /// Uses information in the passed string to set the groups side and create
        /// and store the groups units
        /// </summary>
        /// <param name="groupContents">String containing the entire group class from the mission.sqm
        /// Example:
        /// class Item1
	    /// {
	    ///     side="EAST";
	    ///     class Vehicles
        ///     {
        ///         items=1;
	    /// 	    class Item0
        ///         {
        ///             position[]={1.4001919,5,-0.013671875};
	    /// 		    id=2;
	    /// 		    side="EAST";
	    /// 		    vehicle="O_Soldier_F";
	    /// 		    leader=1;
	    /// 		    skill=0.60000002;
	    /// 	    };
        ///     };
        /// };
        /// 
        /// </param>
        private void SetMembersFromString(ref string groupContents)
        {
            side = GetSideFromString(ref groupContents);
        }

        /// <summary>
        /// returns the Sides enum value that corresponds to the side stored in the passed string
        /// The string must follow the same format as the string in SetMembersFromString
        /// </summary>
        /// <param name="groupContents">Formated string containing the side of the group</param>
        /// <returns></returns>
        private Sides GetSideFromString(ref string groupContents)
        {
            //regular expression for finding the side of the group
            const string sideRegexExpression = "(?<=side=\")(.*?)(?= \";)";

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
        /// The string must follow the same format as the string in SetMembersFromString.
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

        #endregion


        #region public Member variables
        public string name;
        public Sides side;
        //TODO: Add list of units
        #endregion
    }
}
