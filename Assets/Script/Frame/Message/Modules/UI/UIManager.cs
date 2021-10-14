
    class UIManager:ManagerBase
    {
        public static UIManager instance
        {
            get
            {
                return  mInstance??(mInstance=new UIManager(ManagerID.UIManager));
            }
        }
        static UIManager mInstance;

        UIManager(ManagerID id) : base(id)
        {

        }
    }

