using BlossomServer.SharedKernel.Utils;

namespace BlossomServer.Domain.Entities
{
    public class Subscriber : Entity<Guid>
    {
        public string Email { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public Subscriber(
            Guid id,
            string email
        ) : base(id)
        {
            Email = email;
            CreatedAt = TimeZoneHelper.GetLocalTimeNow();
        }

        public void SetEmail(string email) { Email = email; }
    }
}
