using System;
using System.Collections.Generic;
using UnityEngine;

public class UserInputManager : MonoBehaviour
{
    private MouseInput mouseInput;
    private UIManager uiManager;
    private GridManager gridManager;
    
    private GameObject SelectedUnit;
    private ActionBase SelectedAction;
    
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
        if(SelectedAction != null)
            SelectedAction.ShowActionRange(Color.azure);
        else
            gridManager.ResetColourTiles();
    }

    public void SelectUnit(GameObject unit) 
    {
        bool isUnitSelected = unit ==null;
        mouseInput.ReadInputForSelectingAction(isUnitSelected);
        mouseInput.SetSelectedUnit(isUnitSelected);
        
        if (!isUnitSelected)
        {
            uiManager.DeselectUnit();
            return;
        }
        
        SelectedUnit = unit;
        uiManager.ShowUnitAbility(unit.GetComponent<BaseBattleEntity>());
    }
    
    public void SelectAction(ActionBase action)
    {
        showSelectionActionRange = action != null;
        SelectedAction = (showSelectionActionRange) ? action : null;
    }
    
    public ActionBase GetSelectedAction() => SelectedAction;
}
