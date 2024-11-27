using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    bool torchLitty = false;

    float currentPercentage => InventoryManager.Instance.lightPercentage;

    bool hasBatteryLeft => InventoryManager.Instance.HasBatteryLeft();

    Light torch;

    void Start()
    {
        torch = GetComponent<Light>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (torchLitty) // Toggle off
            {
                PlayerSounds.Instance.PlayTorchClick();
                torchLitty = false;
                torch.enabled = torchLitty;
            }
            else if (hasBatteryLeft) // Toggle on condition of battery left
            {
                PlayerSounds.Instance.PlayTorchClick();
                torchLitty = true;
                torch.enabled = torchLitty;
            }
        }

        if (torchLitty)
        {
            if (hasBatteryLeft)
            {
                InventoryManager.Instance.TickOffBattery();
            }
            else
            {
                torchLitty = false;
                torch.enabled = torchLitty;
            } 
        }
        
    }
}
