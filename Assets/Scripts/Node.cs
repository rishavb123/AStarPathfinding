using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Vector3 worldPosition;
    public bool walkable;

    public int gridX;
    public int gridY;

    public double gCost = Mathf.Infinity;
    public double hCost = Mathf.Infinity;
    public double weight;
    public double fCost => gCost + hCost;

    public Node parentNode;

    /*
     * Sets all the instance variables
     */
    public Node(Vector3 worldPosition, bool walkable, int gridX, int gridY, double weight)
    {
        this.worldPosition = worldPosition;
        this.walkable = walkable;
        this.gridX = gridX;
        this.gridY = gridY;
        this.weight = weight;
    }

    /*
     * Calculates the distance to the input node
     */
    public double distanceTo(Node node)
    {
        float dx = Mathf.Abs(gridX - node.gridX);
        float dy = Mathf.Abs(gridY - node.gridY);

        return Mathf.Abs(dx - dy) + Mathf.Sqrt(2) * Mathf.Min(dx, dy);
    }

    public double weightedDistanceTo(Node node)
    {
        float dx = Mathf.Abs(gridX - node.gridX);
        float dy = Mathf.Abs(gridY - node.gridY);

        return (Mathf.Abs(dx - dy) + Mathf.Sqrt(2) * Mathf.Min(dx, dy)) * weight * node.weight;
    }

    override
    public string ToString()
    {
        return "(" + gridX + ", " + gridY + ")";
    }

}