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
    /// Interaction logic for UnitViewerPage.xaml
    /// </summary>
    public partial class UnitViewerPage : Page
    {
        public UnitViewerPage()
        {
            InitializeComponent();

            //Initialize lists to store GroupGUIs 
            activeGuiGroupList = new List<GroupGUI>();
            blueGuiGroupList = new List<GroupGUI>();
            indGuiGroupList = new List<GroupGUI>();
            opfGuiGroupList = new List<GroupGUI>();
            civGuiGroupList = new List<GroupGUI>();
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

        //Lists of GuiGroups
        private List<GroupGUI> activeGuiGroupList;
        private List<GroupGUI> blueGuiGroupList;
        private List<GroupGUI> indGuiGroupList;
        private List<GroupGUI> opfGuiGroupList;
        private List<GroupGUI> civGuiGroupList;
    }
}
