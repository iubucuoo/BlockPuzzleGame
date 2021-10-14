

    class GameManager : ManagerBase
    {
        public static GameManager instance
        {
            get
            {
                return mInstance ?? (mInstance = new GameManager(ManagerID.GameManager));
            }
        }
        static GameManager mInstance;

        GameManager(ManagerID id) : base(id)
        {

        }
    }

