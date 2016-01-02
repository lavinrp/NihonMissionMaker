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
using System.Text.RegularExpressions;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Interaction logic for UnitViewerPage.xaml
    /// </summary>
    public partial class UnitViewerPage : Page
    {
        /// <summary>
        /// Regular expression for finding the contents of the Groups class within the mission.sqm
        /// </summary>
        private const string GroupsRegexExpression = @"(?<=class Groups\n\t\{)(.*?)(=?\n\t\};)";

        /// <summary>
        /// Constructor for UnitViewerPage
        /// Creates the UnitViewerPage and loads all the groups and units from the mission.sqm file
        /// </summary>
        /// <param name="bwmfFilePath"></param>
        public UnitViewerPage(string bwmfFilePath)
        {
            InitializeComponent();

            //File Path
            missionFilePath = bwmfFilePath + "\\mission.sqm";

            //Initialize lists to store GroupGUIs 
            activeGuiGroupList = new List<GroupGUI>();
            blueGuiGroupList = new List<GroupGUI>();
            indGuiGroupList = new List<GroupGUI>();
            opfGuiGroupList = new List<GroupGUI>();
            civGuiGroupList = new List<GroupGUI>();
            logicGuiGroupList = new List<GroupGUI>();

            //store mission.sqf
            ReadMissionFile();
            ImportGroupsFromMission();

        }

        #region Change displayed groups
        /// <summary>
        /// Sets the activeGuiGroupList to the given faction, clears the displayed groupGUIs,
        /// and displays the groupGUIs of the updated activeGuiGroupList.
        /// </summary>
        /// <param name="side">The side whose groups will be displayed. </param>
        public void ChangeDisplayedGroups(Sides side)
        {

            //Set ActiveGuiGroupList to the list of the passed side
            switch (side)
            {
                case Sides.BLUF:
                    activeGuiGroupList = blueGuiGroupList;
                    break;
                case Sides.IND:
                    activeGuiGroupList = indGuiGroupList;
                    break;
                case Sides.OPF:
                    activeGuiGroupList = opfGuiGroupList;
                    break;
                case Sides.CIV:
                    activeGuiGroupList = civGuiGroupList;
                    break;
            }

            //Clear Current displayed groups
            GroupGrid.Children.Clear();
            GroupGrid.RowDefinitions.Clear();

            //Display groups of selected faction
            foreach (GroupGUI group in activeGuiGroupList)
            {
                DisplayGuiGroup(group);
            }

        }

        /// <summary>
        /// Displays the passed GroupGUI at the end of the Groups ScrollBox
        /// </summary>
        /// <param name="guiGroup"> The GroupGUI to display</param>
        private void DisplayGuiGroup(GroupGUI guiGroup)
        {
            //Add group element to the bottom of the grid
            GroupGrid.RowDefinitions.Add(new RowDefinition());
            int gridRow = GroupGrid.RowDefinitions.Count() - 1;
            GroupGrid.Children.Add(guiGroup);
            guiGroup.SetValue(Grid.RowProperty, gridRow);
        }

        /// <summary>
        /// Changes the displayed values to those of the selected faction.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SideSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //get selection
            ComboBoxItem selection = (ComboBoxItem)((ComboBox)sender).SelectedValue;

            //convert selection to string
            string selectionString = (string)selection.Content;

            //Display selected faction briefing
            switch (selectionString)
            {
                case "Blufor":
                    ChangeDisplayedGroups(Sides.BLUF);
                    break;
                case "Opfor":
                    ChangeDisplayedGroups(Sides.OPF);
                    break;
                case "Indfor":
                    ChangeDisplayedGroups(Sides.IND);
                    break;
                case "Civ":
                    ChangeDisplayedGroups(Sides.CIV);
                    break;
            }
        }

        /// <summary>
        /// Creates a new group, adds it to the chosen factions list of groups and displays the new group in the GUI
        /// No more that 144 group can be created for one faction at a given time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewGroupButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO: make sure no more than 144 groups can be created per side
            //Create new group element 
            GroupGUI newGroup = new GroupGUI();

            //Add the group to the active groupList and display it
            activeGuiGroupList.Add(newGroup);
            DisplayGuiGroup(newGroup);
        }
        #endregion

        #region Importing

        /// <summary>
        /// Read in the mission file and save it as a string
        /// </summary>
        private void ReadMissionFile()
        {
            //set default value for the mission file
            missionTxt = "";

            //read in the mission file and save it as a string
            try
            {
                if (File.Exists(missionFilePath))
                {
                    using (StreamReader sr = new StreamReader(missionFilePath))
                    {
                        missionTxt = sr.ReadToEnd();
                    }
                }
                else
                {
                    throw new System.IO.IOException("Mission file not found.");
                }
            }

            //Display error if saving mission file to string fails for any reason
            catch
            {
                MessageBox.Show("Error reading mission.sqm", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

        }

        /// <summary>
        /// Creates groups based on their strings in the mission.sqm and stores them in the correct list of groups
        /// </summary>
        private void ImportGroupsFromMission()
        {
            //Find the group substring and split it into individual strings for each group
            string groupString = GetGroupSubString();
            List<string> groupStrings = GetIndivisualGroupStrings(groupString);

            foreach (string groupInfo in groupStrings)
            {
                GroupGUI newGroup = new GroupGUI(groupInfo);

                //place group in correct list (side)
                switch (newGroup.side)
                {
                    case Sides.BLUF:
                        blueGuiGroupList.Add(newGroup);
                        break;
                    case Sides.OPF:
                        opfGuiGroupList.Add(newGroup);
                        break;
                    case Sides.IND:
                        indGuiGroupList.Add(newGroup);
                        break;
                    case Sides.CIV:
                        civGuiGroupList.Add(newGroup);
                        break;
                    case Sides.LOGIC:
                        //TODO: make sure logic groups are not lost
                        civGuiGroupList.Add(newGroup);
                        break;
                    default:
                        MessageBox.Show("Error storing group. Invalid Side.", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        break;
                }
            }

        }

        /// <summary>
        /// Separates the groups from the contents of the passed text from the Group class of the mission.sqm
        /// returns a list of strings containing the contents of each individual group.
        /// Includes the "Class Item" and curly braces.
        /// </summary>
        /// <param name="GroupsSubString"></param>
        /// <returns>List<string>: A list of all individual group classes from the mission.sqm. Includes the "Class item" and curly braces.</returns>
        private List<string> GetIndivisualGroupStrings(string GroupsSubString)
        {
            //Initialize return list
            List<string> groupStrings = new List<string>();

            //String containing the regular expression for finding each individual group class
            const string individualGroupRegexExpression = @"(\n\t\tclass Item\d*)(.*?)(\n\t\t\};)";

            //find all matches and place them in list of string to return
            var groups = Regex.Matches(GroupsSubString, individualGroupRegexExpression, RegexOptions.Singleline);
            foreach (var match in groups)
            {
                groupStrings.Add(match.ToString());
            }

            return groupStrings;
        }

        /// <summary>
        /// Returns substring of the mission file containing all the groups and their units
        /// </summary>
        /// <returns>String: substring of mission file containing the groups and their units.
        ///  Does not contain the "class Groups" or the curly braces of the groups class</returns>
        private string GetGroupSubString()
        {
            try
            {
                //find and return the first match to the GroupsRegexExpression.
                //If more than one match is found only the first is valid
                var groupTxtMatches = Regex.Matches(missionTxt, GroupsRegexExpression, RegexOptions.Singleline);
                string groupTxt = groupTxtMatches[0].ToString();

                return groupTxt;
            }
            catch
            {
                MessageBox.Show("Error finding groups in mission.sqm", "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
        }

        #endregion

        //Lists of GuiGroups
        private List<GroupGUI> activeGuiGroupList;
        private List<GroupGUI> blueGuiGroupList;
        private List<GroupGUI> indGuiGroupList;
        private List<GroupGUI> opfGuiGroupList;
        private List<GroupGUI> civGuiGroupList;
        private List<GroupGUI> logicGuiGroupList;

        private string missionTxt;

        private string missionFilePath;
    }
}
