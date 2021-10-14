    /// <summary>
    /// Unit事件监听基类
    /// </summary>
    public class UnitEventListenBase :MonoBase
    {
        /// <summary>
        /// 消息注册监听机制
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        public override bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {
            UnitManager.instance.RegistEventListen(tmpListen, msgs);
            return base.RegistEventListen(tmpListen, msgs);
        }
        /// <summary>
        /// 消息发送
        /// </summary>
        /// <param name="msg"></param>
        public override void SendMsg(MessageBase msg)
        {
            UnitManager.instance.SendMsg(msg);            
        }
        /// <summary>
        /// 消息注销
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        public override bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {            
            return UnitManager.instance.UnRegistEventListen(tmpListen, msgs);
        }
        
    }
