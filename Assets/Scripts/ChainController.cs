using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainController : MonoBehaviour
{
    [SerializeField] List<Number> pickUps;
    [SerializeField] float zOffset = 1.5f;
    [SerializeField] float smoothTime = 0.2f;
    [SerializeField] float rotationSmooth = 8f;

    private float xVelocity;

    private void Start()
    {
        
    }

    public void Follow(Transform player)
    {
        Transform target = player.transform;

        for (int i = 0; i < pickUps.Count; i++)
        {
            Number p = pickUps[i];
            xVelocity = 0;

            float targetX = Mathf.SmoothDamp(
                p.transform.position.x,
                target.position.x,
                ref xVelocity,
                smoothTime 
            );

            float targetY = player.transform.position.y; 
            float targetZ = target.position.z - zOffset; 

            p.transform.position = new Vector3(targetX, targetY, targetZ);
            Quaternion targetRot = Quaternion.Euler(30,target.rotation.y,target.rotation.z);
            p.transform.rotation = Quaternion.Slerp(
                p.transform.rotation,
                targetRot,
                rotationSmooth * Time.deltaTime
            );
            target = p.transform;
        }
    }


    private int activeNum = 0;

    public void AddtoChain(int num)
    {
        activeNum = num-1;
        RebuildChain();
        
    }

    void RebuildChain()
    {

        for (int i = 0; i < pickUps.Count; i++)
        {
            if (i < activeNum)
            {
                pickUps[i].gameObject.SetActive(true);
          
                int value = activeNum - i; 
                pickUps[i].value = value;

                Mesh mesh = NumberGenerator.Instance.GenerateNumberMesh(value);
                pickUps[i].SetMesh(mesh);
                pickUps[i].meshCollider.enabled = false;
            }
            else
            {
                pickUps[i].gameObject.SetActive(false);
            }
        }
    }


}
