using UnityEngine;

public class Collectible : MonoBehaviour
{

    private bool destroyed;

    void OnMouseDown()
    {
        // Trigger collection logic
        Collect();
        OnMouseExit();
    }

    void OnMouseOver()
    {
        if (!destroyed)
        UIManager.Instance.ShowInteraction(gameObject.tag);
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }


    void Collect()
    {
        InventoryManager.Instance.AddToInventory(gameObject.tag); 
        Destroy(gameObject);
        destroyed = true;
    }
}
