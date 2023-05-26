using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Retros;

namespace Retros
{
    partial class Workstation {
        public partial class WorkstationImage {
            public class ChangeHistory {
                private Stack<System.Windows.Controls.Image> imageChache = new();
                private Stack<IChange> changes = new();

                public void Add(IChange change) {

                }

                public void Forward() {

                }

                public void Backward() {

                }

                public void Clear() {

                }
            }

        }
    }
}
