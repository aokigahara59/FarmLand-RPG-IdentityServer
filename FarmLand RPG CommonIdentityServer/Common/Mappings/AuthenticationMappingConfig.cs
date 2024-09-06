using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.Refresh;
using Application.Authentication.Commands.Register;
using Contracts.Authentication;
using Mapster;

namespace FarmLand_RPG_CommonIdentityServer.Common.Mappings
{
    public class AuthenticationMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<RegisterRequest, RegisterCommand>();
            config.NewConfig<LoginRequest, LoginCommand>();
            config.NewConfig<RefreshRequest, RefreshCommand>();
        }
    }
}
