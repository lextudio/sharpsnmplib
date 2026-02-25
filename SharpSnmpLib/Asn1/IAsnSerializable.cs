using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.Serialization
{
    public interface IAsnSerializable
    {
        public void WriteTo(AsnWriter writer);
    }
}
