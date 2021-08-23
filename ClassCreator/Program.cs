using System;
using System.Text;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.CodeDom.Compiler;

namespace ClassCreator
{
    class Program
    {
        static async void WriteClassToFile(string fileName, string code)
        {
            await File.WriteAllTextAsync($"{fileName}.cs", code);
        }

        static bool validateString(string toValidate)
        {
            CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
            if (provider.IsValidIdentifier(toValidate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void Main(string[] args)
        {
            string[] types = { "bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", 
                                "uint", "nint", "nuint", "long", "ulong", "short", "ushort", "string" };
            string className;
            string type;
            string propertyName;

            StringBuilder code = new StringBuilder("using System;\n\nnamespace Custom\n{\n");

            bool nameEntered = false;
            do
            {
                Console.WriteLine("Enter class name:");
                className = Console.ReadLine();

                if(validateString(className))
                {
                    code.AppendFormat("\tpublic class {0}\n", className);
                    code.AppendLine("\t{");
                    nameEntered = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again!");
                }

            }
            while (!nameEntered);

            bool propertiesEntered = false;
            do
            {
                Console.WriteLine("Enter property type (or 'done' if all properties are entered):");
                type = Console.ReadLine();

                if(types.Contains(type))
                {
                    Console.WriteLine("Enter property name:");
                    propertyName = Console.ReadLine();

                    if (validateString(propertyName))
                    {
                        code.AppendFormat("\t\tpublic {0} {1} {2}\n", type, propertyName, "{ get; set; }");
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, try again!");
                        continue;
                    }
                }
                else if (type == "done")
                {
                    propertiesEntered = true;
                }
                else
                {
                    Console.WriteLine("Invalid input, try again!");
                }
            }
            while (!propertiesEntered);

            code.AppendLine("\t}");
            code.AppendLine("}");

            WriteClassToFile(className, code.ToString());
            string filePath = Path.GetFullPath($"{className}.cs");

            Console.WriteLine("File has been saved to {0}", filePath);
        }
    }
}
