using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunPath
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Process.Start("explorer", args[0]);
        }
    }
}
