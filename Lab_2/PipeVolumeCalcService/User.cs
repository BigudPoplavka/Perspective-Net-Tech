using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipeVolumeCalcService
{
    public class User
    {
        private readonly string _login;
        private readonly string _password;
        
        public User(string userLogin, string userPassword)
        {
            _login = userLogin;
            _password = userPassword;
        }
    }
}
