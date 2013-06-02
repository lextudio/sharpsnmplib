using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Error registry interface.
    /// </summary>
    [Obsolete("Please use Pro edition")]
    public interface IErrorRegistry
    {
        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="message">The message.</param>
        /// <param name="entity">The entity.</param>
        void AddError(ErrorCategory category, string message, params IConstruct[] entity);
    }
}