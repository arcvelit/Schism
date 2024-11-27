using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class SuccessPage : MonoBehaviour
{
    void Start()
    {
        UIDocument doc = GetComponent<UIDocument>();
        Button button = doc.rootVisualElement.Q("MenuButton") as Button;
        button.RegisterCallback<ClickEvent>(evt =>
        {
            SceneManager.LoadScene("MainMenu");
        });
        
    }
}
