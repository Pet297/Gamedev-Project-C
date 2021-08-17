using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectorScript : MonoBehaviour
{
    GameScoreScript gss;
    HealthPointsScript hps;
    PlayerInventoryScript pis;
    PlayerController pc;

    // Start is called before the first frame update
    void Start()
    {
        gss = gameObject.GetComponent<GameScoreScript>();
        hps = gameObject.GetComponent<HealthPointsScript>();
        pis = gameObject.GetComponent<PlayerInventoryScript>();
        pc = gameObject.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect(string item)
    {
        switch (item)
        {
            case "PV_100": gss.Increase(100); break;
            case "PV_200": gss.Increase(200); break;
            case "PV_500": gss.Increase(500); break;
            case "PV_1000": gss.Increase(1000); break;
            case "PV_2000": gss.Increase(2000); break;
            case "PV_3000": gss.Increase(3000); break;
            case "PV_5000": gss.Increase(5000); break;
            case "PV_10000": gss.Increase(10000); break;

            case "BO_H": hps.IncreaseMax(15); break;
            case "BO_J": pc.IncreaseJumpHeight(); break;
            case "BO_D": pc.IncreaseAttackMult(); break;

            case "CAT_1": gss.SaveCat(1); gss.Increase(2500); break;
            case "CAT_2": gss.SaveCat(2); gss.Increase(2500); break;
            case "CAT_3": gss.SaveCat(3); gss.Increase(2500); break;
            case "CAT_4": gss.SaveCat(4); gss.Increase(2500); break;

            default: pis.AddItem(item); break;
        }
    }
}
