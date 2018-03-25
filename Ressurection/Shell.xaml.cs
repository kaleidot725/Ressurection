using Ressurection.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ressurection
{
    /// <summary>
    /// Shell.xaml の相互作用ロジック
    /// </summary>
    public partial class Shell : Window
    {
        private NavigationService navi;
        private Page setting;
        private Page version;

        public Shell()
        {
            InitializeComponent();
            this.navi = frame.NavigationService;
            this.setting = new SettingPage();
            this.version = new VersionPage();
            navi.Navigate(setting);
        }

        private void Navigated(object sender, NavigationEventArgs e)
        {
            return;
        }

        private void Navigate(object sender, MouseButtonEventArgs e)
        {
            var textblock = sender as TextBlock;
            RedrawMenuBackground(textblock.Name);

            switch (textblock.Name)
            {
                case "Setting":
                    navi.Navigate(this.setting);
                    break;
                case "Version":
                    navi.Navigate(this.version);
                    break;
            }
        }

        private static Brush nonselectedColor = new SolidColorBrush(Color.FromRgb(0x23, 0x23, 0x23));
        private static Brush selectedColor = new SolidColorBrush(Color.FromRgb(0x2c, 0x2c, 0x30));
        private void RedrawMenuBackground(string textblockName)
        {
//          Setting.Background = nonselectedColor;
//          Version.Background = nonselectedColor;

            switch (textblockName)
            {
                case "Setting":
                    //Setting.Background = selectedColor;
                    break;
                case "Version":
                    //Version.Background = selectedColor;
                    break;
            }
        }
    }
}
