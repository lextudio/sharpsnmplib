using System.Collections.Generic;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// SEQUENCE type stub for v12 compatibility.
    /// </summary>
    public sealed class Sequence : ISnmpData
    {
        /// <summary>
        /// Initializes a new instance of <see cref="Sequence"/>.
        /// </summary>
        public Sequence() { }

        /// <summary>
        /// Initializes a new instance of <see cref="Sequence"/>.
        /// </summary>
        public Sequence(byte[]? length, params ISnmpData?[] items) => throw new NotImplementedException();

        /// <summary>
        /// Initializes a new instance of <see cref="Sequence"/>.
        /// </summary>
        public Sequence(IEnumerable<ISnmpData> items) => throw new NotImplementedException();

        /// <inheritdoc/>
        public SnmpType TypeCode => SnmpType.Sequence;

        /// <inheritdoc/>
        public void WriteTo(System.Formats.Asn1.AsnWriter writer) => throw new NotImplementedException();

        /// <summary>
        /// Gets element count.
        /// </summary>
        public int Length => throw new NotImplementedException();

        /// <summary>
        /// Gets element at index.
        /// </summary>
        public ISnmpData this[int index] => throw new NotImplementedException();

        /// <inheritdoc/>
        public override string ToString() => throw new NotImplementedException();

        /// <summary>
        /// Gets the length bytes.
        /// </summary>
        public byte[]? GetLengthBytes() => throw new NotImplementedException();
    }
}
