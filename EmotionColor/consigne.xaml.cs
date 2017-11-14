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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EmotionColor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class consigne : Page
    {
        int i = 0;


        string[] strings =
        {"Vous allez voir une série d'images. " +
                "\\nPour chaque image vous devrez évaluer le plus vite possible le sentiment qu'elle vous évoque :  " +
                "\\nsoit un sentiment positif, soit un sentiment négatif."+
"\\nVeuillez poser vos doigts sur les lettres A et P du clavier. " +
                "\\nVous devez poser un doigt de la main gauche sur la lettre A et un doigt de la main droit sur la lettre P."+
                "\\n\\nAppuyez sur Espace pour continuer.",
                "Fixez du regard la croix située au milieu de l'écran qui apparaitra avant chaque image."+
                "\\nAppuyez sur A lorsque vous pensez que l'image est négative." +
                "\\nAppuyez sur P lorsque vous pensez que l’image est positive."+
                "\\nVous aurez une séquence de 6 images pour vous entrainer."+
                "\\nRépondez le plus vite possible."+
"\\n\\nAppuyez sur la touche Espace si vous avez compris et que vous êtes prêt à commencer l'entrainement. ",

"Test d'entrainement",

"Le test va commencer."+
"\\nRépondez le plus vite possible."


         
        };
        public consigne()
        {
            this.InitializeComponent();
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainText.Text = "";
            AddText(Convert.ToInt32(e.Parameter));
            base.OnNavigatedTo(e);
        }
        public void AddText(int i)
        {

            foreach (string t in strings[i].Split(new string[]{ "\\n"},StringSplitOptions.None))
                {
                    MainText.Text = MainText.Text + Environment.NewLine + t;
                    
                }
               
            

        }    
    }
}
