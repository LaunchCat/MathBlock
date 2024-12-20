using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;

public class PlayerMovement : MonoBehaviour
{
    public InputAction playerControls;
    private bool bMove = false;

    
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    [SerializeField] private AudioSource moveSFX;
    [SerializeField] private AudioSource moveFailSFX;
    [SerializeField] private GameObject WinCanvas;
    [SerializeField] private GameObject LoseCanvas;

    private bool flip = false;
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
                flip = false;
                break;
            case < 0:
                potentialMove += Vector3.left * GridManager.gridManager.gridSize;
                flip = true;
                break;
        }

        switch (input.y)
        {
            case > 0:
                potentialMove += Vector3.forward * GridManager.gridManager.gridSize;
                flip = spriteRenderer.flipX;
                break;
            case < 0:
                potentialMove -= Vector3.forward * GridManager.gridManager.gridSize;
                flip = spriteRenderer.flipX;
                break;
        }

        if (GridManager.gridManager.GetNode(potentialMove) != null)
        {
            spriteRenderer.flipX = flip;
            if (!GridManager.gridManager.GetNode(potentialMove).gameObj)
            {
                transform.position = potentialMove;
                GridManager.gridManager.SnapToGrid(gameObject);
                PlayMoveSound();
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
                            else
                            {
                                PlayMoveFailSound();
                            }
                        }

                        break;
                    case "Wall":
                        PlayMoveFailSound();
                        break;
                    case "Pit":
                        transform.position = potentialMove;
                        GridManager.gridManager.SnapToGrid(gameObject);
                        Destroy(gameObject, 1);
                        LoseCanvas.SetActive(true);
                        break;
                    case "Win":
                        transform.position = potentialMove;
                        GridManager.gridManager.SnapToGrid(gameObject);
                        WinCanvas.SetActive(true);
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

    private void PlayMoveSound()
    {
        moveSFX.pitch = UnityEngine.Random.Range(0.6f, 1.0f);
        moveSFX.PlayOneShot(moveSFX.clip);
    }

    private void PlayMoveFailSound()
    {
        moveFailSFX.pitch = UnityEngine.Random.Range(0.6f, 1.4f);
        moveFailSFX.PlayOneShot(moveFailSFX.clip);
    }
    
}
