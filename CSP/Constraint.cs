using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public abstract class Constraint
    {
        public List<Variable> Variables;
        public abstract bool IsSatisfied();
        public abstract bool Propagate(Variable variable, Propagation propagation);
        public bool IfAllSet()
        {
            if (Variables.Find(v => v.IsSet == false) != null)
            {
                return false;
            }
            return true;
        }

        public bool ContainsVariable(Variable variable)
        {
            return Variables.Contains(variable);
        }
    }


    public class AllNotTheSameConstraint : Constraint
    {
        public AllNotTheSameConstraint(params Variable[] variables)
        {
            Variables = variables.ToList();
        }
        public override bool IsSatisfied()
        {
            if (!IfAllSet())
                return true;

            int value = Variables[0].Value;
            int count = Variables.Count(v => v.Value == value);
            return count != Variables.Count;
        }

        public override bool Propagate(Variable variable, Propagation propagation)
        {
            if (IfAllSet())
                return true;

            int value = variable.Value;
            int index = -1;
            int count = 0;

            for (int i = 0; i < Variables.Count; i++)
            {
                if (Variables[i].IsSet && Variables[i].Value != value)
                    return true;

                if (!Variables[i].IsSet)
                {
                    count++;
                    index = i;
                }
            }

            if (count != 1)
                return true;

            if (Variables[index].Domain.Values.Contains(value))
            {
                Variable checkVar = Variables[index];
                checkVar.Domain.Values.Remove(value);
                propagation.AddElimination(checkVar, value);

                return checkVar.Domain.Values.Count != 0;
            }
            return true;
        }
    }

    public class SumEqualsConstraint : Constraint
    {
        int Sum;
        public SumEqualsConstraint(int sum, params Variable[] variables)
        {
            Sum = sum;
            Variables = variables.ToList();
        }
        public override bool IsSatisfied()
        {
            if (!IfAllSet())
                return true;

            int sum = Variables.Sum(v => v.Value);
            return this.Sum == sum;  
        }

        public override bool Propagate(Variable variable, Propagation propagation)
        {
            if (IfAllSet())
                return true;

            List<Variable> notSetVars = Variables.Where(v => !v.IsSet).ToList();
            int sum = Variables.Where(v => v.IsSet).Sum(s => s.Value);

            foreach (var checkVar in notSetVars)
            {
                List<int> deleteValues = new List<int>();
                for (int i = 0; i < checkVar.Domain.Values.Count; i++)
                {
                    if (checkVar.Domain.Values[i] > this.Sum - sum)
                        deleteValues.Add(checkVar.Domain.Values[i]);
                }

                foreach (var value in deleteValues)
                {
                    checkVar.Domain.Values.Remove(value);
                    propagation.AddElimination(checkVar, value);
                }

                if (checkVar.Domain.Values.Count == 0)
                    return false;
            }

            return true;
        }
    }

    public class UniqueRowsAndColumnsConstraint : Constraint
    {
        public UniqueRowsAndColumnsConstraint(Model model)
        {
            Variables = model.Variables;
        }
        public override bool IsSatisfied()
        {
            int n = (int)Math.Sqrt(Variables.Count);
            List<List<int>> rows = new List<List<int>>();
            List<List<int>> columns = new List<List<int>>();

            for (int i = 0; i < n; i++)
            {
                List<int> row = Variables.GetRange(i * n, n).Select(v => v.Value).ToList();
                List<int> col = new List<int>();
                for (int j = i; j < n*n; j+=n)
                {
                    col.Add(Variables[j].Value);
                }
                if (!row.Contains(-1))
                    rows.Add(row);
                if (!col.Contains(-1))
                    columns.Add(col);
            }
            for (int i = 0; i < rows.Count - 1; i++)
            {
                for (int j = i + 1; j < rows.Count; j++)
                {
                    if (Enumerable.SequenceEqual(rows[i], rows[j]))
                        return false;
                }
            }
            for (int i = 0; i < columns.Count - 1; i++)
            {
                for (int j = i + 1; j < columns.Count; j++)
                {
                    if (Enumerable.SequenceEqual(columns[i], columns[j]))
                        return false;
                }
            }

            return true;
        }

        public override bool Propagate(Variable variable, Propagation propagation)
        {
            return true;
        }
    }

    public class ComparisonConstraint : Constraint
    {
        public Variable Less;
        public Variable Greater;
        public ComparisonConstraint(Variable var1, char sign, Variable var2)
        {
            Variables = new List<Variable> { var1, var2 };
            Less = sign.Equals('<') ? var1 : var2;
            Greater = sign.Equals('>') ? var1 : var2;
        }
        public override bool IsSatisfied()
        {
            if (!IfAllSet())
                return true;

            return Less.Value < Greater.Value;
        }

        public override bool Propagate(Variable variable, Propagation propagation)
        {
            int value = variable.Value;

            if (variable == Less && !Greater.IsSet)
            {
                for (int i = 0; i < Greater.Domain.Values.Count; i++)
                {
                    int lessValue = Greater.Domain.Values[i];
                    if (lessValue <= value)
                    {
                        Greater.Domain.Values.Remove(lessValue);
                        propagation.AddElimination(Greater, lessValue);
                        if (Greater.Domain.Values.Count == 0)
                            return false;
                    }
                }
            }
            else if (variable == Greater && !Less.IsSet)
            {
                for (int i = 0; i < Less.Domain.Values.Count; i++)
                {
                    int greaterValue = Less.Domain.Values[i];
                    if (greaterValue >= value)
                    {
                        Less.Domain.Values.Remove(greaterValue);
                        propagation.AddElimination(Less, greaterValue);
                        if (Less.Domain.Values.Count == 0)
                            return false;
                    }
                }
            }

            return true;
        }
    }

    public class AllDifferrentConstraint : Constraint
    {
        public AllDifferrentConstraint(params Variable[] variables)
        {
            Variables = variables.ToList();
        }
        public override bool IsSatisfied()
        {
            var values = new Dictionary<int, bool>();
            foreach(var v in Variables)
            {
                if (v.Value == -1)
                    continue;
                if (values.ContainsKey(v.Value))
                {
                    return false;
                }
                values.Add(v.Value, true);
            }
            return true;
        }

        public override bool Propagate(Variable variable, Propagation propagation)
        {
            if (IfAllSet())
                return true;
            int value = variable.Value;

            foreach (var checkVar in Variables)
            {
                if (!checkVar.IsSet && checkVar.Domain.Values.Contains(value))
                {
                    checkVar.Domain.Values.Remove(value);
                    propagation.AddElimination(checkVar, value);
                    if (checkVar.Domain.Values.Count == 0)
                        return false;
                }
            }

            return true;
        }
    }
}
