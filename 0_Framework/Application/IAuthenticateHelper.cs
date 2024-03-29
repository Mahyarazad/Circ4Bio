﻿using System.Collections.Generic;

namespace _0_Framework.Application
{
    public interface IAuthenticateHelper
    {
        void Login(AuthViewModel model);
        void Logout();
        bool IsAuthenticated();
        string Username();
        AuthViewModel CurrentAccountRole();
        List<int> GetPermission();

    }
}