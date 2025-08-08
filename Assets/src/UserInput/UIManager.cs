using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public VisualTreeAsset abilityButtonTemplate;
    private UIDocument uiDocument;
    private VisualElement abilityButtonContainer;
    private UserInputManager userInputManager;
    void Start()
    {
        uiDocument = GetComponent<UIDocument>();
        abilityButtonContainer = uiDocument.rootVisualElement.Q<VisualElement>("AbilityList");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowUnitAbility(BaseBattleEntity SelectedBattleEntity)
    {
        //Clear out container
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
            btn.name = ability.Key.ToString();
            btn.Q<Label>("NameLabel").text = ability.Key.ToString();
            btn.clickable.clicked += () => OnButtonClicked(AbilityList[ability.Key]);
        }
    }
    
    public void DeselectUnit()
    {
        // Clear the ability button container when no unit is selected
        abilityButtonContainer.Clear();
        //TODO: hide the ability UI or perform any other necessary cleanup
    }

    public void InjectUserInputManager(UserInputManager userInputManager) => this.userInputManager = userInputManager;
    private void OnButtonClicked(ActionBase actionBase) => userInputManager.SelectAction(actionBase);
    
    
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
