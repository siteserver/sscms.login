using System.Threading.Tasks;
using SSCMS.Login.Models;

namespace SSCMS.Login.Abstractions
{
    public interface IOAuthRepository
    {
        Task<int> InsertAsync(OAuth login);

        Task DeleteAsync(string userName, string source);

        Task<string> GetUserNameAsync(string source, string uniqueId);
    }
}
