using System;
using System.Collections.Generic;
using System.Linq;

namespace CSP
{
    class Program
    {
        static void Main(string[] args)
        {
            //BinaryPuzzle bin = new BinaryPuzzle(6);
            //Model m1 = bin.CreateModel();

            FutoshikiPuzzle fut = new FutoshikiPuzzle(6);
            Model m2 = fut.CreateModel();

            CSP solver = new CSP(m2);
            solver.Solve();

        }
    }

    
}
