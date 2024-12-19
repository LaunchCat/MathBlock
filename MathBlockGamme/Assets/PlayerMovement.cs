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
                potentialMove += Vector3.right * GridManager.gridSize;
                break;
            case < 0:
                potentialMove += Vector3.left * GridManager.gridSize;
                break;
        }

        switch (input.y)
        {
            case > 0:
                potentialMove += Vector3.forward * GridManager.gridSize;
                break;
            case < 0:
                potentialMove -= Vector3.forward * GridManager.gridSize;
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
                var block = GridManager.gridManager.GetNode(potentialMove).gameObj.GetComponent<MathBlock>();
                if (block != null)
                {
                    if (block.Push((block.transform.position - transform.position).normalized))
                    {
                        transform.position = potentialMove;
                        GridManager.gridManager.SnapToGrid(gameObject);
                    }
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
