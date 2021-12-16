using System;

public class ProceduralGeneration
{
    public static ProceduralGeneration instance;

    private Random random;

    public void Initialize(long seed)
    {
        random = new Random((int)(seed % int.MaxValue));
    }
    
    public void Initialize()
    {
        Initialize(Save.instance.session.randomSeed);
    }

    public int Next()
    {
        return random.Next();
    }
    public void MoveToPositon()
    {
        throw new NotImplementedException();
    }
}
