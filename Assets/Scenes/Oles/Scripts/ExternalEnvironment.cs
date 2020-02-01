using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEnvironment : MonoBehaviour
{ 
    [SerializeField] GameObject planet;
    private RaycastHit hit;
    void Start()
    {
        planet = GameObject.FindGameObjectWithTag("Planet");
    }

    void Update()
    {
        List<GameObject> list = GetEffectedTiles();
        foreach(var edit in list)
        {
            Debug.Log(edit.gameObject.name);
        }


    }

    public virtual List<GameObject> GetEffectedTiles()
    {
        List<GameObject> list = new List<GameObject>();

       // Vector3 fwd = transform.TransformDirection(this.gameObject.transform.forward);
        Vector3 forward = planet.transform.position - this.transform.position;
        Debug.DrawRay(transform.position, forward, Color.green);
    
        if (Physics.Raycast(transform.position, forward, out hit))
        {
            Collider[] hitColliders = Physics.OverlapSphere(hit.point, 1);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (this.gameObject != hitColliders[i].gameObject)
                    list.Add(hitColliders[i].gameObject);
                i++;
            }
        }
        return list;
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(hit.point, 1);
    }
}