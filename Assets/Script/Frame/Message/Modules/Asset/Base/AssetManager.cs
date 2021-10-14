
    class AssetManager : ManagerBase
    {
        public static AssetManager instance
        {
            get
            {
                return mInstance ?? (mInstance = new AssetManager(ManagerID.AssetManager));
            }
        }
        static AssetManager mInstance;

        AssetManager(ManagerID id) : base(id)
        {

        }
    }

