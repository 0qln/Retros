using Retros.ClientWorkStation.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Controls.Primitives;
using WpfCustomControls;

namespace Retros.ClientWorkStation {
    public class WorkTable {
        private static RowDefinition tabsRow = new RowDefinition { Height = new GridLength(18) };
        private static RowDefinition tabDefinitionRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
        private static StackPanel tabPanel = new();
        private static List<Tab> tabs = new();
        private static int currentSelectedTab = 0;


        public WorkTable() {
            WorkStation.MainGrid.RowDefinitions.Add(tabsRow);
            WorkStation.MainGrid.RowDefinitions.Add(tabDefinitionRow);
            Helper.SetChildInGrid(WorkStation.MainGrid, tabPanel, 0, 0);

            tabPanel.Orientation = Orientation.Horizontal;
        }


        public void AddTab(Tab tab) {
            tabs.Add(tab);
            tab.Index = tabs.Count - 1;
            tabPanel.Children.Add(tab.FrameworkElement);
            Helper.SetChildInGrid(WorkStation.MainGrid, tabs[tab.Index].Body.FrameworkElement, 1, 0);
            tabs[tab.Index].Body.Hide();

            tab.Handle.FrameworkElement.MouseDown += (s, e) => {
                SelectTab(tab.Index);
            };
        }


        public void SelectTab(int index) {
            if (index >= 0 && index < tabs.Count) {
                tabs[currentSelectedTab].Body.Hide();
                tabs[index].Body.Show();
                currentSelectedTab = index;
            }
        }

    }
}
