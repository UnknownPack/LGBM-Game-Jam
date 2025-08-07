using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public MouseInput mouseInput;
    public GridManager gridManager;
    public List<BaseBattleEntity> battleEntities;
    public bool isPlayersTurn = true;
    public int currentTurn = 0;
    
    bool isPlayerTurn = true;
    void Start()
    {
        mouseInput = GetComponent<MouseInput>();
        gridManager = GetComponent<GridManager>();
        
        mouseInput.InjectBackendSystems(gridManager, this);
        gridManager.InitaliseMap();
        
        foreach (var entity in battleEntities)
        {
            Debug.Log($"{entity.gameObject.name} Initialised with GridManager");
            entity.InjectGridManager(gridManager);
            gridManager.GetNodeFromPosition(entity.transform.position).SetWalkableState(false);
        }
    }

    private void Update()
    {
        if (isPlayersTurn)
            ManageTurns();
    }

    private IEnumerator CycleTurn()
    {
        Debug.Log("Executing Enemy's turns...");
        ResetActionPoints(UnitOwnership.Enemy);
        foreach (var battleEntity in battleEntities)
        {
            if (battleEntity is EnemyBaseEntity output)
            {
                yield return StartCoroutine(output.ExecuteTurn());
            }
        }
        
        Debug.Log("All Enemy Turns Executed");
        // After enemy turns have been executed, return control back to player and replenish spent action points
        ResetActionPoints(UnitOwnership.Player);
        SetPlayerTurn(true);
        currentTurn++;
        Debug.Log($"Current Turn cycle: {currentTurn}");
    }

    private void ManageTurns()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Ending player turn...");
            SetPlayerTurn(false);
            StartCoroutine(CycleTurn());
        }
    }

    private void SetPlayerTurn(bool b)
    {
        isPlayersTurn = b;
        mouseInput.ReadUserInput(b);
        mouseInput.ResetSelectedUnit();
    }

    private void ResetActionPoints(UnitOwnership owner)
    {
        foreach (var battleEntity in battleEntities)
        {
            if (battleEntity.GetUnitOwnerShip == owner)
                battleEntity.ResetActionPoints();
        }
    }
}
