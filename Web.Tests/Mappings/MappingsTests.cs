using Application.Authentication.Commands.Register;
using Contracts.Authentication;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Web.Tests.Mappings
{
    public class MappingsTests
    {
        private readonly IMapper _mapper;

        public MappingsTests()
        {
            var services = new ServiceCollection();
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(FarmLand_RPG_CommonIdentityServer.AssemblyReference).Assembly);

            services.AddSingleton(config);
            services.AddMapster();

            var serviceProvider = services.BuildServiceProvider();

            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        [Fact]
        public void RegisterRequestShouldBeMappedToRegisterCommand()
        {
            // Arrange
            var registerRequest = new RegisterRequest("NickName", "email@email.com", "RealPassword");

            var finalTrueCommand = new RegisterCommand(registerRequest.Nickname, registerRequest.Email, registerRequest.Password);

            // Act 
            var command = _mapper.Map<RegisterCommand>(registerRequest);

            // Assert
            Assert.Equal(finalTrueCommand, command);
        }
    }
}
