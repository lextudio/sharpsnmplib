// Search result.
// Copyright (C) 2010 Lex Li.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this
// software and associated documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons
// to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR
// PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Text;

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Search result.
    /// </summary>
    public sealed class SearchResult
    {
        private readonly uint[] _remaining;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult"/> class.
        /// </summary>
        /// <param name="definition">The definition.</param>
        [CLSCompliant(false)]
        public SearchResult(IDefinition definition) : this(definition, new uint[0])
        {
        }

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

            Definition = definition;
            _remaining = remaining;
        }

        /// <summary>
        /// Gets the definition.
        /// </summary>
        /// <value>The definition.</value>
        [CLSCompliant(false)]
        public IDefinition Definition { get; private set; }

        /// <summary>
        /// Gets the remaining.
        /// </summary>
        /// <value>The remaining.</value>
        [CLSCompliant(false)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public ICollection<uint> GetRemaining()
        {
            return _remaining; 
        }

        /// <summary>
        /// Gets the textual form.
        /// </summary>
        /// <value></value>
        public string Text
        {
            get
            {
                var result =
                    new StringBuilder().Append(Definition.ModuleName).Append("::").Append(
                        Definition.Name);

                foreach (var item in GetRemaining())
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
                var names = new List<string>();
                var current = Definition;
                IDefinition parent;
                while ((parent = current.ParentDefinition) != null)
                {
                    names.Add(current.Name);
                    current = parent;
                }

                if (names.Count == 0)
                {
                    return string.Empty;
                }

                names.Reverse();
                var result = new StringBuilder(".").Append(names[0]);
                for (var i = 1; i < names.Count; i++)
                {
                    result.Append(".").Append(names[i]);
                }

                foreach (var item in GetRemaining())
                {
                    result.Append(".").Append(item);
                }

                return result.ToString();
            }
        }
    }
}