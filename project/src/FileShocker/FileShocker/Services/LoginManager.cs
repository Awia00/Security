namespace FileShocker.Services
{
    public class LoginManager : ILoginManager
    {
        private readonly string _username, _password;

        public LoginManager(string username, string password)
        {
            _username = username;
            _password = password;
        }

        public bool Login(string username, string password)
        {
            return username == _username && password == _password;
        }
    }
}