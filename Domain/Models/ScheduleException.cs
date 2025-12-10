using BookItsUp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class ScheduleException
    {
        public DateOnly Date { get; private set; }

        public ScheduleExceptionType Type { get; private set; }

        public IReadOnlyCollection<LocalTimeRange>? Segments { get; private set; }

        private ScheduleException() { }

        public ScheduleException(DateOnly date, ScheduleExceptionType type, IReadOnlyCollection<LocalTimeRange>? segments)
        {
            Date = date;
            Type = type;

            if (type == ScheduleExceptionType.Closed)
            {
                if (segments != null && segments.Any())
                    throw new ArgumentException("Segments must be null or empty for Closed type.");
                Segments = null;
            }
            else if (type == ScheduleExceptionType.OpenExtra)
            {
                if (segments == null || !segments.Any())
                    throw new ArgumentException("Segments must be provided and non-empty for OpenExtra type.");

                var ordered = segments.OrderBy(s => s.StartLocalTime).ToList();

                for (int i = 1; i < ordered.Count; i++)
                {
                    if (ordered[i].StartLocalTime < ordered[i - 1].EndLocalTime)
                        throw new ArgumentException("Segments must not overlap.");
                }

                Segments = ordered.AsReadOnly();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(type), "Unsupported ScheduleExceptionType.");
            }
        }
    }
}
