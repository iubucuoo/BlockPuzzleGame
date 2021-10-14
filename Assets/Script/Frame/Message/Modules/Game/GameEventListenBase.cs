
using UnityEngine;

    /// <summary>
    /// Game事件监听
    /// </summary>
    public class GameEventListenBase :MonoBase
    {
        
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public override bool RegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {
            GameManager.instance.RegistEventListen(tmpListen, msgs);
            return base.RegistEventListen(tmpListen, msgs);
        }
        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="msg"></param>
        public override void SendMsg(MessageBase msg)
        {
            GameManager.instance.SendMsg(msg);
        }
                
        /// <summary>
        /// 注销
        /// </summary>
        /// <param name="tmpListen"></param>
        /// <param name="msgs"></param>
        /// <returns></returns>
        public override bool UnRegistEventListen(IEventListen tmpListen, params ushort[] msgs)
        {            
            return GameManager.instance.UnRegistEventListen(tmpListen, msgs);
        }        
    }

