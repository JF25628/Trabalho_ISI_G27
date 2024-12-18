using PremierLeagueAPI.Models;
using PremierLeagueAPI.Data;
using PremierLeagueAPI.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PremierLeagueAPI.Services
{
    public class TeamService : ITeamService
    {
        private readonly DatabaseContext _context;

        public TeamService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetAllTeamsAsync()
        {
            var teams = new List<Team>();
            string query = "SELECT * FROM Teams";

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    teams.Add(new Team
                    {
                        TeamId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Founded = reader.GetDateTime(2),
                        Stadium = reader.GetString(3)
                    });
                }
            }

            return teams;
        }

        public async Task<Team> GetTeamByIdAsync(int id)
        {
            string query = $"SELECT * FROM Teams WHERE team_id = {id}";
            Team team = null;

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                if (await reader.ReadAsync())
                {
                    team = new Team
                    {
                        TeamId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Founded = reader.GetDateTime(2),
                        Stadium = reader.GetString(3)
                    };
                }
            }

            return team;
        }

        public async Task CreateTeamAsync(Team team)
        {
            try
            {
                // Formatting the DateTime value correctly
                string matchDate = team.Founded.ToString("yyyy-MM-ddTHH:mm:ss");

                // Creating the SQL query with proper formatting
                string query = $"INSERT INTO Teams (name, founded, stadium) " +
                               $"VALUES ('{team.Name}', '{matchDate}', '{team.Stadium}')";

                // Log the query for debugging
                System.IO.File.AppendAllText("log.txt", query + Environment.NewLine);

                // Executing the query
                await _context.ExecuteQueryAsync(query);
            }
            catch (Exception ex)
            {
                // Log the error
                System.IO.File.AppendAllText("log.txt", "Error: " + ex.Message + Environment.NewLine);
            }
        }


        public async Task UpdateTeamAsync(Team team)
        {
            string query = $"UPDATE Teams SET name = '{team.Name}', founded = '{team.Founded}', stadium = '{team.Stadium}' WHERE team_id = {team.TeamId}";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task DeleteTeamAsync(int id)
        {
            string query = $"DELETE FROM Teams WHERE team_id = {id}";
            await _context.ExecuteQueryAsync(query);
        }
    }
}
