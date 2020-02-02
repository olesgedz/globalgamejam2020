using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] HexController hexController;
    List<ExternalEnvironment> externalList;
    public HexData[] hexObjectsArray;
    public Canvas can;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        hexObjectsArray = Resources.FindObjectsOfTypeAll(typeof(HexData)) as HexData[];
        hexController = FindObjectOfType<HexController>();
        hexController.setObjectsArray(hexObjectsArray);
    }

    // Update is called once per frame
    void Update()
    {
        hexController.UpdateScene();
    }

     public  void winGameOver()
    {
        can.gameObject.SetActive(true);
    }
}
