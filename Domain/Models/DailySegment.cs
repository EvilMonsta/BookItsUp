using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class DailySegment
    {
        public DayOfWeek DayOfWeek { get; private set; }

        public TimeSpan StartLocalTime { get; private set; }

        public TimeSpan EndLocalTime { get; private set; }

        public DailySegment(DayOfWeek dayOfWeek, TimeSpan startLocalTime, TimeSpan endLocalTime)
        {
            if (startLocalTime < TimeSpan.Zero || startLocalTime > TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException(nameof(startLocalTime), "StartLocalTime must be between 00:00 and 24:00.");

            if (endLocalTime <= TimeSpan.Zero || endLocalTime > TimeSpan.FromHours(24))
                throw new ArgumentOutOfRangeException(nameof(endLocalTime), "EndLocalTime must be greater than 00:00 and at most 24:00.");

            if (startLocalTime >= endLocalTime)
                throw new ArgumentException("StartLocalTime must be earlier than EndLocalTime.");

            DayOfWeek = dayOfWeek;
            StartLocalTime = startLocalTime;
            EndLocalTime = endLocalTime;
        }

    }
}
