using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Provides data for a INFORM request received event.
    /// </summary>
    public sealed class InformRequestReceivedEventArgs : EventArgs, IDisposable
    {
        private InformRequestMessage _inform;
        private IPEndPoint _sender;
      
        /// <summary>
        /// Creates a <see cref="InformRequestReceivedEventArgs"/> 
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="inform">Trap message</param>
        public InformRequestReceivedEventArgs(IPEndPoint sender, InformRequestMessage inform)
        {
            _sender = sender;
            _inform = inform;
        }
       
        /// <summary>
        /// INFORM message.
        /// </summary>
        public InformRequestMessage InformRequest
        {
            get
            {
                return _inform;
            }
        }
      
        /// <summary>
        /// Agent.
        /// </summary>
        public IPEndPoint Sender
        {
            get { return _sender; }
        }
        
        /// <summary>
        /// Returns a <see cref="String"/> that represents this <see cref="InformRequestReceivedEventArgs"/>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "INFORM request received event args: INFORM message: " + _inform + "; sender: " + _sender;
        }
        
        private bool _disposed;
        
        /// <summary>
        /// Finalizer of <see cref="InformRequestReceivedEventArgs"/>.
        /// </summary>
        ~InformRequestReceivedEventArgs()
        {
            Dispose(false);
        }
        
        /// <summary>
        /// Releases all resources used by the <see cref="InformRequestReceivedEventArgs"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// Disposes of the resources (other than memory) used by the <see cref="InformRequestReceivedEventArgs"/>.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            
            if (disposing)
            {
                _inform.Dispose();
            }
            
            _disposed = true;
        }
    }
}

