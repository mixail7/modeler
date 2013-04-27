using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace testcoco
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0 && File.Exists(args[0]))
            {
                Scanner scanner = new Scanner(args[0]);
                Parser parser = new Parser(scanner);
                parser.Parse();
                Console.WriteLine("Error: " + parser.errors.count);
                if (parser.errors.count == 0)
                {
                    List<Link> list = parser.Sem.GetLinks();
                    SemanticProc.PrintRes(list);
                }
            }
            else
            {
                Console.WriteLine("Bad param");
            }
        }
    }
}
