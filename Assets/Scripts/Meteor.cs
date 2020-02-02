using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    private Rigidbody rigidbody;
    [SerializeField] float speed;

    void Start()
    {
        target = GameObject.FindWithTag("Planet");
        rigidbody = this.GetComponent<Rigidbody>();
        Vector3 temp = (target.transform.position - this.transform.position).normalized * speed;
        //Debug.Log(temp);
        //Debug.Log(this.transform.position);
        //Debug.Log(target.transform.position);

        rigidbody.AddForce(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);    
    }
}
