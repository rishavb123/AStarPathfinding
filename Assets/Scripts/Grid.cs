using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public LayerMask wallMask;
    public LayerMask planeMask;
    public LayerMask terrainMask;
    public LayerMask waterMask;

    public float nodeHalfWidth;
    public Vector2 totalSize;

    public List<Node> path;

    public bool showPath;
    public bool drawGizmos;

    float nodeWidth => nodeHalfWidth * 2;
    int gridSizeX => Mathf.RoundToInt(totalSize.x / nodeWidth);
    int gridSizeY => Mathf.RoundToInt(totalSize.y / nodeWidth);
    Vector3 gridSize;

    Vector3 totalSize3 => Util.toVector3(totalSize.x, totalSize.y);
    Vector3 origin;

    Node[,] grid;


    /*
     * Instantiates some variables
     */
    void Start()
    {
        path = new List<Node>();

        gridSize = Util.toVector3(gridSizeX, gridSizeY);
        origin = -totalSize3 / 2;

        instantiateGrid();

    }

    /*
     * Called every frame
     */
    void Update()
    {
        
    }

    /*
     * Instantiate the grid array using the physics engine to find where the obstacles are 
     */
    public void instantiateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPosition = toWorldPosition(x, y);
                double weight = 1;
                if (Physics.CheckSphere(worldPosition, nodeHalfWidth, terrainMask)) weight = 1.5;
                if (Physics.CheckSphere(worldPosition, nodeHalfWidth, waterMask)) weight = 3;
                grid[x, y] = new Node(worldPosition, !Physics.CheckSphere(worldPosition, nodeHalfWidth, wallMask), x, y, weight);
            }
        }
    }

    /*
     * Returns a HashSet of the nodes surrounding the input node
     */
    public HashSet<Node> getChildren(Node node)
    {
        HashSet<Node> children = new HashSet<Node>();

        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                int childX = node.gridX + i; int childY = node.gridY + j;
                if(childX >= 0 && childX < gridSizeX && childY >= 0 && childY < gridSizeY)
                    children.Add(getNode(childX, childY));
            }
        }

        children.Remove(node);
        return children;
    }

    /**
     * Draws the grid using gizmos with colors based on the obstacle
     * Red is unwalkable
     * Yellow is terrain
     * Blue is water
     * Green is walkable
     */
     private void OnDrawGizmos()
    {
        if(drawGizmos)
        {
            origin = -totalSize3 / 2;

            Gizmos.color = Color.white;

            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    Vector3 worldPosition = toWorldPosition(x, y);
                    Gizmos.color = (path != null && grid != null && path.Contains(grid[x, y]))? 
                        Color.white : 
                        Physics.CheckSphere(worldPosition, nodeHalfWidth, wallMask) ? 
                            Color.red :
                            Physics.CheckSphere(worldPosition, nodeHalfWidth, terrainMask) ?
                                Color.yellow :
                                Physics.CheckSphere(worldPosition, nodeHalfWidth, waterMask) ?
                                    Color.blue :
                                Color.green;

                    if (showPath)
                    {
                        if (path != null && grid != null && path.Contains(grid[x, y]))
                            Gizmos.DrawWireCube(worldPosition, Vector3.one * nodeWidth);
                    }
                    else
                        Gizmos.DrawWireCube(worldPosition, Vector3.one * nodeWidth);
                }
            }
        }
    }

    /*
     * Converts the grid indexes to a world position
     */
    public Vector3 toWorldPosition(int x, int y)
    {
        return origin + Util.toVector3(x, y) * nodeWidth + Vector3.one * nodeHalfWidth;
    }

    /*
     * Converts a world position to grid indexes
     */
    public (int, int) toGridLocation(Vector3 worldPosition)
    {
        Vector3 result = Util.round(Vector3.Scale(Util.clamp(Util.elementWiseDivide((worldPosition - Vector3.one * nodeWidth / 2 - origin), totalSize3), 0, 1), gridSize));
        float x = result.x; float y = result.z;
        return ((int) x, (int) y);
    }

    /*
     * Gets a node from the world position
     */
    public Node getNode(Vector3 worldPosition)
    {
        (int x, int y) = toGridLocation(worldPosition);
        return getNode(x, y);
    }

    /*
     * Gets a node from grid indexes
     */
    public Node getNode(int x, int y)
    {
        x = Mathf.Clamp(x, 0, gridSizeX - 1);
        y = Mathf.Clamp(y, 0, gridSizeY - 1);
        return grid[x, y];
    }

}