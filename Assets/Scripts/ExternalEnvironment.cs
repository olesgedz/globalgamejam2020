using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEnvironment : MonoBehaviour
{ 
    [SerializeField] GameObject planet;
    private RaycastHit hit;
   [SerializeField] List<GameObject> hexcontroller;

    float timeLeft = 15;
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
    }

    void Update()
    {
        //      List<GameObject> list = GetEffectedTiles();
        GetEffectedTiles();
     timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            Object.Destroy(this.gameObject);
        }
    }

    public void GetEffectedTiles()
    {

       // Vector3 fwd = transform.TransformDirection(this.gameObject.transform.forward);

        Vector3 forward = planet.transform.position - this.transform.position;
        Debug.DrawRay(transform.position, forward, Color.green);
    
        if (Physics.Raycast(transform.position, forward, out hit))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 1);
            int i = 0;
            Debug.Log(hit.collider.gameObject.GetComponentInChildren<HexData>().gameObject.name);
            hit.collider.gameObject.GetComponentInChildren<HexData>().updateWater(7);
         
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 1);
    }

}