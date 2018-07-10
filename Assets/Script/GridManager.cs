using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    private static GridManager s_Instance = null;
    public static GridManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                s_Instance = FindObjectOfType(typeof(GridManager))
                        as GridManager;
                if (s_Instance == null)
                    Debug.Log("Could not locate a GridManager " +
                            "object. \n You have to have exactly " +
                            "one GridManager in the scene.");
            }
            return s_Instance;  

        }
    }

    public int numOfRows;
    public int numOfColumns;
    public Node[,] nodes { get; set; }
    public float gridCellSize;             //单个网格大小
    public bool showGrid = true;

    private Vector3 origin = new Vector3(0,0,0); //地图左上角锚点
    public Vector3 Origin
    {
        get { return origin; }
    }

    public bool showObstacleblocks = true;
    private GameObject[] obstacleList;      //障碍物列表


    void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }
    //创建地图网格和障碍物 node数据
    void CalculateObstacles()
    {
        nodes = new Node[numOfColumns, numOfRows];
        int index = 0;
        for(int i =0;i<numOfColumns;i++)
            for(int j =0;j<numOfRows;j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        //
        if (obstacleList != null && obstacleList.Length > 0)
        {
            foreach(GameObject data in obstacleList)
            {
                int cellIndex = GetGridIndex(data.transform.position);
                int col = GetColumn(cellIndex);
                int row = GetRow(cellIndex);
                nodes[row, col].MarkAsObstacle();
            }
        }
    }
    //返回网格的中心点 Position
    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 position = GetGridCellPosition(index);
        position.x += gridCellSize / 2.0f;
        position.z += gridCellSize / 2.0f;
        return position;
    }
    //返回网格的左上角锚点 Position
    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0, zPosInGrid);
    }

    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
            return -1;
        pos -= Origin;
        int col = (int)(pos.x / gridCellSize);
        int row = (int)(pos.z / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        //整个网格的长和宽
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
                pos.z >= Origin.z && pos.x <= Origin.z + height);  
    }

    //返回行号从0开始
    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }
    //返回列号从0开始
    public int GetColumn(int index)
    {
        int col = index % numOfColumns;
        return col;
    }
    //在当前节点的左右上下四个方向检索其邻接节点.之后,在AssignNeighbour方法中
    //我们检查邻接节点看其是否为障碍物.如果不是我们将其添加neighbours中.
    public void GetNeighbours(Node node,ArrayList neighbours)
    {
        Vector3 nodePos = node.position;
        int nodeIndex = GetGridIndex(nodePos);
        int row = GetRow(nodeIndex);
        int col = GetColumn(nodeIndex);  

        //top neighbor
        AssignNeighbour(row - 1, col, neighbours);
        //bottom
        AssignNeighbour(row + 1, col, neighbours);
        //right
        AssignNeighbour(row, col + 1, neighbours);
        //left
        AssignNeighbour(row, col-1, neighbours);

    }

    void AssignNeighbour(int row, int column, ArrayList neighbors)
    {
        if (row != -1 && column != -1 &&row < numOfRows && column < numOfColumns)
        {
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }

    void OnDrawGizmos() {  
        if (showGrid) 
        {  
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);  
        }  
        Gizmos.DrawSphere(transform.position, 0.5f);  
        if (showObstacleblocks) 
        {  
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f,gridCellSize);  
            if (obstacleList != null && obstacleList.Length > 0) {  
                foreach (GameObject data in obstacleList) {  
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(data.transform.position)), cellSize);  
                }
            }  
        }  
    }  
    public void DebugDrawGrid(Vector3 origin, int numRows, int  numCols,float cellSize, Color color) 
    {  
        float width = (numCols * cellSize);  
        float height = (numRows * cellSize);  
        // Draw the horizontal grid lines  
        for (int i = 0; i < numRows + 1; i++) 
        {  
            Vector3 startPos = origin + i * cellSize * Vector3.forward;  
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f,  0.0f);  
            Debug.DrawLine(startPos, endPos, color);  
        }  
        // Draw the vertial grid lines  
        for (int i = 0; i < numCols + 1; i++) 
        {  
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f,0.0f, 0.0f);  
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f,1.0f);  
            Debug.DrawLine(startPos, endPos, color);  
        }  
    }  

}
