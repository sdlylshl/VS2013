using System;
using System.Windows.Threading;
using System.Security.Permissions;

namespace IPProxy
{
    /// <summary>
    /// 实现C#里Application.DoEvent的效果
    /// 全部显示,处理当前在消息队列中的所有Windows消息
    /// </summary>
    class DispatcherHelper
    {
        [SecurityPermissionAttribute(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void DoEvents()
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
            try { Dispatcher.PushFrame(frame); }
            catch (InvalidOperationException) { }
        }
        private static object ExitFrames(object frame)
        {
            ((DispatcherFrame)frame).Continue = false;
            return null;
        }   
    }
}
