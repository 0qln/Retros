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

            DispatcherTimer actionTimer = new();
            Queue<Action> actionQueue = new();

            public ChangeHistory History { get; }


            public WorkstationImage(string path) {
                History = new();
                Helper.SetImageSource(image, path);
                StartUpdating();
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
