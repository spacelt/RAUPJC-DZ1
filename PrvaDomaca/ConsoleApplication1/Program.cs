using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            IntegerList.IntegerList il = new IntegerList.IntegerList();
            il.ListExample(il);

            GenericList.GenericList<String> stringList = new GenericList.GenericList<string>();
            stringList.Add("1");
            stringList.Add("2");
            stringList.Add("3");
            stringList.Add("4");
            stringList.Add("5");
            stringList.Add("6");
            stringList.Add("7");
            Console.WriteLine("FOREACH");
            // foreach
            foreach (string value in stringList)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine("WITHOUT SYNTAX SUGAR");
            // foreach without the syntax sugar
            IEnumerator<string> enumerator = stringList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string value = (string)enumerator.Current;
                Console.WriteLine(value);
            }
            Console.ReadLine();
        }
    }
}
