using UnityEngine;

public class Collectible : MonoBehaviour
{

    private bool destroyed;

    void OnMouseOver()
    {
        if (!destroyed) {
            UIManager.Instance.SetLookatCollectible(this);
            UIManager.Instance.ShowInteraction(gameObject.tag);
        }
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }


    public void Collect()
    {
        if (!destroyed) {
            InventoryManager.Instance.AddToInventory(gameObject.tag); 
            Destroy(gameObject);
            destroyed = true;
        }
    }
}
