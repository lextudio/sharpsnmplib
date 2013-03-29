// Engine group class.
// Copyright (C) 2009-2010 Lex Li
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
using Lextm.SharpSnmpLib.Messaging;

namespace Lextm.SharpSnmpLib.Pipeline
{
    /// <summary>
    /// Engine group which contains all related objects.
    /// </summary>
    public sealed class EngineGroup
    {
        // TODO: make engine ID configurable from outside and unique.
        private readonly OctetString _engineId =
            new OctetString(new byte[] { 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244 });
        
        private readonly DateTime _start;
        private uint _counterNotInTimeWindow;
        private uint _counterUnknownEngineId;
        private uint _counterUnknownUserName;
        private uint _counterDecryptionError;
        private uint _counterUnknownSecurityLevel;
        private uint _counterAuthenticationFailure;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineGroup"/> class.
        /// </summary>
        public EngineGroup()
        {
            _start = DateTime.UtcNow;
        }
        
        /// <summary>
        /// Gets the engine id.
        /// </summary>
        /// <value>The engine id.</value>
        internal OctetString EngineId
        {
            get { return _engineId; }
        }

        /// <summary>
        /// Gets or sets the engine boots.
        /// </summary>
        /// <value>The engine boots.</value>
        [Obsolete("Please use EngineTimeData")]
        internal int EngineBoots { get; set; }

        /// <summary>
        /// Gets the engine time.
        /// </summary>
        /// <value>The engine time.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [Obsolete("Please use EngineTimeData")]
        public int EngineTime { get; set; }

        /// <summary>
        /// Gets the engine time data.
        /// </summary>
        /// <value>
        /// The engine time data. [0] is engine boots, [1] is engine time.
        /// </value>
        public int[] EngineTimeData
        {
            get
            {
                var now = DateTime.UtcNow;
                var ticks = (now - _start).Ticks / 10000;
                var engineTime = (int)(ticks % int.MaxValue);
                var engineReboots = (int)(ticks / int.MaxValue);
                return new[] { engineReboots, engineTime };
            }
        }

        /// <summary>
        /// Verifies if the request comes in time.
        /// </summary>
        /// <param name="currentTimeData">The current time data.</param>
        /// <param name="pastTime">The past time.</param>
        /// <param name="pastReboots">The past reboots.</param>
        /// <returns>
        ///   <c>true</c> if the request is in time window; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInTime(int[] currentTimeData, int pastTime, int pastReboots)
        {
            // TODO: RFC 2574 page 27
            if (currentTimeData[1] == int.MaxValue)
            {
                return false;
            }

            if (currentTimeData[0] != pastReboots)
            {
                return false;
            }

            var diff = currentTimeData[1] > pastTime ? currentTimeData[1] - pastTime : currentTimeData[1] - pastTime - int.MinValue + int.MaxValue;
            return diff >= 0 && diff <= 150000;
        }

        /// <summary>
        /// Gets not-in-time-window counter.
        /// </summary>
        /// <value>
        /// Counter variable.
        /// </value>
        public Variable NotInTimeWindow 
        {
            get
            {
                return new Variable(Messenger.NotInTimeWindow, new Counter32(_counterNotInTimeWindow++));
            }
        }

        /// <summary>
        /// Gets unknown engine ID counter.
        /// </summary>
        /// <value>
        /// Counter variable.
        /// </value>
        public Variable UnknownEngineId
        {
            get 
            {
                return new Variable(Messenger.UnknownEngineID, new Counter32(_counterUnknownEngineId++));
            }
        }

        /// <summary>
        /// Gets unknown security name counter.
        /// </summary>
        public Variable UnknownSecurityName
        {
            get
            {
                return new Variable(Messenger.UnknownSecurityName, new Counter32(_counterUnknownUserName++));
            }
        }

        /// <summary>
        /// Gets decryption error counter.
        /// </summary>
        public Variable DecryptionError
        {
            get
            {
                return new Variable(Messenger.DecryptionError, new Counter32(_counterDecryptionError++));
            }
        }

        /// <summary>
        /// Gets unsupported security level counter.
        /// </summary>
        public Variable UnsupportedSecurityLevel
        {
            get
            {
                return new Variable(Messenger.UnsupportedSecurityLevel, new Counter32(_counterUnknownSecurityLevel++));
            }
        }

        /// <summary>
        /// Gets authentication failure counter.
        /// </summary>
        public Variable AuthenticationFailure
        {
            get
            {
                return new Variable(Messenger.AuthenticationFailure, new Counter32(_counterAuthenticationFailure++));
            }
        }
    }
}