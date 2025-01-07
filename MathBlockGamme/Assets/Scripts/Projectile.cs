using System;
using TMPro;
using UnityEngine;

public class Projectile : TurnTakerBase
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int SpacesMoved = 1;
    [SerializeField] private TextMeshProUGUI IDText;
    [SerializeField] private static int ID;
    private int ActualID;

    private void Start()
    {
        ID++;
        ActualID = ID;
        IDText.text = ActualID.ToString();
    }

    public override bool TakeTurn()
    {
        Vector3 desiredMove = transform.position + transform.right * (GridManager.gridManager.gridSize * SpacesMoved);
        Node desiredNode = GridManager.gridManager.GetNode(desiredMove);
        if (desiredNode == null)
        {
            LevelManager.instance.QueueRemoveTurnTaker(this);
            GridManager.gridManager.GetNode(transform.position).SetObj(null);
            Debug.Log("Node was NULL");
            Destroy(gameObject);
            return true;
        }

        if (desiredNode.gameObj == null)
        {
            GridManager.gridManager.GetNode(transform.position).SetObj(null);
            transform.position = desiredMove;
            GridManager.gridManager.SnapToGrid(gameObject);
            return true;
        }
        
        switch (desiredNode.gameObj.tag)
        {
            case "Player":
                
                break;
            case "Wall":
               
                GridManager.gridManager.GetNode(transform.position).SetObj(null);
                LevelManager.instance.QueueRemoveTurnTaker(this);
                Destroy(gameObject);
                break;
            case "Answer":
               
                GridManager.gridManager.GetNode(transform.position).SetObj(null);
                LevelManager.instance.QueueRemoveTurnTaker(this);
                Destroy(gameObject);
                break;
            case "MathBlock":
               
                GridManager.gridManager.GetNode(transform.position).SetObj(null);
                LevelManager.instance.QueueRemoveTurnTaker(this);
                LevelManager.instance.QueueRemoveTurnTaker(desiredNode.gameObj.GetComponent<MathBlock>());
                Destroy(gameObject);
                Destroy(desiredNode.gameObj);
                break;
            case "Projectile":
                GridManager.gridManager.GetNode(transform.position).SetObj(null);
                transform.position = desiredMove;
                GridManager.gridManager.SnapToGrid(gameObject);
                break;
                
        }
        return true;
    }
}
