using PremierLeagueAPI.Models;
using PremierLeagueAPI.Data;
using PremierLeagueAPI.Interfaces;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Data;

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

        //public async Task CreateMatchAsync(Match match)
        //{
        //    string query = $"INSERT INTO Matches (home_team_id, away_team_id, stadium_id, match_date, home_score, away_score) " +
        //        $"VALUES ({match.HomeTeamId}, {match.AwayTeamId}, {match.StadiumId}, '{match.MatchDate}', {match.HomeScore}, {match.AwayScore})";
        //    await _context.ExecuteQueryAsync(query);
        //}

        //public async Task CreateMatchAsync(Match match)
        //{
        //    string query = "INSERT INTO Matches (home_team_id, away_team_id, stadium_id, match_date, home_score, away_score) " +
        //                   "VALUES (@HomeTeamId, @AwayTeamId, @StadiumId, @MatchDate, @HomeScore, @AwayScore)";

        //    // Criando os parâmetros
        //    var parameters = new[]
        //    {
        //        new SqlParameter("@HomeTeamId", SqlDbType.Int) { Value = match.HomeTeamId },
        //        new SqlParameter("@AwayTeamId", SqlDbType.Int) { Value = match.AwayTeamId },
        //        new SqlParameter("@StadiumId", SqlDbType.Int) { Value = match.StadiumId },
        //        new SqlParameter("@MatchDate", SqlDbType.DateTime) { Value = match.MatchDate.ToString("yyyy-MM-ddTHH:mm:ss") }, // Certificando que a data está em um formato válido
        //        new SqlParameter("@HomeScore", SqlDbType.Int) { Value = match.HomeScore },
        //        new SqlParameter("@AwayScore", SqlDbType.Int) { Value = match.AwayScore }
        //    };

        //    // Executando a query com parâmetros
        //    await _context.ExecuteQueryAsync(query, parameters);
        //}

        public async Task CreateMatchAsync(Match match)
        {
            try
            {
                // Formatando a data para o formato desejado
                string matchDate = match.MatchDate.ToString("yyyy-MM-ddTHH:mm:ss");

                // Construindo a string de consulta SQL
                string query = $"INSERT INTO Matches (home_team_id, away_team_id, stadium_id, match_date, home_score, away_score) " +
                               $"VALUES ({match.HomeTeamId}, {match.AwayTeamId}, {match.StadiumId}, '{matchDate}', {match.HomeScore}, {match.AwayScore})";

                // Executando a consulta SQL
                await _context.ExecuteQueryAsync(query); // Aqui passa apenas a string da consulta
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao adicionar partida: {ex.Message}");
            }
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
