using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct ObjectIdentifier(string Oid) : IAsnSerializable
    {
        public ObjectIdentifier(uint[] ids)
            : this(string.Join(".", ids))
    {
    }

    public ObjectIdentifier(params byte[] octets) :
        this(Encoding.UTF8.GetString(octets))
    {

    }

    public void WriteTo(AsnWriter writer)
    {
        writer.WriteObjectIdentifier(Oid);
    }

    public static ObjectIdentifier ReadFrom(AsnReader reader)
    {
        var oid = reader.ReadObjectIdentifier();
        return new(oid);
    }

    public override string ToString() => Oid.ToString();

    public static implicit operator string(ObjectIdentifier o) => o.Oid.ToString();

    public static explicit operator ObjectIdentifier(string oid) => new(oid);

    public static explicit operator ObjectIdentifier(byte[] octets) => new(octets);
}
}
