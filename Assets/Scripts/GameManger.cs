using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    private GameManger(){}

    public NodeType[,] nodeTypes;

    public bool[,] hasVisted;

   [Header("地图的长宽")]
    public int width = 40;
    public int height = 40;

    [Header("生成障碍物的百分比概率")]
    public int obstacles = 20;

    [Header("各色材质")]
    public Material redMaterial;
    public Material greenMaterial;

    private Transform PlayerTransform;

    private Vector2 startVec2;
    private Vector2 endVec2;

    private Vector3 mousePos;

    private static GameManger instance;
    public static GameManger Instance()
    {
        if (instance == null)
        {
            instance = GameObject.Find("GameManger").GetComponent<GameManger>();
            return instance;
        }
        else return instance;
    }


    private void Start()
    {
        PlayerTransform = GameObject.Find("Player").GetComponent<Transform>();
        InitMap();
        //print((int)vector2.x + "qweq" + (int)vector2.y);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if(Physics.Raycast(ray,out raycastHit))
            {
                mousePos = raycastHit.point;
                startVec2 = PosToXY(PlayerTransform.position);
                StartFind(startVec2,PosToXY(mousePos));
            }
        }
    }

    /// <summary>
    /// 初始化地图，标明哪些点是障碍物
    /// </summary>
    void InitMap()
    {
        nodeTypes = new NodeType[width, height];
        hasVisted = new bool[width, height];
        //i和j是从左上角算起的
        for(int i=0;i<width;++i)
        {
            for(int j=0;j<height;++j)
            {
                //概率生成障碍物
                NodeType toAdd = Random.Range(0, 100) < obstacles ? NodeType.stop : NodeType.walk;
                nodeTypes[i, j] = toAdd;

                hasVisted[i, j] = false;
                //如果是障碍物生成红色方块
                if(toAdd==NodeType.stop)
                {
                    hasVisted[i, j] = true;
                    GameObject redCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    redCube.transform.position = XYToPos(i, j);
                    redCube.GetComponent<MeshRenderer>().material = redMaterial;
                }
            }
        }
    }


    /// <summary>
    /// 传入格子坐标，开始正式寻路
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void StartFind(Vector2 start,Vector2 end)
    {
        print("start"+start.x+"     "+start.y+"    end"+end.x +"      "+ end.y);
        //每次遍历本次开始节点的附近所有格子，并且算出他们的距离，然后放入开启列表，并且排序，将距离最小的节点放入关闭列表
        //循环直到开始节点等于结束节点，然后关闭列表中的父节点就是路径了


        //得出路径
        AStarManger aStarManger = new AStarManger();
        List<Vector2> Path= aStarManger.FindPath(start, end);
    }






    /// <summary>
    /// 通过格子的xy值得到具体位置
    /// </summary>
    public Vector3 XYToPos(int x,int y)
    {
        Vector3 vector3 = new Vector3(x - 20,0.5f,20-y);
        return vector3;
    }
    /// <summary>
    /// 通过坐标得到格子的x，y值
    /// </summary>
    Vector2 PosToXY(Vector3 vector3)
    {
        float x = vector3.x + 20;
        float y = 40-(vector3.z + 20);
        return new Vector2((int)x,(int) y);
    }
}
