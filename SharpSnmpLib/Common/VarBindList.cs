using DotNetSnmp.Asn1.Serialization;
using System.Collections;
using System.Diagnostics;
using System.Formats.Asn1;

namespace DotNetSnmp.Asn1.SyntaxObjects
{
    /// <summary>
    /// Represents the VarBindList type.
    /// </summary>
    public class VarBindList : IAsnSerializable, IEnumerable<Variable>
    {
        private readonly IList<Variable> _variableBindings;

        // rfc 3416 (4.2)
        private const int MaxVariableBindings = 2147483647;

        /// <summary>
        /// Gets a value indicating whether this list has no variable bindings.
        /// </summary>
        public bool IsEmpty => _variableBindings?.Count == 0;

        /// <summary>
        /// Initializes a new instance of VarBindList.
        /// </summary>
        public VarBindList()
        {
            _variableBindings = new List<Variable>();
        }

        /// <summary>
        /// Initializes a new instance of VarBindList.
        /// </summary>
        public VarBindList(params Variable[] bindings)
        {
            _variableBindings = new List<Variable>(bindings);
        }

        /// <summary>
        /// Initializes a new instance of VarBindList.
        /// </summary>
        public VarBindList(params string[] oids)
        {
            _variableBindings = oids.Select(
                oid => new Variable(oid)).ToList();
        }

        /// <summary>
        /// Initializes a new instance of VarBindList.
        /// </summary>
        public VarBindList(params ObjectIdentifier[] oids)
        {
            _variableBindings = oids.Select(
                oid => new Variable(oid)).ToList();
        }
        /// <summary>
        /// Initializes a new instance of VarBindList.
        /// </summary>
        public VarBindList(VarBindList other)
        {
            _variableBindings = other.ToList();
        }

        /// <summary>
        /// Adds a variable binding to the end of the list.
        /// </summary>
        public VarBindList Add(Variable varBind)
        {
            _variableBindings.Add(varBind);
            return this;
        }

        /// <summary>
        /// Inserts a variable binding at the specified index.
        /// </summary>
        public VarBindList Insert(int index, Variable varBind)
        {
            _variableBindings.Insert(index, varBind);
            return this;
        }

        /// <summary>
        /// Removes and returns the variable binding at the specified index.
        /// </summary>
        public Variable Remove(int index)
        {
            var item = _variableBindings[index];
            _variableBindings.RemoveAt(index);
            return item;
        }

        /// <summary>
        /// Removes all variable bindings from the list.
        /// </summary>
        public void Clear()
        {
            _variableBindings.Clear();
        }

        /// <inheritdoc/>
        public void WriteTo(AsnWriter writer)
        {
            if (_variableBindings == null
                /*|| _variableBindings.Count == 0*/)
            {
                return;
            }

            using (var varBindList = writer.PushSequence())
            {
                foreach (var vb in _variableBindings)
                {
                    vb.WriteTo(writer);
                }
            }
        }

        /// <summary>
        /// Reads a value from an ASN.1 reader.
        /// </summary>
        public static VarBindList ReadFrom(AsnReader reader)
        {
            Span<byte> ipAddressBuff = stackalloc byte[4];

            var sequence = reader.ReadSequence();

            var bindings = new VarBindList();

            if (sequence.HasData == false)
            {
                return bindings;
            }

            var t = sequence.PeekTag();

            while (sequence.HasData)
            {
                var vbSeq = sequence.ReadSequence();
                var oid = vbSeq.ReadObjectIdentifier();
                var tag = vbSeq.PeekTag();

                if (tag == Asn1Tag.PrimitiveOctetString
                    || tag == Asn1Tag.ConstructedOctetString)
                {
                    var octets = vbSeq.ReadOctetString();

                    bindings.Add(
                        new(oid, new OctetString(octets)));
                }
                else if (tag == Asn1Tag.Integer)
                {
                    if (vbSeq.TryReadInt32(out var integer32, AsnTypes.Integer32))
                    {
                        bindings.Add(
                            new(oid, new Integer32(integer32)));
                    }
                }
                else if (tag == AsnTypes.Counter32)
                {
                    if (vbSeq.TryReadUInt32(out var uint32, AsnTypes.Counter32))
                    {
                        bindings.Add(
                            new(oid, new Counter32(uint32)));
                    }
                }
                else if (tag == AsnTypes.Gauge32)
                {
                    if (vbSeq.TryReadUInt32(out var uint32, AsnTypes.Gauge32))
                    {
                        bindings.Add(
                            new(oid, new Gauge32(uint32)));
                    }
                }
                else if (tag == AsnTypes.IpAddress)
                {
                    if (vbSeq.TryReadOctetString(
                        ipAddressBuff,
                        out var len,
                        AsnTypes.IpAddress))
                    {
                        if (len == 4)
                        {
                            bindings.Add(
                                new(oid, new IP(ipAddressBuff.ToArray())));
                        }
                    }
                }
                else if (tag == AsnTypes.TimeTicks)
                {
                    if (vbSeq.TryReadUInt32(out var uint32, AsnTypes.TimeTicks))
                    {
                        bindings.Add(
                            new(oid, new TimeTicks(uint32)));
                    }
                }
                else if (tag == AsnTypes.Unsigned32)
                {
                    if (vbSeq.TryReadUInt32(out var uint32, AsnTypes.Unsigned32))
                    {
                        bindings.Add(
                            new(oid, new Unsigned32(uint32)));
                    }
                }
                else if (tag == Asn1Tag.ObjectIdentifier)
                {
                    var objId = vbSeq.ReadObjectIdentifier();
                    bindings.Add(
                           new(oid, new ObjectIdentifier(objId)));
                }
                else if (tag == SnmpAsnTags.NoSuchObject)
                {
                    bindings.Add(
                           new(oid, new NoSuchObject()));
                }
                else if (tag == SnmpAsnTags.NoSuchInstance)
                {
                    bindings.Add(
                           new(oid, new NoSuchInstance()));
                }
                else if (tag == SnmpAsnTags.EndOfMibView)
                {
                    bindings.Add(
                           new(oid, new EndOfMibView()));
                }
                else if (tag == Asn1Tag.Null)
                {
                    bindings.Add(
                           new(oid, Null.Instance));
                }
                else if (tag == Asn1Tag.Sequence)
                {
                    throw new NotImplementedException();
                }
                else if (tag == AsnTypes.Counter64)
                {
                    bindings.Add(new(oid, Counter64.ReadFrom(vbSeq)));
                }
                else
                {
                    Debug.WriteLine($"Unknown Tag {tag}");
                }
            }

            return bindings;
        }

        /// <summary>
        /// Gets enumerator.
        /// </summary>
        public IEnumerator<Variable> GetEnumerator()
        {
            return _variableBindings?.GetEnumerator()
                ?? Enumerable.Empty<Variable>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
