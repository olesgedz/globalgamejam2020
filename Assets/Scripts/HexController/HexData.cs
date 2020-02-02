using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexState {Dead, Alive};
public enum DisasterState {None, Dry, Fire};
public enum HexProgress {Nothing = 0, Birth = 1 , StableBirth = 2 , Animals = 3, StableAnimals = 4, Tribe = 5, Village = 6, SmallCity = 7, Megapolice = 8, Winner = 9};

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

    /*
    //Depth-first search
    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        foreach(Transform child in aParent)
        {
            if(child.name == aName )
                return child;
            var result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }
    */
}

public class HexData : MonoBehaviour
{
    List<HexData> hexNeibours = new List<HexData>();
    public HexState hexStatusState = HexState.Dead;
    public DisasterState disasterState;
    public HexProgress hexProgressState;

    MeshRenderer model;
    public GameObject stones;
    public GameObject desert;
    public GameObject clay;
    public GameObject grass;
    public GameObject active;
    [SerializeField] float waterBalance;
    [SerializeField] float temperatureBalance;
    [SerializeField] float disasterProgress;
    [SerializeField] float generalProgress;
    [SerializeField] float addition;

    private void setIdle(HexState newHexState)
    {
        this.hexStatusState = newHexState;
        hexProgressState = HexProgress.Nothing;
        waterBalance = 0;
        temperatureBalance = 0;
        disasterProgress = 0;
        disasterState = DisasterState.None;
    }

    private void setIdle()
    {
        hexProgressState = HexProgress.Nothing;
        waterBalance = 0;
        temperatureBalance = 0;
        disasterProgress = 0;
        disasterState = DisasterState.None;
    }

    private void getNeibours()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 1);
        foreach (var gobject in hitColliders)
        {
            if (this.gameObject != gobject)
            {
                hexNeibours.Add(gobject.GetComponent<HexData>());
            }
        }
    }

    private void setTile()
    {
        active.SetActive(false);
        if (hexStatusState == HexState.Dead &
        active != stones)
        {
            active = stones;
        }
        else
        {
            if (disasterState == DisasterState.Dry)
                active = desert;
            else
            {
                if (hexProgressState >= HexProgress.Birth)
                {
                    active = grass;
                }
                else
                {
                    active = clay;
                }
            }
        }
        active.SetActive(true);
    }

    public void influeceNeibours()
    {
        if (hexProgressState >= HexProgress.StableBirth)
        {
            foreach (HexData hex in hexNeibours)
            {
                if (hex)
                {
                    if (hex.isAlive())
                        hex.updateProgress(addition / 2);
                    else
                        hex.live();
                }
            }
        }
    }

    public void die()
    {
        this.setIdle(HexState.Dead);
    }

    public void live()
    {
        Debug.Log("Nothing left");
        this.setIdle(HexState.Alive);
        this.hexProgressState = HexProgress.Birth;
        //model.material.SetColor("_Color", new Color32(162, 167, 160, 255));
    }

    public HexState getState()
    {
        return hexStatusState;
    }

    public void updateHex()
    {
        if (hexStatusState == HexState.Dead)
        {
            return ;
        }
        if (disasterState == DisasterState.Dry 
        | disasterState == DisasterState.Fire)
        {
            disasterProgress += (float)(2.0 * Time.deltaTime);
        }
        if (disasterProgress >= 100)
        {
            this.die();
            return ;
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
        addition = (float)(20 * Time.deltaTime);
        if (generalProgress <= 900)
            generalProgress += addition;
        hexProgressState = (HexProgress)((int)generalProgress / 100);
        this.setTile();
    }

    public void updateProgress(float progress)
    {
        generalProgress += progress;
    }

    public void updateTemperature(float temperature)
    {
        temperatureBalance += temperature;
    }

    public void updateWater(float water)
    {
        waterBalance += water;
    }

    public void setDisaster(DisasterState disaster)
    {
        this.disasterState = disaster;
    }

    public bool isAlive()
    {
        return hexStatusState == HexState.Alive;
    }

    void Start()
    { 
        this.setIdle();
        this.getNeibours();
        grass = transform.FindDeepChild("HexTop_River").gameObject;
        desert = transform.FindDeepChild("HexTop_Desert").gameObject; 
        clay = transform.FindDeepChild("HexTop_ClayGround").gameObject;
        stones = transform.FindDeepChild("HexTop_StoneGround").gameObject;
        grass.SetActive(false);
        desert.SetActive(false);
        clay.SetActive(false);
        stones.SetActive(false);
        active = clay;
        active.SetActive(true);
        Debug.Log(model.material.name);
    }
    
    void Update()
    {
    }
};