using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using System.Diagnostics;
using Windows.System;
using System.Security.Cryptography;
using Windows.UI;
using Windows.UI.Xaml.Media.Imaging;
using Windows.ApplicationModel;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EmotionColor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class pretest : Page
    {
        BitmapImage[] PreLoadedImages;
        string generalpath;
        System.Random rnd ;
        int global = 0;
        bool run = false;
        Color[] colors = { Colors.Red,
                Colors.Green,
               Colors.Green,
                Colors.LightGray,
                Colors.Red,
                Colors.LightGray};
        public pretest()
        {

            this.InitializeComponent();

            MainColorFrame.Fill = new SolidColorBrush(Colors.LightGray);
            PreLoadedImages = new BitmapImage[6];
            generalpath = Windows.ApplicationModel.Package.Current.InstalledLocation.Path;
            rnd = new Random();
            CurrentImage.Visibility = Visibility.Collapsed;
            MainColorFrame.Visibility = Visibility.Collapsed;
            CenterGrid.Visibility = Visibility.Visible;
            MainColorFrame.Fill = new SolidColorBrush(Colors.LightGray);
            CoreWindow.GetForCurrentThread().KeyDown += MainPage_KeyDown;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await preloadImages();
         
            await RunTest();
        }

        async Task
preloadImages()
        {     
            StorageFile file;
            for (int i = 0; i < 6; i++)
            {
                file = await StorageFile.GetFileFromPathAsync( generalpath+@"\Assets\Images\pre\" +i + ".bmp");
                PreLoadedImages[i] = new BitmapImage();
                await (PreLoadedImages[i] as BitmapImage).SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));                
            }

        }

        async 
        Task
RunTest()
        {
            await Task.Delay(1000);

            MainColorFrame.Visibility = Visibility.Visible;
            CenterGrid.Visibility = Visibility.Collapsed;
         
          MainColorFrame.Fill = new SolidColorBrush(colors[global]); 
              
       
            await Task.Delay(50);
            CurrentImage.Source = PreLoadedImages[global++];
            CurrentImage.Visibility = Visibility.Visible;
            run = true;
        }

        private async Task ContinueTest()
        {
            MainColorFrame.Visibility = Visibility.Collapsed;

            MainColorFrame.Fill = new SolidColorBrush(Colors.LightGray);
            CurrentImage.Visibility = Visibility.Collapsed;
            CenterGrid.Visibility = Visibility.Visible;
            await RunTest();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
     
            base.OnNavigatedTo(e);
        }
        private async void MainPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (run == false)
                return;
         
           

            

            switch (args.VirtualKey)
            {
               
                case VirtualKey.A://negatif
                    run = false;
                    if (global < 6)
                        await ContinueTest();
                    else
                    {
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);

                        this.Frame.GoBack();return; // this.Frame.Navigate(typeof(passation), 1); return;
                    }
                    break;
              



                case VirtualKey.P:
                    run = false;
                    if (global < 6)
                        await ContinueTest();
                    else
                    {
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);

                        this.Frame.GoBack(); return;
                    }
                    break;


             /*   case VirtualKey.T: //Nutral
                    run = false;
                    if (global < 6)
                        await ContinueTest();
                    else
                    {
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);
                        Frame.BackStack.RemoveAt(Frame.BackStackDepth - 1);

                        this.Frame.GoBack(); return;

                    }
                    break;
                    */

                default: break;
            }
            
        }


    }
}
