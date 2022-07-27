public interface IMgr
{
    void InitMgr();
    void ResetMgr();
}
public class ScriptMgr : Singleton<ScriptMgr>
{
    IMgr[] m;
    IMgr[] _FirstMgr;
    public void InitFirstScript()
    {
        if (_FirstMgr == null)
        {
            _FirstMgr = new IMgr[]
            {
                new ResCenter(), 
            };
        }
        for (int i = 0; i < _FirstMgr.Length; i++)
        {
            _FirstMgr[i].InitMgr();
        }
    }
    public void Init()
    {
        if (m == null)
        {
            m = new IMgr[] {
				//new NetCenter(),//网络连接管理
				//new TcpSocket(),//tcp收发包管理
			};
        }
        for (int i = 0; i < m.Length; i++)
        {
            m[i].InitMgr();
        }
    }
    public void Reset()
    {
        if (m != null)
        {
            for (int i = 0; i < m.Length; i++)
            {
                m[i].ResetMgr();
            }
        }

        for (int i = 0; i < _FirstMgr.Length; i++)
        {
            _FirstMgr[i].ResetMgr();
        }
    }
}
