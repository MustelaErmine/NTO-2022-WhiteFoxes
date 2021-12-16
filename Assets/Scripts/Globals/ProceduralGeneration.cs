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
        Initialize(Save.instance.randomSeed);
    }

    public int Next()
    {
        return random.Next();
    }
}
