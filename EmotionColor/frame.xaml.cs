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
    public sealed partial class FramePage : Page
    {         

        bool loaded = false;
        bool filled = false;
        string generalpath;
        int setName { get; set; }
        int betweentime = 1000;
        bool go = false;
        int frameofset = 50;
        int itemcount = 36;
        int global;
        int CountNeutral;
        int CountPositif;
        int CountNegatif;
        Stopwatch ReactionTimeStop;
        int[] Answers;
        long[] ReactionTime;
        string imagepath;
        BitmapImage[] PreLoadedImages;

        List<int> ImagePositif;
        List<int> ImageNegatif;
        List<int> ImageNeutral;

        List<int> ImagePositifFrame;
        List<int> ImageNegatifFrame;
        List<int> ImageNeutralFrame;

        List<int> GlobalOrder;
        BitmapImage CurrentImageSource;
        SolidColorBrush FrameColorSource;

        public FramePage()
        {
            this.Dispatcher.CurrentPriority =   CoreDispatcherPriority.High;
            imagepath = Package.Current.InstalledLocation.Path;
            generalpath = ApplicationData.Current.LocalFolder.Path;
            setName = new int();
         
            this.InitializeComponent();
            this.DataContext = this;
            ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
            PreLoadedImages = new BitmapImage[itemcount];
            ReactionTime = new long[itemcount];
            FrameColorSource = new SolidColorBrush(Colors.LightGray);
            MainColorFrame.Fill = FrameColorSource; 
            CurrentImageSource = new BitmapImage();
            CurrentImage.Source = CurrentImageSource;
            CoreWindow.GetForCurrentThread().KeyDown += MainPage_KeyDown;
            ReactionTimeStop = new Stopwatch();
            ReactionTimeStop.Reset();
            Answers = new int[itemcount];
            //image frame color

            CurrentImage.Visibility = Visibility.Collapsed;
            MainColorFrame.Visibility = Visibility.Collapsed;
            CenterGrid.Visibility = Visibility.Visible;
        }
        void createSet()
        {
            ImagePositifFrame = new List<int>();
            ImageNegatifFrame = new List<int>();
            ImageNeutralFrame = new List<int>();

            for (int i = 0; i < itemcount / 9; i++)
            {
                for (int z = 0; z < 3; z++)
                {
                    ImagePositifFrame.Add(z);
                    ImageNegatifFrame.Add(z);
                    ImageNeutralFrame.Add(z);
                }
            }
            Shuffle<int>(ImagePositifFrame);
            Shuffle<int>(ImageNegatifFrame);
            Shuffle<int>(ImageNeutralFrame);
            Shuffle<int>(ImagePositifFrame);
            Shuffle<int>(ImageNegatifFrame);
            Shuffle<int>(ImageNeutralFrame);

            //Image order
            ImagePositif = new List<int>();
            ImageNegatif = new List<int>();
            ImageNeutral = new List<int>();
            for (int i = 0; i < itemcount / 3; i++)
            {
                ImagePositif.Add(i);
                ImageNegatif.Add(i);
                ImageNeutral.Add(i);
            }
            Shuffle<int>(ImagePositif);
            Shuffle<int>(ImageNegatif);
            Shuffle<int>(ImageNeutral);

            //Global Order
            GlobalOrder = new List<int>();

            for (int i = 0; i < itemcount / 3; i++)
            {
                for (int z = 0; z < 3; z++)
                {
                    GlobalOrder.Add(z);
                }
            }
            Shuffle<int>(GlobalOrder);
            Shuffle<int>(GlobalOrder);
            Shuffle<int>(GlobalOrder);
            filled = true;

        }
        async Task
preloadImages()
        {

            CountNeutral = 0;
            CountPositif = 0;
            CountNegatif = 0;
            while (!filled)
                await Task.Delay(50);
            StorageFile file;
            for (int i = 0; i < itemcount; i++)
            {
                switch (GlobalOrder[i])
                {
                    case 0:
                        file = await StorageFile.GetFileFromPathAsync( imagepath+@"\Assets\Images\pos\" + ImagePositif[CountPositif++] + ".bmp");
                        break;
                    case 1:
                        file = await StorageFile.GetFileFromPathAsync( imagepath+@"\Assets\Images\neg\" + ImageNegatif[CountNegatif++] + ".bmp");

                        break;

                    case 2:
                        file = await StorageFile.GetFileFromPathAsync( imagepath+@"\Assets\Images\neu\" + ImageNeutral[CountNeutral++] + ".bmp");
                        break;
                    default: file = await StorageFile.GetFileFromPathAsync( imagepath+@"\Assets\Images\neu\" + "0" + ".bmp"); break;

                }
                PreLoadedImages[i] = new BitmapImage();
                await (PreLoadedImages[i] as BitmapImage).SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));

            }
            CountNeutral = 0;
            CountPositif = 0;
            CountNegatif = 0;
            loaded = true;
        }
        static void Shuffle<T>(IList<T> list)
        {

            RandomNumberGenerator provider = RandomNumberGenerator.Create();

            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        async void RunTest()
        {
            switch (GlobalOrder[global])
            {
                case 0:
                    SetFrame(ImagePositifFrame[CountPositif]);
                    SetImage();
                    CountPositif++;
                    break;

                case 1:
                    SetFrame(ImageNegatifFrame[CountNegatif]);
                    SetImage();
                    CountNegatif++;
                    break;

                case 2:
                    SetFrame(ImageNeutralFrame[CountNeutral]);
                    SetImage();
                    CountNeutral++;
                    break;
            }
            global++;
            await Task.Delay(betweentime);
            CenterGrid.Visibility = Visibility.Collapsed;

            MainColorFrame.Visibility = Visibility.Visible;
            await Task.Delay(frameofset);
            ReactionTimeStop.Start();
            CurrentImage.Visibility = Visibility.Visible;
            go = true;


        }

        private void SetImage()
        {

            CurrentImage.Source = PreLoadedImages[global];
        }

        private void SetFrame(int count)
        {
            switch (count)
            {
                case 0: MainColorFrame.Fill = new SolidColorBrush(Colors.Green); break;
                case 1: MainColorFrame.Fill = new SolidColorBrush(Colors.Red); break;
                case 2: MainColorFrame.Fill = new SolidColorBrush(Colors.LightGray); break;
            }
        }

        void SetImage(int FrameType, int ImageType)
        {
            switch (FrameType)
            {
                case 0:

                    break;
                case 1: break;
                case 2: break;
            }
            MainColorFrame.Fill = new SolidColorBrush(Colors.Red);
            MainColorFrame.Visibility = Visibility.Visible;
        }
        private void MainPage_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            if (args.VirtualKey == VirtualKey.Escape)
            { }

            else if (args.VirtualKey == VirtualKey.A /*|| args.VirtualKey == VirtualKey.T */|| args.VirtualKey == VirtualKey.P)
            {
                if (!go)
                {

                    return;
                }
                else
                    go = false;
                ReactionTimeStop.Stop();
                MainColorFrame.Visibility = Visibility.Collapsed;

                FrameColorSource.Color = Colors.LightGray;
                CurrentImage.Visibility = Visibility.Collapsed;
                CenterGrid.Visibility = Visibility.Visible;
                ReactionTime[global - 1] = ReactionTimeStop.ElapsedMilliseconds;



                ReactionTimeStop.Reset();
                switch (args.VirtualKey)
                {

                    case VirtualKey.P:
                        Answers[global - 1] = 0;
                        break;



                    case VirtualKey.A://negatif
                        Answers[global - 1] = 1;
                        break;




                   /* case VirtualKey.T: //Nutral
                        Answers[global - 1] = 2;
                        break;*/


                    case VirtualKey.Escape:
                        break;

                    default: Answers[global - 1] = -1 * (int)args.VirtualKey; break;
                }
                if (global < itemcount)
                    RunTest();
                else
                    EndSequence();
            }
        }
  
        private async void EndSequence()
        {
            ContentDialog savedialog = new ContentDialog
            {
                Title = "Sauvegarder les resultats",
                Content = "Voulez-vous sauvegarder les resultats",
                PrimaryButtonText = "OUI",
                SecondaryButtonText="NON"
            };

            ContentDialogResult result = await savedialog.ShowAsync();

            if (result == ContentDialogResult.Secondary)
            {
                ContentDialog sure = new ContentDialog
                {
                    Title = "Sure?",
                    Content = "Etez-vous sure??",
                    PrimaryButtonText = "OUI",
                    SecondaryButtonText = "NON"
                };

                ContentDialogResult result1 = await sure.ShowAsync();
                if (result1 == ContentDialogResult.Primary) { this.Frame.Navigate(typeof(passation)); return; }
                if (result1 == ContentDialogResult.Secondary)
                {
                    EndSequence(); return;
                }
            }
            
            CountNeutral = 0;
            CountPositif = 0;
            CountNegatif = 0;
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
            StorageFile file = await localFolder.CreateFileAsync(setName.ToString(), CreationCollisionOption.ReplaceExisting);


            using (var stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                // Create the data writer object backed by the in-memory stream.
                using (var dataWriter = new Windows.Storage.Streams.DataWriter(stream))
                {
                    for (int i = 0; i < itemcount; i++)
                    {
                        switch (GlobalOrder[i])
                        {
                            case 0:
                                dataWriter.WriteByte(0);
                                dataWriter.WriteInt32(ImagePositif[CountPositif]);
                                dataWriter.WriteInt32(ImagePositifFrame[CountPositif++]);
                                dataWriter.WriteDouble(ReactionTime[i]);
                                dataWriter.WriteInt32(Answers[i]);
                                break;
                            case 1:
                                dataWriter.WriteByte(1);
                                dataWriter.WriteInt32(ImageNegatif[CountNegatif]);
                                dataWriter.WriteInt32(ImageNegatifFrame[CountNegatif++]);
                                dataWriter.WriteDouble(ReactionTime[i]);
                                dataWriter.WriteInt32(Answers[i]);
                                break;
                            case 2:
                                dataWriter.WriteByte(2);
                                dataWriter.WriteInt32(ImageNeutral[CountNeutral]);
                                dataWriter.WriteInt32(ImageNeutralFrame[CountNeutral++]);
                                dataWriter.WriteDouble(ReactionTime[i]);
                                dataWriter.WriteInt32(Answers[i]);
                                break;


                        }

                    }

                    await dataWriter.StoreAsync();
                    await stream.FlushAsync();

                    this.Frame.Navigate(typeof(passation),1);
                }
            }
        }
        async Task ShowStart() { await pptData.ShowAsync(); }


        private async void pptData_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

            if (age.Text != "")
            {
                pptData.Hide();
                CenterGrid.Visibility = Visibility.Visible;
                // showConsigne();
                while (!loaded)
                    await Task.Delay(50);
                RunTest();

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
            GetLastPpt();
            createSet();
            await preloadImages();

            
            RunTest();
            //"phase test"
            // preTest();
            //  

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
          

        }
        private void pptData_CloseButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            Application.Current.Exit();
        }
    }
}