using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuOperations : MonoBehaviour
{
    private UIDocument _document;

    private Button _startButton;

    private Button _controlButton;
    
    private Button _exitButton;

    public string _nextScene;

    public string _controlScene;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnStartClick);
        
        _controlButton = _document.rootVisualElement.Q("SettingsButton") as Button;
        _controlButton.RegisterCallback<ClickEvent>(OnControlClick);
        
        _exitButton = _document.rootVisualElement.Q("ExitButton") as Button;
        _exitButton.RegisterCallback<ClickEvent>(OnExitClick);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartClick);
        _controlButton.UnregisterCallback<ClickEvent>(OnControlClick);
    }

    private void OnStartClick(ClickEvent ce)
    {
        SceneManager.LoadScene(_nextScene);
    }
    
    private void OnControlClick(ClickEvent ce)
    {
        SceneManager.LoadScene(_controlScene);
    }

    private void OnExitClick(ClickEvent ce)
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}