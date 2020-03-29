using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    walk,
    stop
}

public class AStarNode :IComparer<AStarNode>
{
    public int x { get; set; }
    public int y { get; set; }

    public float totalDistance { get; set; }
    public float startDistance { get; set; }
    private float endDistance;

    public AStarNode fatherNode { get; set; }

    public AStarNode(int x,int y,AStarNode fatherNode)
    {
        this.x = x;
        this.y = y;
        this.fatherNode = fatherNode;
    }

    public void GetDistance(float fatherDistance,Vector2 start, Vector3 end)
    {
        startDistance = Vector2.Distance(new Vector2(x, y), start);
        endDistance = Mathf.Abs(end.x-x)+Mathf.Abs(end.y-y);
        totalDistance =fatherDistance+ startDistance + endDistance;
    }

    public int Compare(AStarNode x, AStarNode y)
    {
        if (x.totalDistance < y.totalDistance) return -1;
        else if (x.totalDistance > y.totalDistance) return 1;
        else return 0;
    }
}
