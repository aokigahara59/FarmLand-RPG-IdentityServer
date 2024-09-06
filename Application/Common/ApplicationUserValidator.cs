using Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Common
{
    public class ApplicationUserValidator : IUserValidator<ApplicationUser>
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user)
        {
            if (await manager.Users.AnyAsync(x => x.Nickname == user.Nickname))
            {
                return IdentityResult.Failed(new IdentityError { Code = "Nickname already exists."});
            }

            return IdentityResult.Success;
        }
    }
}
