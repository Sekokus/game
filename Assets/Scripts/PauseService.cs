using System;
using System.Collections.Generic;
using System.Linq;

public class PauseService
{
    private readonly Dictionary<PauseSource, int> _pauseSources = new Dictionary<PauseSource, int>();

    private readonly Dictionary<PauseSource, PauseObserver> _pauseObservers =
        new Dictionary<PauseSource, PauseObserver>();

    public PauseSource StrongestSource { get; private set; }

    public PauseService()
    {
        foreach (var value in Enum.GetValues(typeof(PauseSource)))
        {
            var source = (PauseSource)value;
            _pauseSources[source] = 0;
            _pauseObservers[source] = new PauseObserver(this, source);
        }
    }

    public PauseObserver GetObserver(PauseSource threshold)
    {
        return _pauseObservers[threshold];
    }

    public void Pause(PauseSource source)
    {
        _pauseSources[source]++;
        RecalculateStrongestPauseSource();
    }

    public void Unpause(PauseSource source)
    {
        _pauseSources[source]--;
        RecalculateStrongestPauseSource();
    }

    private void RecalculateStrongestPauseSource()
    {
        StrongestSource = (PauseSource)_pauseSources
            .Where(pair => pair.Value > 0)
            .Select(pair => (int)pair.Key)
            .DefaultIfEmpty()
            .Max();
    }
}