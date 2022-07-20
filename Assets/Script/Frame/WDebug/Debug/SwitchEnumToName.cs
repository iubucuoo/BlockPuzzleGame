using UnityEngine;

namespace WDebug
{
    internal class SwitchEnumToName
    {
        private static string[] mFilterName;

        public static string[] FilterName
        {
            get
            {
                if (mFilterName == null)
                {
                    string[] result = new string[6]
                    {
                        "系统","1号","2号","3号","4号","5号"
                    };
                    mFilterName = result;
                    return result;
                }
                return mFilterName;
            }
        }

        public static string[] ReadConfig(WWW line)
        {
            return line.text.Split(" ".ToCharArray());
        }
    }
}
