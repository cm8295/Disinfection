using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Presentation;
using System.Xml;
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

namespace Disinfection_Fin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            setDefaultColor();
        }
        void setDefaultColor()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("LastThemeColor");
            string colorstr = xn1.Attributes["index"].Value;
            string[] colorstrsplit = colorstr.Split(',');
            if (colorstr.Trim().Count() >= 2)
            {
                int rc, gc, bc;
                rc = Convert.ToInt32(colorstrsplit[0]);
                gc = Convert.ToInt32(colorstrsplit[1]);
                bc = Convert.ToInt32(colorstrsplit[2]);
                AppearanceManager.Current.AccentColor = Color.FromRgb((byte)rc, (byte)gc, (byte)bc);
            }
        }


    }
}
