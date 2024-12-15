using PremierLeagueAPI.Models;
using PremierLeagueAPI.Data;
using System.Collections.Generic;
using PremierLeagueAPI.Interfaces;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace PremierLeagueAPI.Services
{
    public class StadiumService : IStadiumService
    {
        private readonly DatabaseContext _context;

        public StadiumService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Stadium>> GetAllStadiumsAsync()
        {
            var stadiums = new List<Stadium>();
            string query = "SELECT * FROM Stadiums";

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                while (await reader.ReadAsync())
                {
                    stadiums.Add(new Stadium
                    {
                        StadiumId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        City = reader.GetString(2),
                        Capacity = reader.GetInt32(3)
                    });
                }
            }

            return stadiums;
        }

        public async Task<Stadium> GetStadiumByIdAsync(int id)
        {
            string query = $"SELECT * FROM Stadiums WHERE stadium_id = {id}";
            Stadium stadium = null;

            using (var reader = await _context.ExecuteQueryReaderAsync(query))
            {
                if (await reader.ReadAsync())
                {
                    stadium = new Stadium
                    {
                        StadiumId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        City = reader.GetString(2),
                        Capacity = reader.GetInt32(3)
                    };
                }
            }

            return stadium;
        }

        //public async Task CreateStadiumAsync(Stadium stadium)
        //{
        //    string query = $"INSERT INTO Stadiums (name, city, capacity) VALUES ('{stadium.Name}', '{stadium.City}', {stadium.Capacity})";
        //    await _context.ExecuteQueryAsync(query);
        //}

        public async Task CreateStadiumAsync(Stadium stadium)
        {
            try
            {
                // Construindo a string de consulta SQL
                string query = $"INSERT INTO Stadiums (name, city, capacity) " +
                    $"VALUES ('{stadium.Name}', '{stadium.City}', {stadium.Capacity})";
                System.IO.File.AppendAllText("log.txt", query);
                // Executando a consulta SQL
                await _context.ExecuteQueryAsync(query); // Aqui passa apenas a string da consulta
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Erro ao adicionar partida: {ex.Message}");
            }
        }

        public async Task UpdateStadiumAsync(Stadium stadium)
        {
            string query = $"UPDATE Stadiums SET name = '{stadium.Name}', city = '{stadium.City}', capacity = {stadium.Capacity} WHERE stadium_id = {stadium.StadiumId}";
            await _context.ExecuteQueryAsync(query);
        }

        public async Task DeleteStadiumAsync(int id)
        {
            string query = $"DELETE FROM Stadiums WHERE stadium_id = {id}";
            await _context.ExecuteQueryAsync(query);
        }
    }
}
