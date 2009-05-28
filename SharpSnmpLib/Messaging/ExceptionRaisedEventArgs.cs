/*
 * Created by SharpDevelop.
 * User: lexli
 * Date: 2008-12-7
 * Time: 12:14
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Provides data for exception raised event.
    /// </summary>
    public sealed class ExceptionRaisedEventArgs : EventArgs
    {
        private readonly Exception _ex;
        
        /// <summary>
        /// Creates an <see cref="ExceptionRaisedEventArgs"/>.
        /// </summary>
        /// <param name="ex">Exception.</param>
        public ExceptionRaisedEventArgs(Exception ex)
        {
            _ex = ex;
        }
        
        /// <summary>
        /// Exception.
        /// </summary>
        public Exception Exception
        {
            get { return _ex; }
        }
    }
}
