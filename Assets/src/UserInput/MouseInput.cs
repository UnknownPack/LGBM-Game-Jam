using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private bool unitsSelected = false, readUserInput = false, selectingAction = false, actionSelected;
    private UserInputManager userInputManager;
    
    void Start()
    {
    }

    void Update()
    {
        if(!readUserInput)
            return;
        
        if (!unitsSelected)
            ManageUserInput();
        else
            ManageSelectedUnit();
    }

    void ManageUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider == null)
                return;
            
            GameObject SelectedUnit = (hit.collider.CompareTag("PlayerUnit"))? hit.collider.gameObject : null;
            userInputManager.SelectUnit(SelectedUnit);
            
        }
    }

    void ManageSelectedUnit()
    {
        ActionBase SelectedAction = userInputManager.GetSelectedAction();
        if (SelectedAction == null)
        {
            Debug.LogWarning("SelectedAction is null.");
            return;
        }

        BaseBattleEntity SelectedEntity = SelectedAction.GetBaseBattleEntity;
        
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        
        if(hit.collider == null)
            return;

        if (!SelectedEntity.isActionPointAvailable(SelectedAction.GetActionType))
        {
            Debug.LogWarning("Selected action is not available.");
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            GameObject Target = hit.collider.gameObject;
            
            if (Target.CompareTag(SelectedAction.GetActionTargetType.ToString()))
            {
                Debug.LogWarning("Invalid target type selected.");
                return;
            }

            if (!SelectedAction.TargetWiihinRange(Target))
            {
                Debug.LogWarning("Invalid target within range.");
                return;
            }
            
            Debug.Log("Executing Action...");
            StartCoroutine(SelectedAction.Action(Target));
        }
    }
    
    

    #region Public Helper Functions
    public void ReadInputForSelectingAction(bool readUserInput)=> selectingAction = readUserInput;
    public void SetSelectedUnit(bool readUserInput)=> unitsSelected = readUserInput;
    public void InjectUserInputManager(UserInputManager userInputManager) => this.userInputManager = userInputManager;
    public void ReadUserInput(bool b) => readUserInput = b;

    #endregion

}
