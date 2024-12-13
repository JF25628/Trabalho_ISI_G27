namespace PremierLeagueAPI.Models
{
    public class Match
    {
    public int MatchId { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public int StadiumId { get; set; }
    public DateTime MatchDate { get; set; }
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
    }
}

