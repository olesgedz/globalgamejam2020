using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum HexState { Dead, Alive };
public enum DisasterState { None, Dry, Fire };
public enum HexProgress { Nothing = 0, Birth = 1, StableBirth = 2, Animals = 3, StableAnimals = 4, Tribe = 5, Village = 6, SmallCity = 7, Megapolice = 8, Winner = 9 };

public static class TransformDeepChildExtension
{
    //Breadth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        Queue<Transform> queue = new Queue<Transform>();
        queue.Enqueue(aParent);
        while (queue.Count > 0)
        {
            var c = queue.Dequeue();
            if (c.name == aName)
                return c;
            foreach (Transform t in c)
                queue.Enqueue(t);
        }
        return null;
    }
}
public class HexData : MonoBehaviour
{
    [SerializeField] List<HexData> hexNeibours = new List<HexData>();
    public HexState hexStatusState = HexState.Dead;
    public DisasterState disasterState;
    public HexProgress hexProgressState;
    MeshRenderer model;
    public GameObject stones;
    public GameObject desert;
    public GameObject clay;
    public GameObject grass;
    public GameObject active;
    public GameObject sbirth;
    public GameObject animal;
    public GameObject sanimal;
    public GameObject tribe;
    public GameObject village;
    public GameObject smallcity;
    public GameObject megapolice;
    public GameObject activecontent;
    [SerializeField] float waterBalance;
    [SerializeField] float temperatureBalance;
    [SerializeField] float disasterProgress;
    [SerializeField] float generalProgress;
    [SerializeField] float addition;
    [SerializeField] int randVal;
    [SerializeField] float timespeedProg = 5.0f;
    [SerializeField] float timespeedDisaster = 1.0f;
    [SerializeField] float timespeedDry = 0.5f;
    [SerializeField] float timespeedCold = 0.5f;
   // public GameObject gmc;

    private GameObject sun;
    private void setIdle(HexState newHexState)
    {
        this.hexStatusState = newHexState;
        hexProgressState = HexProgress.Birth;
        waterBalance = 0;
        temperatureBalance = 0;
        disasterProgress = 0;
        disasterState = DisasterState.None;
    }

    private void setIdle()
    {
        hexStatusState = HexState.Dead;
        hexProgressState = HexProgress.Nothing;
        waterBalance = 0;
        temperatureBalance = 0;
        disasterProgress = 0;
        disasterState = DisasterState.None;
    }

    private void getNeibours()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 0.5f);
        foreach (var gobject in hitColliders)
        {
            Debug.Log(gobject.gameObject.name);

            HexData temp;
            if (gobject.TryGetComponent<HexData>(out temp))
            {
                if (temp != this.gameObject)
                    hexNeibours.Add(temp);
            }
        }
    }

    private void setTile()
    {
        if (active)
            active.SetActive(false);
        if (hexStatusState == HexState.Dead)
        {
            if (randVal == 1)
            {
                active = clay;
            }
            else
            {
                active = stones;
            }
        }
        else
        {
            if (disasterState == DisasterState.Dry)
                active = desert;
            else
            {
                if (hexProgressState > HexProgress.Birth)
                    active = grass;
                else
                {
                    if (randVal == 1)
                    {
                        active = clay;
                    }
                    else
                    {
                        active = stones;
                    }
                }

            }
        }
        if (active)
            active.SetActive(true);
    }

    public void influeceNeibours()
    {
        if (hexProgressState >= HexProgress.StableBirth)
        {
            foreach (HexData hex in hexNeibours)
            {
                hex.updateProgress(addition * Time.deltaTime);
            }
        }
   }

    public void die()
    {
        this.setIdle(HexState.Dead);
        generalProgress = 0;
    }

    public void live()
    {
        this.setIdle(HexState.Alive);
        generalProgress = 101;
        Debug.Log("I'm ALIVE");
    }

    public HexState getState()
    {
        return hexStatusState;
    }

    public void updateHex()
    {
        this.recountHex();
        this.checkHex();
        this.setTile();
        this.updateContent();
        if (hexProgressState == HexProgress.Winner)
        {
            GameController.instance.winGameOver();
        }
    }

    private void recountHex()
    {
        if (this.hexStatusState == HexState.Alive)
        {
            waterBalance -= timespeedDry * Time.deltaTime;
            temperatureBalance -= timespeedCold * Time.deltaTime;
            addition = (timespeedProg / ((int)hexProgressState + 1) - Math.Abs(temperatureBalance)) * Time.deltaTime;
            if (generalProgress <= 900)
                generalProgress += addition;
            if (generalProgress <= 0)
                generalProgress = 0;
            hexProgressState = (HexProgress)(((int)generalProgress / 100));
        }
    }

    private void checkHex()
    {
        if (disasterState == DisasterState.Dry
        | disasterState == DisasterState.Fire)
        {
            disasterProgress += (float)(timespeedDisaster * Time.deltaTime);
        }
        if (disasterProgress >= 100)
        {
            this.die();
            return;
        }
        if (temperatureBalance >= 100 |
        temperatureBalance <= -100)
        {
            this.die();
            return;
        }
        if (waterBalance >= 100 |
        waterBalance <= -100)
        {
            this.die();
            return;
        }
        if (generalProgress >= 100 & hexStatusState == HexState.Dead)
        {
            this.live();
            return;
        }
    }

    public void updateProgress(float progress)
    {
        this.generalProgress += progress * Time.deltaTime;
    }

   public void updateTemperature(float temperature)
    {
         Debug.Log(temperature);
    
        this.temperatureBalance += temperature * Time.deltaTime;
    }

    public void updateWater(float water)
    {
        this.waterBalance += water * Time.deltaTime;
    }

    public void setDisaster(DisasterState disaster)
    {
        this.disasterState = disaster;
    }

    public bool isAlive()
    {
        return hexStatusState == HexState.Alive;
    }

    private void updateContent()
    {
        if (activecontent)
            activecontent.SetActive(false);
        switch (hexProgressState)
        {
            case HexProgress.Nothing:
                activecontent = null;
                break;
            case HexProgress.Birth:
                activecontent = null;
                break;
            case HexProgress.StableBirth:
                activecontent = sbirth;
                break;
            case HexProgress.Animals:
                activecontent = animal;
                break;
            case HexProgress.StableAnimals:
                activecontent = sanimal;
                break;
            case HexProgress.Tribe:
                activecontent = tribe;
                break;
            case HexProgress.Village:
                activecontent = village;
                break;
            case HexProgress.SmallCity:
                activecontent = smallcity;
                break;
            case HexProgress.Megapolice:
                activecontent = megapolice;
                break;
            case HexProgress.Winner:
                break;
        }
        if (activecontent)
            activecontent.SetActive(true);
    }

    void Start()
    {
        this.setIdle();
        this.getNeibours();
        sun = GameObject.FindWithTag("Sun");
        grass = transform.FindDeepChild("HexTop_River").gameObject;
        desert = transform.FindDeepChild("HexTop_Desert").gameObject;
        clay = transform.FindDeepChild("HexTop_ClayGround").gameObject;
        stones = transform.FindDeepChild("HexTop_StoneGround").gameObject;

        sbirth = transform.FindDeepChild("hex_tile_plant1").gameObject;
        animal = transform.FindDeepChild("hex_tile_trees1").gameObject;
        sanimal = transform.FindDeepChild("hex_tile_animals2").gameObject;
        tribe = transform.FindDeepChild("hex_tile_tribe1").gameObject;
        village = transform.FindDeepChild("hex_tile_vilage2").gameObject;
        transform.FindDeepChild("hex_tile_Small_town1").gameObject.SetActive(false);
        smallcity = transform.FindDeepChild("hex_tile_Middle_town1").gameObject;
        megapolice = transform.FindDeepChild("hex_tile_Megapolis").gameObject;
        activecontent = null;
        grass.SetActive(false);
        desert.SetActive(false);
        clay.SetActive(false);
        stones.SetActive(false);

        sbirth.SetActive(false);
        animal.SetActive(false);
        sanimal.SetActive(false);
        tribe.SetActive(false);
        village.SetActive(false);
        smallcity.SetActive(false);
        megapolice.SetActive(false);
        UnityEngine.Random rand = new UnityEngine.Random();
        randVal = UnityEngine.Random.Range(0, 2);
        if (randVal == 1)
        {
            active = clay;
        }
        else
        {
            active = stones;
        }
        active.SetActive(true);


    }


float GetIllumination()
    {
        //this.gameobject.trasnform.postion - sun
        //GameObject tile = this.gameObject;//this.GetComponentInParent<GameObject>();
        //float Scalar = Vector3.Dot(sun.transform.rotation.eulerAngles, this.transform.rotation.eulerAngles);
        //Debug.Log(Scalar);
        //return Scalar;
        // float Scalar = sun.transform.rotation * this.transform.rotation;
        //Vector3 length =  sun.transform.position - tile.transform.position;
        //float angle = Vector3.Angle(length, transform.forward);
        //Debug.Log(angle);
        //return Mathf.Cos(angle);

        if (Vector3.Distance(this.transform.position, sun.transform.position) < 26)
        {
            if (hexStatusState == HexState.Alive)
                this.updateTemperature(5);
        }
        /*
        // Vector3 fwd = transform.TransformDirection(this.gameObject.transform.forward);
        Vector3 forward = sun.transform.position - this.transform.position;
        Debug.DrawRay(transform.position, forward, Color.green);

        if (Physics.Raycast(transform.position, forward, out hit))
        {
            HexData outData;
            if (TryGetComponent<HexData>(out outData))
            {
                outData.updateTemperature(5);
            }
        }*/

        return 0;
    }


   
    void Update()
    {
       GetIllumination();

    }
}


