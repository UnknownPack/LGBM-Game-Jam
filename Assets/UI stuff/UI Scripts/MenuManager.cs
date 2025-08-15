using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private UIDocument _document;
    private Button StartButton, EndApplicationButton;
    
    private void Start()
    {
        _document = GetComponent<UIDocument>();
        StartButton = _document.rootVisualElement.Q<Button>("Start");
        StartButton.clickable.clicked += () => SceneManager.LoadScene("MainGame");
        EndApplicationButton = _document.rootVisualElement.Q<Button>("Exit");
        EndApplicationButton.clickable.clicked += () => Application.Quit();
    } 
}
