using System.Threading.Tasks;

namespace CC.Functions.Interfaces
{
    public interface IAuthProvider
    {
        Task<string> GetAccessToken();
    }
}
