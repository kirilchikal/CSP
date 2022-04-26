using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSP
{
    public class Domain
    {
        public List<int> Values = new List<int>();

        public Domain Copy()
        {
            var copy = (Domain)this.MemberwiseClone();
            copy.Values = new List<int>(this.Values);
            return copy;
        }
    }
}