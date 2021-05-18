using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using CoreImplementation.Model;

namespace CoreImplementation.ServiceInterfaces
{
    public interface IUserService
    {
        VMUser ExtractUserData(ClaimsPrincipal principal);
    }
}
