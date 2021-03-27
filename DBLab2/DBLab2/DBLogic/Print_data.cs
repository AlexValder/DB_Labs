using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBLab2.DBLogic
{
    static class Class1
    {
        static int size = 25; 
        public static void PrintData(List<List<string>> Data)
        {
            foreach (var f in Data)
            {
                Console.Write("|");
                foreach (var ff in f)
                {
                    Console.Write("{0," + size + "}|", ff);
                }
                Console.WriteLine();
                Console.WriteLine(new String('=', (size + 1) * f.Count + 1));
            }
        }
    }
}
