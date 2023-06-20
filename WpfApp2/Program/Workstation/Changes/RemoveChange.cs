using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros.Program.Workstation.Changes {
    public class RemoveFilter : RemoveFilterBase {
        public RemoveFilter(IFilter filterInstance) : base(filterInstance) { }

        public override IChange Clone() {
            return new RemoveFilter(RemovedFilter);
        }
    }
}
