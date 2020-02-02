using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] Vector3 Axis;
    [SerializeField] float RotationSpeed;
    // Start is called before the first frame update

    private void OnEnable()
    {
        if (Axis == Vector3.zero)
        {
            Axis = transform.up;
        }
        else
        {
            Axis = Axis.normalized;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Axis, RotationSpeed * Time.deltaTime);
    }
}
