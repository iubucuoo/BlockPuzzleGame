using UnityEngine;

namespace GKDebuger
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
                    string[] result = new string[1]
                    {
                        "系统"
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
