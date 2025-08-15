using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset abilityButtonTemplate;
    public Color MovePointColor, ActionPointColor, emptyPointColor;
    private UIDocument uiDocument;
    private VisualElement mainUi, abilityButtonContainer, movePoint, actionPoint, pauseContainer, enemyTurnContainer;
    private Label MainText, UnitsText;
    private Button resume, restart, MainMenu;
    private UserInputManager userInputManager;
    private Dictionary<string, Button> CurrentAbilityButtons = new Dictionary<string, Button>();
    private bool isPaused = false, enemyTurn = false;
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        mainUi = uiDocument.rootVisualElement.Q<VisualElement>("MainUI");
        abilityButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("AbilityList");
        movePoint = uiDocument.rootVisualElement.Q<VisualElement>("movePoint");
        actionPoint = uiDocument.rootVisualElement.Q<VisualElement>("actionPoint");
        
        pauseContainer = uiDocument.rootVisualElement.Q<VisualElement>("PauseMenu");
        resume = uiDocument.rootVisualElement.Q<Button>("Resume");
        resume.clickable.clicked += () => isPaused = false;
        
        restart = uiDocument.rootVisualElement.Q<Button>("Restart");
        restart.clickable.clicked += () => SceneManager.LoadScene("MainGame");
        
        MainMenu = uiDocument.rootVisualElement.Q<Button>("MainMenu");
        MainMenu.clickable.clicked += () => SceneManager.LoadScene("MainMenuScene");
        
        MainText = uiDocument.rootVisualElement.Q<Label>("Text");
        pauseContainer.style.display = DisplayStyle.None;
        
        UnitsText = uiDocument.rootVisualElement.Q<Label>("UnitName");
        
        enemyTurnContainer = uiDocument.rootVisualElement.Q<VisualElement>("EnemyTurn");
        enemyTurnContainer.style.display = DisplayStyle.None;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            isPaused = !isPaused;
        
        Time.timeScale = !isPaused ? 1f : 0f;
        pauseContainer.style.display = isPaused ? DisplayStyle.Flex : DisplayStyle.None;
        if(!enemyTurn)
            mainUi.style.display = !isPaused ? DisplayStyle.Flex : DisplayStyle.None;

    }

    public void ShowEndScreen(string endScreenName)
    {
        isPaused = true;
        mainUi.style.display = DisplayStyle.None;
        pauseContainer.style.display = DisplayStyle.Flex;
        resume.style.display = DisplayStyle.None;
        MainText.text = endScreenName; 
    }


    public void ShowUnitAbility(BaseBattleEntity SelectedBattleEntity)
    {
        CurrentAbilityButtons = new Dictionary<string, Button>();
        abilityButtonContainer.Clear();
        UnitsText.text = SelectedBattleEntity.gameObject.name;
        Dictionary<AbilityName, ActionBase> AbilityList = SelectedBattleEntity.GetAbilityList;

        foreach (var ability in AbilityList)
        {
            if (abilityButtonTemplate == null)
            {
                Debug.LogError("Button template is not assigned. Please assign a UXML template in the inspector.");
                return;
            } 
            
            var instance = abilityButtonTemplate.Instantiate();
            Button btn = instance.Q<Button>(); 
            abilityButtonContainer.Add(instance);
            string abilityName = ability.Key.ToString();
            btn.name = abilityName;
            btn.text = abilityName;
            btn.clickable.clicked += () => OnButtonClicked(abilityName, AbilityList[ability.Key]);
            CurrentAbilityButtons.Add(abilityName, btn);
        }
    }
    
    public void UpdateAvailableAbility(BaseBattleEntity SelectedBattleEntity)
    {
        if(CurrentAbilityButtons.Count == 0)
            return;
        
        Dictionary<ActionType, int> ActionPoints = SelectedBattleEntity.GetActionPoints;
        movePoint.style.unityBackgroundImageTintColor = (ActionPoints[ActionType.MovePoint] <= 0) ? emptyPointColor : MovePointColor;
        actionPoint.style.unityBackgroundImageTintColor = (ActionPoints[ActionType.ActionPoint] <= 0) ? emptyPointColor : ActionPointColor;
        
        Dictionary<AbilityName, ActionBase> AbilityList = SelectedBattleEntity.GetAbilityList;
        
        foreach (var ability in AbilityList)
        {
            bool isButtonActive = ActionPoints[ability.Value.GetActionType] > 0;
            CurrentAbilityButtons[ability.Key.ToString()].SetEnabled(isButtonActive);
        }
    }
    
    public void DeselectUnit()
    {
        // Clear the ability button container when no unit is selected
        abilityButtonContainer.Clear();
        CurrentAbilityButtons.Clear();
    }
    
    public void ShowEnemyTurnScreen(bool showScreen)
    {
        enemyTurn = showScreen;
        enemyTurnContainer.style.display = showScreen ? DisplayStyle.Flex : DisplayStyle.None;
    }
    

    public void InjectUserInputManager(UserInputManager userInputManager) => this.userInputManager = userInputManager;

    private void OnButtonClicked(String actionName, ActionBase actionBase)
    {
        Debug.Log($"Clicked on {actionName}{actionBase} button! Selection type: {actionBase.GetActionTargetType}");
        userInputManager.SelectAction(actionBase);
    } 
    
    
    #region Test Methods

    [ContextMenu("Generate Ability Buttons")]
    public void GenerateButtons()
    {
        uiDocument = GetComponent<UIDocument>();
        abilityButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("AbilityList");
        for(int i = 0; i < 5; i++)
        {
            Button btn;
            if (abilityButtonTemplate == null)
            {
                Debug.LogError("Button template is not assigned. Please assign a UXML template in the inspector.");
                return;
            } 
            // clone a UXML template that defines a <Button> in your .uxml
            var instance = abilityButtonTemplate.Instantiate();
            btn = instance.Q<Button>(); 
            abilityButtonContainer.Add(instance);
        }
    }

    [ContextMenu("Delete Generated Buttons")]
    public void ClearButtons() => abilityButtonContainer.Clear();

    #endregion 
}
