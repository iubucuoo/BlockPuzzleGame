using System;
using System.Collections.Generic;
using WUtils.Dictionary;

namespace WUtils.Utils
{
    public class SingleLinkedList
    {
        private Dictionary<string, Node> linkTree = new Dictionary<string, Node>();

        public Node PopAboutName(string keyName)
        {
            return linkTree.GHaveGet(keyName);
        }

        public void Push(string keyName, object obj)
        {
            Push(keyName, new Node(obj));
        }

        public void Push(string keyName, Node thisNode)
        {
            if (!linkTree.ContainsKey(keyName))
            {
                linkTree.Add(keyName, thisNode);
            }
            else
            {
                Node node = linkTree[keyName];
                while (node.next != null)
                {
                    node = node.next;
                }
                node.next = thisNode;
            }
        }

        public void DisposeAll(string keyName)
        {
            if (linkTree.ContainsKey(keyName))
            {
                Node node = linkTree[keyName];
                while (node != null)
                {
                    Node node2 = node;
                    node = node.next;
                    node2.Dispose();
                }
                linkTree.Remove(keyName);
            }
        }
    }
}
