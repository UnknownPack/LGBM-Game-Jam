using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseUIF : MonoBehaviour
{
   
    
    public void PauseMenuFunctionality(VisualElement root, UIManager manager)
    {
        var resumeButton = root.Q<Button>("ResumeButton"); // name in pause_menu.uxml
        resumeButton.clicked += () =>
        {
            manager.Unpause();
        };
        
        var settingsButton = root.Q<Button>("SettingsButton"); // name in pause_menu.uxml
        settingsButton.clicked += () =>
        {
            Debug.Log("Settings!");
        };

        var quitButton = root.Q<Button>("QuitButton");
        quitButton.clicked += () =>
        {
            Application.Quit();
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif
        };
    }
}
