using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    private UIDocument _document;
    private Button _button;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();
        _button = _document.rootVisualElement.Q<Button>("StartGameButton");
        _button.RegisterCallback<ClickEvent>(StartGame);
    }
    public void StartGame(ClickEvent evt)
    {
        SceneManager.LoadScene("SampleScene");
    }
}
