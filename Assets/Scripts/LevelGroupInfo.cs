public readonly struct LevelGroupInfo
{
    public readonly bool IsAllLevelsBeaten;
    public readonly float CompletedPercent;

    public LevelGroupInfo(bool isAllLevelsBeaten, float completedPercent)
    {
        IsAllLevelsBeaten = isAllLevelsBeaten;
        CompletedPercent = completedPercent;
    }
}