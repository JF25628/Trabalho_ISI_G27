namespace PremierLeagueAPI.Models
{
    public class Stadium
    {
        public int StadiumId { get; set; } // ID do Estádio
        public string Name { get; set; } // Nome do Estádio
        public string City { get; set; } // Cidade onde o Estádio está localizado
        public int Capacity { get; set; } // Capacidade do Estádio
    }
}
