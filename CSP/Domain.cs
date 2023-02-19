using System.Collections.Generic;

namespace CSP
{
    public class Domain
    {
        public List<int> Values { get; set; } = new List<int>();

        public Domain Copy()
        {
            var copy = (Domain)this.MemberwiseClone();
            copy.Values = new List<int>(this.Values);
            return copy;
        }
    }
}