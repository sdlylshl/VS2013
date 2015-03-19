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

using DotNet4.Utilities;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;

namespace IPProxy
{
    public abstract class ModelBase : INotifyPropertyChanged
    {
        SynchronizationContext context;
        public ModelBase(SynchronizationContext _context)
        {
            context = _context;
            OnPropertyChanged = propertyName =>
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    context.Post(t => handler(this, new PropertyChangedEventArgs((string)t)), propertyName);
                }
            };
        }
        public ModelBase()
        {
            OnPropertyChanged = propertyName =>
            {
                PropertyChangedEventHandler handler = this.PropertyChanged;
                if (handler != null)
                {
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected Action<string> OnPropertyChanged;
    }

    public class A : ModelBase
    {
        string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    OnPropertyChanged("Name");
                }
            }
        }
    }//示例代码

    public class ProxyIP : ModelBase
    {
        #region 多线程中异步回调触发
        string _IP;
        public string IP
        {
            get { return _IP; }
            set
            {
                if (_IP != value)
                {
                    _IP = value;
                    OnPropertyChanged("IP");
                }
            }
        }
        string _Port;
        public string Port
        {
            get { return _Port; }
            set
            {
                if (_Port != value)
                {
                    _Port = value;
                    OnPropertyChanged("Port");
                }
            }
        }
        string _Index;
        public string Index
        {
            get { return _Index; }
            set
            {
                if (_Index != value)
                {
                    _Index = value;
                    OnPropertyChanged("Index");
                }
            }
        }
        string _Place="未知";
        public string Place
        {
            get { return _Place; }
            set
            {
                if (_Place != value)
                {
                    _Place = value;
                    OnPropertyChanged("Place");
                }
            }
        }
        string _Anonymity = "未知";
        public string Anonymity
        {
            get { return _Anonymity; }
            set
            {
                if (_Anonymity != value)
                {
                    _Anonymity = value;
                    OnPropertyChanged("Anonymity");
                }
            }
        }
        string _Speed = "未知";
        public string Speed
        {
            get { return _Speed; }
            set
            {
                if (_Speed != value)
                {
                    _Speed = value;
                    OnPropertyChanged("Speed");
                }
            }
        }
        #endregion

        #region 非异步回调
        //public string IP { get; set; }
        //public string Port { get; set; }
        //public string Index { get; set; }
        //private string _Place = "未知";
        //public string Place
        //{
        //    get { return _Place; }
        //    set { _Place = value; }
        //}
        //private string _Anonymity = "未知";
        //public string Anonymity
        //{
        //    get { return _Anonymity; }
        //    set { _Anonymity = value; }
        //}
        //private string _Speed = "未知";
        //public string Speed
        //{
        //    get { return _Speed; }
        //    set { _Speed = value; }
        //}
        #endregion
    }
    /**以上代码其实就是定义了一个ProxyIP类，为什么会怎么麻烦呢？其实这段代码是CSDN一位大神告诉我的，
     我也没看懂，主要是解决多线程操作List数组时发生的错误，因为我的ListView控件的数据源和myProxyList数组
     绑在一起，当后台多线程操作myProxyList数组时就会引发错误,目前依然没有好的解决方法**/
    class ProxyHelper
    {
        private string GetWebHtml(string url)//获取网页源码
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem();
            item.URL = url;
            item.Timeout = 5000;
            try
            {
                HttpResult result = http.GetHtml(item);
                return result.Html;
            }
            catch
            {
                return "";//出错返回空
            }
        }
        public void SetProxy(string ip, string port)//设置代理
        {
            //打开注册表键 
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            //设置代理可用 
            rk.SetValue("ProxyEnable", 1);
            //设置代理IP和端口 
            rk.SetValue("ProxyServer", ip + ":" + port);
            rk.Flush(); //刷新注册表  
            rk.Close();
        }
        public void CancelProxy()//取消代理
        {
            //打开注册表键 
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Internet Settings", true);

            //设置代理不可用 
            rk.SetValue("ProxyEnable", 0);
            rk.Flush(); //刷新注册表  
            rk.Close();
        }
        public string GetLocalIP(List<ProxyIP> myproxylist,string strLocal)//获取本地IP地址
        {
            string pattern = "\\d+\\.\\d+\\.\\d+\\.\\d+:\\d{2,4}";
            if (Regex.IsMatch(strLocal, pattern))
            {
                int i = 0;
                foreach (Match m in Regex.Matches(strLocal, pattern))
                {
                    ProxyIP myproxy = new ProxyIP();
                    string strContent = m.ToString();
                    pattern = "\\d+\\.\\d+\\.\\d+\\.\\d+";
                    if (Regex.IsMatch(strContent, pattern))
                    {
                        myproxy.IP = Regex.Match(strContent, pattern).ToString();
                        strContent = Regex.Replace(strContent, pattern, "");
                        strContent = strContent.Replace(":", "");
                        myproxy.Port = strContent.Trim();
                        myproxy.Index = i.ToString();
                        myproxylist.Add(myproxy);
                        i++;
                    }
                }
                i--;
                string strBack=string.Format("共为您找到{0}个匹配项",i.ToString());
                return strBack;
            }
            else
            {
                return "未能找到匹配项！";
            }
        }
        public string GetCnproxyFreeIP(List<ProxyIP> myproxylist)//正则匹配cn-proxy的ip地址
        {
            string strHtml = GetWebHtml("http://cn-proxy.com/");//国内代理
            if (!string.IsNullOrEmpty(strHtml))
            {
                DateTime dt1 = DateTime.Now;
                string pattern = "<td>\\d+\\.\\d+\\.\\d+\\.\\d+([\\s\\S])*?<div";
                if (Regex.IsMatch(strHtml, pattern))
                {
                    int i = 0;
                    foreach (Match m in Regex.Matches(strHtml, pattern))
                    {
                        ProxyIP myproxy = new ProxyIP();
                        string strContent = m.ToString();
                        pattern = "\\d+\\.\\d+\\.\\d+\\.\\d+";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            myproxy.IP = Regex.Match(strContent, pattern).ToString();
                        }
                        pattern = "<td>(\\d){2,4}</td>";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            string strPort = Regex.Match(strContent, pattern).ToString();
                            strPort = strPort.Replace("<td>", "");
                            myproxy.Port = strPort.Replace("</td>", "");
                        }
                        pattern = "<td>(\\D){2}(\\s){1}(\\D){0,4}</td>";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            string strPlace = Regex.Match(strContent, pattern).ToString();
                            strPlace = strPlace.Replace("<td>", "");
                            strPlace = strPlace.Replace("</td>", "");
                            myproxy.Place = strPlace.Trim();
                        }
                        myproxy.Index = i.ToString();
                        myproxylist.Add(myproxy);
                        i++;
                        //myproxy.speed = mypro.ValidateProxy(myproxy.ip, myproxy.port).ToString();
                        string strOut = string.Format("NO.{0} IP:{1} Port:{2} Anonymity:{3} Place:{4} Speed:{5}", i, myproxy.IP, myproxy.Port, myproxy.Anonymity, myproxy.Place, myproxy.Speed);
                        Trace.WriteLine(strOut);
                    }
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;

                    return "成功获取该网站数据，共获取"+myproxylist.Count.ToString()+"个匹配项，耗时"+((long)ts.TotalMilliseconds).ToString()+"毫秒";
                }
                else
                    return "连接网站成功，但数据内容不匹配！";
            }
            else
            {
                return "网站连接超时，可能是您未连接互联网或者该网站无法打开!";
            }
        }
        public string GetXiciFreeIP(List<ProxyIP> myproxylist, int kind)//正则匹配西刺的IP地址
        {
            string strHtml="";
            switch (kind)
            {
                case 0:
                    strHtml = GetWebHtml("http://www.xici.net.co/nn/");//国内高匿
                    break;
                case 1:
                    strHtml = GetWebHtml("http://www.xici.net.co/nt/");//国内普通
                    break;
                case 2:
                    strHtml = GetWebHtml("http://www.xici.net.co/wn/");//国外高匿
                    break;
                case 3:
                    strHtml = GetWebHtml("http://www.xici.net.co/wt/");//国外普通
                    break;
            }
            if (!string.IsNullOrEmpty(strHtml))
            {
                string pattern = "<tr class=([\\s\\S])*?</tr>";
                if (Regex.IsMatch(strHtml, pattern))
                {
                    int i = 0;
                    DateTime dt1 = DateTime.Now;
                    foreach (Match m in Regex.Matches(strHtml, pattern))
                    {
                        ProxyIP myproxy = new ProxyIP();
                        string strContent = m.ToString();
                        pattern = "\\d+\\.\\d+\\.\\d+\\.\\d+";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            myproxy.IP = Regex.Match(strContent, pattern).ToString();
                        }
                        pattern = "<td>(\\d){2,4}</td>";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            string strPort = Regex.Match(strContent, pattern).ToString();
                            strPort = strPort.Replace("<td>", "");
                            myproxy.Port = strPort.Replace("</td>", "");
                        }
                        pattern = "<td>[\\u4e00-\\u9fa5]{2}</td>";
                        if (Regex.IsMatch(strContent, pattern))
                        {
                            string strAnonymity = Regex.Match(strContent, pattern).ToString();
                            strAnonymity = strAnonymity.Replace("<td>", "");
                            myproxy.Anonymity = strAnonymity.Replace("</td>", "");
                        }
                        if (kind == 2 || kind ==3)
                        {
                            pattern = "<td>\\s{1,}[\\S]{2,12}\\s{1,}</td>";
                            if (Regex.IsMatch(strContent, pattern))
                            {
                                string strPlace = Regex.Match(strContent, pattern).ToString();
                                strPlace = strPlace.Replace("<td>", "");
                                strPlace = strPlace.Replace("</td>", "");
                                myproxy.Place = strPlace.Trim();
                            }
                        }
                        else if(kind==0||kind==1)
                        {
                            pattern = ">[\\u4e00-\\u9fa5]{2,10}</a>";
                            if (Regex.IsMatch(strContent, pattern))
                            {
                                string strPlace = Regex.Match(strContent, pattern).ToString();
                                strPlace = strPlace.Replace(">", "");
                                strPlace = strPlace.Replace("</a", "");
                                myproxy.Place = strPlace.Trim();
                            }
                        }
                        myproxy.Index = i.ToString();
                        myproxylist.Add(myproxy);
                        i++;
                        //myproxy.speed = mypro.ValidateProxy(myproxy.ip, myproxy.port).ToString();
                        string strOut = string.Format("NO.{0} IP:{1} Port:{2} Anonymity:{3} Place:{4} Speed:{5}", i, myproxy.IP, myproxy.Port, myproxy.Anonymity, myproxy.Place,myproxy.Speed);
                        Trace.WriteLine(strOut);
                    }
                    DateTime dt2 = DateTime.Now;
                    TimeSpan ts = dt2 - dt1;

                    return "成功获取该网站数据，共获取" + myproxylist.Count.ToString() + "个匹配项，耗时" + ((long)ts.TotalMilliseconds).ToString() + "毫秒";
                }
                else
                    return "连接网站成功，但数据内容不匹配！";
            }
            else
                return "网站连接超时，可能是您未连接互联网或者该网站无法打开!";
        }
    }
}
