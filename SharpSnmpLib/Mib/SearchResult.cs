using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib.Mib
{
    /// <summary>
    /// Search result.
    /// </summary>
    public class SearchResult
    {
        private readonly IDefinition _definition;
        private readonly uint[] _remaining;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        /// <param name="remaining">The remaining.</param>
        [CLSCompliant(false)]
        public SearchResult(IDefinition definition, uint[] remaining)
        {
            if (definition == null)
            {
                throw new ArgumentNullException("definition");
            }

            if (remaining == null)
            {
                throw new ArgumentNullException("remaining");
            }

            _definition = definition;
            _remaining = remaining;
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        [CLSCompliant(false)]
        public IDefinition Definition
        {
            get { return _definition; }
        }

        /// <summary>
        /// Gets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        [CLSCompliant(false)]
        public uint[] Remaining
        {
            get { return _remaining; }
        }

        /// <summary>
        /// Gets the textual form.
        /// </summary>
        /// <value></value>
        public string Text
        {
            get
            {
                StringBuilder result =
                    new StringBuilder().Append(Definition.ModuleName).Append("::").Append(
                        Definition.Name);

                foreach (uint item in Remaining)
                {
                    result.Append(".").Append(item);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Gets the alternative textual form.
        /// </summary>
        /// <value></value>
        public string AlternativeText
        {
            get
            {
                List<string> names = new List<string>();
                IDefinition current = Definition;
                while (current.ParentDefinition != null)
                {
                    names.Add(current.Name);
                    current = current.ParentDefinition;
                }

                if (names.Count == 0)
                {
                    return String.Empty;
                }

                names.Reverse();
                StringBuilder result = new StringBuilder(names[0]);
                for (int i = 1; i < names.Count; i++)
                {
                    result.Append(".").Append(names[i]);
                }

                foreach (uint item in Remaining)
                {
                    result.Append(".").Append(item);
                }

                return result.ToString();
            }
        }
    }
}