
    /// <summary>
    /// Asset 监听 基类
    /// </summary>
    public class AssetEventListenBase : MonoBase
    {
        
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public override bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {
            AssetManager.instance.RegistEventListen(tmpListen, msgs);
            return base.RegistEventListen(tmpListen,msgs);
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="msg"></param>
        public override void SendMsg(MessageBase msg)
        {
            AssetManager.instance.SendMsg(msg);
        }
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public override bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {
            AssetManager.instance.UnRegistEventListen(tmpListen, msgs);
            return true;
        }
        /*
        /// <summary>
        /// 发送通用类
        /// </summary>
        /// <param name="id"></param>
        /// <param name="_back"></param>
        /// <param name="_objs"></param>
        public virtual void SendMsg(object id, MsgCallBack _back, params object[] _objs)
        {
            SendMsg(new Message(id, _back, _objs));
        }*/
    }

