namespace Infrastructure.RefreshToken
{
    public class RefreshTokenSettings
    {
        public const string SectionName = "RefreshTokenSettings";

        public int ExpiryDays { get; set; }
    }
}
