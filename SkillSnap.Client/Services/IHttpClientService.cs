using SkillSnap.Shared;
using System.Threading.Tasks;

namespace SkillSnap.Client.Services
{
    public interface IHttpClientService
    {
        Task<PortfolioUserDto?> GetPortfolioUserAsync(int id);
    }
}
