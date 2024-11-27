using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class ControlController : MonoBehaviour
{
    private UIDocument _document;

    private Button _controlButton;
    
    public string _nextScene;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _controlButton = _document.rootVisualElement.Q("ExitButton") as Button;
        _controlButton.RegisterCallback<ClickEvent>(OnStartClick);
    }

    private void OnStartClick(ClickEvent ce)
    {
        SceneManager.LoadScene(_nextScene);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
