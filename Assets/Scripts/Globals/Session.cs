using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Session
{
    public long randomSeed = 37613761, gold = 0;
    public int step = 0, years = 0;
    public uint randomGenerationsWas = 0;
    public uint randomGenerationsWasInOldStep = 0;

    public Dictionary<Skills, int> skills;
    public List<ItemType> inventory;
    public List<Item> cases;
    public List<ShipDetail> shipDetails;

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
            {Skills.Monitor, 2}
        };
        inventory = new List<ItemType> {};
        shipDetails = new List<ShipDetail>();
        cases = new List<Item>();
    }

    public void NextStep()
    {
        randomGenerationsWasInOldStep = randomGenerationsWas;
        step += 1;
        years += 10;
        Save.Keep();
    }
}

