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
using System.IO;
using System.IO.Ports;
using System.Xml;
using FirstFloor.ModernUI.Windows.Controls;
namespace Disinfection_Fin.Pages
{    
    /// <summary>
    /// Interaction logic for ComStrokeConfig.xaml
    /// </summary>    
    public partial class ComStrokeConfig : UserControl
    {
        static public double MillimetreToPixel = 3.78;    //毫米转像素参数
        public ComStrokeConfig()
        {
            InitializeComponent();
            LoadXML();
            string[] ports = SerialPort.GetPortNames();
            foreach (string s in ports)
            {
                ComboBoxItem cbi = new ComboBoxItem();
                cbi.Content = s;
                cbi.Selected += new RoutedEventHandler(cb_Change);
                comcb.Items.Add(cbi);
            }
        }
        int com; XmlDocument xd = new XmlDocument();
        void LoadXML()
        {
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            tb.Text = xn2.Attributes["st"].Value;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("COM");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            xn1.Attributes["comnum"].Value = com.ToString();
            xn2.Attributes["st"].Value = tb.Text;
            xd.Save("config.xml");
            MessageBox.Show("修改成功");
        }
        private void cb_Change(object sender, RoutedEventArgs e)
        {
            string comstr = ((ComboBoxItem)sender).Content.ToString().Substring(3,1);
            com = Convert.ToInt32(comstr);
        }
        /* private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
      {
          com = System.Convert.ToInt32(((ComboBoxItem)sender).Content.ToString().Substring(3));
      }
      void LoadXML()
      {
          xd.Load("config.xml");
          XmlNode xn = xd.SelectSingleNode("SysConfig");
          XmlNode xn1 = xn.SelectSingleNode("COM");
          XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
          tb.Text = xn2.Attributes["st"].Value;
          com = System.Convert.ToInt32(xn1.Attributes["comnum"].Value);
          COMcb.SelectedIndex = com - 1;
      }*/

    }
}
