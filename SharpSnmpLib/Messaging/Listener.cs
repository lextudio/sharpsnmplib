/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2008/4/23
 * Time: 19:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Security;

namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Listener class.
    /// </summary>
    public class Listener
    {
        private UserRegistry _users;

        /// <summary>
        /// Error message for non IP v6 OS.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "Pv")]
        public const string ErrorIPv6NotSupported = "cannot use IP v6 as the OS does not support it";

        /// <summary>
        /// Initializes a new instance of the <see cref="Listener"/> class.
        /// </summary>
        public Listener()
        {
            Adapters = new List<IListenerAdapter>();
            Bindings = new List<ListenerBinding>();
        }

        /// <summary>
        /// Gets or sets the users.
        /// </summary>
        /// <value>The users.</value>
        public UserRegistry Users
        {
            get { return _users ?? (_users = UserRegistry.Default); }
            set { _users = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Listener"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        public bool Active { get; private set; }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            foreach (ListenerBinding binding in Bindings)
            {
                binding.Stop();
            }

            Active = false;
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            foreach (ListenerBinding binding in Bindings)
            {
                binding.Start();
            }

            Active = true;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            foreach (ListenerBinding binding in Bindings)
            {
                binding.Dispose();
            }
        }

        /// <summary>
        /// Gets or sets the bindings.
        /// </summary>
        /// <value>The bindings.</value>
        private IList<ListenerBinding> Bindings { get; set; }

        /// <summary>
        /// Gets or sets the adapters.
        /// </summary>
        /// <value>The adapters.</value>
        public IList<IListenerAdapter> Adapters { get; private set; }

        /// <summary>
        /// Occurs when an exception is raised.
        /// </summary>
        public event EventHandler<ExceptionRaisedEventArgs> ExceptionRaised;

        /// <summary>
        /// Occurs when a message is received.
        /// </summary>
        public event EventHandler<MessageReceivedEventArgs<ISnmpMessage>> MessageReceived;

        /// <summary>
        /// Adds the binding.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        public void AddBinding(IPEndPoint endpoint)
        {
            var binding = new ListenerBinding(Users, Adapters, endpoint);
            binding.ExceptionRaised += (o, args) =>
            {
                var handler = ExceptionRaised;
                if (handler != null)
                {
                    handler(o, args);
                }
            };
            binding.MessageReceived += (o, args) =>
            {
                var handler = MessageReceived;
                if (handler != null)
                {
                    handler(o, args);
                }
            };
            Bindings.Add(binding);
        }

        /// <summary>
        /// Clears the bindings.
        /// </summary>
        public void ClearBindings()
        {
            foreach (ListenerBinding binding in Bindings)
            {
                binding.Stop();
                binding.Dispose();
            }

            Bindings.Clear();
        }
    }
}
