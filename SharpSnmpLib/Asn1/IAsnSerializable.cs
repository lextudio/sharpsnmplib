using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.Serialization
{
    /// <summary>
    /// Defines the contract for values that can serialize themselves as ASN.1.
    /// </summary>
    public interface IAsnSerializable
    {
        /// <summary>
        /// Writes this value to the supplied ASN.1 writer.
        /// </summary>
        /// <param name="writer">The writer that receives the encoded value.</param>
        public void WriteTo(AsnWriter writer);
    }
}
