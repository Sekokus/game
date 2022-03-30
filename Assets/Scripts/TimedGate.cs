public class TimedGate
{
    private float _timeRemained;

    public bool IsLocked { get; private set; }

    public bool IsUnlocked => !IsLocked;

    public void Lock()
    {
        IsLocked = true;
    }

    public void Unlock(float time)
    {
        _timeRemained = time;
    }

    public void Step(float timeStep)
    {
        if (_timeRemained <= 0)
        {
            return;
        }

        _timeRemained -= timeStep;
        if (_timeRemained > 0)
        {
            return;
        }

        _timeRemained = 0;
        IsLocked = false;
    }
}
