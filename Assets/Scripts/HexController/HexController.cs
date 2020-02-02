using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexController : MonoBehaviour
{
    public HexData[] hexObjectsArray;
    public List<GameObject> effectedByMeteor;
    public List<GameObject> GetEffectedTiles;
    public ExternalEnvironment extr;
    public List<GameObject> rainlist;
    void Start()
    {
    }

    public void setObjectsArray(HexData[] hexArray)
    {
        hexObjectsArray = hexArray;
    }


    public void setlistRain(List<GameObject> listRain)
    {
        GetEffectedTiles = listRain;
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
        foreach (var hexobject in hexObjectsArray)
        {
            hexobject.influeceNeibours();
        }
        foreach (var hexobject in GetEffectedTiles)
        {
            GameObject temp;
            if (hexobject.TryGetComponent<GameObject>(out temp))
            {
                temp.GetComponent<HexData>().updateTemperature(4);
            }
        }
      extr.GetEffectedTiles();
       

    }
}
