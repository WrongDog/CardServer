using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DynamicMethod
{
    public class EvaluatorPool
    {
        Dictionary<int, IDynamicEvaluator> evaluators = new Dictionary<int, IDynamicEvaluator>();
        //HashAlgorithm hash = MD5.Create();
        public IDynamicEvaluator GetEvaluator(BuildMethod method)
        {
            IDynamicEvaluator stored;

            int key = method.GetHashCode();//Convert.ToBase64String(hash.ComputeHash(System.Text.Encoding.Unicode.GetBytes(methods.ToString())));
            if (evaluators.ContainsKey(key))
            {
                evaluators.TryGetValue(key, out stored);        
            }
            else
            {
                stored = new ReflectionEvaluator(method);
                evaluators.Add(key, stored);
            }
            return stored;
        }
    }
}
