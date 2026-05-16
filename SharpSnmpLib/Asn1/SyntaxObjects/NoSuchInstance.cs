using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the NoSuchInstance type.
    /// </summary>
    public readonly record struct NoSuchInstance : ISnmpData
    {
        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            writer.WriteNull(tag: SnmpAsnTags.NoSuchInstance);
        }
    }
}
