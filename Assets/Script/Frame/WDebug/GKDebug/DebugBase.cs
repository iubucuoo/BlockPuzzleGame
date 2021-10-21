using System;
using UnityEngine;

namespace WDebug
{
    internal class DebugBase
    {
        public Action<string> callback;

        public DebugBase(Action<string> back)
        {
            callback = back;
        }

        public DebugBase()
        {
        }

        internal void Log(object message, UnityEngine.Object context = null)
        {
            if (!CanLogSocket())
            {
                Debug.Log(message, context);
            }
            else
            {
                SocketLog(message);
            }
        }

        private void SocketLog(object message)
        {
            callback(message.ToString());
        }

        internal void LogError(object message, UnityEngine.Object context = null)
        {
            if (!CanLogSocket())
            {
                Debug.LogError(message, context);
            }
            else
            {
                Log(message, null);
            }
        }

        internal void LogWarning(object message, UnityEngine.Object context = null)
        {
            if (!CanLogSocket())
            {
                Debug.LogWarning(message, context);
            }
            else
            {
                Log(message, null);
            }
        }

        private static bool CanLogSocket()
        {
            if (!Debuger.IsUdpLog)
            {
                return false;
            }
            return true;
        }
    }
}
