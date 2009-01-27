using System;
using System.Collections.Generic;

namespace Lextm.SharpSnmpLib.Mib
{
    ///<summary>
    /// Object registry interface.
    ///</summary>
    public interface IObjectRegistry
    {
        /// <summary>
        /// This event occurs when new documents are loaded.
        /// </summary>
        event EventHandler<EventArgs> OnChanged;

        /// <summary>
        /// Object tree.
        /// </summary>
        [CLSCompliant(false)]
        IObjectTree Tree { get; }

        /// <summary>
        /// Gets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path { get; }

        /// <summary>
        /// Gets numercial form from textual form.
        /// </summary>
        /// <param name="textual">Textual</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        uint[] Translate(string textual);

        /// <summary>
        /// Gets numerical form from textual form.
        /// </summary>
        /// <param name="module">Module name</param>
        /// <param name="name">Object name</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        uint[] Translate(string module, string name);

        /// <summary>
        /// Gets textual form from numerical form.
        /// </summary>
        /// <param name="numerical">Numerical form</param>
        /// <returns></returns>
        [CLSCompliant(false)]
        string Translate(uint[] numerical);

        /// <summary>
        /// Loads a folder of MIB files.
        /// </summary>
        /// <param name="folder">Folder</param>
        /// <param name="pattern">MIB file pattern</param>
        void CompileFolder(string folder, string pattern);

        /// <summary>
        /// Loads MIB files.
        /// </summary>
        /// <param name="fileNames">File names.</param>
        void CompileFiles(IEnumerable<string> fileNames);

        /// <summary>
        /// Loads a MIB file.
        /// </summary>
        /// <param name="fileName">File name</param>
        void Compile(string fileName);

        /// <summary>
        /// Validates the table.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns></returns>
        bool ValidateTable(ObjectIdentifier identifier);

        /// <summary>
        /// Refreshes this instance.
        /// </summary>
        void Refresh();

        /// <summary>
        /// Imports the specified modules.
        /// </summary>
        /// <param name="modules">The modules.</param>
        void Import(IEnumerable<MibModule> modules);
    }
}