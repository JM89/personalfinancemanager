using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalFinanceManager.Api.Contracts.UserAccount
{
    public class AuthenticatedUser
    {
        public string UserId { get; set; }

        public string Token { get; set; }
    }
}
