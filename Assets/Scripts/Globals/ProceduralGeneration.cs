public class ProceduralGeneration
{
    public static ProceduralGeneration instance;

    private System.Random random;

    public void Initialize(long seed)
    {
        //Random.InitState();
        random = new System.Random((int)(seed % int.MaxValue));
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
