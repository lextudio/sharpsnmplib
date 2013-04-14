namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Error registry interface.
    /// </summary>
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