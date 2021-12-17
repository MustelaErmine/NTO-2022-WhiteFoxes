using System;

public class ProceduralGeneration
{
    public static ProceduralGeneration instance;

    private Random random;

    public ProceduralGeneration(long seed, uint step)
    {
        random = new Random((int)(seed % int.MaxValue));
        MoveToPositon(step);
    }

    public ProceduralGeneration(long seed) : this(seed, 0) {}
    
    public ProceduralGeneration() : this(Save.instance.session.randomSeed) {}

    public int Next()
    {
        Save.instance.session.randomGenerationsWas += 1;
        return random.Next();
    }
    void MoveToPositon(uint step)
    {
        for (uint i = 0; i < step; i++)
        {
            Next();
        }
    }
}
