using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class Provider
    {
        public const int MAX_NAME_LENGTH = 255;

        public const int MAX_TIME_ZONE_LENGTH = 100;

        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public string TimeZone { get; private set; } = string.Empty;

        public int Capacity { get; private set; }

        public WeeklySchedule WeeklySchedule { get; private set; }

        public bool IsActive { get; private set; }

        public IReadOnlyCollection<ScheduleException> ScheduleExceptions { get; private set; } =
            Array.Empty<ScheduleException>();

        private Provider() { }

        public Provider(Guid id, Guid organizationId, string name, string timeZone, int capacity, WeeklySchedule weeklySchedule, bool isActive, IReadOnlyCollection<ScheduleException>? scheduleExceptions = null)
        {
            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));

            if (organizationId == Guid.Empty)
                throw new ArgumentNullException(nameof(organizationId));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));
            if (name.Length > 255)
                throw new ArgumentOutOfRangeException(nameof(name));
            if (string.IsNullOrWhiteSpace(timeZone))
                throw new ArgumentNullException(nameof(timeZone));
            if (capacity < 1)
                throw new ArgumentException("Capacity cannot be lower than 1");
            if (weeklySchedule is null) throw new ArgumentNullException(nameof(weeklySchedule));


            Id = id;
            OrganizationId = organizationId;
            Name = name.Trim();
            TimeZone = timeZone.Trim();
            Capacity = capacity;
            WeeklySchedule = weeklySchedule;
            IsActive = isActive;

            if (scheduleExceptions is null || scheduleExceptions.Count == 0)
            {
                ScheduleExceptions = Array.Empty<ScheduleException>();
            }
            else
            {
                var normalized = scheduleExceptions
                    .Where(e => e is not null)
                    .GroupBy(e => e.Date)
                    .Select(g => g.First())
                    .OrderBy(e => e.Date)
                    .ToList();

                ScheduleExceptions = normalized.AsReadOnly();

            }
        }

    }
}
