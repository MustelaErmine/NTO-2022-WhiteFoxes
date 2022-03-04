using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDisplayings : MonoBehaviour
{
    [SerializeField] BlockType[] _typeCodes;
    [SerializeField] Mesh[] _typeMeshes;
    [SerializeField] Material[] _typeTextures;
    public static BlockType[] typeCodes;
    public static Mesh[] typeMeshes;
    public static Material[] typeTextures;

    private void Awake()
    {
        typeCodes = _typeCodes;
        typeMeshes = _typeMeshes;
        typeTextures = _typeTextures;
    }
}
