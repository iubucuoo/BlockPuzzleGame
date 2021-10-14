using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class UnitManager : ManagerBase
    {
        public static UnitManager instance
        {
            get
            {
                return mInstance ?? (mInstance = new UnitManager(ManagerID.UnitManager));
            }
        }
        static UnitManager mInstance;

        UnitManager(ManagerID id) : base(id)
        {

        }
    }
