using DotNetSnmp.Asn1.Serialization;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    public readonly record struct Variable : IAsnSerializable
    {
        public readonly ObjectIdentifier Id { get; }

        public readonly IAsnSerializable Data { get; }

        public Variable(string oid, IAsnSerializable? value = null)
        {
            Id = new ObjectIdentifier(oid);
            Data = value ?? Null.Instance;
        }

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

        public void Deconstruct(out string name, out object value)
        {
            name = Id;
            value = Data;
        }

        public override string ToString()
        {
            return $"{Id} = {Data}";
        }

        public static explicit operator Variable(string oid) => new(oid);
    }
}
