public enum UILayer
{
    Background = 1,//背景
    Main,//主界面
    Panel,//主面板
    Dialog,//对话框
    MessageBox,//消息框
    Prompt,//提示信息
    Tooltip,//悬浮提示
    Loading,  //加载界面

}
public enum UIHideType
{
    Hide = 1,  // 隐藏
    Destroy,   // 销毁
    WaitDestroy,   // 过段时间销毁
}
public enum UIHideFunc
{
    MoveOutOfScreen = 1,  // 移到屏幕外
    Deactive,   // 关闭
}

public static class UIStatic
{
    public static int WAIT_DESTROY_TIME = 10000;    // 10秒内没使用就销毁
}
