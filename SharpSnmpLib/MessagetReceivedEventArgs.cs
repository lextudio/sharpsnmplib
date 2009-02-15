using System;
using System.Net;

namespace Lextm.SharpSnmpLib
{
	/// <summary>
	/// The base <see cref="EventArgs"/> for all "SNMP message arrived" events .
	/// </summary>
	public class MessageReceivedEventArgs : EventArgs
	{
		ISnmpMessage _message;
		IPEndPoint _sender;

		/// <summary>
		/// Creates a <see cref="MessageReceivedEventArgs"/>.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="message">The message received.</param>
		public MessageReceivedEventArgs(IPEndPoint sender, ISnmpMessage message)
		{
			_sender = sender;
			_message = message;
		}

		/// <summary>
		/// The received message.
		/// </summary>
		public ISnmpMessage Message
		{
			get { return _message; }
		}

		/// <summary>
		/// Sender.
		/// </summary>
		public IPEndPoint Sender
		{
			get { return _sender; }
		}

		/// <summary>
		/// Returns a <see cref="String"/> that represents this <see cref="MessageReceivedEventArgs"/>
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return _message.GetType().ToString() + _message + "; sender: " + _sender;
		}
	}

	/// <summary>
	/// The <see cref="EventArgs"/> for one kind of <see cref="ISnmpMessage"/>, used when that kind of message is received.
	/// </summary>
	public class MessageReceivedEventArgs<T> : MessageReceivedEventArgs where T : ISnmpMessage
	{
		/// <summary>
		/// Creates a <see cref="MessageReceivedEventArgs{T}"/>.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="message">The message received.</param>
		public MessageReceivedEventArgs(IPEndPoint sender, T message)
			: base(sender, message)
		{
		}

		/// <summary>
		/// The received message.
		/// </summary>
		public new T Message
		{
			get { return (T)base.Message; }
		}
	}

}
