using Application.Common.Models;
using Application.Common.Models.DTO;
using Mapster;

namespace Application.Common.Mappings
{
    public class ApplicationMappingsConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RefreshToken, UserRefreshToken>()
                .Map(dest => dest.ExpiryTime, src => DateTime.UtcNow.AddDays(src.ExpiryDays));
        }
    }
}
