using Lextm.SharpSnmpLib;
using System.Formats.Asn1;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Represents the Variable type.
    /// </summary>
    public readonly record struct Variable : ISnmpData
    {
        /// <summary>
        /// Gets id.
        /// </summary>
        public readonly ObjectIdentifier Id { get; }

        /// <summary>
        /// Gets data.
        /// </summary>
        public readonly ISnmpData Data { get; }

        /// <summary>
        /// Initializes a new instance of Variable.
        /// </summary>
        public Variable(string oid, ISnmpData? value = null)
        {
            Id = new ObjectIdentifier(oid);
            Data = value ?? Null.Instance;
        }

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            using (var varBind = writer.PushSequence())
            {
                writer.WriteObjectIdentifier(Id.Oid);

                if (Data == null)
                {
                    writer.WriteNull();
                }
                else
                {
                    Data.WriteTo(writer);
                }
            }
        }

        /// <summary>
        /// Deconstructs the value into its components.
        /// </summary>
        public void Deconstruct(out string name, out object value)
        {
            name = Id;
            value = Data;
        }

        /// <summary>
        /// Returns a string representation of the current value.
        /// </summary>
        public override string ToString()
        {
            return $"{Id} = {Data}";
        }

        /// <summary>
        /// Performs a conversion to Variable.
        /// </summary>
        public static explicit operator Variable(string oid) => new(oid);
    }
}
