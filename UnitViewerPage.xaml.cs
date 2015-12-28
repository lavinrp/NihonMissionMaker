﻿using System;
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

        public void LoadGroups()
        {

        }

        private void SideSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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

            //Add the group to the active groupList
            activeGuiGroupList.Add(newGroup);
            
            //Add group element to the bottom of the grid
            GroupGrid.RowDefinitions.Add(new RowDefinition());
            int gridRow = GroupGrid.RowDefinitions.Count() - 1;
            GroupGrid.Children.Add(newGroup);
            newGroup.SetValue(Grid.RowProperty, gridRow);
        }

        private List<GroupGUI> activeGuiGroupList;
        private List<GroupGUI> blueGuiGroupList;
        private List<GroupGUI> indGuiGroupList;
        private List<GroupGUI> opfGuiGroupList;
        private List<GroupGUI> civGuiGroupList;
    }
}
