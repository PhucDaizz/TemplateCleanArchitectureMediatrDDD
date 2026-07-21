using AuthService.Domain.Common;
using AuthService.Domain.Exceptions;

namespace AuthService.Domain.Entities
{
    public class Subscriber: BaseEntity<Guid>, AggregateRoot
    {
        public string Email { get; private set; }
        public string FullName { get; private set; }
        public bool IsActive { get; private set; }

        private Subscriber() { }

        public static Subscriber Create(string email, string fullName)
        {
            if (email == null) throw new DomainException("Email can not null");
            if (fullName == null) throw new DomainException("Full name can not null");

            return new Subscriber
            {
                Email = email,
                FullName = fullName,
                IsActive = true
            };
        }
    }
}
