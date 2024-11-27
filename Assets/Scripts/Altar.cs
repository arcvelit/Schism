using UnityEngine;

public class Altar : MonoBehaviour
{

    public int id;

    private bool filled;

    public GameObject man;

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
        if (!ProgressGlobal.Instance.FoundManuscriptId(id)) {
            return;
        }

        MeshRenderer mesh = man.GetComponent<MeshRenderer>();
        mesh.enabled = true;

        PlayerSounds.Instance.PlayMonumentCollect();

        filled = true;
        Debug.Log("Placing at altar " + id);
    }
}
