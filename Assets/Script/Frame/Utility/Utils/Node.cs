using System;

public class Node
{
    public object Value;

    public Node next;

    public Node()
    {
        Value = null;
        next = null;
    }

    public Node(object obj)
    {
        Value = obj;
        next = null;
    }

    public void Dispose()
    {
        Value = null;
        next = null;
    }
}
