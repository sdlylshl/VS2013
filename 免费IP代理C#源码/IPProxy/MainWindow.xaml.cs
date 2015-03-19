/// <summary>
/// 本程序使用VS2012开发 开发语言C#（WPF）
/// 本人也是初学C#，学C#也仅仅是出于兴趣爱好，很多问题都是半懂半不懂
/// 如果你有好的改进或想法，尤其是在多线程和数据源绑定上，欢迎和我联系~
/// 联系方式：tech56@qq.com
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;
using DotNet4.Utilities;
using System.Net;
using System.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;


namespace IPProxy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public class SettingDate
        {
            public int timeout;//TimeOut时间
            public int threadnum;//线程数
        }
        #region 全局变量声明
        List<ProxyIP> tempProxyList= new List<ProxyIP>();
        List<ProxyIP> myProxyList  = new List<ProxyIP>();
        List<ProxyIP> oldProxyList = new List<ProxyIP>();
        List<Thread> myThreadList  = new List<Thread>();
        SettingDate mySetting      = new SettingDate();
        bool isRunning;
        #endregion
        public static string Search_string(string s, string s1, string s2)  //获取在S1与S2之间的字符串  
        {
            int n1, n2;
            n1 = s.IndexOf(s1, 0) + s1.Length;   //开始位置  .            
            n2 = s.IndexOf(s2, n1);               //结束位置  
            return s.Substring(n1, n2 - n1);   //取搜索的条数，用结束的位置-开始的位置,并返回  
        }
        public ProxyIP GetProxyIP()
        {
            lock ("ip")
            {
                if (oldProxyList.Count > 0)
                {
                    ProxyIP p = oldProxyList[0];
                    oldProxyList.RemoveAt(0);
                    return p;
                }
                else
                {
                    return null;
                }
            }
        }
        public void AddProxyIP(ProxyIP p)
        {
            lock ("add")
            {
                tempProxyList.Add(p);
            }
        }

        public void checkproxy() {
            ProxyIP pp = new ProxyIP();
            pp.IP = "101.71.27.120";
            pp.Port = "80";
            pp.Place = "未知";
            if (pp != null)
            {

                HttpWebRequest Req;
                WebProxy proxyObject = new WebProxy(pp.IP, Convert.ToInt32(pp.Port));// port为端口号 整数型
                Req = WebRequest.Create("http://www.google.com/") as HttpWebRequest;
                Req.Proxy = proxyObject; //设置代理
                Req.Timeout = mySetting.timeout;   //超时
                DateTime dt1 = DateTime.Now;
                try
                {
                    if (pp.Place == "未知")
                    {
                        HttpHelper http = new HttpHelper();
                        HttpItem item = new HttpItem();
                        item.URL = "http://www.ip.cn/index.php?ip=" + pp.IP;
                        HttpResult result = http.GetHtml(item);
                        string strPlace = result.Html;
                        if (!string.IsNullOrEmpty(strPlace))
                        {
                            strPlace = Search_string(strPlace, "来自：", "</p><p>");
                            pp.Place = strPlace;
                            //AddProxyIP(pp);
                        }
                    }
                }
                catch { }//忽略错误
                try
                {
                    HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                    DateTime dt2 = DateTime.Now;
                    Encoding bin = Encoding.GetEncoding("utf-8");
                    StreamReader sr = new StreamReader(Resp.GetResponseStream(), bin);
                    string str = sr.ReadToEnd();
                    sr.Close();
                    sr.Dispose();
                    if (str.Contains("百度"))
                    {
                        //result = true;
                        TimeSpan ts = dt2 - dt1;
                        pp.Speed = ((long)ts.TotalMilliseconds).ToString();
                        //AddProxyIP(pp);
                    }
                }
                catch (Exception ex)
                {
                    pp.Speed = "超时";
                    AddProxyIP(pp);
                    Trace.WriteLine(ex.Message);
                }
            }
                  
        }
        public void CheckProxyWork()//验证IP地址
        {
            while (true)
            {
                ProxyIP pp = GetProxyIP();
                if (pp != null)
                {
            
                    HttpWebRequest Req;
                    WebProxy proxyObject = new WebProxy(pp.IP, Convert.ToInt32(pp.Port));// port为端口号 整数型
                    Req = WebRequest.Create("http://www.baidu.com/") as HttpWebRequest;
                    Req.Proxy = proxyObject; //设置代理
                    Req.Timeout = mySetting.timeout;   //超时
                    DateTime dt1 = DateTime.Now;
                    try
                    {
                        if (pp.Place == "未知")
                        {
                            HttpHelper http = new HttpHelper();
                            HttpItem item = new HttpItem();
                            item.URL = "http://www.ip.cn/index.php?ip=" + pp.IP;
                            HttpResult result = http.GetHtml(item);
                            string strPlace = result.Html;
                            if (!string.IsNullOrEmpty(strPlace))
                            {
                                strPlace = Search_string(strPlace, "来自：", "</p><p>");
                                pp.Place = strPlace;
                                AddProxyIP(pp);
                            }
                        }
                    }
                    catch{}//忽略错误
                    try
                    {
                        HttpWebResponse Resp = (HttpWebResponse)Req.GetResponse();
                        DateTime dt2 = DateTime.Now;
                        Encoding bin = Encoding.GetEncoding("utf-8");
                        StreamReader sr = new StreamReader(Resp.GetResponseStream(), bin);
                        string str = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                        if (str.Contains("百度"))
                        {
                            //result = true;
                            TimeSpan ts = dt2 - dt1;
                            pp.Speed = ((long)ts.TotalMilliseconds).ToString();
                            //AddProxyIP(pp);
                        }
                    }
                    catch (Exception ex)
                    {
                        pp.Speed = "超时";
                        AddProxyIP(pp);
                        Trace.WriteLine(ex.Message);
                    }
                }
                else
                {
                    break;
                }
            }
        }
        //private void UpdateItems()
        //{
        //    //retrievedItems is the data you received from the service
        //    //foreach (ProxyIP item in tempProxyList) 
        //       //Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, new ParameterizedThreadStart(AddItem));
        //}

        //private void AddItem()
        //{
        //    //myProxyList.Add(item);
        //}
        //private void IsCheckFinished()
        //{
        //    while (true)
        //    {
        //        Thread.Sleep(4000);
        //        if (tempProxyList.Count == myProxyList.Count)
        //        {
        //            break;
        //        }
        //    }
        //    myProxyList.Clear();
        //    int i = 0;
        //    foreach (ProxyIP p in tempProxyList)
        //    {
        //        p.Index = i.ToString();
        //        myProxyList.Add(p);
        //        i++;
        //    }
        //    tempProxyList.Clear();
        //}
        private void MultiCheckProxy()//多线程验证IP有效性
        {
            foreach (ProxyIP p in myProxyList)
            {
                oldProxyList.Add(p);
            }
            //Thread th = new Thread(IsCheckFinished);
            //th.Start();
            for (int i = 0; i < mySetting.threadnum; i++)
            {
                Thread tt = new Thread(CheckProxyWork);
                myThreadList.Add(tt);
                tt.IsBackground = true;
                tt.Name = i.ToString();
                tt.Start();
            }
        }

  
        private void Button_Click_1(object sender, RoutedEventArgs e)//获取数据
        {
            //new Thread(new ThreadStart(delegate
            //{
            //    msgLabel.Dispatcher.Invoke(new Action(delegate
            //    {
            //        msgLabel.Content = "123";
            //    }), null);            
            //})).Start();
            msgLabel.Content = "正在获取中，请稍后...";
            DispatcherHelper.DoEvents();
            myProxyList.Clear();
            myListView.ItemsSource = null;
            myListView.ItemsSource = myProxyList;
            ProxyHelper proxy=new ProxyHelper();
            string strReturn;
            switch (srcComboBox.SelectedIndex)
            {
                case 0:
                    strReturn=proxy.GetCnproxyFreeIP(myProxyList);
                    break;
                case 1:
                    strReturn=proxy.GetXiciFreeIP(myProxyList, 0);
                    break;
                case 2:
                    strReturn=proxy.GetXiciFreeIP(myProxyList, 1);
                    break;
                case 3:
                    strReturn=proxy.GetXiciFreeIP(myProxyList, 2);
                    break;
                case 4:
                    strReturn=proxy.GetXiciFreeIP(myProxyList, 3);
                    break;
                default:
                    strReturn = "未知错误";
                    break;
            }
            msgLabel.Content = strReturn;
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            GetLocalIP();
        }

        private void msgLabel_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //msgLabel.Content = strMsg;
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)//一键验证
        {
            AbortAllThread();
            msgLabel.Content = "验证时间由网络和电脑情况决定，请耐心等候";
            int threadNum;
            try
            {
                threadNum = Convert.ToInt32(threadTextBox.Text);
            }
            catch
            {
                msgLabel.Content = "线程数必须为1以上500以下的数字！";
                return;
            }
            mySetting.threadnum = threadNum;
            int timeout;
            try
            {
                timeout = Convert.ToInt32(timeOutTextBox.Text);
            }
            catch
            {
                msgLabel.Content = "请输入正确的超时时间！";
                return;
            }
            if (timeout >= 10 && timeout < 99999)
            {
                mySetting.timeout = timeout;
            }
            else
            {
                msgLabel.Content = "请输入正确的超时时间！";
                return;
            }
            if (myProxyList.Count > 0)
            {
                myListView.ItemsSource = null;
                MultiCheckProxy();
                myListView.ItemsSource = myProxyList;
            }
            else
            {
                msgLabel.Content = "没有IP地址可以验证！";
                msgLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            
        }
        public void GetLocalIP() //导入本地IP
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem();
            item.URL = "http://www.ip.cn/";
            try
            {
                HttpResult result = http.GetHtml(item);
                if (!string.IsNullOrEmpty(result.Html))
                {
                    localIPLabel.Content = Search_string(result.Html, "当前 IP：<code>", "</code>");
                    ipAddressLabel.Content = Search_string(result.Html, "来自：", "</p>");
                }
                else
                    localIPLabel.Content = "获取IP失败";
            }
            catch
            {
                localIPLabel.Content = "获取IP失败";
            }

        }
        #region 设置IP代理
        /// <summary>
        /// 设置IP代理
        /// </summary>
        private void SetProxyIP()
        {
            if (myListView.SelectedIndex == -1)
            {
                MessageBox.Show("没有选中列表项！");

            }
            else
            {
                ProxyIP pp = myListView.SelectedItem as ProxyIP;
                if (string.IsNullOrEmpty(pp.IP) || string.IsNullOrEmpty(pp.Port))
                    MessageBox.Show("设置代理失败！");
                else
                {
                    try
                    {
                        ProxyHelper proxy = new ProxyHelper();
                        proxy.SetProxy(pp.IP, pp.Port);
                        MessageBox.Show("设置代理成功！");
                        msgLabel.Content = "设置代理成功，请重启浏览器！";
                    }
                    catch
                    {
                        MessageBox.Show("设置代理失败！");
                    }
                }
            }
        }
        #endregion
        #region 取消IP代理
        /// <summary>
        /// 取消IP代理
        /// </summary>
        private void CancelProxyIP() 
        {
            try
            {
                ProxyHelper proxy = new ProxyHelper();
                proxy.CancelProxy();
                MessageBox.Show("取消代理成功！");
              }
             catch
             {
                MessageBox.Show("取消代理失败！");
             }
        }
        #endregion
        private void AbortAllThread()//终止多线程
        {
            if (myThreadList.Count > 0)
            {
                foreach (Thread tt in myThreadList)
                {
                    if (tt != null)
                    {
                        if (tt.ThreadState == System.Threading.ThreadState.Running || tt.ThreadState == System.Threading.ThreadState.Background)
                        {
                            tt.Abort();
                        }
                    }
                }
                myThreadList.Clear();
            }
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)//一键刷新
        {
            GetLocalIP();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)//设置代理
        {
            SetProxyIP();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)//取消代理
        {
            CancelProxyIP();

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)//设置代理
        {
            SetProxyIP();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)//取消代理
        {
            CancelProxyIP();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)//一键获取
        {
            msgLabel.Content = "正在获取中，请稍后...";
            DispatcherHelper.DoEvents();
            myProxyList.Clear();
            myListView.ItemsSource = null;
            myListView.ItemsSource = myProxyList;
            ProxyHelper proxy = new ProxyHelper();
            string strReturn;
            switch (srcComboBox.SelectedIndex)
            {
                case 0:
                    strReturn = proxy.GetCnproxyFreeIP(myProxyList);
                    break;
                case 1:
                    strReturn = proxy.GetXiciFreeIP(myProxyList, 0);
                    break;
                case 2:
                    strReturn = proxy.GetXiciFreeIP(myProxyList, 1);
                    break;
                case 3:
                    strReturn = proxy.GetXiciFreeIP(myProxyList, 2);
                    break;
                case 4:
                    strReturn = proxy.GetXiciFreeIP(myProxyList, 3);
                    break;
                default:
                    strReturn = "未知错误";
                    break;
            }
            msgLabel.Content = strReturn;
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)//一键验证
        {
            AbortAllThread();//终止多线程
            msgLabel.Content = "验证时间由网络和电脑情况决定，请耐心等候，验证结果请手动刷新！";
            int threadNum;
            try
            {
                threadNum = Convert.ToInt32(threadTextBox.Text);
            }
            catch
            {
                msgLabel.Content = "线程数必须为1以上500以下的数字！";
                return;
            }
            mySetting.threadnum = threadNum;
            int timeout;
            try
            {
                timeout = Convert.ToInt32(timeOutTextBox.Text);
            }
            catch
            {
                msgLabel.Content = "请输入正确的超时时间！";
                return;
            }
            if (timeout >= 10 && timeout < 9999)
            {
                mySetting.timeout = timeout;
            }
            else
            {
                msgLabel.Content = "请输入正确的超时时间！";
                return;
            }
            if (myProxyList.Count > 0)
            {
                myListView.ItemsSource = null;
                MultiCheckProxy();
                myListView.ItemsSource = myProxyList;
            }
            else
            {
                msgLabel.Content = "没有IP地址可以验证！";
                msgLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)//导入文本
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "txt文本|*.txt"; ;
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "txt";
            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            //string fileName = openFileDialog.FileName;
            StreamReader sr = new StreamReader(openFileDialog.FileName, Encoding.Default);
            string strTxt = sr.ReadToEnd();
            sr.Close();
            if (string.IsNullOrEmpty(strTxt))
            {
                msgLabel.Content = "文本内容读取失败！";
            }
            else
            {
                myProxyList.Clear();
                myListView.ItemsSource = null;
                myListView.ItemsSource = myProxyList;
                ProxyHelper proxy = new ProxyHelper();
                string strReturn=proxy.GetLocalIP(myProxyList, strTxt);
                msgLabel.Content = strReturn;
            }
            //Trace.Write(strTxt);
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)//刷新结果
        {
            myListView.ItemsSource = null;
            myListView.ItemsSource = myProxyList;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)//刷新
        {
            myListView.ItemsSource = null;
            myListView.ItemsSource = myProxyList;

        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)//导出文本
        {
            
            if (myProxyList.Count <= 0)
            {
                msgLabel.Content = "当前没有数据！";
                return;
            }
            System.Windows.Forms.FolderBrowserDialog saveFileDialog= new System.Windows.Forms.FolderBrowserDialog();
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = saveFileDialog.SelectedPath;
                filepath =filepath+"\\"+DateTime.Now.ToString("yyyyMMddHHmmssff")+".txt";
                using (StreamWriter sw = new StreamWriter(filepath))
                {
                    foreach (ProxyIP pp in myProxyList)
                    {
                        string strWrite = pp.IP + ":" + pp.Port;
                        sw.WriteLine(strWrite);
                        //Trace.WriteLine(strWrite);
                    }
                    sw.Close();
                }
                msgLabel.Content = "导出数据成功！位置："+filepath;
                Trace.WriteLine(filepath);
            }
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)//一键去除超时项
        {
            myListView.ItemsSource = null;
            tempProxyList.Clear();
            foreach (ProxyIP pp in myProxyList)
            {
                if (pp.Speed != "超时")
                {
                    tempProxyList.Add(pp);
                }
            }
            myProxyList.Clear();
            int index=0;
            foreach (ProxyIP pp in tempProxyList)
            {
                pp.Index = index.ToString();
                myProxyList.Add(pp);
                index++;
            }
            myListView.ItemsSource = myProxyList;
        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            AbortAllThread();
            if ((bool)IsCancelCheckBox.IsChecked)
            {
                try
                {
                    ProxyHelper proxy = new ProxyHelper();
                    proxy.CancelProxy();
                    //MessageBox.Show("取消代理成功！");
                }
                catch
                {
                    //MessageBox.Show("取消代理失败！");
                }
            }
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)//一键去除超时未知项
        {
            AbortAllThread();
            myListView.ItemsSource = null;
            tempProxyList.Clear();
            foreach (ProxyIP pp in myProxyList)
            {
                if (pp.Speed != "超时" && pp.Speed!="未知")
                {
                    tempProxyList.Add(pp);
                }
            }
            myProxyList.Clear();
            int index = 0;
            foreach (ProxyIP pp in tempProxyList)
            {
                pp.Index = index.ToString();
                myProxyList.Add(pp);
                index++;
            }
            myListView.ItemsSource = myProxyList;
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)//去重
        {
            myListView.ItemsSource = null;
            tempProxyList.Clear();
            if (myProxyList.Count > 0)
            {
                int repeatednum = 0;
                bool repeated = false;
                foreach (ProxyIP pp in myProxyList)
                {
                    if (tempProxyList.Count > 0)
                    {
                        foreach (ProxyIP p in tempProxyList)
                        {
                            if (pp.IP == p.IP)
                            {
                                repeatednum++;
                                repeated = true;
                                break;
                            }
                        }
                        if (repeated)
                        {
                            repeated = false;
                        }
                        else
                        {
                            tempProxyList.Add(pp);
                        }
                    }
                    else
                        tempProxyList.Add(pp);
                }
                myProxyList.Clear();
                int index = 0;
                foreach (ProxyIP pp in tempProxyList)
                {
                    pp.Index = index.ToString();
                    myProxyList.Add(pp);
                    index++;
                }
                msgLabel.Content = "去重成功，共为您去除" + repeatednum.ToString() + "个重复IP！";
            }
            else
                msgLabel.Content = "当前列表没有数据，无法去重！";
            myListView.ItemsSource = myProxyList;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)//google 测试
        {
            checkproxy();

        }

        private void threadTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            Regex regex = new Regex(@"[0-8]");
            
            if (!regex.IsMatch(9.ToString()))
            {
                e.Handled = true;
            }
        }

        private void timeOutTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            mySetting.timeout = Convert.ToInt32(timeOutTextBox.Text);
            
        }
       
    }
}
