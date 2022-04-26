using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class FutoshikiPuzzle
    {
        int n;

        public FutoshikiPuzzle(int n)
        {
            this.n = n;
        }

        public Model CreateModel()
        {
            Model model = new Model(Enumerable.Range(1, n).ToList());

            for (int i = 0; i < n * n; i++)
            {
                model.AddVariable();
            }

            string[] data = System.IO.File.ReadAllLines($"../../../Data/futoshiki_{n}x{n}");
            string constraints = "";

            int x = 0;
            for (int j = 0; j < data.Length; j++)
            {
                if (j % 2 == 0)
                {
                    for (int i = 0; i < n * 2 - 1; i++)
                    {
                        if (Char.IsDigit(data[j][i]))
                        {
                            model.Variables[x].Set(data[j][i] - '0');
                            model.Variables[x++].Domain.Values.Clear();
                        }
                        else if (data[j][i] == 'x') x++;
                        else
                        {
                            constraints += data[j][i];
                            if (data[j][i] != '-')
                            {
                                Variable v1 = model.Variables[x - 1];
                                Variable v2 = model.Variables[x];
                                model.Constraints.Add(new ComparisonConstraint(v1, data[j][i], v2));
                            }
                        }
                    }
                }
                else
                {
                    constraints += data[j];
                    for (int i = 0; i < n; i++)
                    {
                        if (data[j][i] != '-')
                        {
                            Variable v1 = model.Variables[x - n + i];
                            Variable v2 = model.Variables[x + i];
                            model.Constraints.Add(new ComparisonConstraint(v1, data[j][i], v2));
                        }
                    }
                }
            }

            model.Futoshiki = constraints;


            //ADD CONSTRAINTS TO MODEL
            
            for (int i = 0; i < n; i++)
            {
                Variable[] row = model.Variables.GetRange(i * n, n).ToArray();
                model.Constraints.Add(new AllDifferrentConstraint(row));

                List<Variable> col = new List<Variable>();
                for (int j = i; j < n * n; j += n)
                {
                    col.Add(model.Variables[j]);
                }
                model.Constraints.Add(new AllDifferrentConstraint(col.ToArray()));
            }

            return model;
        }
    }
}
