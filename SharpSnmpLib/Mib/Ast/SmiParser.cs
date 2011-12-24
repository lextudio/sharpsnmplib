namespace Lextm.SharpSnmpLib.Mib.Ast
{
    partial class SmiParser
    {
        public MibDocument GetDocument()
        {
            var doc = statement().result;
            if (doc.Modules.Count == 0)
            {
                throw new SemanticException("This file does not contain any SMI modules.");
            }

            return doc;
        }
    }
}
