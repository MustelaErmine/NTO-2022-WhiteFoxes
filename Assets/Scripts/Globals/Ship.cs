using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class Ship
{
    public List<ShipBlock> blocks;

    private int MaxNumber
    {
        get
        {
            int max = 0;
            foreach(ShipBlock block in blocks)
            {
                max = Mathf.Max(block.number, max);
            }
            return max;
        }
    }

    public Ship ()
    {
        blocks = new List<ShipBlock> {
            new ShipBlock(BlockType.Main, Vector3.zero, Vector3.zero, Vector3.zero, 1f, 0, -1) 
        };
    }

    public void AddBlock(BlockType type, Vector3 position, Vector3 offset, Vector3 rotation, float dest, int parent=-1)
    {
        blocks.Add(new ShipBlock(type, position, offset, rotation, dest, MaxNumber + 1, parent));
    }

    public void DeleteBlock(int number)
    {
        blocks.RemoveAll((ShipBlock block) => block.number == number);
        foreach(ShipBlock block in blocks)
        {
            if (block.parent == number)
            {
                DeleteBlock(block.number);
            } 
        }
    }

    public static (Mesh, Texture) GetBlockDisplay(BlockType type)
    {
        int pos = 0;
        for (int i = 0; i < BlockDisplayings.typeCodes.Length; i++)
        {
            if (BlockDisplayings.typeCodes[i].Equals(type)) {
                pos = i;
            }
        }
        return (BlockDisplayings.typeMeshes[pos], BlockDisplayings.typeTextures[pos]);
    }
}

