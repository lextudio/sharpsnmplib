﻿// Segment interface.
// Copyright (C) 2008-2010 Malcolm Crowe, Lex Li, and other contributors.
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

namespace Lextm.SharpSnmpLib
{
    /// <summary>
    /// Segment interface.
    /// </summary>
    public interface ISegment
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns>
        /// The data, or <c>null</c> if not available.</returns> 
        ISnmpData? GetData(VersionCode version);

        /// <summary>
        /// Converts to <see cref="Sequence"/> object.
        /// </summary>
        /// <returns>
        /// A <see cref="Sequence"/> object representing this segment.</returns>
        Sequence ToSequence();
    }
}
