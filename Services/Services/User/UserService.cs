using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using CoreImplementation.Model;
using CoreImplementation.ServiceInterfaces;

namespace Services.Services.User
{
    public class UserService : IUserService
    {

        public VMUser ExtractUserData(ClaimsPrincipal principal)
        {
            if (string.IsNullOrEmpty(principal?.Identity?.Name))
                return new VMUser();

            var nameObj = principal.FindFirst(ClaimTypes.GivenName);
            string name = nameObj?.Value ?? principal.Identity.Name;

            var surnameObj = principal.FindFirst(ClaimTypes.Surname);
            string surname = surnameObj?.Value ?? "";

            var avatarObj = principal.FindFirst("urn:google:image");
            string avatar = avatarObj?.Value ?? "";

            var emailObj = principal.FindFirst(ClaimTypes.Email);
            string email = emailObj?.Value ?? "";

            VMUser result = new VMUser()
            {
                AvatarUrl = avatar,
                Email = email,
                Name = name
                //, surname
            };

            return result;
        }

    }
}
