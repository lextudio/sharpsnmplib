using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the NoSuchInstance type.
    /// </summary>
    public readonly record struct NoSuchInstance : IAsnSerializable
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.NoSuchInstance);
        }
    }
}
