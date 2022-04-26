using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Propagation
    {
        public List<(Variable, int)> Eliminations;

        public Propagation()
        {
            Eliminations = new  List<(Variable, int)>();
        }

        public void AddElimination(Variable variable, int value)
        {
            Eliminations.Add((variable, value));
        }

        public void UndoEliminations()
        {
            for (int i = 0; i < Eliminations.Count;i++)
            {
                (Variable variable, int value) = Eliminations[i];
                variable.Domain.Values.Add(value);
            }
            Eliminations = new List<(Variable, int)>();
        }
    }
}
