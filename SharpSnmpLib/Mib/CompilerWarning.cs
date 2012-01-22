using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    public class CompilerWarning
    {
        public CompilerWarning(IToken token, string fileName, string details)
        {
            Token = token;
            FileName = fileName;
            Details = details;
        }

        public string Details { get; set; }

        protected string FileName { get; set; }

        protected IToken Token { get; set; }
    }
}