using System.Collections.Generic;

public class Session
{
    public long randomSeed = 37613761;
    public int step = 0;
    public uint randomGenerationsWas = 0;
    public uint randomGenerationsWasInOldStep = 0;

    public Dictionary<Skills, int> skills;

    public Session()
    {
        step = -1;
        randomSeed = ((System.DateTimeOffset)System.DateTime.Now).ToUnixTimeSeconds();
        ProceduralGeneration.instance = new ProceduralGeneration(randomSeed, randomGenerationsWasInOldStep);
        skills = new Dictionary<Skills, int>() { 
            {Skills.HyperDriveForce, 2}, 
            {Skills.HyperDriveRecharge, 40},
            {Skills.TimeSlowCapacity, 7},
            {Skills.TimeSpeedCapacity, 10},
            {Skills.Monitor, 0}
        };
    }

    public void NextStep()
    {
        randomGenerationsWasInOldStep = randomGenerationsWas;
        step += 1;
    }
}

