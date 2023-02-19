using System.Collections.Generic;
using CSP.Interfaces;

namespace CSP
{
    public class BinaryPuzzle : IProblem    //use test
    {
        int n;

        public BinaryPuzzle(int n)
        {
            this.n = n;
        }

        public Model CreateModel()
        {
            Model model = new Model(new List<int> { 0, 1 });

            for (int i = 0; i < n * n; i++)
            {
                model.AddVariable();
            }

            string data = System.IO.File.ReadAllText($"../../../Data/binary_{n}x{n}").Replace("\n", "").Replace("\r", "");

            for (int i = 0; i < n * n; i++)
            {
                if (data[i] != 'x')
                {
                    model.Variables[i].Set(data[i] - '0');
                    model.Variables[i].Domain.Values.Clear();
                }
            }

            //ADD CONSTRAINTS TO MODEL

            // First constrant
            for (int i = 0; i < n; i++)
            {
                Variable v1, v2, v3;
                // Constraints for rows
                for (int j = i * n; j < (i + 1) * n - 2; j++)
                {
                    v1 = model.Variables[j];
                    v2 = model.Variables[j + 1];
                    v3 = model.Variables[j + 2];
                    model.Constraints.Add(new AllNotTheSameConstraint(v1, v2, v3));
                }
                // Constraints for columns
                for (int j = i; j < n * (n - 2); j += n)
                {
                    v1 = model.Variables[j];
                    v2 = model.Variables[j + n];
                    v3 = model.Variables[j + 2 * n];
                    model.Constraints.Add(new AllNotTheSameConstraint(v1, v2, v3));
                }
            }

            //Second constraint
            model.Constraints.Add(new UniqueRowsAndColumnsConstraint(model));

            // Third constraint
            for (int i = 0; i < n; i++)
            {
                Variable[] row = model.Variables.GetRange(i * n, n).ToArray();

                model.Constraints.Add(new SumEqualsConstraint(n / 2, row));

                List<Variable> col = new List<Variable>();
                for (int j = i; j < n * n; j += n)
                {
                    col.Add(model.Variables[j]);
                }
                model.Constraints.Add(new SumEqualsConstraint(n / 2, col.ToArray()));
            }



            return model;
        }
    }


}
