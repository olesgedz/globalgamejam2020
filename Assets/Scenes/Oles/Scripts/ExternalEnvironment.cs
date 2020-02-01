using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalEnvironment : MonoBehaviour
{ 
    [SerializeField] GameObject planet;

    void Start()
    {
        
    }

    void Update()
    {
        GetEffectedTiles();
    }

    public virtual List<GameObject> GetEffectedTiles()
    {
        List<GameObject> list = new List<GameObject>();
        RaycastHit hit;
        Vector3 fwd = transform.TransformDirection(this.gameObject.transform.forward);
        if (Physics.Raycast(transform.position, fwd, out hit))
            Debug.Log(hit.rigidbody.gameObject.name);
        return list;
    }
}
