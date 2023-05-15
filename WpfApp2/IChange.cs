using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retros {
    public interface IChange {
        public void Apply();
        public bool Applied();
    }
}
