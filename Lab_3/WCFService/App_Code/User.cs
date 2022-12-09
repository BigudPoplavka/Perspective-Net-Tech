using System.Runtime.Serialization;
using System.ServiceModel;

namespace PipeVolumeCalcService
{
    [DataContract]
    public class User
    {
        [DataMember]
        private string _login;
        [DataMember]
        private string _password;
        [DataMember]
        public string Login { get { return _login; } set { _login = value; } }
        [DataMember]
        public string Password { get { return _password; } set { _password = value; } }

        
        public User(string userLogin, string userPassword)
        {
            _login = userLogin;
            _password = userPassword;
        }
    }
}
