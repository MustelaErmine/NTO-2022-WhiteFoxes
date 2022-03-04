using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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

    public int Engines
    {
        get => OfType(BlockType.Engine);
    }

    public Ship ()
    {
        blocks = new List<ShipBlock>();
    }

    public Ship (bool nn) : this()
    {
        blocks = new List<ShipBlock> {
            new ShipBlock(BlockType.Main, Vector3.zero, Vector3.zero, Vector3.zero, 1f, 0, -1) 
        };
    }

    public void AddBlock(BlockType type, Vector3 position, Vector3 offset, Vector3 rotation, float dest, int parent=-1)
    {
        blocks.Add(new ShipBlock(type, position, offset, rotation, dest, MaxNumber + 1, parent));
    }

    public BlockType[] DeleteBlock(int number)
    {
        Queue<int> queue = new Queue<int>();
        queue.Enqueue(number);
        List<BlockType> deleted = new List<BlockType>();
        while (queue.Count > 0)
        {
            int i = queue.Dequeue();
            deleted.Add(blocks.Where((ShipBlock block) => block.number == i).ToArray()[0].type);
            blocks.RemoveAll((ShipBlock block) => block.number == i);
            foreach (ShipBlock block in blocks)
            {
                if (block.parent == i)
                {
                    queue.Enqueue(block.number);
                }
            }
        }
        return deleted.ToArray();
    }

    public int OfType(BlockType type)
    {
        return blocks.Where((ShipBlock block) => block.type == type).ToArray().Length;
    } 

    public static (Mesh, Material) GetBlockDisplay(BlockType type)
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
    public bool CanBePlaced(Vector3 pos)
    {
        return !blocks.Any((ShipBlock block) => (block.Position - pos).magnitude < 0.1f);
    }
}

