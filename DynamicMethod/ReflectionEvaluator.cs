using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;
using System.IO;
using Microsoft.CSharp;

namespace DynamicMethod
{
   
    /// <summary>
    /// memory leak
    /// </summary>
    public class ReflectionEvaluator : IDynamicEvaluator
    {
        private dynamic expressionObject;
        private Assembly assembly;
        protected const string className = "DynamicEvaluator";
        protected const string methodName = "Evaluate";
        public ReflectionEvaluator(BuildMethod buildMethod)
        {
            buildMethod.Name = methodName;
            buildMethod.ParameterList.Add(new BuildMethod.Parameter("dynamic", "obj"));
            buildMethod.ReturnType = "object";
            BuildAssembly buildAssembly = new BuildAssembly();
            BuildNameSpace buildNameSpace = new BuildNameSpace();
            BuildClass buildclass = new BuildClass();
            buildclass.ClassName = className;
            buildclass.BuildMethods.Add(buildMethod);
            buildNameSpace.BuildClass.Add(buildclass);
            buildAssembly.BuildNameSpaces.Add(buildNameSpace);


            assembly = Compiler.Compile(buildAssembly);
            if (assembly != null)
            {
                expressionObject = Activator.CreateInstance(assembly.GetType(className));
            }
        }

        


        public dynamic Evaluate(dynamic obj)
        {
            return expressionObject.Evaluate(obj);
        }
    }
    public sealed class Isolated<T> : IDisposable where T : MarshalByRefObject
    {
        private AppDomain domain;
        private T instance;

        public Isolated()
        {
            domain = AppDomain.CreateDomain("Isolated:" + Guid.NewGuid(),
               null, AppDomain.CurrentDomain.SetupInformation);

            Type type = typeof(T);

            instance = (T)domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
        }

        public T Instance
        {
            get
            {
                return instance;
            }
        }

        public void Dispose()
        {
            if (domain != null)
            {
                AppDomain.Unload(domain);

                domain = null;
            }
        }
    }
    #region compiler
    public enum CompileLanguage
    {
        CSharp,
        VisualBasic
    }
    #region BuildTypes
    public interface IBuild
    {
        string ToSource();
    }
    public class BuildAssembly : IBuild
    {
        public List<BuildNameSpace> BuildNameSpaces = new List<BuildNameSpace>();
        public List<string> DefaultUsingNameSpace = new List<string>(){
            "System",
            "System.Collections.Generic",
            "System.Linq",
            "System.Text",
            "System.Configuration"
        };
        public List<string> UsingNameSpace
        {
            get
            {
                List<string> result = new List<string>();
                foreach (BuildNameSpace buildnamespace in BuildNameSpaces)
                {
                    foreach (BuildClass buildClass in buildnamespace.BuildClass)
                    {
                        foreach (string refnamespace in buildClass.UsingNameSpace)
                        {
                            if (!result.Contains(refnamespace)) result.Add(refnamespace);
                        }
                    }
                }
                foreach (string defaultNameSpace in DefaultUsingNameSpace)
                {
                    if (!result.Contains(defaultNameSpace)) result.Add(defaultNameSpace);
                }
                return result;
            }
        }
        public List<string> ReferenceAssemblies = new List<string>(){
            "system.dll",
            "system.core.dll",
            "Microsoft.CSharp.dll",
            "system.xml.dll",
            "system.xml.linq.dll",
            "system.data.dll",
            "system.Data.DataSetExtensions.dll",
            "System.Configuration.dll"
        };
        public string ToSource()
        {
            string source = string.Empty;
            foreach (BuildNameSpace buildns in BuildNameSpaces)
            {
                source += buildns.ToSource();
            }
            return source;
        }
    }
    public class BuildNameSpace : IBuild
    {
        public List<BuildClass> BuildClass = new List<BuildClass>();
        public string Name { get; set; }

        public string ToSource()
        {
            string source = string.Empty;
            foreach (BuildClass buildclass in BuildClass)
            {
                source += buildclass.ToSource();
            }
            if (!string.IsNullOrEmpty(Name))
            {
                source = "namespace " + Name + "{" + System.Environment.NewLine + source + "}" + System.Environment.NewLine;
            }
            return source;
        }
    }
    public class BuildClass : IBuild
    {

        public List<string> UsingNameSpace
        {
            get
            {
                List<string> result = new List<string>();
                foreach (BuildMethod method in BuildMethods)
                {
                    foreach (string refnamespace in method.UsingNameSpaces)
                    {
                        if (!result.Contains(refnamespace)) result.Add(refnamespace);
                    }
                }
                return result;
            }
        }
        public List<BuildMethod> BuildMethods { get; set; }
        public string ClassName { get; set; }
        public string ToSource()
        {
            string namespacestring = string.Empty;
            foreach (string usingnamespace in UsingNameSpace)
            {
                namespacestring += "using " + usingnamespace + ";" + System.Environment.NewLine;
            }
            string methodstring = string.Empty;
            foreach (BuildMethod method in BuildMethods)
            {
                methodstring += method.ToString() + System.Environment.NewLine;
            }

            string source = "    public class {0}" +
                           "    {" +
                           "       {1}" +
                           "    }" + System.Environment.NewLine;
            return String.Format(source, ClassName, methodstring);
        }
    }
    public class BuildMethod : IBuild
    {
        public List<string> UsingNameSpaces { get; set; }
        public List<string> References { get; set; }
        public class Parameter
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public Parameter(string type, string name)
            {
                this.Type = type;
                this.Name = name;

            }
        }
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<Parameter> ParameterList { get; set; }
        public string MethodBody { get; set; }
        public string Parameters
        {
            get
            {
                string parameters = string.Empty;
                foreach (Parameter p in this.ParameterList)
                {
                    parameters += p.Type + " " + p.Name + " ,";
                }
                if (parameters.Length > 1) parameters = parameters.Substring(0, parameters.Length - 1);
                return parameters;
            }
        }
        public string ToSource()
        {
            return String.Format("        public {1} {0}({2})" +
                                  "        {" +
                                  "            {3}" +
                                  "        }" + System.Environment.NewLine,
                Name,
                ReturnType,
                MethodBody,
                Parameters);
        }
        public override int GetHashCode()
        {
            return ToSource().GetHashCode();
        }



    }
    #endregion
    public class Compiler
    {
        public static string TypeToCSString(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                var expr = new CodeTypeReferenceExpression(type);

                var prov = new CSharpCodeProvider();
                prov.GenerateCodeFromExpression(expr, sw, new CodeGeneratorOptions());
            }
            return sb.ToString();
        }
        public static Assembly Compile(string sourcecode, CompileLanguage language = CompileLanguage.CSharp)
        {
            List<string> referencedassemblies = new List<string>();
            referencedassemblies.Add("system.dll");
            referencedassemblies.Add("system.core.dll");
            referencedassemblies.Add("Microsoft.CSharp.dll");
            referencedassemblies.Add("system.xml.dll");
            referencedassemblies.Add("system.xml.linq.dll");
            referencedassemblies.Add("system.data.dll");
            referencedassemblies.Add("System.Data.DataSetExtensions.dll");

            return Compile(sourcecode, referencedassemblies, language);
        }
        public static Assembly Compile(BuildAssembly buildAssembly)
        {
            return Compiler.Compile(buildAssembly.ToSource(), buildAssembly.ReferenceAssemblies);
        }

        public static Assembly Compile(string sourcecode, List<string> references, CompileLanguage language = CompileLanguage.CSharp)
        {
            CodeDomProvider comp = null;
            switch (language)
            {
                case CompileLanguage.VisualBasic:
                    comp = new Microsoft.VisualBasic.VBCodeProvider();
                    break;
                case CompileLanguage.CSharp:
                default:
                    comp = new CSharpCodeProvider();
                    break;
            }
            CompilerParameters cp = new CompilerParameters();
            foreach (string reference in references)
            {
                cp.ReferencedAssemblies.Add(reference);
            }
            cp.GenerateInMemory = true;



            CompilerResults cr = comp.CompileAssemblyFromSource(cp, sourcecode);
            if (cr.Errors.HasErrors)
            {
                string error = string.Empty;
                foreach (CompilerError err in cr.Errors)
                {
                    error += err.ErrorText + System.Environment.NewLine;
                }
                System.Diagnostics.Trace.WriteLine(error);
                return null;
            }

            return cr.CompiledAssembly;
        }

    }
    #endregion
}
