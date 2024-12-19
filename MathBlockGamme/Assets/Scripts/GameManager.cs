using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager gameManager;

    [SerializeField] private float targetScore = 2;
    [SerializeField] private List<MathBlock> mathBlocks;
    [SerializeField] private Level level;
    [SerializeField] private MathBlock mathBlockPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManager != null)
        {
            Destroy(gameManager);
        }
        gameManager = this;
        

        targetScore = level.levelTarget;
        
        Vector3 StartPosition = Vector3.zero;
        StartPosition.y = 10;

        for (int i = 0; i < level.levelData.Count; i++)
        {
            MathBlock instance = Instantiate(mathBlockPrefab, StartPosition, Quaternion.identity);
            instance.operation = level.levelData[i].operation;
            instance.value = level.levelData[i].value;
            mathBlocks.Add(instance);
            StartPosition.z += 2;

        }

    }
    
    public static GameManager Get() { return gameManager; }

    //Called in Answer update to check if 
    private void CheckGameState(float currentAnswer)
    {
        if (currentAnswer.Equals(targetScore))
        {
            Debug.Log("You win!");
        }
        else if (mathBlocks.Count == 0)
        {
            Debug.Log("You lose!");
        }
    }

    //Called in the answer zone when a block has made contact with it. 
    public void AnswerUpdated(MathBlock mathBlock, float currentAnswer)
    {
        mathBlocks.Remove(mathBlock);
        Destroy(mathBlock.gameObject);
        CheckGameState(currentAnswer);
        
    }
}
