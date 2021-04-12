using Implementation.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Helpers
{
    // Для целей демонстрации у нас только один юзер. 
    // Реальная авторизация и аутентификация - во вторую итерацию.

    public static class UserProvider
    {
        private static User currUser = new User() { Login = "TestUser" };

        public static User GetCurrentUser()
        {
            return currUser;
        }
    }
}