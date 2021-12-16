public class Session
{
    public long randomSeed = 37613761;
    public uint step = 0;
    public uint randomGenerationsWas = 0;

    public Session()
    {
        randomSeed = ((System.DateTimeOffset)System.DateTime.Now).ToUnixTimeSeconds();
        step = 0;
    }
}

