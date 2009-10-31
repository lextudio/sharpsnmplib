using System;
using System.Timers;

namespace Lextm.SharpSnmpLib
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

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            EventHandler<EventArgs> handler = Bark;
            if (handler != null && Enabled)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Feeds this dog.
        /// </summary>
        public void Feed()
        {
            if (Enabled)
            {
                _timer.Stop();
                _timer.Start();
            }
        }
    }
}
