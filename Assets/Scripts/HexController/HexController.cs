using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    public HexData[] hexObjectsArray;
    public List<GameObject> effectedByMeteor;
    void Start()
    {
    }

    public void setObjectsArray(HexData[] hexArray)
    {
        hexObjectsArray = hexArray;
    }

    public void UpdateScene(List<ExternalEnvironment> enviromentList = null)
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
