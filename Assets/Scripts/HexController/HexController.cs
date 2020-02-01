using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    public HexData[] hexObjectsArray;
    void Start()
    {
    }

    public void setObjectsArray(HexData[] hexArray)
    {
        hexObjectsArray = hexArray;
    }

    public void UpdateScene(List<ExternalEnvironment> enviromentList)
    {
        foreach (var hexobject in hexObjectsArray)
        {
            hexobject.updateHex();
        }
        foreach (var hexobject in hexObjectsArray)
        {
            hexobject.influeceNeibours();
        }
    }
}
