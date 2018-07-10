using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar {

    public static PriorityQueue closedList, openList;
    //f = g + h 中的 cost
    //使用欧氏距离
    private static float HCost(Node curNode,Node targetNode)
    {
        Vector3 vecCost = curNode.position - targetNode.position;
        return vecCost.magnitude; // 模长
    }
    //寻路算法
    public static ArrayList FindPath(Node start,Node target)
    {
        openList = new PriorityQueue();
        closedList = new PriorityQueue();
        start.gCost = 0.0f;
        start.fCost = HCost(start, target);
        openList.Push(start); 
        Node node = null;
        while(openList.Length !=0)
        {
            node = openList.First();
            //检查是否到达目的地
            if (node.position == target.position)
            {
                return CalculatePath(node);
            }
            //获取当前点的邻居节点
            ArrayList neighbours = new ArrayList();
            GridManager.instance.GetNeighbours(node, neighbours); 
            for(int i =0;i<neighbours.Count;i++)
            {
                Node neighbourNode = (Node)neighbours[i];
                if(!closedList.Contains(neighbourNode))
                {
                    //更新neighbourNode cost信息
                    float cost = HCost(node, neighbourNode); // 到neighbour的cost
                    float neighbourNodeEstCost = HCost(neighbourNode, target);

                    neighbourNode.gCost = node.gCost + cost;
                    neighbourNode.parent = node;
                    neighbourNode.fCost = neighbourNode.gCost + neighbourNodeEstCost;  

                    if (!openList.Contains(neighbourNode))
                        openList.Push(neighbourNode);
                }
            }
            //Push the current node to the closed list  
            closedList.Push(node);
            //and remove it from openList  
            openList.Remove(node);  
        }

        if (node.position != target.position)
        {
            Debug.LogError("Target Not Found");
            return null;
        }
        return CalculatePath(node);  

    }


    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        //倒序输出
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }  
}
