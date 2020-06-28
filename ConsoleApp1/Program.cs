using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost sh = new ServiceHost(typeof(Servis.Service1));
            sh.Open();
            Console.WriteLine("Servis pokrenut... <Enter> za kraj.");
            Console.ReadLine();
            sh.Close();
        }
    }
}
