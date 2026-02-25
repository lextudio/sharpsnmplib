using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the Null type.
    /// </summary>
    public readonly record struct Null : IAsnSerializable
    {
        /// <summary>
        /// A reusable SNMP NULL value instance.
        /// </summary>
        public readonly static Null Instance = new();

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull();
        }

        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="Null"/>.
        /// </summary>
        public override string ToString()
        {
            return "NULL";
        }
    }
}
