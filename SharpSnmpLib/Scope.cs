// Scope type.
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

using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Scope segment.
    /// </summary>
    public sealed class Scope : ISegment
    {
        private readonly Sequence _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public Scope(Sequence data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            _container = data;
            ContextEngineId = (OctetString)data[0];
            ContextName = (OctetString)data[1];
            Pdu = (ISnmpPdu)data[2];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="contextEngineId">The context engine ID.</param>
        /// <param name="contextName">Name of the context.</param>
        /// <param name="pdu">The PDU.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pdu", Justification = "definition")]
        public Scope(OctetString contextEngineId, OctetString contextName, ISnmpPdu pdu)
        {
            if (contextEngineId == null)
            {
                throw new ArgumentNullException(nameof(contextEngineId));
            }

            if (contextName == null)
            {
                throw new ArgumentNullException(nameof(contextName));
            }

            if (pdu == null)
            {
                throw new ArgumentNullException(nameof(pdu));
            }

            ContextEngineId = contextEngineId;
            ContextName = contextName;
            Pdu = pdu;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="pdu">The PDU.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "pdu", Justification = "definition")]
        public Scope(ISnmpPdu pdu)
        {
            if (pdu == null)
            {
                throw new ArgumentNullException(nameof(pdu));
            }

            Pdu = pdu;
        }

        /// <summary>
        /// Gets the PDU.
        /// </summary>
        /// <value>The PDU.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Pdu", Justification = "definition")]
        public ISnmpPdu Pdu { get; private set; }

        #region ISegment Members

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ISnmpData GetData(VersionCode version)
        {
            if (version == VersionCode.V3)
            {
                return ToSequence();
            }

            return Pdu;
        }

        /// <summary>
        /// Gets or sets the name of the context.
        /// </summary>
        /// <value>The name of the context.</value>
        public OctetString ContextName { get; private set; }

        /// <summary>
        /// Gets or sets the context engine id.
        /// </summary>
        /// <value>The context engine id.</value>
        public OctetString ContextEngineId { get; private set; }

        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns></returns>
        public Sequence ToSequence()
        {
            return _container ?? new Sequence(null, ContextEngineId, ContextName, Pdu);
        }

        #endregion
    }
}
