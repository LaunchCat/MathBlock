using UnityEngine;

public class Pit : Obstacle
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   

    // Update is called once per frame
    void Update()
    {
        GridManager.gridManager.SnapToGrid(gameObject);
    }
}
