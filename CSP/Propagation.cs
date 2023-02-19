using System.Collections.Generic;

namespace CSP
{
    public class Propagation
    {
        private List<(Variable, int)> _eliminations;

        public Propagation()
        {
            _eliminations = new List<(Variable, int)>();
        }

        public void AddElimination(Variable variable, int value)
        {
            _eliminations.Add((variable, value));
        }

        public void UndoEliminations()
        {
            for (int i = 0; i < _eliminations.Count; i++)
            {
                (Variable variable, int value) = _eliminations[i];
                variable.Domain.Values.Add(value);
            }
            _eliminations = new List<(Variable, int)>();
        }
    }
}
