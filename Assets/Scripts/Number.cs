using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Number : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] public MeshCollider meshCollider;

    [SerializeField] public int value = 0;

    private void Start()
    {
        Mesh mesh = NumberGenerator.Instance.GenerateNumberMesh(value);
        SetMesh(mesh);

    }

    public void SetMesh(Mesh newMesh)
    {
        meshFilter.mesh = newMesh;
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = newMesh;
        this.transform.rotation = Quaternion.Euler(30, 0, 0);
    }

}
