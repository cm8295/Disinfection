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
using System.Diagnostics;
using FirstFloor.ModernUI.Windows.Controls;

namespace Disinfection_Fin.Pages
{
    /// <summary>
    /// Interaction logic for Login_user_Test.xaml
    /// </summary>
    public partial class Login_user_Test : UserControl
    {
        bool bol = false;
        public Login_user_Test()
        {
            InitializeComponent();
            loginbtm.Click += new RoutedEventHandler(Login_down);
        }
        private void Login_down(object sender, RoutedEventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn2 = xn.SelectSingleNode("TestModel");
            if (xn2.Attributes["tm"].Value == "true")
            {
                bol = true;
            }
            else
            { bol = false; }
            if ( bol == true)
            {
                if (File.Exists(Environment.CurrentDirectory + @"/Userinformation.mdf") && File.Exists(Environment.CurrentDirectory + @"/Userinformation_log.ldf"))
                {
                    if (File.Exists(Environment.CurrentDirectory + @"/SensorDataGet.dll"))
                    {
                        DatabaseControl datc = new DatabaseControl();
                        if (datc.Login(uidbox.Text, pwbox.Password, "student") == "Success")
                        {                            
                            XmlNode xn1 = xn.SelectSingleNode("LastUserName");
                            xn1.Attributes["name"].Value = uidbox.Text;
                            xd.Save("config.xml");
                            Process proc = Process.Start(Environment.CurrentDirectory + @"\ExamWin\ExamWin.exe");
                            if (proc != null)
                            {
                                pwbox.Clear();
                                proc.WaitForExit();
                            }
                        }
                        else
                        {                            
                            ModernDialog.ShowMessage("用户名不存在或密码错误!", "提示", MessageBoxButton.OK);
                            pwbox.Clear();
                        }
                    }
                    else
                    {                        
                        ModernDialog.ShowMessage("找不到SensorDataGet.dll", "提示", MessageBoxButton.OK);

                    }
                }
                else
                {                    
                    ModernDialog.ShowMessage("缺少数据库文件！", "提示", MessageBoxButton.OK);
                }
            }
            else
            {                
                ModernDialog.ShowMessage("考核模式正处于关闭状态中...", "提示", MessageBoxButton.OK);
            }
        }        

        /// <summary>
        /// 获取登陆焦点
        /// </summary>
        private void uidbox_Loaded(object sender, RoutedEventArgs e)
        {
            uidbox.Focus();
        }

        

    }
}
