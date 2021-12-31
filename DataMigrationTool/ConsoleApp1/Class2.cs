using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Class2:IDisposable
    {
        public static bool IsDisposable = true;

        public Class2()
        {
            Class1.OnDataUpdate += Class1_OnDataUpdate;
        }

        private void Class1_OnDataUpdate()
        {
            Console.WriteLine("Class1_OnDataUpdate");
        }

        public void Start(string xx)
        {
            Console.WriteLine(xx);
            
            for (int i = 0; i < 100; i++)
            {

                Class1.Add();

            }
        }
        public void Dispose()
        {
            IsDisposable = true;
        }
    }
}
