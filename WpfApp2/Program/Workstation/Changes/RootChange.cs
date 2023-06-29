using Retros.Program.Workstation.Image;
using Retros.Program.Workstation.TabUI;
using Retros.Program.Workstation.TabUI.Tabs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Retros.Program.Workstation.Changes {
    public class RootChange : IChange {
        private Workstation _workstation;

        public Action Value { get; private set; }
        public string UITypeName => throw new NotImplementedException();


        public RootChange(Workstation workstation) {
            _workstation = workstation;
            Value = delegate {

                workstation.ImageElement.Page.Image = new System.Windows.Controls.Image { Source = new BitmapImage(workstation.ImageElement.Source) };

                IEnumerator<Tab> enumerator = workstation.TabElement.GetEnumerator();
                while (enumerator.MoveNext()) {
                    if (enumerator.Current.GetType() != typeof(ImageHistoryTab)) {
                        enumerator.Current.Reset();
                    }
                }

            };
        }


        public IChange Clone() {
            RootChange clone = new(_workstation);
            return clone;
        }
    }
}
