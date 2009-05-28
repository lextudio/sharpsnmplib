using System;
using System.ComponentModel;

#pragma warning disable 612,618
namespace Lextm.SharpSnmpLib.Messaging
{
    /// <summary>
    /// Agent component.
    /// </summary>
    public sealed class Agent : Component
    {           
        /// <summary>
        /// Initiates an <see cref="Agent"/> instance.
        /// </summary>
        public Agent()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Agent"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public Agent(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            
            container.Add(this);
            InitializeComponent();
        }

        #region Component Designer generated code
// ReSharper disable all
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
// ReSharper restore all

        #endregion
    }
}
#pragma warning restore 612,618