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
    
    bool isPlayerTurn = true;
    void Start()
    {
        mouseInput = GetComponent<MouseInput>();
        gridManager = GetComponent<GridManager>();
        gridManager.InitaliseMap();
        foreach (var entity in battleEntities)
        {
            Debug.Log($"{entity.gameObject.name} Initialised with GridManager");
            entity.InjectGridManager(gridManager);
        }
    }

    private void Update()
    {
         
    }

    private IEnumerator CycleTurn()
    {
        foreach (var battleEntity in battleEntities)
        {
            //battleEntity
        }
        yield return null;
    }
}
