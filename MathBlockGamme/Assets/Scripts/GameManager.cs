using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    
    
    [SerializeField] private int turnsTaken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManager != null)
        {
            Destroy(gameManager);
        }
        gameManager = this;

        turnsTaken = 0;
  

    }
    public static GameManager Get() { return gameManager; }

    public void IncrementTurnsTaken()
    {
        turnsTaken++;
    }
    public int GetTurnsTaken() { return turnsTaken; }

}
