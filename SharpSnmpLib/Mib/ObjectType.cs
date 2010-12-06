using System.Collections.Generic;
using System.Text;
using System;

namespace Lextm.SharpSnmpLib.Mib
{
    internal sealed class ObjectType : IEntity
    {
        private readonly string _module;
        private string _parent;
        private readonly uint _value;
        private readonly string _name;
        private ITypeAssignment _syntax;
        private string _units;
        private MaxAccess _access;
        private Status _status;
        private string _description;
        private string _reference;
        private IList<string> _indices;
        private string _augment;
        private Symbol _defVal;


        public ObjectType(string module, IList<Symbol> header, Lexer lexer)
        {
            _module = module;
            _name = header[0].ToString();
            ParseProperties(header);
            lexer.ParseOidValue(out _parent, out _value);
        }

        private void ParseProperties(IEnumerable<Symbol> header)
        {
            IEnumerator<Symbol> enumerator = header.GetEnumerator();
            Symbol temp = enumerator.NextNonEOLSymbol();

            // Skip name
            temp = enumerator.NextNonEOLSymbol();

            temp.Expect(Symbol.ObjectType);
            temp = enumerator.NextNonEOLSymbol();

            temp.Expect(Symbol.Syntax);
            temp = enumerator.NextNonEOLSymbol();

            if (temp == Symbol.Bits)
            {
                _syntax = new BitsType(_module, string.Empty, enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.Integer || temp == Symbol.Integer32)
            {
                _syntax = new IntegerType(_module, string.Empty, enumerator, ref temp);
            }
            else if (temp == Symbol.Octet)
            {
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.String);
                _syntax = new OctetStringType(_module, string.Empty, enumerator, ref temp);
            }
            else if (temp == Symbol.Opaque)
            {
                _syntax = new OctetStringType(_module, string.Empty, enumerator, ref temp);
            }
            else if (temp == Symbol.IpAddress)
            {
                _syntax = new IpAddressType(_module, string.Empty, enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.Counter64)
            {
                _syntax = new Counter64Type(_module, string.Empty, enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.Unsigned32 || temp == Symbol.Counter32 || temp == Symbol.Gauge32 || temp == Symbol.TimeTicks)
            {
                _syntax = new UnsignedType(_module, string.Empty, enumerator, ref temp);
            }
            else if (temp == Symbol.Object)
            {
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.Identifier);
                _syntax = new ObjectIdentifierType(_module, string.Empty, enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.Sequence)
            {
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.Of);
                temp = enumerator.NextNonEOLSymbol();

                _syntax = new TypeAssignment(_module, string.Empty, enumerator, ref temp);
            }
            else
            {
                _syntax = new TypeAssignment(_module, string.Empty, enumerator, ref temp);
            }


            if (temp == Symbol.Units)
            {
                temp = enumerator.NextNonEOLSymbol();

                _units = temp.ToString();
                temp = enumerator.NextNonEOLSymbol();
            }

            if (temp == Symbol.MaxAccess || temp == Symbol.Access)
            {
                temp = enumerator.NextNonEOLSymbol();

                switch (temp.ToString())
                {
                    case "not-accessible":
                        _access = MaxAccess.notAccessible;
                        break;
                    case "accessible-for-notify":
                        _access = MaxAccess.accessibleForNotify;
                        break;
                    case "read-only":
                        _access = MaxAccess.readOnly;
                        break;
                    case "read-write":
                        _access = MaxAccess.readWrite;
                        break;
                    case "read-create":
                        _access = MaxAccess.readCreate;
                        break;
                    case "write-only":
                        _access = MaxAccess.readWrite;
                        break;
                    default:
                        temp.Validate(true, "Invalid access");
                        break;
                }
            }
            else
            {
                temp.Validate(true, "missing access");
            }

            temp = enumerator.NextNonEOLSymbol();

            temp.Expect(Symbol.Status);
            temp = enumerator.NextNonEOLSymbol();

            try
            {
                _status = (Status)Enum.Parse(typeof(Status), temp.ToString());
                temp = enumerator.NextNonEOLSymbol();
            }
            catch (ArgumentException)
            {
                temp.Validate(true, "Invalid status");
            }

            if (temp == Symbol.Description)
            {
                temp = enumerator.NextNonEOLSymbol();

                _description = temp.ToString().Trim(new char[] { '"' });
                temp = enumerator.NextNonEOLSymbol();
            }

            if (temp == Symbol.Reference)
            {
                temp = enumerator.NextNonEOLSymbol();

                _reference = temp.ToString();
                temp = enumerator.NextNonEOLSymbol();
            }

            if (temp == Symbol.Index)
            {
                temp = enumerator.NextNonEOLSymbol();

                _indices = ParseIndexTypes(enumerator);
                temp = enumerator.NextNonEOLSymbol();
            }
            else if (temp == Symbol.Augments)
            {
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.OpenBracket);
                temp = enumerator.NextNonEOLSymbol();

                _augment = temp.ToString();
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.CloseBracket);
                temp = enumerator.NextNonEOLSymbol();
            }

            if (temp == Symbol.DefVal)
            {
                temp = enumerator.NextNonEOLSymbol();

                temp.Expect(Symbol.OpenBracket);
                temp = enumerator.NextNonEOLSymbol();

                if (temp == Symbol.OpenBracket)
                {
                    var depth = 1;
                    // TODO: decode this.
                    while (depth > 0)
                    {
                        temp = enumerator.NextNonEOLSymbol();
                        if (temp == Symbol.OpenBracket)
                        {
                            depth++;
                        }
                        else if (temp == Symbol.CloseBracket)
                        {
                            depth--;
                        }

                    }
                }
                else
                {
                    _defVal = temp;
                    temp = enumerator.NextNonEOLSymbol();
                }


            }
        }

        private IList<string> ParseIndexTypes(IEnumerator<Symbol> enumerator)
        {
            IList<string> list = new List<string>();
            Symbol temp = enumerator.Current; enumerator.MoveNext();

            while (temp != Symbol.CloseBracket)
            {
                if (temp != Symbol.Comma && temp != Symbol.EOL)
                {
                    list.Add(temp.ToString());
                }
                enumerator.MoveNext();
                temp = enumerator.Current;
            }

            return list;
        }

        private static bool IsProperty(Symbol sym)
        {
            string s = sym.ToString();
            return s == "SYNTAX" || s == "MAX-ACCESS" || s == "STATUS" || s == "DESCRIPTION";
        }

        public string ModuleName
        {
            get { return _module; }
        }

        public string Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public uint Value
        {
            get { return _value; }
        }

        public string Name
        {
            get { return _name; }
        }

        public string Description
        {
            get { return _description; }
        }
    }
}