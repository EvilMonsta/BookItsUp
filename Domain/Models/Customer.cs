using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookItsUp.Domain
{
    public class Customer
    {
        public const int MAX_NAME_LENGTH = 255;

        public const int MAX_EMAIL_LENGTH = 255;

        public const int MAX_PHONE_LENGTH = 15;

        public Guid Id { get; private set; }

        public Guid OrganizationId { get; private set; }

        public string FullName { get; private set; } = string.Empty;

        public string? Email { get; private set; }

        public string? Phone { get; private set; }

        public Customer(Guid id, Guid organizationId, string fullName, string? email, string? phone)
        {

            if (id == Guid.Empty) throw new ArgumentException("Id must not be empty.", nameof(id));

            if (organizationId == Guid.Empty) throw new ArgumentException("OrganizationId must not be empty.", nameof(organizationId));

            if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("FullName must not be empty.", nameof(fullName));

            if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
                throw new ArgumentException("Either Email or Phone must be provided.");


            Id = id;
            OrganizationId = organizationId;
            FullName = fullName.Trim();
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
            Phone = string.IsNullOrWhiteSpace(phone) ? null : phone.Trim();
        }
    }
}
