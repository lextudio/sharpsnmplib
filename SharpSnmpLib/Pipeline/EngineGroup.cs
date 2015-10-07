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
		internal static readonly OctetString EngineIdDefault = new OctetString(new byte[] { 128, 0, 31, 136, 128, 233, 99, 0, 0, 214, 31, 244 });
		private const int EngineIdMinLength = 5;
		private const int EngineIdMaxLength = 32;
		internal static readonly OctetString ContextNameDefault = OctetString.Empty;

		private readonly OctetString _engineId = EngineIdDefault;
		private readonly OctetString _contextName = ContextNameDefault;
		private readonly DateTime _start;
		private uint _counterNotInTimeWindow;
		private uint _counterUnknownEngineId;
		private uint _counterUnknownUserName;
		private uint _counterDecryptionError;
		private uint _counterUnknownSecurityLevel;
		private uint _counterAuthenticationFailure;
		private int engineBoots;
		private bool customEngineBootsSet = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="EngineGroup"/> class with
		/// default engine ID and engine boots behavior.
		/// </summary>
		public EngineGroup()
		{
			_start = DateTime.UtcNow;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EngineGroup"/> class with default engine ID and
		///	custom engine boots value.
		/// </summary>
		/// <param name="engineBoots">The number of reboots of this engine, use <c>Int32.MaxValue</c> if unknown.</param>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="engineBoots"/> is negative.</exception>
		public EngineGroup(int engineBoots)
			: this()
		{
			if (engineBoots < 0)
			{
				throw new ArgumentOutOfRangeException("The Engine Boots value must not be negative.");
			}

			this.engineBoots = engineBoots;
			this.customEngineBootsSet = true;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EngineGroup"/> class with custom engine ID and
		/// engine boots value.
		/// </summary>
		/// <param name="engineId">The engine ID to use. At minimum 5 bytes length, at maximum 32 bytes length.
		///		The most significant bit of the first byte must be set. Please follow RFC 3411, p 40ff.</param>
		///	<param name="contextName">Name of the context. Must be unique within the engine. Must not be null, but can be empty.</param>
		/// <param name="engineBoots">The number of reboots of this engine, use <c>Int32.MaxValue</c> if unknown.</param>
		/// <exception cref="System.ArgumentNullException">Thrown if either <paramref name="engineId"/> or <paramref name="contextName"/> is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown if <paramref name="engineId"/> is invalid.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown if <paramref name="engineBoots"/> is negative.</exception>
		public EngineGroup(OctetString engineId, OctetString contextName, int engineBoots)
			: this(engineBoots)
		{
			if (engineId == null)
			{
				throw new ArgumentNullException("engineId");
			}

			var engineIdRaw = engineId.GetRaw();
			if ((engineIdRaw.Length < EngineIdMinLength) || (engineIdRaw.Length > EngineIdMaxLength))
			{
				throw new ArgumentException(string.Format("The length of the Engine ID must be >= {0} and <= {1}.", EngineIdMinLength, EngineIdMaxLength));
			}

			if ((engineIdRaw[0] & 0x80) != 0x80)
			{
				throw new ArgumentException("The most significant bit of the first byte of the Engine ID must be set.");
			}

			if (contextName == null)
			{
				throw new ArgumentNullException("contextName");
			}

			this._engineId = engineId;
			this._contextName = contextName;
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
		/// Gets the context name.
		/// </summary>
		/// <value>The context name.</value>
		internal OctetString ContextName
		{
			get { return _contextName; }
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
				var engineReboots = (this.customEngineBootsSet ? (this.engineBoots) : (int)(ticks / int.MaxValue));
				return new[] { engineReboots, engineTime };
			}
		}

		/// <summary>
		/// Verifies if the request comes in time.
		/// </summary>
		/// <param name="currentTimeData">The current time data.</param>
		/// <param name="pastReboots">The past reboots.</param>
		/// <param name="pastTime">The past time.</param>
		/// <returns>
		///   <c>true</c> if the request is in time window; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsInTime(int[] currentTimeData, int pastReboots, int pastTime)
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
				return new Variable(Messenger.UnknownEngineId, new Counter32(_counterUnknownEngineId++));
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