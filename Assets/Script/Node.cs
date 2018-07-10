using UnityEngine;  
using System.Collections;  
using System;  
 //每一个格子的信息
public class Node : IComparable
{
    //代价值(G和H),
    public float gCost;     //从开始节点到当前节点的代价
    public float fCost;     //fCoust
    public bool bObstacle;  //是否为障碍物
    public Node parent;
    public Vector3 position;

    public Node()
    {
        this.fCost = 0.0f;
        this.gCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
    }

    public Node(Vector3 pos)
    {
        this.fCost = 0.0f;
        this.gCost = 1.0f;
        this.bObstacle = false;
        this.parent = null;
        this.position = pos;
    }

    public void MarkAsObstacle()
    {
        this.bObstacle = true;
    }
    //实现接口的排序方法
    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        //Negative value means object comes before this in the sort order.  
        if (this.fCost < node.fCost)
            return -1;
        //Positive value means object comes after this in the sort order.  
        if (this.fCost > node.fCost)
            return 1;
        return 0;  
    }
}