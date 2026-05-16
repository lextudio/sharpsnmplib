using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the NoSuchObject type.
    /// </summary>
    public readonly record struct NoSuchObject : ISnmpData
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.NoSuchObject);
        }
    }
}
