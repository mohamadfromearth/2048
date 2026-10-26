public class PlayerService : IPlayerService
{
    private readonly IPlayerRepository _playerRepository;


    public PlayerService(IPlayerRepository playerRepository)
    {
        _playerRepository = playerRepository;
    }


    public int GetBestScore() => _playerRepository.GetBestScore();

    public void SetBestScore(int score)
    {
        var bestScore = _playerRepository.GetBestScore();
        if (score > bestScore) _playerRepository.SetBestScore(score);
    }
}