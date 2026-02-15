using UnityEngine;
using System.Collections.Generic;

public class NumberGenerator : MonoBehaviour
{
    public static NumberGenerator Instance { get; private set; }

    [SerializeField] private Mesh[] digitMeshes = new Mesh[10];
    [SerializeField] private float spacing = 1.1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Mesh GenerateNumberMesh(int value)
    {
        string numberString = value.ToString();

        List<CombineInstance> combine = new List<CombineInstance>();

        int length = numberString.Length;

        float totalWidth = (length - 1) * spacing;
        float startOffset = -totalWidth * 0.5f;

        float offset = startOffset;

        for (int i = 0; i < length; i++)
        {
            int digit = numberString[i] - '0';

            if (digit < 0 || digit > 9) continue;

            CombineInstance ci = new CombineInstance();
            ci.mesh = digitMeshes[digit];
            ci.transform = Matrix4x4.TRS(
                new Vector3(offset, 0f, 0f),
                Quaternion.identity,
                Vector3.one
            );

            combine.Add(ci);

            offset += spacing;
        }

        Mesh finalMesh = new Mesh();
        finalMesh.CombineMeshes(combine.ToArray(), true, true);

        return finalMesh;
    }
}
