// Message factory type.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/1
 * Time: 11:17
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Factory that creates <see cref="ISnmpMessage"/> instances from byte format.
    /// </summary>
    public static class MessageFactory
    {
        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from a string.
        /// </summary>
        /// <param name="bytes">Byte string.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(IEnumerable<char> bytes, UserRegistry registry)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            return ParseMessages(ByteTool.Convert(bytes), registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, UserRegistry registry)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            return ParseMessages(buffer, 0, buffer.Length, registry);
        }

        /// <summary>
        /// Creates <see cref="ISnmpMessage"/> instances from buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">The index.</param>
        /// <param name="length">The length.</param>
        /// <param name="registry">The registry.</param>
        /// <returns></returns>
        public static IList<ISnmpMessage> ParseMessages(byte[] buffer, int index, int length, UserRegistry registry)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (registry == null)
            {
                throw new ArgumentNullException(nameof(registry));
            }

            IList<ISnmpMessage> result = new List<ISnmpMessage>();
            using Stream stream = new MemoryStream(buffer, index, length, true);
            int first;
            while ((first = stream.ReadByte()) != -1)
            {
                result.Add(ParseMessage(first, stream, registry));
            }

            return result;
        }

        private static ISnmpMessage ParseMessage(int first, Stream stream, UserRegistry registry)
        {
            var array = DataFactory.CreateSnmpData(first, stream);
            if (array.TypeCode != SnmpType.Sequence)
            {
                throw new SnmpException("not an SNMP message");
            }

            var body = (Sequence)array;
            if (body.Length != 3 && body.Length != 4)
            {
                throw new SnmpException("not an SNMP message");
            }

            var version = (VersionCode)((Integer32)body[0]).ToInt32();
            Header header;
            SecurityParameters parameters;
            IPrivacyProvider privacy;
            Scope scope;
            if (body.Length == 3)
            {
                header = Header.Empty;
                parameters = SecurityParameters.Create((OctetString)body[1]);
                privacy = DefaultPrivacyProvider.DefaultPair;
                scope = new Scope((ISnmpPdu)body[2]);
            }
            else
            {
                header = new Header(body[1]);
                parameters = new SecurityParameters((OctetString)body[2]);
                var temp = registry.Find(parameters.UserName);
                if (temp == null)
                {
                    // handle decryption exception.
                    return new MalformedMessage(header.MessageId, parameters.UserName, body[3]);
                }

                privacy = temp;
                var code = body[3].TypeCode;
                if (code == SnmpType.Sequence)
                {
                    // v3 not encrypted
                    scope = new Scope((Sequence)body[3]);
                }
                else if (code == SnmpType.OctetString)
                {
                    // v3 encrypted
                    try
                    {
                        scope = new Scope((Sequence)privacy.Decrypt(body[3], parameters));
                    }
                    catch (SnmpException)
                    {
                        // If decryption failed or gave back invalid data, handle parsing exceptions.
                        return new MalformedMessage(header.MessageId, parameters.UserName, body[3]);
                    }
                }
                else
                {
                    throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "invalid v3 packets scoped data: {0}", code));
                }

                if (!privacy.VerifyHash(version, header, parameters, body[3], body.GetLengthBytes()))
                {
                    parameters.IsInvalid = true;
                }
            }

            var scopeCode = scope.Pdu.TypeCode;
            try
            {
                return scopeCode switch
                {
                    SnmpType.TrapV1Pdu => new TrapV1Message(body),
                    SnmpType.TrapV2Pdu => new TrapV2Message(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.GetRequestPdu => new GetRequestMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.ResponsePdu => new ResponseMessage(version, header, parameters, scope, privacy, false, body.GetLengthBytes()),
                    SnmpType.SetRequestPdu => new SetRequestMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.GetNextRequestPdu => new GetNextRequestMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.GetBulkRequestPdu => new GetBulkRequestMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.ReportPdu => new ReportMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    SnmpType.InformRequestPdu => new InformRequestMessage(version, header, parameters, scope, privacy, body.GetLengthBytes()),
                    _ => throw new SnmpException(string.Format(CultureInfo.InvariantCulture, "unsupported pdu: {0}", scopeCode)),
                };
            }
            catch (Exception ex)
            {
                if (ex is SnmpException)
                {
                    throw;
                }

                throw new SnmpException("message construction exception", ex);
            }
        }
    }
}
