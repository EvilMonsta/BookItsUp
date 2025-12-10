using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class WeeklySchedule
    {
        public IReadOnlyCollection<DailySegment> Segments { get; private set; } = new List<DailySegment>();

        private WeeklySchedule() { }

        public WeeklySchedule(IEnumerable<DailySegment> segments)
        {
            if (segments == null) throw new ArgumentNullException(nameof(segments));

            var segmentsList = segments.ToList();

            var duplicateDays = segmentsList
                .GroupBy(s => s.DayOfWeek)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateDays.Any())
            {
                var days = string.Join(",", duplicateDays);
                throw new ArgumentException($"Only one segment per day is allowed. Duplicates found for: {days}");
            }

            Segments = segmentsList.AsReadOnly();

        }

    }

}
