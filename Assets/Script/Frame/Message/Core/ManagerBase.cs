using System.Collections.Generic;
class ManagerBase : IEventListen
{
    public ushort[] messageIds { get; set; }
    Dictionary<ushort, EventNode> eventTree = new Dictionary<ushort, EventNode>();
    protected ManagerID ID;

    protected ManagerBase(ManagerID id)
    {
        ID = id;
    }


    /// <summary>
    /// 消息处理
    /// </summary>
    /// <param name="tmpMsg">消息事件</param>
    public virtual void ProcessEvent(MessageBase tmpMsg)
    {
        EventNode tmp;
        if (!eventTree.TryGetValue(tmpMsg.messageId, out tmp))
        {
            Log.Error(string.Format("在{0}中没有对 这个消息进行监听", tmpMsg.GetMsgType()));//, MessageCenter.instance.MsgIDToString(tmpMsg.messageId)
            return;
        }
        else
        {
            do
            {
                tmp.listen.ProcessEvent(tmpMsg);
                tmp = tmp.next;
            } while (tmp != null);
        }
        PoolMgr.Recycle(tmpMsg);
    }

    /// <summary>
    /// 发送消息,子节点监听发送到管理者
    /// </summary>
    /// <param name="msg">消息</param>
    public void SendMsg(MessageBase msg)
    {
        if (msg.GetMsgType() == ID)
        {
            ProcessEvent(msg);
        }
        else
        {
            MessageCenter.instance.SendMessage(msg);//c#->net,c#->lua                
        }

    }
    /// <summary>
    /// 销毁监听事件
    /// </summary>
    /// <param name="tmpListen">监听事件</param>
    /// <param name="msgs">若干个ID</param>
    public bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        for (ushort i = 0; i < msgs.Length; i++)
        {
            UnRegistEventListen(tmpListen, msgs[i]);
        }
        return false;
    }
    /// <summary>
    /// 一个监听对多个兴趣事件注册
    /// </summary>
    /// <param name="tmpListen">要注册的监听</param>
    /// <param name="msgs">一个注册可以注册多个msg</param>
    public bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
    {
        for (ushort i = 0; i < msgs.Length; i++)
        {
            RegistEventListen(new EventNode(tmpListen), msgs[i]);
        }
        return true;
    }

    /// <summary>
    /// 销毁单个监听事件
    /// </summary>
    /// <param name="tmpListen"></param>
    /// <param name="Id"></param>
    void UnRegistEventListen(IEventListen tmpListen, ushort Id)
    {
        EventNode tmpNode;
        if (eventTree.TryGetValue(Id, out tmpNode))
        {
            if (tmpNode.listen == tmpListen)
            {
                if (tmpNode.next == null)
                {
                    eventTree.Remove(Id);
                }
                else
                {
                    eventTree[Id] = tmpNode.next;
                    tmpNode.next = null;
                }
            }
            else
            {
                while (tmpNode.next != null && tmpNode.next.listen != tmpListen)
                {
                    tmpNode = tmpNode.next;
                }
                //有两种情况 1是没有下一个节点，2是下一个节点就是我们要的节点
                EventNode curNode = tmpNode.next;
                if (curNode == null)
                {
                    Log.Warning(string.Format("当前{0}的事件树下:{1}的节点中没有此监听", ID.ToString(), Id));
                }
                else
                {
                    tmpNode.next = curNode.next;
                    curNode.next = null;
                }
            }
        }
        else
        {
            Log.Warning(string.Format("当前{0}的事件树下没有此ID:{1}的节点", ID.ToString(), Id));
        }
    }
    /// <summary>
    /// 注册单个节点事件
    /// </summary>
    /// <param name="tmpNode">消息节点</param>
    /// <param name="tmpId">消息ID</param>
    void RegistEventListen(EventNode tmpNode, ushort tmpId)
    {
        if (!eventTree.ContainsKey(tmpId))
        {
            eventTree.Add(tmpId, tmpNode);
        }
        else
        {
            EventNode tmp = eventTree[tmpId];
            while (tmp.next != null)
            {
                tmp = tmp.next;
            }
            tmp.next = tmpNode;
        }
    }

    public void InitEventListen()
    {
    }
    public void Clear()
    {
        eventTree.Clear();
    }
}
