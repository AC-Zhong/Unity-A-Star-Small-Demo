using System.Collections.Generic;
using System;
public class NodeCompare:IComparer<AStarNode>
{
    public int Compare(AStarNode x, AStarNode y)
    {
        if (x.totalDistance < y.totalDistance) return -1;
        else if (x.totalDistance > y.totalDistance) return 1;
        else return 0;
    }
}