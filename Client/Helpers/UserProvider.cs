using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreImplementation.Model;

namespace Client.Helpers
{
    // Для целей демонстрации у нас только один юзер. 
    // Реальная авторизация и аутентификация - во вторую итерацию.

    public static class UserProvider
    {
        private static VMUser currUser = new VMUser() { Login = "TestUser" };

        public static VMUser GetCurrentUser()
        {
            return currUser;
        }
    }
}