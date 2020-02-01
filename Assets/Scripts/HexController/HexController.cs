using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    public HexData[] hexObjectsArray;
    void Start()
    {
        hexObjectsArray = Resources.FindObjectsOfTypeAll(typeof(HexData)) as HexData[];
        hexObjectsArray[0].live();
    }

    void UpdateScene(List<GameObject> hexNeibours)
    {
        foreach (var hexobject in hexObjectsArray)
        {
            Debug.Log(hexobject.hexStatusState);
            hexobject.updateHex();
            hexobject.influeceNeibours();
        }
    }
}
