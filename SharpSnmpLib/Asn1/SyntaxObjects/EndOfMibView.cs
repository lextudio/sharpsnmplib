using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct EndOfMibView : IAsnSerializable
    {
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.EndOfMibView);
        }
    }
}
