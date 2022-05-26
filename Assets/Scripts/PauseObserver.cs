public class PauseObserver
{
    private readonly PauseService _pauseService;
        
    public readonly PauseSource Threshold;

    public PauseObserver(PauseService pauseService, PauseSource threshold)
    {
        _pauseService = pauseService;
        Threshold = threshold;
    }

    public bool IsPaused => _pauseService.StrongestSource >= Threshold;
    public bool IsUnpaused => !IsPaused;
}