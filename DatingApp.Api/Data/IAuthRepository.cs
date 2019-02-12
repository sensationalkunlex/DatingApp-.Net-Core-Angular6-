using System.Threading.Tasks;
using DatingApp.Api.Model;

namespace DatingApp.Api.Data
{
    public interface IAuthRepository
    {
         
         Task<User>  Register(User user, string password);
      Task<User>  Login(string username, string password); 
       Task<bool>  UserExists(string username);
    }
}