using System;
using System.Collections.Generic;
using System.Net;
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// SNMP context interface.
    /// </summary>
    public interface ISnmpContext
    {
        /// <summary>
        /// Handles the authentication failure.
        /// </summary>
        void HandleAuthenticationFailure();

        /// <summary>
        /// Generates the response.
        /// </summary>
        /// <param name="variables">The variables.</param>
        void GenerateResponse(IList<Variable> variables);

        /// <summary>
        /// Copies the request variable bindings to response.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="index">The index.</param>
        void CopyRequest(ErrorCode status, int index);

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>The binding.</value>
        IListenerBinding Binding { get; }

        /// <summary>
        /// Gets the created time.
        /// </summary>
        /// <value>The created time.</value>
        DateTime CreatedTime { get; }

        /// <summary>
        /// Gets the request.
        /// </summary>
        /// <value>The request.</value>
        ISnmpMessage Request { get; }

        /// <summary>
        /// Gets the response.
        /// </summary>
        /// <value>The response.</value>
        ISnmpMessage Response { get; }

        /// <summary>
        /// Gets the sender.
        /// </summary>
        /// <value>The sender.</value>
        IPEndPoint Sender { get; }

        /// <summary>
        /// Gets a value indicating whether [too big].
        /// </summary>
        /// <value><c>true</c> if the response is too big; otherwise, <c>false</c>.</value>
        bool TooBig { get; }

        /// <summary>
        /// Sends out response message.
        /// </summary>
        void SendResponse();

        /// <summary>
        /// Handles the membership authentication.
        /// </summary>
        /// <returns></returns>
        bool HandleMembership();

        /// <summary>
        /// Generates too big message.
        /// </summary>
        void GenerateTooBig();
    }
}