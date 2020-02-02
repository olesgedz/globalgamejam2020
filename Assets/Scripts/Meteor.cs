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
        Collider[] hitColliders = Physics.OverlapSphere(collision.GetContact(1).point, 1);
        foreach (var col in hitColliders)
        {
            //hexController.effectedByMeteor.Add(col.gameObject);
            if (col.gameObject.TryGetComponent<HexData>(out HexData hexData))
            {
                hexData.live();
                hexData.setDisaster(DisasterState.Fire);
            }
        }
        Destroy(this.gameObject);    
    }
}
