/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/5/17
 * Time: 16:33
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Description of SharpMibException.
    /// </summary>
    [Serializable]
    public sealed class SharpMibException : SharpSnmpException
    {
        private Symbol _symbol;
        
        /// <summary>
        /// Symbol.
        /// </summary>
        public Symbol Symbol
        {
            get { return _symbol; }
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMibException"/>.
        /// </summary>
        public SharpMibException()
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpSnmpException"/> instance with a specific <see cref="String"/>.
        /// </summary>
        /// <param name="message">Message</param>
        public SharpMibException(string message) : base(message)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMibException"/> instance with a specific <see cref="String"/> and an <see cref="Exception"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="inner">Inner exception</param>
        public SharpMibException(string message, Exception inner)
            : base(message, inner)
        {
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMibException"/> instance.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        private SharpMibException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            
            _symbol = (Symbol)info.GetValue("Symbol", typeof(Symbol));
        }
        
        /// <summary>
        /// Gets object data.
        /// </summary>
        /// <param name="info">Info</param>
        /// <param name="context">Context</param>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("Symbol", _symbol);
        }
        
        /// <summary>
        /// Details on error.
        /// </summary>
        public override string Details
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "wrong symbol {0} in file \"{1}\". Row {2}. Column: {3}",
                    _symbol,
                    _symbol.File,
                    _symbol.Row,
                    _symbol.Column);
            }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="SharpMibException"/>.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "SharpMibException: " + Details;
        }
        
        /// <summary>
        /// Creates a <see cref="SharpMibException"/> with a specific <see cref="Symbol"/>.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="symbol">Symbol</param>
        /// <returns></returns>
        public static SharpMibException Create(string message, Symbol symbol)
        {
            SharpMibException ex = new SharpMibException(message + ". Wrong entity, " + symbol.ToString() + " in file \"" + symbol.File + "\". row: " + symbol.Row + "; column: " + symbol.Column);
            ex._symbol = symbol;
            return ex;
        }
    }
}