// Watch dog class.
// Copyright (c) 2009, Lex Li
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
using System.Timers;

namespace Lextm.Common
{
    /// <summary>
    /// Watch dog class that simulates a hardware watch dog.
    /// </summary>
    public class WatchDog
    {
        /// <summary>
        /// Occurs when the dog is hungry and barks.
        /// </summary>
        public event EventHandler<EventArgs> Bark;
        private bool _keepBarking;
        private readonly Timer _timer = new Timer();
        private bool _enabled;

        /// <summary>
        /// Initializes a new instance of the <see cref="WatchDog"/> class.
        /// </summary>
        /// <param name="interval">The interval.</param>
        public WatchDog(double interval)
        {
            _timer.Elapsed += TimerElapsed;
            _timer.Interval = interval;
            _timer.Enabled = false;
            _enabled = false;
            _keepBarking = false;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WatchDog"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this dog keeps barking when hungry.
        /// </summary>
        /// <value><c>true</c> so it keeps barking; otherwise, <c>false</c>.</value>
        public bool KeepBarking
        {
            get { return _keepBarking; }
            set { _keepBarking = value; }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            EventHandler<EventArgs> handler = Bark;
            if (handler != null && Enabled)
            {
                handler(sender, e);
            }
            
            if (!KeepBarking)
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// Feeds this dog.
        /// </summary>
        public void Feed()
        {
            if (!Enabled)
            {
                return;
            }

            _timer.Stop();
            _timer.Start();
        }
    }
}
