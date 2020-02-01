using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexState {Dead, Alive};
public enum DisasterState {None, Dry, Fire};
public enum HexProgress {Nothing = 0, Birth = 1 , StableBirth = 2 , Animals = 3, StableAnimals = 4, Tribe = 5, Village = 6, SmallCity = 7, Megapolice = 8, Winner = 9};

public class HexData : MonoBehaviour
{
    List<HexData> hexNeibours = new List<HexData>();
    public HexState hexStatusState = HexState.Dead;
    public DisasterState disasterState;
    public HexProgress hexProgressState;

        MeshRenderer model;
        [SerializeField] float waterBalance;
        [SerializeField] float temperatureBalance;
        [SerializeField] float disasterProgress;
        [SerializeField] float generalProgress;
        [SerializeField] float addition;
        [SerializeField] Color startColorLife;
        [SerializeField] Color endColorLife;

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

    private void updateColor()
    {
        float prog;
        if (generalProgress > 200)
            prog = 200;
        else
            prog = generalProgress;
        model.material.SetColor("_Color", Color32.Lerp(startColorLife, endColorLife, Mathf.PingPong(prog / 200, 1)));
    }

    public void influeceNeibours()
    {
        if (hexProgressState >= HexProgress.StableBirth)
        {
            foreach (HexData hex in hexNeibours)
            {
                if (hex.isAlive())
                    hex.updateProgress(addition / 2);
                else
                    hex.live();
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
        model.material.SetColor("_Color", new Color32(162, 167, 160, 255));
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
        startColorLife = new Color32(162, 167, 160, 255);
        endColorLife = new Color32(165, 209, 67, 255);
        model = gameObject.GetComponentInParent<MeshRenderer>();
        model.material.SetColor("_Color", new Color32(162, 167, 160, 255));
        Debug.Log(model.material.name);
    }
    
    void Update()
    {
        this.updateColor();
    }
};