using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AStar寻路管理器
/// </summary>
public class AStarManger
{
    private List<AStarNode> openList=new List<AStarNode>();
    private List<AStarNode> clolseList=new List<AStarNode>();
    private List<Vector2> Path = new List<Vector2>();

    private Vector2 start;
    private Vector2 end;

    private int times;
    
    /// <summary>
    /// 传入起点和终点，返回一个路径
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns>用返回的坐标数组来表示路径</returns>
    public List<Vector2> FindPath(Vector2 start,Vector2 end)
    {
        this.start = start;
        this.end = end;
        openList.Clear();
        clolseList.Clear();
        Path.Clear();
        times = 0;
        
        AStarNode startNode=new AStarNode((int)start.x,(int)start.y,null);
        clolseList.Add(startNode);
        Find(startNode);
        Debug.Log("关闭列表");
        foreach(var end1 in clolseList)
        {
            Debug.Log(end1.x + "," + end1.y);
            GameObject greenCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            greenCube.transform.position = GameManger.Instance().XYToPos(end1.x, end1.y);
            greenCube.GetComponent<MeshRenderer>().material = GameManger.Instance().greenMaterial;
        }
        return null;
    }
    
    
    
/// <summary>
/// 用来递归循环的方法，给定一个点，遍历周围可用的点，分别算出距离后，存入打开列表并将同一个父节点距离最小的转入关闭列表
/// </summary>
/// <param name="pos"></param>
/// <param name="fatherNode"></param>
    public void Find(AStarNode fatherNode)
    {
        times++;
        if(times>50)
        {
            Debug.Log("检测到死循环");
            times = 0;
            return;
        }

        if (clolseList[clolseList.Count - 1].x == end.x && clolseList[clolseList.Count - 1].y == end.y)
        {
            return;
        }

        GameManger.Instance().hasVisted[fatherNode.x, fatherNode.y] = true;

        int originCount = openList.Count;


        //遍历周围的9个点，当遇到坐标越界，和自身相同，是阻挡物，以及存在于开启或者关闭列表时跳过
        for (int i = fatherNode.x - 1; i <= fatherNode.x + 1; i++)
        {
            if (i < 0 || i >= GameManger.Instance().width) continue;
            for (int j = fatherNode.y - 1; j <= fatherNode.y + 1; j++)
            {
                if (j < 0 || j >= GameManger.Instance().height) continue;
                else if (fatherNode.x == i && fatherNode.y == j) continue;
                else if (GameManger.Instance().nodeTypes[i, j] == NodeType.stop) continue;
                else if (GameManger.Instance().hasVisted[i,j] == true) continue;



                GameManger.Instance().hasVisted[i, j] = true;
                Debug.Log("i=" + i + "j=" + j);
                AStarNode newNode=new AStarNode(i,j,fatherNode);
                newNode.GetDistance(fatherNode.startDistance,start,end);
                openList.Add(newNode);
                Debug.Log("add x=" + newNode.x + "add y" + newNode.y+"distance"+newNode.totalDistance);
                Debug.Log("times" + times);

                foreach (var item in openList)
                {
                    Debug.Log("字典中"+"x="+item.x+"y"+item.y);
                }
            }
        }
        if (openList.Count == originCount)
        {
            Debug.Log("死路");
            return;
        }

        //对openList新加入的元素进行排序
        Debug.Log("originCount" + originCount + "openList.Count-1=" + (openList.Count - 1));
        openList.Sort(originCount,openList.Count-originCount,fatherNode);
        clolseList.Add(openList[originCount]);
        openList.RemoveAt(originCount);
        Find(clolseList[clolseList.Count-1]);
    }
}
