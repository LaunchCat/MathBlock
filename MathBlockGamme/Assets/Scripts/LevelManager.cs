using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    [SerializeField] private List<TurnTakerBase> turnTakers;
    private List<TurnTakerBase> turnTakersToAdd = new();
    private List<TurnTakerBase> turnTakersToRemove = new();
    [SerializeField] private PlayerMovement player;
    
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        turnTakers = new List<TurnTakerBase>(FindObjectsByType<TurnTakerBase>(FindObjectsSortMode.None));
        
    }


    public void QueueRemoveTurnTaker(TurnTakerBase turnTaker)
    {
        turnTakersToRemove.Add(turnTaker);
    }
    public void QueueAddTurnTaker(TurnTakerBase turnTaker)
    {
        turnTakersToAdd.Add(turnTaker);
    }
    
    
    public void RunTurn()
    {
        for (int i = 0; i < turnTakers.Count; i++)
            turnTakers[i].TakeTurn();

        for (int i = 0; i < turnTakersToRemove.Count; i++)
            turnTakers.Remove(turnTakersToRemove[i]);
        
        if (turnTakersToRemove.Count != 0)
            turnTakersToRemove.Clear();

        for (int i = 0; i < turnTakersToAdd.Count; i++)
            turnTakers.Add(turnTakersToAdd[i]);


        if (turnTakers.Count != 0)
            turnTakersToAdd.Clear();
        

        player.GivePlayerTurn();
        
    }
}
