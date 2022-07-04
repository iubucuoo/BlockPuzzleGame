using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum UILayer
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
enum UIHideType
{
    Hide = 1,  // 隐藏
    Destroy,   // 销毁
    WaitDestroy,   // 过段时间销毁
}
enum UIHideFunc
{
    MoveOutOfScreen = 1,  // 移到屏幕外
    Deactive,   // 关闭
}
enum UIEscClose
{
    DontClose = 1,  // 不关闭
    Close,   // 关闭
    Block,   // 不关闭且阻止下层界面关闭
}
public static class UIStatic
{
    public static string UI_DEFAULT_LAYER = "Panel";
    public static string UI_DEFAULT_HIDE_TYPE = "WaitDestroy";
    public static string UI_DEFAULT_HIDE_FUNC = "MoveOutOfScreen";
    public static string UI_DEFAULT_ESC_CLOSE = "DontClose";
    public static int WAIT_DESTROY_TIME = 300;    // 300秒内没使用就销毁
}
