using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace class_library
{
    public class VMGrid
    {
        public VMGrid(int L, double B, double E)
        {
            Length = L;
            Begin = B;
            End = E;
            Step = (E - B) / (L - 1);
        }
        public int Length { get; set; }
        public double Begin { get; set; }
        public double End { get; set; }
        public double Step { get; }
    }
}