using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour
{

    public Transform player;
    public Transform target;

    public Vector3 lastTargetPosition;

    public float speed;
    
    bool firstTime;

    Grid grid;
    private float maxSpeed;

    private Quaternion fixedRotation;

    /*
     * Setting up variables
     */
    void Start()
    {
        grid = GetComponent<Grid>();
        maxSpeed = speed * 2;
    }

    /*
     * Generates a new grid based on the new movement and calculates a new path every frame
     */
    void Update()
    {
        if (grid.getNode(target.position).walkable)
        {
            grid.path = calculatePath(player.position, target.position);
            lastTargetPosition = target.position;
        }
        else
            grid.path = calculatePath(player.position, lastTargetPosition);

        if (grid.path.Count > 0)
        {
            if (speed > maxSpeed) speed = maxSpeed;
            else speed += 0.001f;
            double weight = grid.path[0].weight;
            Vector3 newPosition = Vector3.MoveTowards(player.position, grid.path[0].worldPosition, Time.deltaTime * speed / (float) weight);
            //Vector3 oldRotation = player.rotation.eulerAngles;
            player.LookAt(newPosition);
            fixedRotation = player.rotation;
            //player.rotation = Quaternion.Euler(0.1f * player.rotation.eulerAngles + 0.9f * oldRotation);
            //player.rotation = Quaternion.Euler(Vector3.MoveTowards(oldRotation, player.rotation.eulerAngles, Time.deltaTime * speed * 10));
            //player.rotation = Quaternion.Euler(Vector3.MoveTowards(oldRotation, player.rotation.eulerAngles, Time.deltaTime * speed * 100));
            player.position = newPosition;
        }
    }

    /*
     * Calculates the optimal path using the A* algorithm from the starting (world) location to the ending (world) location
     */
    public List<Node> calculatePath(Vector3 startLocation, Vector3 endLocation)
    {
        grid.instantiateGrid();
        Node startNode = grid.getNode(startLocation);
        Node endNode = grid.getNode(endLocation);

        startNode.gCost = 0;
        startNode.hCost = startNode.distanceTo(endNode);

        HashSet<Node> openSet = new HashSet<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);
        
        while(openSet.Count > 0)
        {
            Node current = null;
            foreach(Node node in openSet)
            {
                if (current == null || node.fCost < current.fCost || (node.fCost == current.fCost && node.hCost < current.hCost))
                    current = node;
            }
            if (current == endNode) return backtrace(current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Node child in grid.getChildren(current))
            {
                if (!child.walkable || closedSet.Contains(child) || openSet.Contains(child)) continue;

                double tentative_gCost = current.gCost + current.weightedDistanceTo(child);
                if(tentative_gCost <= child.gCost)
                {
                    child.parentNode = current;
                    child.gCost = tentative_gCost;
                    child.hCost = child.distanceTo(endNode);

                    openSet.Add(child);
                }

            }

        }

        return new List<Node>();

    }

    /* 
     * Starts at the final node and then backtraces to the original node through the parent property of the Node class 
     */
    private List<Node> backtrace(Node current)
    {
        List<Node> list = new List<Node>();

        for (list.Add(current); current.parentNode != null; current = current.parentNode)
            list.Add(current);

        list.Reverse();

        return list;
    }

}
