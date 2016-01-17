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

        #region constructors
        public Unit()
        {
            InitializeComponent();

            playability = "PLAY CDG";
            skill = "0.60000002";
            azimut = "0.51626098";
            string rank = "";
            UpdateGUIRank(ref rank);
            positionXTextBox.Text = "0";
            positionYTextBox.Text = "0";
            positionZTextBox.Text = "0";
        }

        public Unit(string unitString)
        {
            InitializeComponent();
            ImportFromSqm(ref unitString);
        }

        public Unit(Sides side)
        {
            InitializeComponent();

            playability = "PLAY CDG";
            skill = "0.60000002";
            azimut = "0.51626098";
            string rank = "";
            UpdateGUIRank(ref rank);
            UpdateGuiSide(side);
            positionXTextBox.Text = "0";
            positionYTextBox.Text = "0";
            positionZTextBox.Text = "0";
        }

        #endregion

        #region import
        /// <summary>
        /// Finds parameters of a sqm unit using the passed regex pattern
        /// </summary>
        /// <param name="unitString">Formated string from mission.sqm containing the units properties</param>
        /// <param name="regexPattern">Regular expression pattern for finding the desired parameter</param>
        /// <returns>String containing the parameter from the unit. Empty string if parameter was not found.</returns>
        private string ImportUnitParam(ref string unitString, ref string regexPattern)
        {
            //find parameter
            var match = Regex.Match(unitString, regexPattern, RegexOptions.Singleline);
            string paramValue = match.ToString();

            return paramValue;
        }

        /// <summary>
        /// Gets the faction prefix, group name, and position abbreviation from
        /// the full variable name of the unit.
        /// </summary>
        /// <param name="fullVariableName">The full variable name of the unit as it appears in the mission.sqm</param>
        /// <returns>
        /// Array of strings. Position 0 is the faction prefix, position 1 is the group name
        /// and position 2 is the position abbreviation
        /// </returns>
        private string[] GetVariableNameComponents(string fullVariableName)
        {
            const int expectedVariableNameComponents = 3;

            //create return array 
            string[] components;
            components = fullVariableName.Split('_');

            //return components if the expected number is found. Return generic values otherwise.
            if (components.Length == expectedVariableNameComponents)
            {
                return components;
            }
            else
            {
                MessageBox.Show("Imported unit does not follow the expected naming convention. Replacing values of "
                    + unitDisplayNameTextBox.Text + "with defaults", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                string[] genericReturn = {"examplePrefix", "ExampleGroup", "EX"};
                return genericReturn;
            }
        }

        /// <summary>
        /// Imports all possible unit values from the mission.sqm text
        /// </summary>
        /// <param name="unitString"></param>
        private void ImportFromSqm(ref string unitString)
        {
            //Regex patterns
            string displayNameRegexPattern = "(?<=description=\")(.*)(?=\")";
            string sideRegexPattern = "(?<=side=\")(.*?)(?=\")";
            string vehicleNameRegexPattern = "(?<=vehicle=\")(.*?)(?=\")";
            string rankRegexPattern = "(?<=rank=\")(.*?)(?=\")";
            string azimutRegexPattern = "(?<=azimut=)(.*?)(?=;)";
            string playabilityRegexPattern = "(?<=player=\")(.*?)(?=\")";
            string skillRegexPattern = "(?<=skill=)(.*?)(?=;)";
            string variableRegexPattern = "(?<=text=\")(.*?)(?=\")";
            string leaderRegexPattern = "(?<=leader=)(.*?)(?=;)";
            string positionRegexPattern = "(?<=position\\[\\]=\\{)(.*?)(?=\\})";
            
            //DisplayName
            unitDisplayNameTextBox.Text = ImportUnitParam(ref unitString, ref displayNameRegexPattern);

            //side
            string side = ImportUnitParam(ref unitString, ref sideRegexPattern);
            UpdateGuiSide(side);

            //vehicleName
            vehicleNameTextBox.Text = ImportUnitParam(ref unitString, ref vehicleNameRegexPattern);

            //Rank
            string rank = ImportUnitParam(ref unitString, ref rankRegexPattern);
            UpdateGUIRank(ref rank);

            //azimut
            azimut = ImportUnitParam(ref unitString, ref azimutRegexPattern);

            //playability
            playability = ImportUnitParam(ref unitString, ref playabilityRegexPattern);

            //skill
            skill = ImportUnitParam(ref unitString, ref skillRegexPattern);

            //variable name
            completeVariableName = ImportUnitParam(ref unitString, ref variableRegexPattern);
            string[] variableNameComponents = GetVariableNameComponents(completeVariableName);
            factionPrefixTextBox.Text = variableNameComponents[0];
            GroupName = variableNameComponents[1];
            unitPositionAbbreviation.Text = variableNameComponents[2];

            //leader
            string leaderString = ImportUnitParam(ref unitString, ref leaderRegexPattern);
            if (leaderString == "1")
            {
                isLeader = true;
            }
            else
            {
                isLeader = false;
            }

            //position
            string positionString = ImportUnitParam(ref unitString, ref positionRegexPattern);
            const int positionDimensions = 3;
            string[] positionCoordinates = positionString.Split(',');
            if (positionCoordinates.Count() == positionDimensions)
            {
                positionXTextBox.Text = positionCoordinates[0];
                positionYTextBox.Text = positionCoordinates[1];
                positionZTextBox.Text = positionCoordinates[2];
            }
        }
        #endregion

        #region Update GUI
        /// <summary>
        /// Sets the rank combo box to the value of the passed rank string
        /// </summary>
        /// <param name="rank">Rank to display.</param>
        private void UpdateGUIRank(ref string rank)
        {
            //Assign ranks based off string
            //Private
            if (string.IsNullOrEmpty(rank))
            {
                rankComboBox.SelectedItem = rankPrivate;
            }
            else if (rank == "CORPORAL")
            {
                rankComboBox.SelectedItem = rankCorporal;
            }
            else if (rank == "SERGEANT")
            {
                rankComboBox.SelectedItem = rankSergeant;
            }
            else if (rank == "LIEUTENANT")
            {
                rankComboBox.SelectedItem = rankLieutenant;
            }
            else if (rank == "CAPTAIN")
            {
                rankComboBox.SelectedItem = rankCaptain;
            }
            else if (rank == "MAJOR")
            {
                rankComboBox.SelectedItem = rankMajor;
            }
            else if (rank == "COLONEL")
            {
                rankComboBox.SelectedItem = rankColonel;
            }
            else
            {
                MessageBox.Show("Invalid rank passed to unit.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                //select no rank
                rankComboBox.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Sets the selected side in the GUI to the passed side
        /// </summary>
        /// <param name="side">New side to display</param>
        private void UpdateGuiSide(string side)
        {
            switch (side)
            {
                case "WEST":
                    sideComboBox.SelectedItem = sideBluefor;
                    break;
                case "GUER":
                    sideComboBox.SelectedItem = sideIndfor;
                    break;
                case "EAST":
                    sideComboBox.SelectedItem = sideOpfor;
                    break;
                case "CIV":
                    sideComboBox.SelectedItem = sideCiv;
                    break;
                default:
                    //if no valid selection select nothing
                    sideComboBox.SelectedIndex = -1;
                    break;
            }
        }

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
        #endregion

        #region Member Variables
        //Public Member variables
        public string playability;
        public string skill;
        public string azimut;
        public bool isLeader = false;

        /// <summary>
        /// Public getter and setter for groupName.
        /// If the setter fails to validate input an empty string is used. 
        /// </summary>
        public string GroupName
        {
            get
            {
                return groupName;
            }
            set
            {
                //TODO: add validation here
                groupName = value;
            }
        }

        //Private members 
        /// <summary>
        /// Combination of units prefix, group, and position abbreviation 
        /// separated by commas.
        /// 
        /// This is what appears in the mission sqm under the "text"
        /// </summary>
        private string completeVariableName;
        /// <summary>
        /// Internal variable to store the name of the group that the unit belongs to.
        /// </summary>
        private string groupName;
        #endregion
    }
}
