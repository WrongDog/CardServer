using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeakLib
{
    public class ClassA
    {
        private int a = 10;
        List<WeakReference> weaks = new List<WeakReference>();
        public void AddNotice(ClassB b)//Action<int> bact)
        {
             weaks.Add(new WeakReference(b));
        }
        public int Refresh()
        {
            foreach (WeakReference weak in weaks)
            {
                if (weak != null && weak.IsAlive && weak.Target != null)
                {
                    ((ClassB)weak.Target).Print(a);
                }
            }
           return weaks.RemoveAll((w) => w.IsAlive == false);
        }
    }
    public class ClassB:IDisposable{
        public void Print(int value)
        {
            Console.WriteLine(value);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
