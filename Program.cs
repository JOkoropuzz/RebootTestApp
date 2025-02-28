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
            var forceFlag = true;
           
            Console.WriteLine("Test to set privileges");
            Console.WriteLine($"forceFlag = {forceFlag}\nEnter to continue");
            Console.ReadLine();

            reboot.halt(true, forceFlag);
            Console.WriteLine("Do you want to reboot system? (y/n)");
            var answer = Console.ReadLine();
            if(answer = "y")
            {
                reboot.Shutdown(true, forceFlag);
            }

        }
    }
}
