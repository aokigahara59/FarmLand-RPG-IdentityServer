using System.Text.Json.Serialization;

namespace Application.Common.Models
{
    public class UserRefreshToken
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Token { get; set; }
        public DateTime ExpiryTime { get; set; }
        public string ApplicationUserId { get; set; } 
        public ApplicationUser ApplicationUser { get; set; } 
    }
}
