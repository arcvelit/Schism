using UnityEngine;

public class Altar : MonoBehaviour
{

    public int id;

    private bool filled;

    public GameObject man;

    private static string NOT_IN_INVENTORY_MESSAGE = "...";

    void Start()
    {

    }

    void OnMouseOver()
    {
        Vector3 diff = gameObject.transform.position - PlayerGlobal.Instance.GetTransform().position;
        if (diff.magnitude > 3f || filled)
        {
            OnMouseExit();
        }
        else 
        {
            UIManager.Instance.SetLookatAltar(this);
            UIManager.Instance.ShowAltarInteraction(id);
        }
    }

    void OnMouseExit()
    {
        UIManager.Instance.RemoveInteraction();
    }

    public void Interact()
    {
        if (!ProgressGlobal.Instance.TryDepositBookId(id)) {
            StartCoroutine(UIManager.Instance.GameMessage(NOT_IN_INVENTORY_MESSAGE, 0.7f, null));
            return;
        }

        MeshRenderer mesh = man.GetComponent<MeshRenderer>();
        mesh.enabled = true;

        PlayerSounds.Instance.PlayMonumentCollect();

        filled = true;
        Debug.Log("Placing at altar " + id);
    }
}