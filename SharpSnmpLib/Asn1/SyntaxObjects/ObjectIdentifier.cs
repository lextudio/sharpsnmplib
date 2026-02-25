using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the ObjectIdentifier type.
    /// </summary>
    public readonly record struct ObjectIdentifier(string Oid) : IAsnSerializable
    {
        /// <summary>
        /// Initializes a new instance of ObjectIdentifier.
        /// </summary>
        [System.CLSCompliant(false)]
    public ObjectIdentifier(uint[] ids)
            : this(string.Join(".", ids))
    {
    }

    /// <summary>
    /// Initializes a new instance of ObjectIdentifier.
    /// </summary>
    public ObjectIdentifier(params byte[] octets) :
        this(Encoding.UTF8.GetString(octets))
    {

    }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer)
    {
        writer.WriteObjectIdentifier(Oid);
    }

    /// <summary>
    /// Reads a value from an ASN.1 reader.
    /// </summary>
    public static ObjectIdentifier ReadFrom(AsnReader reader)
    {
        var oid = reader.ReadObjectIdentifier();
        return new(oid);
    }

    /// <summary>
    /// Returns a string representation of the current value.
    /// </summary>
    public override string ToString() => Oid.ToString();

    /// <summary>
    /// Performs a conversion to string.
    /// </summary>
    public static implicit operator string(ObjectIdentifier o) => o.Oid.ToString();

    /// <summary>
    /// Performs a conversion to ObjectIdentifier.
    /// </summary>
    public static explicit operator ObjectIdentifier(string oid) => new(oid);

    /// <summary>
    /// Performs a conversion to ObjectIdentifier.
    /// </summary>
    public static explicit operator ObjectIdentifier(byte[] octets) => new(octets);
}
}
