public class ResultData
{
    public float timePlayed;
    public int wavesCompleted;
    public int sessionExperience;

    private static ResultData instance;

    public static void SetData(float timePlayed, int wavesCompleted, int experience)
    {
        instance = new ResultData
        {
            timePlayed = timePlayed,
            wavesCompleted = wavesCompleted,
            sessionExperience = experience
        };
        AnalyticsManager.Instance.RecordPlayTime(timePlayed);

    }

    public static ResultData GetData()
    {
        return instance ?? new ResultData();
    }
}
