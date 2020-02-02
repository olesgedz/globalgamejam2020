using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    private Rigidbody rigidbody;
    [SerializeField] float speed;
    [SerializeField] HexController hexController;
    private Vector3 point;

    void Start()
    {
        target = GameObject.FindWithTag("Planet");
        rigidbody = this.GetComponent<Rigidbody>();
        Vector3 temp = (target.transform.position - this.transform.position).normalized * speed;
        //Debug.Log(temp);
        //Debug.Log(this.transform.position);


        rigidbody.AddForce(temp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider[] hitColliders = Physics.OverlapSphere(collision.GetContact(0).point, 1);
        point = collision.GetContact(0).point;
    
            //hexController.effectedByMeteor.Add(col.gameObject);
            HexData hexData;
            if (hitColliders[0].gameObject.TryGetComponent<HexData>(out hexData))
            { 
                hexData.live();
                //hexData.setDisaster(DisasterState.Fire);
            }
        Destroy(this.gameObject);    
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(point, 1);
    }
}
