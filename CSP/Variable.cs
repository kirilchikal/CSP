using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Variable
    {
        public int Nr;
        public int Value = -1;
        public bool IsSet;
        public Domain Domain { get; set; }
        public Propagation Propagation;

        public void Set(int value)
        {
            IsSet = true;
            Value = value;
            Domain.Values.Remove(value);
        }

        public void UnSet(int value)
        {
            IsSet = false;
            Value = -1;
            Domain.Values.Add(value);
        }

        public void UndoEliminations()
        {
            Propagation.UndoEliminations();
        }
    }
}
