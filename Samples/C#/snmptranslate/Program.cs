using System;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Mib;

namespace snmptranslate
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("This application takes one parameter.");
            }

            string oid = args[0];
            ReloadableObjectRegistry registry = new ReloadableObjectRegistry("modules");
            string textual = registry.Translate(ObjectIdentifier.Convert(oid));
            Console.WriteLine(textual);
        }
    }
}
