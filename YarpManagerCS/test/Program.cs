using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YarpManagerCS;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            YarpPort yarpPort;
            yarpPort = new YarpPort();
            yarpPort.openSender("/test");

            yarpPort.sendData("prova");
          
            Console.ReadLine();
        }
    }
}
