using PremierLeagueAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Interfaces
{
    public interface IStadiumService
    {
        Task<IEnumerable<Stadium>> GetAllStadiumsAsync();
        Task<Stadium> GetStadiumByIdAsync(int id);
        Task CreateStadiumAsync(Stadium stadium);
        Task UpdateStadiumAsync(Stadium stadium);
        Task DeleteStadiumAsync(int id);
    }
}
