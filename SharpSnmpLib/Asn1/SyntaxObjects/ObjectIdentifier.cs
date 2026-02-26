using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;
using System.Text;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the ObjectIdentifier type.
    /// </summary>
    public readonly record struct ObjectIdentifier(string Oid) : IAsnSerializable, IComparable<ObjectIdentifier>, IComparable
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
    public ObjectIdentifier(params byte[] octets)
        : this(Encoding.UTF8.GetString(octets))
    {
    }

    /// <inheritdoc/>
    public void WriteTo(AsnWriter writer)
    {
        writer.WriteObjectIdentifier(Normalize(Oid));
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
    public override string ToString() => Normalize(Oid);

    /// <summary>
    /// Compares two object identifiers lexicographically by OID segments.
    /// </summary>
    public int CompareTo(ObjectIdentifier other)
    {
        var left = Oid.Split('.', StringSplitOptions.RemoveEmptyEntries);
        var right = other.Oid.Split('.', StringSplitOptions.RemoveEmptyEntries);
        var count = Math.Min(left.Length, right.Length);

        for (var i = 0; i < count; i++)
        {
            if (!ulong.TryParse(left[i], out var leftPart))
            {
                leftPart = 0;
            }

            if (!ulong.TryParse(right[i], out var rightPart))
            {
                rightPart = 0;
            }

            if (leftPart != rightPart)
            {
                return leftPart.CompareTo(rightPart);
            }
        }

        return left.Length.CompareTo(right.Length);
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj)
    {
        if (obj is ObjectIdentifier other)
        {
            return CompareTo(other);
        }

        return 1;
    }

    /// <summary>
    /// Performs a conversion to string.
    /// </summary>
    public static implicit operator string(ObjectIdentifier o) => Normalize(o.Oid);

    /// <summary>
    /// Performs a conversion to ObjectIdentifier.
    /// </summary>
    public static explicit operator ObjectIdentifier(string oid) => new(oid);

    /// <summary>
    /// Performs a conversion to ObjectIdentifier.
    /// </summary>
    public static explicit operator ObjectIdentifier(byte[] octets) => new(octets);

    /// <summary>
    /// Compares whether left is greater than right.
    /// </summary>
    public static bool operator >(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) > 0;

    /// <summary>
    /// Compares whether left is less than right.
    /// </summary>
    public static bool operator <(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) < 0;

    /// <summary>
    /// Compares whether left is greater than or equal to right.
    /// </summary>
    public static bool operator >=(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Compares whether left is less than or equal to right.
    /// </summary>
    public static bool operator <=(ObjectIdentifier left, ObjectIdentifier right) => left.CompareTo(right) <= 0;

    private static string Normalize(string oid)
    {
        if (string.IsNullOrWhiteSpace(oid))
        {
            return "0.0";
        }

        var normalized = string.Join(".", oid.Split('.', StringSplitOptions.RemoveEmptyEntries));
        return string.IsNullOrEmpty(normalized) ? "0.0" : normalized;
    }
}
}
