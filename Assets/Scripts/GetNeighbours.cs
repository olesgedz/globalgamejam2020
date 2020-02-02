using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNeighbours : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }

    List<GameObject> GetGetNeighbours()
    { 
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
        int i = 0;
        List<GameObject> list = new List<GameObject>();
        while (i < hitColliders.Length)
        {
            if (this.gameObject != hitColliders[i].gameObject)
                list.Add(hitColliders[i].gameObject);
            i++;
        }
        return list;
    }

    void Update()
    {
        List<GameObject> list = GetGetNeighbours();
        foreach(GameObject obj in list)
        {
            Debug.Log(obj.name);
        }

    }

}
