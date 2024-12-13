using PremierLeagueAPI.Models;
using PremierLeagueAPI.Data;
using PremierLeagueAPI.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace PremierLeagueAPI.Services
{
    [Authorize]
    public class MatchService : IMatchService
    {
        private readonly DatabaseContext _context;

        public MatchService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            var matches = new List<Match>();
            string query = "SELECT * FROM Matches";

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    matches.Add(new Match
                    {
                        MatchId = reader.GetInt32(0),
                        HomeTeamId = reader.GetInt32(1),
                        AwayTeamId = reader.GetInt32(2),
                        StadiumId = reader.GetInt32(3),
                        MatchDate = reader.GetDateTime(4),
                        HomeScore = reader.GetInt32(5),
                        AwayScore = reader.GetInt32(6)
                    });
                }
            }

            return matches;
        }

        public async Task<Match> GetMatchByIdAsync(int id)
        {
            string query = $"SELECT * FROM Matches WHERE match_id = {id}";
            Match match = null;

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                if (await reader.ReadAsync())
                {
                    match = new Match
                    {
                        MatchId = reader.GetInt32(0),
                        HomeTeamId = reader.GetInt32(1),
                        AwayTeamId = reader.GetInt32(2),
                        StadiumId = reader.GetInt32(3),
                        MatchDate = reader.GetDateTime(4),
                        HomeScore = reader.GetInt32(5),
                        AwayScore = reader.GetInt32(6)
                    };
                }
            }

            return match;
        }

        public async Task CreateMatchAsync(Match match)
        {
            string query = $"INSERT INTO Matches (home_team_id, away_team_id, stadium_id, match_date, home_score, away_score) " +
                $"VALUES ({match.HomeTeamId}, {match.AwayTeamId}, {match.StadiumId}, '{match.MatchDate}', {match.HomeScore}, {match.AwayScore})";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task UpdateMatchAsync(Match match)
        {
            string query = $"UPDATE Matches SET home_team_id = {match.HomeTeamId}, away_team_id = {match.AwayTeamId}, " +
                $"stadium_id = {match.StadiumId}, match_date = '{match.MatchDate}', home_score = {match.HomeScore}, away_score = {match.AwayScore} " +
                $"WHERE match_id = {match.MatchId}";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task DeleteMatchAsync(int id)
        {
            string query = $"DELETE FROM Matches WHERE match_id = {id}";
            await _context.ExecuteQueryAsync(query);
        }
    }
}
