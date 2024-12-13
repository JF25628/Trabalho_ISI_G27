using PremierLeagueAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Interfaces
{
    public interface IMatchService
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match> GetMatchByIdAsync(int id);
        Task CreateMatchAsync(Match match);
        Task UpdateMatchAsync(Match match);
        Task DeleteMatchAsync(int id);
    }
}
