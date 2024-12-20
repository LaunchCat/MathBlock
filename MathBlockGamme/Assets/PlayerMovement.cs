using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    private bool bMove = false;

    private void Start()
    {
         GridManager.gridManager.SnapToGrid(gameObject);
    }

    private void Update()
    {
        if (playerControls.WasPressedThisFrame()) 
            bMove = true;
    }

    private void FixedUpdate()
    {
        if (!bMove) return;
        var input = playerControls.ReadValue<Vector2>();
        GridManager.gridManager.GetNode(transform.position).SetObj(null);

        Vector3 potentialMove = transform.position;
        switch (input.x)
        {
            case > 0:
                potentialMove += Vector3.right * GridManager.gridManager.gridSize;
                break;
            case < 0:
                potentialMove += Vector3.left * GridManager.gridManager.gridSize;
                break;
        }

        switch (input.y)
        {
            case > 0:
                potentialMove += Vector3.forward * GridManager.gridManager.gridSize;
                break;
            case < 0:
                potentialMove -= Vector3.forward * GridManager.gridManager.gridSize;
                break;
        }

        if (GridManager.gridManager.GetNode(potentialMove) != null)
        {
            if (!GridManager.gridManager.GetNode(potentialMove).gameObj)
            {
                transform.position = potentialMove;
                GridManager.gridManager.SnapToGrid(gameObject);
            }
            else
            {
                switch (GridManager.gridManager.GetNode(potentialMove).gameObj.tag)
                {
                    case "MathBlock" :
                        var block = GridManager.gridManager.GetNode(potentialMove).gameObj.GetComponent<MathBlock>();
                        if (block != null)
                        {
                            if (block.Push((block.transform.position - transform.position).normalized))
                            {
                                transform.position = potentialMove;
                                GridManager.gridManager.SnapToGrid(gameObject);
                            }
                        }

                        break;
                    case "Wall":
                        break;
                    case "Pit":
                        transform.position = potentialMove;
                        GridManager.gridManager.SnapToGrid(gameObject);
                        Destroy(gameObject, 1);
                        break;
                    case "Win":
                        transform.position = potentialMove;
                        GridManager.gridManager.SnapToGrid(gameObject);
                        Debug.Log("You Win!");
                        break;
                    
                }
            }
        }

        bMove = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
}
