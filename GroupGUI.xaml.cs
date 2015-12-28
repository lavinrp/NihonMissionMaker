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
    /// Interaction logic for GroupGUI.xaml
    /// </summary>
    public partial class GroupGUI : UserControl
    {
        public GroupGUI()
        {
            InitializeComponent();
        }

        #region public Member variables
        public string GroupName;
        #endregion
    }
}
