using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}