using System;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Error category.
    /// </summary>
    [Obsolete("Please use Pro edition")]
    public enum ErrorCategory
    {
        /// <summary>
        /// Sematic error.
        /// </summary>
        SematicError = 1,

        /// <summary>
        /// A symbol cannot be imported from module.
        /// </summary>
        MissingImportedSymbol = 2,

        /// <summary>
        /// Parent node is missing.
        /// </summary>
        MissingParent = 3,
 
        /// <summary>
        /// Entity type cannot be resolved.
        /// </summary>
        MissingEntityType = 4,

        /// <summary>
        /// Error occurred during type expansion.
        /// </summary>
        TypeExpansionFailed = 5,

        /// <summary>
        /// A module cannot be imported.
        /// </summary>
        MissingDependency = 6,

        /// <summary>
        /// Duplicate type definitions are found.
        /// </summary>
        DuplicateTypes = 7,

        /// <summary>
        /// Duplicate entity definitions are found.
        /// </summary>
        DuplicateEntities = 8,

        /// <summary>
        /// No index is found for table entry.
        /// </summary>
        MissingIndex = 9
    }
}