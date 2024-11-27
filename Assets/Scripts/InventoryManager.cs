using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public float LIGHT_CAP_SEC;
    public float lightTime;

    public int lightPercentage => (int)(100 * (lightTime/LIGHT_CAP_SEC)); 

    public static int PUZZLE_OBJECTIVE = 10;
    public static int BATTERY_CAP = 99;
    public int batteries = 0;
    public int manuscripts = 0;

    public bool ACHIEVED_OBJECTIVE => manuscripts == PUZZLE_OBJECTIVE;

    void Start()
    {
        lightTime = LIGHT_CAP_SEC;
    }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddToInventory(string tag)
    {
        Debug.Log("Picked up "+ tag);

        switch (tag)
        {
            case "Battery":
            if (batteries < BATTERY_CAP) IncrementBatteries(); 

            break;

            case "Manuscript":
            if (manuscripts < PUZZLE_OBJECTIVE) IncrementManuscripts();

            break;
        }
    }

    public void ConsumeBattery()
    {
        if (batteries > 0) 
        {
            Debug.Log("Consume battery");
            lightTime = LIGHT_CAP_SEC;
            DecrementBatteries();
        } else 
        {
            lightTime = 0;
        }
    }

    public void TickOffBattery()
    {
        lightTime -= Time.deltaTime;

        if (lightTime < 0)
        {
            ConsumeBattery();
        }
        else 
        {
            UIManager.Instance.UpdateTorchLife(lightPercentage);
        }
    }


    public bool HasBatteryLeft()
    {
        return batteries > 0 || lightTime > 0;
    }

    void DecrementBatteries()
    {
        batteries--;
        UIManager.Instance.UpdateBatteries(batteries);

    }

    void IncrementBatteries()
    {
        batteries++;
        UIManager.Instance.UpdateBatteries(batteries);
    }

    void IncrementManuscripts()
    {
        manuscripts++;
        UIManager.Instance.UpdateManuscripts(manuscripts);
    }
    

}
