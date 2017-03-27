namespace FileShocker.Services
{
    public interface ILoginManager
    {
        bool Login(string username, string password);
    }
}
