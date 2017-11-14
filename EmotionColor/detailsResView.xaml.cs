using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace EmotionColor
{
    public sealed partial class detailsResView : UserControl
    {
        public detailsResView(double[] Res)
        {
            this.InitializeComponent();
            PVP.Text = Res[0].ToString();
            PVN.Text = Res[1].ToString();
            NVP.Text = Res[2].ToString();
            NVN.Text= Res[3].ToString();

            PRP.Text = Res[4].ToString();
            PRN.Text = Res[5].ToString();
            NRP.Text = Res[6].ToString();
            NRN.Text = Res[7].ToString();

            PN.Text = ((Res[1] + Res[5] )/ 2).ToString();
            NP.Text= ((Res[2] + Res[6]) / 2).ToString();
        }
    }
}
