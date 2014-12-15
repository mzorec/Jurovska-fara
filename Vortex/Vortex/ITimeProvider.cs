using System;

namespace Vortex
{
    /// <summary>
    /// Defines Interface for TimeProvider.
    /// Needed For testing purposes.
    /// </summary>
    public interface ITimeProvider
    {
        /// <summary>
        /// Gets the CurrentTime
        /// </summary>
        DateTime Now { get; }
    }
}