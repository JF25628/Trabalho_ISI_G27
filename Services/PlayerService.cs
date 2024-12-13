using PremierLeagueAPI.Models;
using PremierLeagueAPI.Data;
using System.Collections.Generic;
using PremierLeagueAPI.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly DatabaseContext _context;

        public PlayerService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Player>> GetAllPlayersAsync()
        {
            var players = new List<Player>();
            string query = "SELECT * FROM Players";

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    players.Add(new Player
                    {
                        PlayerId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        BirthDate = reader.GetDateTime(2),
                        Position = reader.GetString(3),
                        TeamId = reader.GetInt32(4)
                    });
                }
            }

            return players;
        }

        public async Task<Player> GetPlayerByIdAsync(int id)
        {
            string query = $"SELECT * FROM Players WHERE player_id = {id}";
            Player player = null;

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                if (await reader.ReadAsync())
                {
                    player = new Player
                    {
                        PlayerId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        BirthDate = reader.GetDateTime(2),
                        Position = reader.GetString(3),
                        TeamId = reader.GetInt32(4)
                    };
                }
            }

            return player;
        }

        public async Task CreatePlayerAsync(Player player)
        {
            string query = $"INSERT INTO Players (name, birth_date, position, team_id) VALUES ('{player.Name}', '{player.BirthDate}', '{player.Position}', {player.TeamId})";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task UpdatePlayerAsync(Player player)
        {
            string query = $"UPDATE Players SET name = '{player.Name}', birth_date = '{player.BirthDate}', position = '{player.Position}', team_id = {player.TeamId} WHERE player_id = {player.PlayerId}";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task DeletePlayerAsync(int id)
        {
            string query = $"DELETE FROM Players WHERE player_id = {id}";
            await _context.ExecuteQueryAsync(query);
        }
    }
}
