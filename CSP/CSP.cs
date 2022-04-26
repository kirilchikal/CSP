using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSP
{
    public class CSP
    {
        private Model Model;
        int solutions = 0;
        int backtracks = 0;
        int assignments = 0;
        long firstSolutionSearchTime;
        int firstSolutionBacktracks;
        int firstSolutionAssignments;
        Stopwatch stopwatch = new Stopwatch();

        public CSP(Model model)
        {
            this.Model = model;
        }

        public void Solve()
        {
            stopwatch.Start();

            // Propagate constraints for already assigned variables
            for (int i = 0; i < Model.Variables.Count; i++)
            {
                if (Model.Variables[i].IsSet)
                    CheckForward(Model.Variables[i]);
            }

            ForwardCheckingSearch();

            stopwatch.Stop();

            Console.WriteLine($"\nFound {solutions} solutions:\n-Total time search: {stopwatch.ElapsedMilliseconds}ms" +
                $"\n-Total backtrack: { backtracks}\n-Tolal assignments: {assignments}\n\n" +
                $"-First solution time search: " +
                $"{firstSolutionSearchTime}ms\n-First solution backtrack: {firstSolutionBacktracks}" +
                $"\n-First solution assignments: {firstSolutionAssignments}");
        }

        public void ForwardCheckingSearch()
        {
            int index = NextVariable();

            if (index == -1)
            {
                if (solutions == 0) //estimates of finding the first solution
                {
                    firstSolutionSearchTime = stopwatch.ElapsedMilliseconds;
                    firstSolutionBacktracks = backtracks;
                    firstSolutionAssignments = assignments;
                }
                Console.WriteLine($"\nSolution {++solutions}:");
                Model.Print();
            }
            else
            {
                Variable variable = Model.Variables[index];
                //var values = OrderValues(variable);
                var values = new List<int>(variable.Domain.Values);

                for (int i = 0; i < values.Count; i++)
                {
                    int val = values[i];
                    variable.Set(val);
                    assignments++;

                    // set value and propagte consraints 
                    // backtrack if a domain of a variable becomes empty
                    if (CheckForward(variable))
                    {
                        if (Model.isSatisfy())
                        {
                            ForwardCheckingSearch();
                        }
                        else
                            backtracks++;
                    }
                    else
                        backtracks++;

                    variable.UnSet(val);
                    variable.UndoEliminations();
                }
            }
        }


        public bool CheckForward(Variable variable)
        {
            Propagation propagation = new Propagation();
            foreach (var constraint in Model.Constraints)
            {
                if (constraint.ContainsVariable(variable))
                {
                    if (!constraint.Propagate(variable, propagation))
                    {
                        variable.Propagation = propagation;
                        return false;
                    }

                }
            }

            variable.Propagation = propagation;
            return true;
        }

        // simple backtracking search without constraint propagation
        public void BacktrackingSearch()
        {
            int index = NextVariable();

            if (index == -1)
            {
                if (solutions == 0)
                {
                    firstSolutionSearchTime = stopwatch.ElapsedMilliseconds;
                    firstSolutionBacktracks = backtracks;
                    firstSolutionAssignments = assignments;
                }
                Console.WriteLine($"\nSolution {++solutions}:");
                Model.Print();
            }
            else
            {
                Variable variable = Model.Variables[index];
                var values = OrderValues(variable);

                for (int i = 0; i < values.Count; i++)
                {
                    int val = values[i];
                    variable.Set(val);
                    assignments++;

                    if (Model.isSatisfy())
                    {
                        BacktrackingSearch();
                    }
                    else
                        backtracks++;

                    variable.UnSet(val);
                }
            }
        }


        //************************
        //HEURESTICS--------------
        //************************

        // MRV - Minimum remaining values 
        public int NextVariable()
        {
            int nr = -1;    //return if all Variables are assigned
            int min = int.MaxValue;
            for (int i = 0; i < Model.Variables.Count; i++)
            {
                Variable varaible = Model.Variables[i];
                if (!varaible.IsSet && varaible.Domain.Values.Count < min)
                {
                    nr = i;
                    min = varaible.Domain.Values.Count;
                }
            }

            return nr;
        }


        // Order values based on how many times a value is shared in constraint heuristic
        // btw this heuristic is realy needless toward csp efficiency and only slows down execution
        public List<int> OrderValues(Variable variable)
        {
            var values = new Dictionary<int, int>();
            foreach (var value in variable.Domain.Values)
            {
                values.Add(value, CountValueOccurrence(value));
            }

            return values.OrderBy(v => v.Value).Select(v => v.Key).ToList();
        }

        // Count how many times the value appears in unsign variables under constraints
        private int CountValueOccurrence(int value)
        {
            int count = 0;

            foreach (var constraint in Model.Constraints)
            {
                foreach (var variable in constraint.Variables)
                {
                    if (variable.Domain.Values.Contains(value) && !variable.IsSet)
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }



}
