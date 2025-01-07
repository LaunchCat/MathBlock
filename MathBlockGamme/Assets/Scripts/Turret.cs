using System;
using UnityEngine;

public class Turret : TurnTakerBase
{
    public int fireInterval = 2;
    private int TurnsTaken; 
    private Node spawnNode;
    
    
    
 
    [SerializeField] private Projectile projectilePrefab;
    private void Start()
    {
        spawnNode = GridManager.gridManager.GetNode(transform.position +
                                                    transform.right * GridManager.gridManager.gridSize);
        
       
    }

    //Spawn projectile at node in front
    
    
    //Set Projectile to move 
    
    public void FireTurret()
    {
        Projectile spawnedProj = Instantiate(projectilePrefab, spawnNode.position, transform.rotation);
        GridManager.gridManager.SnapToGrid(spawnedProj.gameObject);
        LevelManager.instance.QueueAddTurnTaker(spawnedProj);
        
        

    }

    public override bool TakeTurn()
    {
        if (GameManager.Get().GetTurnsTaken() % fireInterval == 0)
        {
            FireTurret();
        }
        return true;
    }

    private void OnDestroy()
    {
        LevelManager.instance.QueueRemoveTurnTaker(this);
    }
}
