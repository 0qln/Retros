using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.Program.Workstation.Changes
{
    /*

    public class RemoveChange : INegativeChange
    {
        private Type _valueType;

        public IPositiveChange? NegativeValue { get; }
        public Type ValueType => _valueType;


        private RemoveChange(IPositiveChange? value, Type valueType)
        {
            _valueType = valueType;
            NegativeValue = value;
        }
        public RemoveChange(IPositiveChange value)
        {
            NegativeValue = value;
            _valueType = value.GetType();
        }
        public RemoveChange(Type type)
        {
            _valueType = type;
        }


        public IChange Clone() => new RemoveChange(NegativeValue, ValueType);
    }

    public class FilterHierachyChange : IFilterHierachyChange
    {
        public IPositiveChange[] FilterHierachy { get; }

        public FilterHierachyChange() { }
        public FilterHierachyChange(IPositiveChange[] filters)
        {
            FilterHierachy = filters;
        }

        public IChange Clone() => new FilterHierachyChange((IPositiveChange[])FilterHierachy.Clone());
    }
    */
}
