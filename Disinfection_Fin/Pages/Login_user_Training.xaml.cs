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
using System.Xml;
using System.Threading;
using FirstFloor.ModernUI.Windows.Controls;
using System.Diagnostics;
using System.Data.SqlClient;
namespace Disinfection_Fin.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Login_user_Training : UserControl
    {
        bool bol = false;
        public Login_user_Training()
        {
            InitializeComponent();                     
            loginbtm.Click += new RoutedEventHandler(Login_down);
            this.PreviewKeyDown += new KeyEventHandler(Key_down);
           
        }
        private void Key_down(object sender,KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginbtm.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        private void Login_down(object sender, RoutedEventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn2 = xn.SelectSingleNode("TrainModel");
            if (xn2.Attributes["TM"].Value == "true")
            {
                bol = true;
            }
            else
            { bol = false; }
            if (bol == true)
            {
                if (File.Exists(Environment.CurrentDirectory + @"/Userinformation.mdf") && File.Exists(Environment.CurrentDirectory + @"/Userinformation_log.ldf"))
                {
                    if (File.Exists(Environment.CurrentDirectory + @"/SensorDataGet.dll"))
                    {
                        DatabaseControl datc = new DatabaseControl();
                        if (datc.Login(uidbox.Text, pwbox.Password, "student") == "Success")
                        {
                            try
                            {                                
                                XmlNode xn1 = xn.SelectSingleNode("LastUserName");
                                xn1.Attributes["name"].Value = uidbox.Text;
                                xd.Save("config.xml");
                                Process proc = Process.Start(Environment.CurrentDirectory + @"\TrainingWin\TrainingWin.exe");
                                if (proc != null)
                                {
                                    pwbox.Clear();
                                    proc.WaitForExit();
                                }
                            }
                            catch (ArgumentException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            ModernDialog.ShowMessage("    用户名不存在或密码错误!", "错误", MessageBoxButton.OK);
                            pwbox.Clear();
                        }
                    }
                    else
                    {
                        ModernDialog.ShowMessage("找不到SensorDataGet.dll", "缺少文件", MessageBoxButton.OK);
                    }
                }
                else
                {
                    ModernDialog.ShowMessage("缺少数据库文件！", "缺少文件", MessageBoxButton.OK);                    
                }
            }
            else
            {                
                ModernDialog.ShowMessage("训练模式正处于关闭状态中...", "提示", MessageBoxButton.OK);  
            }
        }


        private void uidbox_Loaded(object sender, RoutedEventArgs e)
        {
            uidbox.Focus();
        }

    }
}
