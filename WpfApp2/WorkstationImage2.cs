using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;
using Image = System.Windows.Controls.Image;
using System.Windows.Threading;
using WpfCustomControls;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Media;

namespace Retros {
    partial class Workstation {
        public partial class WorkstationImage : IFrameworkElement {
            public FrameworkElement FrameworkElement => Image;
            private Image image = new();
            public Image Image {
                get => image; 
                set => image = value;
            }
            private Image original = new();
            public Image Original => original;

            DispatcherTimer actionTimer = new();
            Queue<Action> actionQueue = new();

            public ChangeHistory History => changeHistory;
            private ChangeHistory changeHistory = new();

            public FilterManager GetFilterManager => filterManager;
            private FilterManager filterManager = new();


            public WorkstationImage(string path) {
                StartUpdating();
                Helper.SetImageSource(image, path);
                Helper.SetImageSource(original, path);
            }

            private void StartUpdating() {
                actionTimer.Interval = UIManager.Framerate;
                actionTimer.Tick += (s, e) => {
                    if (actionQueue.Count > 0) {
                        Action action = actionQueue.Dequeue();
                        action.Invoke();
                    }
                };
                actionTimer.Start();
            }

            public void AddTaskToQueue(Action action) {
                actionQueue.Enqueue(action);
            }


        }
    }
}
