using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    private MouseInput mouseInput;
    private UIManager uiManager;
    private GridManager gridManager;
    
    private BaseBattleEntity SelectedUnit;
    private ActionBase SelectedAction;
    private BaseBattleEntity battleEntity;
    private bool showSelectionActionRange = false;

    void Start()
    {
        mouseInput = GetComponent<MouseInput>();
        mouseInput.InjectUserInputManager(this);
        uiManager = GetComponent<UIManager>();
        uiManager.InjectUserInputManager(this);
        gridManager = GetComponent<GridManager>();
    }

    private void Update()
    {
        gridManager.ResetColourTiles();
        if(showSelectionActionRange)
        {
            Color color = (SelectedAction.GetActionType == ActionType.MovePoint)? Color.blue : Color.orange;
            SelectedAction.ShowActionRange(color);
        }
    }

    public void SelectUnit(BaseBattleEntity unit) 
    {
        bool UnitSelected = unit !=null;
        mouseInput.SetSelectedUnit(UnitSelected);
        
        gridManager.ResetColourTiles();
        uiManager.DeselectUnit();
        
        if (!UnitSelected)
            return;
        
        Debug.Log($"Selected Unit: {unit.name}");
        SelectedUnit = unit;
        uiManager.ShowUnitAbility(unit);
        uiManager.UpdateAvailableAbility(unit);
    }
    
    public void SelectAction(ActionBase action)
    {
        showSelectionActionRange = action != null;
        mouseInput.HasActionBeenSelected(showSelectionActionRange);
        SelectedAction = (showSelectionActionRange) ? action : null;
        uiManager.UpdateAvailableAbility(SelectedUnit);
    }
    
    public void ResetInputs()
    {
        SelectUnit(null);
        SelectAction(null);
        uiManager.DeselectUnit();
    }
    
    public ActionBase GetSelectedAction() => SelectedAction;
}
