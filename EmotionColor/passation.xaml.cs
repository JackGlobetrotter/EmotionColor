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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EmotionColor
{


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class passation : Page
    {
        int wait = 0;
        bool loaded = false;
        bool filled = false;
        string generalpath;
        int setName { get; set; }
   
        bool go = false;





        
        public passation()
        {
            
            generalpath = ApplicationData.Current.LocalFolder.Path;
            setName = new int();
            setName = 10;
            this.InitializeComponent();
            this.DataContext = this;
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
   
            CoreWindow.GetForCurrentThread().KeyDown += MainPage_KeyDown;
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            //image frame color





        }


        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (!go)
            {
                if (args.VirtualKey == VirtualKey.Space)
                    wait++;

                return;
            }
       
                
                if(args.VirtualKey== VirtualKey.Escape)
                    return;
            
        }


                async Task ShowStart() { await pptData.ShowAsync(); }


                private async void pptData_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
                {

                    if (age.Text != "") {
                        pptData.Hide();
                        
                        while (!loaded)
                            await Task.Delay(50);
                       

                    }
                }

                private async void pptData_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
                {
                    var dialog = new Windows.UI.Popups.MessageDialog(
                        "Fermer l'appli ou voir les resultats?");

                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Fermer") { Id = 0 });
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Voir Resultats") { Id = 1 });



                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    var result = await dialog.ShowAsync();

                    if ((int)result.Id == 1)
                    {
                        resultview resWindow = new resultview();
                        Window.Current.Content = resWindow;


                    }
                    else if ((int)result.Id == 0)
                        Application.Current.Exit();

                }

                private async void Page_Loaded(object sender, RoutedEventArgs e)
                {
            if (go)
            { await startTest(); return; }
            GetLastPpt();
           await ShowStart();
            loaded = true;
            this.Frame.Navigate(typeof(consigne), 0);
            while (wait == 0)
                await Task.Delay(50);
            this.Frame.Navigate(typeof(consigne), 1);
            while (wait == 1)
                await Task.Delay(50);
            this.Frame.Navigate(typeof(consigne), 2);          
                await Task.Delay(5000);
            CoreWindow.GetForCurrentThread().KeyDown -= MainPage_KeyDown;
            this.Frame.Navigate(typeof(pretest));
            
            //"phase test"
            // preTest();
            //  

        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try {
                if (Convert.ToInt32(e.Parameter) == 1)
                { go = false; loaded = false; this.Frame.BackStack.Clear(); Frame.Navigate(typeof(passation)); }
            }
            catch { }
            if (loaded)
            {
                go = true;
                Task.Run(async () => { await startTest(); });
            }
            
            
           base.OnNavigatedTo(e);
        }

        async Task startTest()
        {
            
            this.Frame.Navigate(typeof(consigne), 3); 
            await Task.Delay(5000);
            this.Frame.Navigate(typeof(FramePage));

        }
        async void GetLastPpt()
                {


                    StorageFolder localFolder;
                    try
                    {
                        localFolder = await StorageFolder.GetFolderFromPathAsync(generalpath + "\\results");
                    }
                    catch
                    {
                        localFolder = await StorageFolder.GetFolderFromPathAsync(generalpath);
                        await localFolder.CreateFolderAsync("results");
                        localFolder = await StorageFolder.GetFolderFromPathAsync(generalpath + "\\results");

                    }
                    IReadOnlyList<StorageFile> files = await localFolder.GetFilesAsync();
                    if (files.Count == 0)
                        setName = 0;
                    else
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (files[i].Name != i.ToString())
                            {
                                setName = i;
                                break;
                            }
                            setName = i + 1;
                        }
                    }
                    age.Text = setName.ToString();

                }
                private void pptData_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
                {
                    Application.Current.Exit();
                }
            }
}