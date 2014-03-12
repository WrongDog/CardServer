using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeakLib;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            ClassA A = new ClassA();
            Console.WriteLine(A.Refresh());
            List<ClassB> classblist = new List<ClassB>();
            classblist.Add(new ClassB());

            A.AddNotice(classblist[0]);
            Console.WriteLine("before remove");
            GC.Collect();
            Console.WriteLine(A.Refresh());

             

            classblist.RemoveAt(0);
            GC.Collect();
            Console.WriteLine("after remove");
            Console.WriteLine(A.Refresh());
            Console.ReadLine();
        }
    }
}
