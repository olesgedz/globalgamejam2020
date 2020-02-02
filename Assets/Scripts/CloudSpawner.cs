using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] GameObject cloud;
    private GameObject parent;
    void Start()
    {
        parent = GameObject.FindWithTag("Planet");
        StartCoroutine(SpawnCloud());
    }

    void Update()
    {
        
    }

   IEnumerator SpawnCloud()
    {
        //Vector3 pos = Random.onUnitSphere * 10;
        //Debug.Log(pos);


        //Vector3 targetDir = parent.transform.position - pos;
        //float angleX = 360 - Vector3.Angle(targetDir, parent.transform.right);
        //float angleY = 360 - Vector3.Angle(targetDir, parent.transform.up);
        //float angleZ = 360 - Vector3.Angle(targetDir, parent.transform.forward);
        //GameObject newCloud = Instantiate(cloud, pos + parent.transform.position, Quaternion.Euler(angleX, angleY, 0)) as GameObject;


        Vector3 pos = RandomCircle(parent.transform.position, 20f);
        Quaternion rot = Quaternion.FromToRotation(cloud.transform.right, parent.transform.position - pos);
        GameObject newCloud =  Instantiate(cloud, pos, rot) as GameObject;
        //newCloud.transform.LookAt(newCloud.transform, parent.transform.position);
        newCloud.transform.SetParent(parent.transform);

        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnCloud());
    }

    Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}
