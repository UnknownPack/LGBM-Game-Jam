using System;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour
{
    private GameObject SelectedUnit;
    private bool unitsSelected = false;
    private ActionBase currentSelectedAction;
    private GridManager _gridManager;
    
    void Start()
    {
        _gridManager = GameObject.Find("CoreManager").GetComponent<GridManager>();
        if(_gridManager == null)
        {
            Debug.LogError("GridManager not found in the scene. Please ensure it is present.");
            return;
        }
    }

    void Update()
    {
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
            
            // Check if the hit object has a specific tag
            if (hit.collider.CompareTag("PlayerUnit"))
            {
                unitsSelected = true;
                SelectedUnit = hit.collider.gameObject;
                Debug.Log($"{hit.collider.gameObject.name} selected!");
            }
            else
            { 
                unitsSelected = false; 
                SelectedUnit = null;
            }
            
        }
    }

    void ManageSelectedUnit()
    {
        if(SelectedUnit == null)
        {
            unitsSelected = false;
            Debug.LogError("No unit selected.");
            return;
        }
        
        BaseBattleEntity battleEntity = SelectedUnit.GetComponent<BaseBattleEntity>();
        if(battleEntity == null)
        {
            Debug.LogError("Unable to get battleEntity component.");
            return;
        }

        currentSelectedAction = battleEntity.GetAbilityList[0];
        
        if(Input.GetKey(KeyCode.LeftShift))
            currentSelectedAction.ShowActionRange();
        else
            _gridManager.ResetColourTiles();
        
        
        //TODO: Implement logic for managing the selected unit and selecting actions
        if (Input.GetMouseButtonDown(1)) // Right-click to clear selection
        {
            _gridManager.ResetColourTiles();
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

            if (hit.collider == null)
                return;
            
            if (hit.collider.CompareTag("Tile"))
            {
                if (!currentSelectedAction.TargetWiihinRange(hit.collider.gameObject))
                {
                    Debug.LogWarning("Failure! You tried to execute action outside it's range");
                    return;
                }
                Node node = _gridManager.GetNodeFromPosition(mouseWorldPos);
                Debug.LogWarning($"Executing Action!");
                StartCoroutine(currentSelectedAction.Action(node.GetTileObject));
            }
        }
    }

    #region Public Getters

    public GameObject GetSelectedUnit => SelectedUnit;
    public bool IsUnitSelected => unitsSelected;

    #endregion

}
