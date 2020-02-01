using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetNeighbours : MonoBehaviour
{
    // Start is called before the first frame update

    void Start()
    {

    }
    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5);
        int i = 0;
        while (i < hitColliders.Length)
        {
            Debug.Log(hitColliders[i].gameObject.name);
            i++;
        }

    }
}
