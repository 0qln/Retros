using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Utillities.Wpf;
using Retros.Program.Workstation.TabUI.Tabs;
using System.CodeDom;
using System.Collections;

namespace Retros.Program.Workstation.TabUI
{
    // old name: WorkstationTable
    public partial class WorkstationTabPresenter : IFrameworkElement, IEnumerable
    {
        private RowDefinition tabsRow = new RowDefinition { Height = new GridLength(18) };
        private RowDefinition tabDefinitionRow = new RowDefinition { Height = new GridLength(1, GridUnitType.Star) };
        private StackPanel tabPanel = new();
        private List<Tab> tabs = new();
        private int currentSelectedTab = 0;
        private Grid mainGrid = new();

        public FrameworkElement FrameworkElement => mainGrid;
        

        public WorkstationTabPresenter()
        {
            mainGrid.RowDefinitions.Add(tabsRow);
            mainGrid.RowDefinitions.Add(tabDefinitionRow);
            Helper.SetChildInGrid(mainGrid, tabPanel, 0, 0);

            tabPanel.Orientation = Orientation.Horizontal;
        }


        public void AddTab(Tab tab)
        {
            tabs.Add(tab);
            tab.Index = tabs.Count - 1;
            tabPanel.Children.Add(tab.FrameworkElement);
            Helper.SetChildInGrid(mainGrid, tabs[tab.Index].Body.FrameworkElement, 1, 0);
            tabs[tab.Index].Body.Hide();

            tab.Handle.FrameworkElement.MouseDown += (s, e) =>
            {
                SelectTab(tab.Index);
            };
        }

        public Tab? GetTab(Type type)
        {            
            return tabs.FirstOrDefault(tab => tab.GetType() == type);            
        }
        public Tab? GetTab<T>() {
            return tabs.FirstOrDefault(tab => tab.GetType() == typeof(T));
        }

        public void SelectTab(int index)
        {
            if (index >= 0 && index < tabs.Count)
            {
                tabs[currentSelectedTab].Body.Hide();
                tabs[index].Body.Show();
                currentSelectedTab = index;
            }
        }


        public IEnumerator<Tab> GetEnumerator() {
            return new TabEnumerator(tabs);
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public class TabEnumerator : IEnumerator<Tab> {
        private List<Tab> _tabs;
        private int _position = -1;

        public Tab Current {
            get {
                try {
                    return _tabs[_position];
                }
                catch (IndexOutOfRangeException) {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => Current;


        public TabEnumerator(List<Tab> tabs) {
            _tabs = tabs;
        }


        public void Dispose() {
            throw new NotImplementedException();
        }

        public bool MoveNext() {
           _position++;
            return _position < _tabs.Count;
        }

        public void Reset() {
            _position = -1;
        }
    }
}

