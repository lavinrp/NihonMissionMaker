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

            //DisplayName
            unitDisplayNameTextBox.Text = ImportDisplayName(ref unitString);

            //side
            side = ImportSide(ref unitString);
            UpdateGuiSide(side);

            //vehicleName
            vehicleNameTextBox.Text = ImportVehicleName(ref unitString);

            //Rank
            
        }

        #region Import from SQM
        /// <summary>
        /// Imports the unit description from the mission.sqm
        /// </summary>
        /// <param name="unitString">Formated string from mission.sqm containing the units properties</param>
        /// <returns></returns>
        private string ImportDisplayName(ref string unitString)
        {
            //find display name
            const string displayNameRegexExpression = "(?<=description=\")(.*)(?=\")";
            var match = Regex.Match(unitString, displayNameRegexExpression, RegexOptions.Singleline);
            string displayName = match.ToString();

            //Return name or error
            if (string.IsNullOrEmpty(displayName))
            {
                MessageBox.Show("Error reading display name of unit", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
            return displayName;
        }

        /// <summary>
        /// Imports the unit side from the mission.sqm
        /// </summary>
        /// <param name="unitString">Formated string from mission.sqm containing the units properties</param>
        /// <returns>Side of the unit</returns>
        private Sides ImportSide(ref string unitString)
        {
            //Find side
            const string sideRegexExpression = "(?<=side=\")(.*?)(?=\")";
            var match = Regex.Match(unitString, sideRegexExpression, RegexOptions.Singleline);
            string sideString = match.ToString();

            switch (sideString)
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
                default:
                    MessageBox.Show("Could not import unit side", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return Sides.EMPTY;
            }

        }

        /// <summary>
        /// Imports the vehicle parameter from the mission.sqm
        /// </summary>
        /// <param name="unitString">Formated string from mission.sqm containing the units properties.</param>
        /// <returns>vehicle name found in the passed text.</returns>
        private string ImportVehicleName(ref string unitString)
        {
            //find vehicle name
            const string vehicleNameRegexExpression = "(?<=vehicle=\")(.*?)(?=\")";
            var match = Regex.Match(unitString, vehicleNameRegexExpression, RegexOptions.Singleline);
            string vehicleName = match.ToString();

            //Return vehicle name or error
            if (string.IsNullOrEmpty(vehicleName))
            {
                MessageBox.Show("Error finding unit vehicle name.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
            return vehicleName;
        }

        private void ImportRank(ref string unitString)
        {
            //TODO: fill stub
        }
        #endregion

        /// <summary>
        /// Sets the selected side in the GUI to the passed side
        /// </summary>
        /// <param name="side">New side to display</param>
        private void UpdateGuiSide(Sides side)
        {
            switch (side)
            {
                case Sides.BLUF:
                    sideComboBox.SelectedItem = sideBluefor;
                    break;
                case Sides.IND:
                    sideComboBox.SelectedItem = sideIndfor;
                    break;
                case Sides.OPF:
                    sideComboBox.SelectedItem = sideOpfor;
                    break;
                case Sides.CIV:
                    sideComboBox.SelectedItem = sideCiv;
                    break;
                default:
                    //if no valid selection select nothing
                    sideComboBox.SelectedIndex = -1;
                    break;
            }
        }

        //Member variables
        public Sides side;
        public bool player;
    }
}
