using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private MouseInput mouseInput;
    private GridManager gridManager;
    private UserInputManager UserInputManager;
    private UIManager UIManager;
    public List<BaseBattleEntity> battleEntities;
    private List<StatusEffects> statusEffects = new List<StatusEffects>();
    public bool isPlayersTurn = true;
    public int currentTurn = 0;
    
    bool isPlayerTurn = true;
    void Start()
    {
        mouseInput = GetComponent<MouseInput>();
        gridManager = GetComponent<GridManager>();
        UIManager = GetComponent<UIManager>();
        gridManager.InjectTurnManager(this);
        UserInputManager = GetComponent<UserInputManager>();
        gridManager.InitaliseMap();
        foreach (var entity in battleEntities)
        {
            Debug.Log($"{entity.gameObject.name} Initialised with GridManager");
            entity.InjectGridManager(gridManager, this);
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
            if(battleEntity == null)
            {
                Debug.LogWarning("Battle entity is null, skipping...");
                continue;
            }
            if (battleEntity is EnemyBaseEntity output)
            {
                yield return StartCoroutine(output.ExecuteTurn());
                gridManager.UpdateGrid();
            }
        }
        battleEntities.RemoveAll(battleEntity => battleEntity == null );
        ApplyStatusEffect();
        Debug.Log("All Enemy Turns Executed");
        // After enemy turns have been executed, return control back to player and replenish spent action points
        ResetActionPoints(UnitOwnership.Player);
        UserInputManager.ResetInputs();
        SetPlayerTurn(true);
        currentTurn++;
        Debug.Log($"Current Turn cycle: {currentTurn}");
    }

    private bool IsAreaCleared()
    {
        foreach (var battleEntity in battleEntities)
        {
            if (battleEntity.GetUnitOwnerShip == UnitOwnership.Enemy)
                return false;
        }
        return true;
    }

    private bool PlayerFailed()
    {
        foreach (var battleEntity in battleEntities)
        {
            if (battleEntity.GetUnitOwnerShip == UnitOwnership.Player)
                return false;
        }
        return true;
    }
    
    public void AddStatusEffect(StatusEffects statusEffect) => statusEffects.Add(statusEffect);

    private void ApplyStatusEffect()
    {
        statusEffects.RemoveAll(se => !se.IsActive); // drop inactive first

        foreach (var se in statusEffects)
            se.tickDown();

        statusEffects.RemoveAll(se => !se.IsActive); // drop any that expired
    }

    private void ManageTurns()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Ending player turn...");
            SetPlayerTurn(false);

            StartCoroutine(CycleTurn());
        }
        if(IsAreaCleared())
            UIManager.ShowEndScreen("Game Over! You Won!");
        if(PlayerFailed())
            UIManager.ShowEndScreen("Game Over! You lost noob!");
    }
    

    private void SetPlayerTurn(bool b)
    {
        isPlayersTurn = b;
        mouseInput.ReadUserInput(b);
        mouseInput.SetSelectedUnit(false);
    }

    private void ResetActionPoints(UnitOwnership owner)
    {
        foreach (var battleEntity in battleEntities)
        {
            if (battleEntity.GetUnitOwnerShip == owner)
                battleEntity.ResetActionPoints();
        }
    }
    
    public GridManager GetGridManager => gridManager;
    
}
