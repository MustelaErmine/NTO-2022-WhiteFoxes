using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDisplayings : MonoBehaviour
{
    [SerializeField] BlockType[] _typeCodes;
    [SerializeField] Mesh[] _typeMeshes;
    [SerializeField] Texture[] _typeTextures;
    public static BlockType[] typeCodes;
    public static Mesh[] typeMeshes;
    public static Texture[] typeTextures;

    private void Start()
    {
        typeCodes = _typeCodes;
        typeMeshes = _typeMeshes;
        typeTextures = _typeTextures;
    }
}
