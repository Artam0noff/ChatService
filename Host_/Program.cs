﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Host_
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new ServiceHost(typeof(Client_Server_Chat.ServiceChat)))
            {
                host.Open();
                Console.WriteLine("Host started");
                Console.ReadLine();
            }
        }
    }
}
