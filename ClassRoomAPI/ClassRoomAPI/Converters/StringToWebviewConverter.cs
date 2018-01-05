using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ClassRoomAPI.Converters
{
    // Refer to https://www.cnblogs.com/T-ARF/p/6430787.html
    public class StringToWebviewConverter
    {
        //注册一个附加属性，括号内的参数，分别是 要添加的附加属性的名字，附加属性的类型，附加属性的所有者，附加属性的元数据，并且创建一个返回
        public static readonly DependencyProperty setHtmlString = DependencyProperty.RegisterAttached
            (
            "HtmlString",

            typeof(string),

           typeof(StringToWebviewConverter),

           new PropertyMetadata(null, CallBack)
            );

        //这里对你要控件的操作
        private static void CallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //将附加对象进行强制转换
            WebView setData = d as WebView;
            //添加值
            setData.NavigateToString(e.NewValue.ToString());
        }
        //读取静态方法
        public static string GetHtmlString(DependencyObject D)
        {
            return (string)D.GetValue(setHtmlString);
        }
        // 赋值静态方法
        public static void SetHtmlString(DependencyObject D, string HtmlString)
        {
            D.SetValue(setHtmlString, HtmlString);
        }
    }
}
