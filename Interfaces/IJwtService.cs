namespace PremierLeagueAPI.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string username); // Gerar token com o nome de usuário
    }
}
