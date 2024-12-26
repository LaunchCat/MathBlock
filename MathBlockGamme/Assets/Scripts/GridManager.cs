using System;
using UnityEditor;
using UnityEngine;

public class Node
{
    public Vector3 position;
    public GameObject gameObj;

    public Node(Vector3 pos)
    {
        position = pos;
        gameObj = null;
    }

    public void SetObj(GameObject obj)
    {
        gameObj = obj;
    }
}

public class GridManager : MonoBehaviour
{
    public static GridManager gridManager;
    [SerializeField]
    public int gridSize = 1;
    [SerializeField]
    private int height = 100;
    [SerializeField]
    private int width = 100;

    [SerializeField] private bool showDebugGrid = false;
    private Node[,] grid;

    private void Awake()
    {
        if(gridManager == null)
            gridManager = this;
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void OnValidate()
    {
        grid = new Node[width, height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = new Node(new Vector3(x * gridSize +(0.5f*gridSize), 0, y * gridSize+(0.5f*gridSize)));
            }
        }
    }

    private void Start()
    {
        
    }

    public Node GetNode(Vector3 position)
    {
        int x = (int)Mathf.Floor(position.x / gridSize);
        int y = (int)Mathf.Floor(position.z / gridSize);
        if (x < 0 || y < 0 ||
            x >= width || y >= height)
        {
            return null;
        }
        return grid[x, y];
    }
    
    private void OnDrawGizmos()
    {
        if (!showDebugGrid) return;
        if (grid == null) return;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (grid[x, y].gameObj)
                {
                    Gizmos.color = Color.yellow;
                }
                else
                {
                    Gizmos.color = Color.white;
                }

                Gizmos.DrawWireCube(grid[x, y].position, new Vector3(gridSize - 0.1f, 0.1f, gridSize - 0.1f));
            }
        }
    }

    public void SnapToGrid(GameObject go)
    {
        Node n = GetNode(go.transform.position);
        if (n == null) return;
        go.transform.position = n.position;
        GetNode(go.transform.position).SetObj(go);
    }

}
