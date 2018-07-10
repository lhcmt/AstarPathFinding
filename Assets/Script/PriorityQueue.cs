using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//实现一个最短优先队列 基于Arraylist
public class PriorityQueue  {

    private ArrayList nodes = new ArrayList();

    public int Length
    {
        get { return this.nodes.Count; }
    }

    public bool Contains(object node)
    {
        return nodes.Contains(node);
    }

    public Node First()
    {
        if (this.nodes.Count > 0)
        {
            return (Node)this.nodes[0];
        }
        return null;
    }

    public void Push(Node node)
    {
        this.nodes.Add(node);
        //调用了Sort方法.这将调用Node对象的CompareTo方法,且将使用estimatedCost值来排序节点.
        this.nodes.Sort();
    }

    public void Remove(Node node)
    {
        this.nodes.Remove(node);
        this.nodes.Sort();
    }
}
