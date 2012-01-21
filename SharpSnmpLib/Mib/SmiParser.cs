using System.Collections.Generic;
using Antlr.Runtime;

namespace Lextm.SharpSnmpLib.Mib
{
    public partial class SmiParser
    {
        public readonly IList<CompilerError> Errors = new List<CompilerError>();
        private string _fileName;

        public MibDocument GetDocument(string fileName)
        {
            Errors.Clear();
            _fileName = fileName;
            var document = statement().result;
            document.FileName = fileName;
            return document;
        }

        public MibDocument GetDocument()
        {
            return GetDocument(string.Empty);
        }

        public override void ReportError(RecognitionException e)
        {
            Errors.Add(new CompilerError(e) {FileName = _fileName});
            base.ReportError(e);
        }
    }
}
