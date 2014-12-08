using System;

namespace Vortex
{
    /// <summary>
    /// The default implementation of <see cref="ITimeProvider"/> interface.
    /// </summary>
    public class TimeProvider : ITimeProvider
    {
        /// <summary>
        /// Gets the CurrentTime
        /// </summary>
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}