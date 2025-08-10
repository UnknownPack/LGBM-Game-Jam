using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset abilityButtonTemplate;
    public Color movePointColor, actonPointColor, emptyPointColor;
    private UIDocument uiDocument;
    private VisualElement abilityButtonContainer, movePoint, actionPoint;
    private UserInputManager userInputManager;
    private Dictionary<string, Button> CurrentAbilityButtons = new Dictionary<string, Button>();
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        abilityButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("AbilityList");
        movePoint = uiDocument.rootVisualElement.Q<VisualElement>("movePoint");
        actionPoint = uiDocument.rootVisualElement.Q<VisualElement>("actionPoint");
    }
    

    public void ShowUnitAbility(BaseBattleEntity SelectedBattleEntity)
    {
        CurrentAbilityButtons = new Dictionary<string, Button>();
        abilityButtonContainer.Clear();
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
            btn.Q<Label>("Name").text = ability.Key.ToString();
            btn.clickable.clicked += () => OnButtonClicked(abilityName, AbilityList[ability.Key]);
            CurrentAbilityButtons.Add(abilityName, btn);
        }
    }
    
    public void UpdateAvailableAbility(BaseBattleEntity SelectedBattleEntity)
    {
        if(CurrentAbilityButtons.Count == 0)
            return;
        
        Dictionary<ActionType, int> ActionPoints = SelectedBattleEntity.GetActionPoints;
        movePoint.style.backgroundColor = (ActionPoints[ActionType.MovePoint] > 0) ? movePointColor : emptyPointColor;
        actionPoint.style.backgroundColor = (ActionPoints[ActionType.ActionPoint] > 0) ? actonPointColor : emptyPointColor;
        
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
