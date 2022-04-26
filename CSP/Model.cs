using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Model
    {
        public List<Variable> Variables = new List<Variable>();
        public Domain Domain;
        public List<Constraint> Constraints = new List<Constraint>();
        public string Futoshiki;

        public Model(List<int> domain)
        {
            Domain = new Domain { Values = domain };
        }

        public void AddVariable()
        {
            Variable variable = new Variable { Nr = Variables.Count, Domain = this.Domain.Copy()};
            Variables.Add(variable);
        }

        public bool isSatisfy()
        {
            for (int i = 0; i < Constraints.Count; i++)
            {
                if (!Constraints[i].IsSatisfied())
                {
                    return false;
                }
            }
            return true;
        }

        public void Print()
        {
            if (Futoshiki != null)
                PrintFutoshiki();
            else
            {
                int size = (int)Math.Sqrt(Variables.Count);
                int i = 0;

                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < size; k++)
                    {
                        Console.Write(Variables.First(v => v.Nr == i).Value + " ");
                        i++;
                    }
                    Console.WriteLine();
                }
            }
        }


        private void PrintFutoshiki()
        {
            int size = (int)Math.Sqrt(Variables.Count);
            int i = 0, c = 0;

            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    Console.Write(Variables.First(v => v.Nr == i).Value);
                    i++;
                    if (k != size - 1)
                    {
                        Console.Write(Futoshiki[c++] + " ");
                    }
                }
                Console.WriteLine();
                if (j != size - 1)
                {
                    for (int k = 0; k < size; k++)
                    {
                        Console.Write(Futoshiki[c++] + "  ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
