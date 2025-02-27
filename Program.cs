using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebootTestApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var reboot = new Reboot();
            Console.WriteLine("Тест получения привелегий\nEnter для продолжения");
            Console.ReadLine();

            reboot.halt(false, true);
            Console.ReadLine();

        }
    }
}
