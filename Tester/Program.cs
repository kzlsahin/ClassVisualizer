using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassVisualizer;
using Tester.TestTypes;

namespace Tester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClassVisualizer.ClassVisualizer.Visualize(typeof(TestClass1));
        }
    }
}
