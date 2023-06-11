using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Retros.ProgramWindow {
    public class RootChange : IEmptyChange {
        public bool Applied => false;
        public void Generate(WriteableBitmap bitmap) { }
        public IChange Clone() => new RootChange();
    }

    public class RemoveChange : INegativeChange {
        public IPositiveChange? Value { get; }
        public Type? ValueType { get; }


        public RemoveChange(IPositiveChange value) => Value = value;
        public RemoveChange(Type type) => ValueType = type;

        private RemoveChange(IPositiveChange? value, Type? valueType) {
            ValueType = valueType;
            Value = value;
        }


        public IChange Clone() => new RemoveChange(Value, ValueType);
    }
}
