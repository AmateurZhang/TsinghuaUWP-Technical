using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ClassRoomAPI.Models;
using ClassRoomAPI.ViewModels;
using ClassRoomAPI.Controls;
using Windows.Storage.Streams;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace ClassRoomAPI.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Performance : Page
    {
        public Performance()
        {
            this.InitializeComponent();

        }

        
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //加载背景
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/Hall.png", UriKind.Absolute));
            Performance_Page.Background = imageBrush;
            //创建和自定义 FileOpenPicker  
            //var picker = new Windows.Storage.Pickers.FileOpenPicker();
            //picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail; //可通过使用图片缩略图创建丰富的视觉显示，以显示文件选取器中的文件  
            //picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            //picker.FileTypeFilter.Add(".jpg");
            //picker.FileTypeFilter.Add(".jpeg");
            //picker.FileTypeFilter.Add(".png");
            //picker.FileTypeFilter.Add(".gif");

            ////选取单个文件  
            //Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            ////文件处理  
            //if (file != null)
            //{
            //    IRandomAccessStream ir = await file.OpenAsync(FileAccessMode.Read);
            //    BitmapImage bi = new BitmapImage();
            //    await bi.SetSourceAsync(ir);
            //    imageHidden.Source = bi;
            //}
            try
            {
                var _DataLocal = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Demo);
                
                if ((_DataLocal.Date.Date - DateTime.Now.Date).Days < 0)
                    throw new Exception("The Data are out-of-date.");
                else
                    MainPivot.ItemsSource = _DataLocal.ListShowInfo;
                
                var notifyPopup = new NotifyPopup("数据更新于"+ _DataLocal.Date);
                notifyPopup.Show();


                //foreach (ShowInfo items in _DataLocal.ListShowInfo)
                //{

                //    if (items.ListPerformanceInfo.Count == 0)
                //    {
                //        reloaded..Add(new PerformanceType { Type = items.PerformanceType, isEmpty = true });
                //    }
                //}


            }
            catch
            {
                try
                {
                    var _DataRemote = await PerformaceViewModels.GetHallInfoViewModel(ParseDataMode.Demo);
                    MainPivot.ItemsSource = _DataRemote.ListShowInfo;
                    var notifyPopup = new NotifyPopup("演出信息已更新至最新状态！");
                    notifyPopup.Show();
                }
                catch
                {
                    var notifyPopup = new NotifyPopup("网络异常，刷新失败。");
                    notifyPopup.Show();
                }

            }
        }


        private void MainPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //PerformanceData.SelectedItem = MainPivot.SelectedIndex;
            //if (MainPivot.SelectedIndex == 0 && )
            //{
            //    var isEmptyPopup = new NotifyPopup("暂时没有电影相关信息！\n请扫码关注公众号获取最新信息");
            //    isEmptyPopup.SetHorizonAlignment(400);
            //    isEmptyPopup.Show();
            //}
            //if (MainPivot.SelectedIndex == 1 && MainPivot.Items.Count == 0)
            //{
            //    var isEmptyPopup = new NotifyPopup("暂时没有演出相关信息！\n请扫码关注公众号获取最新信息");
            //    isEmptyPopup.SetHorizonAlignment(400);
            //    isEmptyPopup.Show();
            //}
            //if (MainPivot.SelectedIndex == 2 && MainPivot.Items.Count == 0)
            //{
            //    var isEmptyPopup = new NotifyPopup("暂时没有讲座相关信息！\n请扫码关注公众号获取最新信息");
            //    isEmptyPopup.SetHorizonAlignment(400);
            //    isEmptyPopup.Show();
            //}
            //if (MainPivot.SelectedIndex == 3 && MainPivot.Items.Count == 0)
            //{
            //    var isEmptyPopup = new NotifyPopup("暂时没有电影艺术丛林信息！\n请扫码关注公众号获取最新信息");
            //    isEmptyPopup.SetHorizonAlignment(400);
            //    isEmptyPopup.Show();
            //}
        }
    }

    

}
