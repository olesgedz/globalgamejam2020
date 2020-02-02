using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField] GameObject meteor;
    // Start is called before the first frame update
    [SerializeField] GameObject camera;
    [SerializeField] GameObject target;

    void Start()
    {
        camera = GameObject.FindWithTag("MainCamera");
        target = GameObject.FindWithTag("Planet");
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
       // Vector3 pos = Random.insideUnitSphere * 100f;




        float radius = Mathf.Tan(Mathf.Deg2Rad * 60 / 2) * 50;
        Vector2 circle = Random.insideUnitCircle * radius;
        Vector3 pos = target.transform.position + -camera.transform.forward * 50 + target.transform.rotation * new Vector3(circle.x, circle.y);



        //pos.x = Mathf.Abs(pos.x);
        Instantiate(meteor, pos, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        StartCoroutine(SpawnMeteor());
    }
}
