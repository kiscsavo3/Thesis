using Domain.Common;

namespace Domain.Entities
{
    public class UserDetails : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfileText { get; set; }
        public byte[] ProfilePicture { get; set; }
    }
}
