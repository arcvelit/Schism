using UnityEngine;

public class Collectible : MonoBehaviour
{
    private bool destroyed;

    public int id;

    void OnMouseOver()
    {
        Vector3 diff = gameObject.transform.position - PlayerGlobal.Instance.GetTransform().position;
        if (diff.magnitude > 2.5f) 
        {
            OnMouseExit();
        }            
        else if (!destroyed) {
            UIManager.Instance.SetLookatCollectible(this);
            UIManager.Instance.ShowCollectibleInteraction();
        }
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }


    public void Collect()
    {
        if (!destroyed) {

            if (gameObject.tag == "Battery") PlayerSounds.Instance.PlayItemTake();
            if (gameObject.tag == "Manuscript") {
                PlayerSounds.Instance.PlayBookTake();
                ProgressGlobal.Instance.CollectBookId(id);
                UIManager.Instance.PerformScrollViewEnter(id);
                Debug.Log("Collected book " + id);
            }

            UIManager.Instance.RemoveInteraction();
            InventoryManager.Instance.AddToInventory(gameObject.tag); 
            Destroy(gameObject);
            destroyed = true;
        }
    }
}
