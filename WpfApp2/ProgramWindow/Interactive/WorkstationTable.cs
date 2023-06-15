using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utillities.Wpf;
using Retros.ProgramWindow.Interactive.Tabs;
using Retros.ProgramWindow.Interactive.Tabs.Bodies;
using Retros.ProgramWindow.Interactive.Tabs.Handles;

namespace Retros.ProgramWindow.Interactive {
    public partial class WorkstationTable : IFrameworkElement {
        private RowDefinition tabsRow = new RowDefinition { Height = new GridLength(18) };
        private RowDefinition tabDefinitionRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
        private StackPanel tabPanel = new();
        private List<Tab> tabs = new();
        private int currentSelectedTab = 0;
        private Grid mainGrid = new();

        public FrameworkElement FrameworkElement => mainGrid;


        public WorkstationTable() {
            mainGrid.RowDefinitions.Add(tabsRow);
            mainGrid.RowDefinitions.Add(tabDefinitionRow);
            Helper.SetChildInGrid(mainGrid, tabPanel, 0, 0);

            tabPanel.Orientation = Orientation.Horizontal;
        }


        public void AddTab(Tab tab) {
            tabs.Add(tab);
            tab.Index = tabs.Count - 1;
            tabPanel.Children.Add(tab.FrameworkElement);
            Helper.SetChildInGrid(mainGrid, tabs[tab.Index].Body.FrameworkElement, 1, 0);
            tabs[tab.Index].Body.Hide();

            tab.Handle.FrameworkElement.MouseDown += (s, e) => {
                SelectTab(tab.Index);
            };
        }

        public Tab? GetTab(Type type) {
            try {
                return tabs.First(tab => tab.GetType() == type);
            }
            catch (InvalidOperationException e) {
                DebugLibrary.Console.Log(e);
                return null;
            }
            catch (ArgumentNullException e) {
                DebugLibrary.Console.Log(e);
                return null;
            }
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

