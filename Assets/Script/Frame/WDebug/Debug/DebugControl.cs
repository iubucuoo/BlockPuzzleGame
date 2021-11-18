using GKDebuger;
using System;
using UnityEngine;

namespace WDebug
{
    internal class DebugControl
    {
        private static Action<string> callback;

        private static DebugBase _base;

        public DebugControl(Action<string> back)
        {
            callback = back;
        }

        private static DebugBase GetBase()
        {
            if (_base == null)
            {
                _base = new DebugBase(callback);
            }
            return _base;
        }

        public void Log(object message, object _filter = null, UnityEngine.Object context = null)
        {
            if (Debuger.EnableLog && GetFilter(_filter))
            {
                GetBase().Log($"[{SwitchFilerName(_filter)}]{message}", context);
            }
        }

        public void LogError(object message, object _filter = null, UnityEngine.Object context = null)
        {
            if (Debuger.EnableLog && GetFilter(_filter))
            {
                GetBase().LogError($"[{SwitchFilerName(_filter)}]{message}", context);
            }
        }

        public void LogWarning(object message, object _filter = null, UnityEngine.Object context = null)
        {
            if (Debuger.EnableLog && GetFilter(_filter))
            {
                GetBase().LogWarning($"[{SwitchFilerName(_filter)}]{message}", context);
            }
        }

        private static bool GetFilter(object _filter)
        {
            if (_filter != null && (int)_filter != Debuger.filter && Debuger.filter != 0)
            {
                return false;
            }
            return true;
        }

        private string SwitchFilerName(object index)
        {
            if (index == null)
            {
                return SwitchEnumToName.FilterName[0];
            }
            if ((int)index < SwitchEnumToName.FilterName.Length)
            {
                return SwitchEnumToName.FilterName[(int)index];
            }
            return "NONE";
        }
    }
}
