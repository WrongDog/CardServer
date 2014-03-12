using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardServer
{
    public class TypeListDictionary<T, T2> where T : struct,IConvertible where T2:class
    {
        protected Dictionary<T, T2> dictionary = new Dictionary<T, T2>();
        public TypeListDictionary()
        {
        }
        public T2 this[T type]
        {
            get
            {
                T2 item;
                if (dictionary.ContainsKey(type))
                {
                    dictionary.TryGetValue(type, out item);     
                }
                else
                {
                    item = Activator.CreateInstance<T2>();
                    dictionary.Add(type, item);
                }
                return item;
            }
        }
    }
}
