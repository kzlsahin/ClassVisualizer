using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Tester.TestTypes
{
    internal class TestClass1
    {
        public int Id { get; set; }
        public int amount { get; set; }
        public string Name { get; set; }
        public TestClass2 OtherClass { get; set; }

    }
}
