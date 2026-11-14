using System;
using System.Collections.Generic;
using System.Text;

namespace Tinos3.Api.Utilities
{
    /// <summary>
    /// Provides time, date, and other time-related utilities.
    /// Retrieves the current system time and date in various formats.
    /// </summary>
    public class Clock
    {
        /// <summary>
        /// Gets the current system date and time.
        /// </summary>
        /// <returns>The current local date and time.</returns>
        public static DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }

        /// <summary>
        /// Gets the current time formatted as HH:mm:ss.
        /// </summary>
        /// <returns>The current local time as a formatted string.</returns>
        public static string GetCurrentTimeString()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// Gets the current date formatted as yyyy-MM-dd.
        /// </summary>
        /// <returns>The current local date as a formatted string.</returns>
        public static string GetCurrentDateString()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }
}
