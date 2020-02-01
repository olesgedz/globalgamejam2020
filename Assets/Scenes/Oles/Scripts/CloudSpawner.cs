using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] GameObject cloud;

    void Start()
    {
        StartCoroutine(SpawnCloud());
    }

    void Update()
    {
        
    }

   IEnumerator SpawnCloud()
    {
        Vector3 pos = Random.onUnitSphere * 5;
        GameObject parent = GameObject.FindWithTag("Planet");

        Vector3 targetDir = pos - parent.transform.position;
        float angle = Vector3.Angle(targetDir, parent.transform.forward);

        GameObject newCloud = Instantiate(cloud, pos, Quaternion.Euler(angle, angle, angle)) as GameObject;
       
        newCloud.transform.parent = parent.transform;
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnCloud());
    }
}
