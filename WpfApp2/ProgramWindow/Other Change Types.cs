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
        private Type _valueType;

        public IPositiveChange? Value { get; }
        public Type ValueType => _valueType;


        public RemoveChange(IPositiveChange value) {
            Value = value;
            _valueType = value.GetType();
        }
        public RemoveChange(Type type) {
            _valueType = type;
        }

        private RemoveChange(IPositiveChange? value, Type valueType) {
            _valueType = valueType;
            Value = value;
        }


        public IChange Clone() => new RemoveChange(Value, ValueType);
    }
}
