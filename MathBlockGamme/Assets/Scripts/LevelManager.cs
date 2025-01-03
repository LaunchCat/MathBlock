using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    
    private List<TurnTakerBase> turnTakers;
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

    public void RemoveTurnTaker(TurnTakerBase turnTaker)
    {
        turnTakers.Remove(turnTaker);
    }
    
    public void RunTurn()
    {
        for (int i = 0; i < turnTakers.Count; i++)
        {
            turnTakers[i].TakeTurn();
        }
        player.GivePlayerTurn();
    }
}
