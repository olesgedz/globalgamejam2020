using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] int randVal;
    [SerializeField] float timespeedProg = 20.0f;
    [SerializeField] float timespeedDisaster = 1.0f;
    [SerializeField] float timespeedDry = 1.0f;
    [SerializeField] float timespeedCold = 1.0f;

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
            if (this.gameObject != gobject & gobject.TryGetComponent<HexData>(out temp))
            {
                hexNeibours.Add(temp);
            }
        }
    }

    private void setTile()
    {
        active.SetActive(false);
        if (hexStatusState == HexState.Dead &
        active != stones)
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
                if (hexProgressState >= HexProgress.Birth)
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
        active.SetActive(true);
    }

    public void influeceNeibours()
    {
        if (hexProgressState >= HexProgress.StableBirth)
        {
            foreach (HexData hex in hexNeibours)
            {
                hex.updateProgress(addition / 20);
            }
        }
    }

    public void die()
    {
        this.setIdle(HexState.Dead);
    }

    public void live()
    {
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
        this.recountHex();
        this.checkHex();
    }

    private void recountHex()
    {
        if (hexStatusState == HexState.Alive)
        {
            waterBalance -= timespeedDry * Time.deltaTime;
            temperatureBalance -= timespeedCold * Time.deltaTime;
            addition = (float)(timespeedProg * Time.deltaTime);
            if (generalProgress <= 900)
                generalProgress += addition;
            hexProgressState = (HexProgress)((int)generalProgress / 100);
            this.setTile();
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
            this.setTile();
            return;
        }
        if (temperatureBalance >= 100 |
        temperatureBalance <= -100)
        {
            this.die();
            this.setTile();
            return;
        }
        if (waterBalance >= 100 |
        waterBalance <= -100)
        {
            this.die();
            this.setTile();
            return;
        }
        if (hexProgressState > HexProgress.Nothing)
        {
            this.live();
            return;
        }
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
        Random rand = new Random();
        randVal = Random.Range(0, 2);
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

    void Update()
    {
    }
};