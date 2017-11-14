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
using MyToolkit;
using System.Collections.ObjectModel;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EmotionColor
{
    public enum ColorCode
    {
        Vert,
        Rouge,
        Gris
    }

    public enum EmoCode
    {
        Postitif,
        Négatif,
        Neutre
    }

    public class PPT
    {         //ImageAnswer
        ObservableCollection<PPTData> _posGreen;
        ObservableCollection<PPTData> _posRed;
        ObservableCollection<PPTData> _posGray;

        ObservableCollection<PPTData> _negGreen;
        ObservableCollection<PPTData> _negRed;
        ObservableCollection<PPTData> _negGray;

        ObservableCollection<PPTData> _neuGreen;
        ObservableCollection<PPTData> _neuRed;
        ObservableCollection<PPTData> _neuGray;

        double _meanPosGreen;
        double _meanPosRed;
        double _meanPosGray;

        double _meanNegGreen;
        double _meanNegRed;
        double _meanNegGray;

        double _meanNeuGreen;
        double _meanNeuRed;
        double _meanNeuGray;

        double _ratioNegRedPosGreen;
        double _ratioNegRedNegGreen;
        double _ratioPosGreenPosRed;


        double _diffNegRedPosGreen;
        double _diffNegRedNegGreen;
        double _diffPosGreenPosRed;

        // deviation data
        private ObservableCollection<PPTData> allPPT;
        double _stddev;
        double _globalmean;

        //  double _diffNegRed_

        public ObservableCollection<PPTData> PosGreen { get => _posGreen; set => _posGreen = value; }
        public ObservableCollection<PPTData> PosRed { get => _posRed; set => _posRed = value; }
        public ObservableCollection<PPTData> PosGray { get => _posGray; set => _posGray = value; }
        public ObservableCollection<PPTData> NegGreen { get => _negGreen; set => _negGreen = value; }
        public ObservableCollection<PPTData> NegRed { get => _negRed; set => _negRed = value; }
        public ObservableCollection<PPTData> NegGray { get => _negGray; set => _negGray = value; }
        public ObservableCollection<PPTData> NeuGreen { get => _neuGreen; set => _neuGreen = value; }
        public ObservableCollection<PPTData> NeuRed { get => _neuRed; set => _neuRed = value; }
        public ObservableCollection<PPTData> NeuGray { get => _neuGray; set => _neuGray = value; }
        public double MeanPosGreen { get => _meanPosGreen; set => _meanPosGreen = value; }
        public double MeanPosRed { get => _meanPosRed; set => _meanPosRed = value; }
        public double MeanPosGray { get => _meanPosGray; set => _meanPosGray = value; }
        public double MeanNegGreen { get => _meanNegGreen; set => _meanNegGreen = value; }
        public double MeanNegRed { get => _meanNegRed; set => _meanNegRed = value; }
        public double MeanNegGray { get => _meanNegGray; set => _meanNegGray = value; }
        public double MeanNeuGreen { get => _meanNeuGreen; set => _meanNeuGreen = value; }
        public double MeanNeuRed { get => _meanNeuRed; set => _meanNeuRed = value; }
        public double MeanNeuGray { get => _meanNeuGray; set => _meanNeuGray = value; }
        public ObservableCollection<PPTData> AllPPT { get => allPPT; set => allPPT = value; }
        public double RatioNegRedPosGreen { get => _ratioNegRedPosGreen; set => _ratioNegRedPosGreen = value; }
        public double RatioNegRedNegGreen { get => _ratioNegRedNegGreen; set => _ratioNegRedNegGreen = value; }
        public double RatioPosGreenPosRed { get => _ratioPosGreenPosRed; set => _ratioPosGreenPosRed = value; }

        public PPT() { //initalise collections
            PosGreen = new ObservableCollection<PPTData>();
            PosRed = new ObservableCollection<PPTData>();
            PosGray = new ObservableCollection<PPTData>();

            NegGreen = new ObservableCollection<PPTData>();
            NegRed = new ObservableCollection<PPTData>();
            NegGray = new ObservableCollection<PPTData>();

            NeuGreen = new ObservableCollection<PPTData>();
            NeuRed = new ObservableCollection<PPTData>();
            NeuGray = new ObservableCollection<PPTData>();

            AllPPT = new ObservableCollection<PPTData>();
        }
        public void AddDataSet(PPTData set) {

            AllPPT.Add(set);


        }

        private void SortAnswer(PPTData set)
        {

            switch (set.Type)
            {
                case 0: //Pos
                    if (set.FrameColor == 0) //Green
                    {
                        PosGreen.Add(set);
                        getMean(PosGreen, ref _meanPosGreen);
                    }
                    else if (set.FrameColor == 1)
                    { PosRed.Add(set); getMean(PosRed, ref _meanPosRed); }//Red
                    else
                    { PosGray.Add(set); getMean(PosGray, ref _meanPosGray); }//gray
                    break;

                case 1://Neg

                    if (set.FrameColor == 0) //G
                    { NegGreen.Add(set); getMean(NegGreen, ref _meanNegGreen); }
                    else if (set.FrameColor == 1)
                    { NegRed.Add(set); getMean(NegRed, ref _meanNegRed); }//Red
                    else
                    { NegGray.Add(set); getMean(NegGray, ref _meanNegGray); }//gray
                    break;


                case 2: //Neu

                    if (set.FrameColor == 0) //G
                    { NeuGreen.Add(set); getMean(NeuGreen, ref _meanNeuGreen); }
                    else if (set.FrameColor == 1)
                    { NeuRed.Add(set); getMean(NeuRed, ref _meanNeuRed); }//Red
                    else
                    { NeuGray.Add(set); getMean(NeuGray, ref _meanNeuGray); }//gray
                    break;


            }
        }

        public void calcData(bool delExt)
        {
            getStdDeviation();
            if(delExt)
            getExtrem();
            for (int i = 0; i < AllPPT.Count; i++)
            {
                SortAnswer(AllPPT[i]);

            }
            getRatio();

        }
        void getStdDeviation()
        {
            _globalmean = 0;
            _stddev = 0;
            for (int i = 0; i < AllPPT.Count; i++)
            {
                _globalmean = _globalmean + AllPPT[i].ReactionTime;
            }
            _globalmean = _globalmean / AllPPT.Count;

            for (int i = 0; i < AllPPT.Count; i++)
            {
                _stddev = _stddev + Math.Pow((AllPPT[i].ReactionTime - _globalmean), 2);
            }
            _stddev = Math.Sqrt(_stddev / AllPPT.Count - 1);
        }


        void getExtrem()
        {
            for (int i = 0; i < AllPPT.Count; i++)
            {
                if (AllPPT[i].ReactionTime > _globalmean + 2 * _stddev || AllPPT[i].ReactionTime < _globalmean - 2 * _stddev)

                {
                    Debug.WriteLine(i);
                    Debug.WriteLine(AllPPT[i].ReactionTime + ": " + _globalmean + "+ 2x" + _stddev);
                    AllPPT.RemoveAt(i);
                    i--;
                }
            }

        }

        void getMean(ObservableCollection<PPTData> tosort, ref double mean)
        {
            double somme = 0;
            for (int i = 0; i < tosort.Count; i++)
            {
                somme = somme + tosort[i].ReactionTime;
            }
            mean = somme / tosort.Count;
        }

        void getRatio()
        {
            RatioNegRedNegGreen = (MeanNegRed / MeanNegGreen)*100;
            RatioNegRedPosGreen = (MeanNegRed / MeanPosGreen)*100;
            RatioPosGreenPosRed = (MeanPosGreen / MeanPosRed)*100;

            _diffNegRedNegGreen = MeanNegRed - MeanNegGreen;
            _diffNegRedPosGreen = MeanNegRed - MeanPosGreen;
            _diffPosGreenPosRed = MeanPosGreen - MeanPosRed;

        }

    }
    public class PPTData
    {
        byte _type;
        double _reactionTime;
        int _ImageNumber;
        int _FrameColor;
        int _Answer;

        public double ReactionTime { get => _reactionTime; set => _reactionTime = value; }
        public int ImageNumber { get => _ImageNumber; set => _ImageNumber = value; }
        public int FrameColor { get => _FrameColor; set => _FrameColor = value; }
        public int Answer { get => _Answer; set => _Answer = value; }
        public byte Type { get => _type; set => _type = value; }

        public PPTData(byte t, int Image, int Frame, double RT, int Aswr)
        {
            _type = t;
            _ImageNumber = Image;
            _FrameColor = Frame;
            _reactionTime = RT;
            _Answer = Aswr;

        }
    }


    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class resultview : Page
    {
        ObservableCollection<PPT> generalData;
        string generalpath;

        ObservableCollection<ObservableCollection<PPTData>> _dataSet { get; set; }
        public resultview()
        {
            generalData = new ObservableCollection<PPT>();

            _dataSet = new ObservableCollection<ObservableCollection<PPTData>>();
            this.DataContext = this;
            this.InitializeComponent();
            generalpath = ApplicationData.Current.LocalFolder.Path;

        }


        async Task LoadFiles()
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
            string test = localFolder.Path;
            IReadOnlyList<IStorageFile> files = await localFolder.GetFilesAsync();
            for (int i = 0; i < files.Count; i++)
            {
                generalData.Add(new PPT());
                System.Diagnostics.Debug.WriteLine(files[i].Name);
                using (var stream = await files[i].OpenAsync(FileAccessMode.Read))
                {
                    using (var dataReader = new Windows.Storage.Streams.DataReader(stream))
                    {

                        await dataReader.LoadAsync((uint)stream.Size);
                        while ((dataReader.UnconsumedBufferLength > 0))
                        {
                            generalData[generalData.Count - 1].AddDataSet(new PPTData(dataReader.ReadByte(),
                                                    dataReader.ReadInt32(),
                                                    dataReader.ReadInt32(),
                                                    dataReader.ReadDouble(),
                                                    dataReader.ReadInt32()));
                        }
                    }
                }


                generalData[generalData.Count - 1].calcData(true);
              
            }

            //ResultsList.ItemsSource = results;
            FillListBox();

        }

        void FillbyPPTBox(PPT item)
        {
            int u = 1;
            Grid pptGrid = new Grid();
            pptGrid.Margin = new Thickness(0, 20, 0, 20);
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.ColumnDefinitions.Add(new ColumnDefinition());
            pptGrid.RowDefinitions.Add(new RowDefinition());

            string[] headers = { "Négatif Gris", "Négatif Vert", "Négatif Rouge", "Positif Gris", "Positif Vert", "Positif Rouge" };
            for (int z = 0; z < 6; z++)
            {
                Border bdl = new Border();
                bdl.BorderThickness = new Thickness(2);
                bdl.Background = new SolidColorBrush(Colors.LightGreen);
                bdl.BorderBrush = new SolidColorBrush(Colors.Black);
                TextBlock header = new TextBlock();
                header.Width = 100;
                bdl.Child = header;
                header.Text = headers[z];
                pptGrid.Children.Add(bdl);
                Grid.SetColumn(bdl, z);
                Grid.SetRow(bdl, 0);
            }
            TextBlock[] itm = new TextBlock[6];
            itm[0] = new TextBlock();
            itm[1] = new TextBlock();
            itm[2] = new TextBlock();
            itm[3] = new TextBlock();
            itm[4] = new TextBlock();
            itm[5] = new TextBlock();
            Border bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            pptGrid.RowDefinitions.Add(new RowDefinition());

            itm[0].Text = item.MeanNegGray.ToString("######.##");
            bord.Child = itm[0];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 0);
            Grid.SetRow(bord, u);

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[1].Text = item.MeanNegGreen.ToString("######.##");
            bord.Child = itm[1];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 1);
            Grid.SetRow(bord, u);

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[2].Text = item.MeanNegRed.ToString("######.##");
            bord.Child = itm[2];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 2);
            Grid.SetRow(bord, u);

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[3].Text = item.MeanPosGray.ToString("######.##");
            bord.Child = itm[3];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 3);
            Grid.SetRow(bord, u);

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[4].Text = item.MeanPosGreen.ToString("######.##");
            bord.Child = itm[4];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 4);
            Grid.SetRow(bord, u);

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[5].Text = item.MeanPosRed.ToString("######.##");
            bord.Child = itm[5];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 5);
            Grid.SetRow(bord, u);

            pptGrid.RowDefinitions.Add(new RowDefinition());
            u++;

            string[] headers1 = { "Négatif Rouge / Négatif vert", "Négatif Rouge / Positif Vert", "Positif Vert/ Positif Rouge" };

            for (int z = 0; z < 5; z+=2)
            {
                Border bdl = new Border();
                bdl.BorderThickness = new Thickness(2);
                bdl.Background = new SolidColorBrush(Colors.LightGreen);
                bdl.BorderBrush = new SolidColorBrush(Colors.Black);
                TextBlock header = new TextBlock();
              //  header.Width = 100;
                bdl.Child = header;
                header.Text = headers1[z/2];
                pptGrid.Children.Add(bdl);
                Grid.SetColumn(bdl, z);
                Grid.SetColumnSpan(bdl, 2);
                Grid.SetRow(bdl, u);
            }
         
            u++;
            itm = new TextBlock[3];
            itm[0] = new TextBlock();
            itm[1] = new TextBlock();
            itm[2] = new TextBlock();
            pptGrid.RowDefinitions.Add(new RowDefinition());

            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[0].Text = item.RatioNegRedNegGreen.ToString("######.####");
            bord.Child = itm[0];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 0);
            Grid.SetColumnSpan(bord, 2);
            Grid.SetRow(bord, u);
            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[1].Text = item.RatioNegRedPosGreen.ToString("######.####");
            bord.Child = itm[1];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 2);
            Grid.SetColumnSpan(bord, 2);
            Grid.SetRow(bord, u);
            bord = new Border();
            bord.BorderThickness = new Thickness(2);
            bord.Background = new SolidColorBrush(Colors.LightGray);
            bord.BorderBrush = new SolidColorBrush(Colors.Black);
            itm[2].Text = item.RatioPosGreenPosRed.ToString("######.####");
            bord.Child = itm[2];
            pptGrid.Children.Add(bord);
            Grid.SetColumn(bord, 4);
            Grid.SetColumnSpan(bord, 2);
            Grid.SetRow(bord, u);
            participantsList.Items.Add(pptGrid);
        }
        void FillListBox() //redo
        {
            foreach (PPT item in generalData)
            {
                FillbyPPTBox(item);     
            }
            


        }





        private async void ExportExcel_Click(object sender, RoutedEventArgs e)
        {
           //  int u = 0;
            var savePicker = new Windows.Storage.Pickers.FolderPicker();
            Windows.Storage.StorageFolder localFolder;
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.Desktop;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeFilter.Add("*");
            // Default file name if the user does not type one in or select a file to replace

            Windows.Storage.StorageFolder folder = await savePicker.PickSingleFolderAsync();
            if (folder != null)
            {
                // StorageFolder localFolder;
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
                //string test = localFolder.Path;
                IReadOnlyList<IStorageFile> files = await localFolder.GetFilesAsync();
                for (int i = 0; i < files.Count; i++)
                {
                  await  files[i].CopyAsync(folder);
                  // await newfile.CopyAsync(folder);
                      
                }

             

               
            }
        }

    
      
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private async void MainGrid_Loaded(object sender, RoutedEventArgs e)
        {
           await LoadFiles();
           string test =  generalData[0].ToString();
        }
    }
}
