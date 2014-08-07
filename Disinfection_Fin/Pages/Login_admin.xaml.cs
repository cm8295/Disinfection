using System;
using FirstFloor.ModernUI.Windows.Controls;
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
using System.Xml;
using System.IO;
using System.Data;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.DataForm;
using System.Data.SqlClient;

namespace Disinfection_Fin.Pages
{
    
    /// <summary>
    /// Interaction logic for BasicPage1.xaml
    /// </summary>
    public partial class Login_admin : UserControl
    {
        public Login_admin()
        {
            InitializeComponent();
            Initial();           //初始化                       
            loginbtm.Click += new RoutedEventHandler(admin_click);
            Exitbt.Click += new RoutedEventHandler(admin_exit);
            this.PreviewKeyDown += new KeyEventHandler(Enter_key);           
            LaunchTimer();
            sv.Visibility = Visibility.Collapsed;
            btn_register.Click += new RoutedEventHandler(register_click);
            txt_num.LostFocus += new RoutedEventHandler(txt_num_LostFocus);
            txt_pass.LostFocus += new RoutedEventHandler(txt_pass_LostFocus);
            txt_pass_Copy.LostFocus += new RoutedEventHandler(rpwbox_LostFocus);
            StudentData.ItemsSource = dbc.GetAllStudentInfor().Tables[0].DefaultView;
            AdminData.ItemsSource = dbc.GetAllAdminInfor().Tables[0].DefaultView;
            ExamData.ItemsSource = dbc.GetAllStudentExamInfor().Tables[0].DefaultView;
        }

        #region 定义和初始化
        DatabaseControl dbc = new DatabaseControl();
        DataSet ds = new DataSet();
        DataSet Studentds = new DataSet();
        DataSet Adminds = new DataSet();
        DataSet Examds = new DataSet();
        UserData userdata1 = new UserData();       
        static bool testmodel,      //考试模式false
                    trainmodel;     //训练模式true
        #endregion

        

        public void Initial()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("TrainModel");
            XmlNode xn2 = xn.SelectSingleNode("TestModel");
            if (xn1.Attributes["TM"].Value == "true")
            {
                btn_train.Content = "点击关闭训练模式";
            }
            else
            { btn_train.Content = "点击打开训练模式"; }
            if (xn2.Attributes["tm"].Value == "false")
            {
                btn_test.Content = "点击打开考核模式";
            }
            else
            { btn_test.Content = "点击关闭考核模式"; }
        }
        
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void admin_click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(Environment.CurrentDirectory + @"/Userinformation.mdf") && File.Exists(Environment.CurrentDirectory + @"/Userinformation_log.ldf"))
            {
                if (File.Exists(Environment.CurrentDirectory + @"/SensorDataGet.dll"))
                {
                    DatabaseControl datc = new DatabaseControl();
                    if (datc.Login(uidbox.Text, pwbox.Password, "administrator") == "Success")
                    {
                        svlogin.Visibility = Visibility.Hidden;
                        sv.Visibility = Visibility.Visible; 
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

        /// <summary>
        /// 退出管理员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void admin_exit(object sender, RoutedEventArgs e)
        {
            svlogin.Visibility = Visibility.Visible;
            sv.Visibility = Visibility.Collapsed;
            uidbox.Clear();
            pwbox.Clear();
        }

        private void CheckSearch()
        {
            ds = dbc.Search_stuInfor(this.txt_search.Text);
            dataNew.DataContext = ds;
        }

        /// <summary>
        /// 导出学生信息为Excle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_click(object sender, RoutedEventArgs e)
        {
            try
            {
                ExcelControl xlcon = new ExcelControl();
                if (xlcon.ExportCanExcute(ds))
                {
                    xlcon.ExportExcute(ds.Tables[0]);
                }
            }
            catch
            {
                ModernDialog.ShowMessage("无法导出空的表格！","",MessageBoxButton.OK);
            }
        }        

        /// <summary>
        /// 按键发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enter_key(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                loginbtm.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }

        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addadmin_Click(object sender, RoutedEventArgs e)   
        {
            //admintb.Visibility = Visibility.Hidden;
            Add_Admin.Visibility = Visibility.Visible;
        }


        /// <summary>
        /// 退出应用程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void endbtm_Click(object sender, RoutedEventArgs e)  
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// 显示当前管理员用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void loginbtm_Click(object sender, RoutedEventArgs e)
        {
            // loginbtm.Click += new RoutedEventHandler(admin_click);
            admin_user.Text = uidbox.Text;
            admin_user_Copy.Text = uidbox.Text;
        }
  
        DatabaseControl dbcon = new DatabaseControl();
        BitmapImage errbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\err.png"));
        BitmapImage rightbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\right.png"));
        UserData admindat = new UserData();
        bool bl = false;

        /// <summary>
        /// 增加管理员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void register_click(object sender, RoutedEventArgs e)
        {
            if (admindat.notnull_admin())
            {
                dbcon.AddAdmin(admindat);
                admindat.cleardata();
            }
        }

        /// <summary>
        /// 获取焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_num_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();
            if (((TextBox)sender).Text.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                iderr.Background = imbrash;
            }
            else if (dbcon.CheckSameID_Admin(((TextBox)sender).Text, "IDnumber", "Administrator"))
            {
                imbrash.ImageSource = errbitmap;
                iderr.Background = imbrash;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                iderr.Background = imbrash;
                admindat.addidnum(txt_num.Text.Trim());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_pass_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();
            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                pwerr.Background = imbrash;
                bl = false;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                pwerr.Background = imbrash;
                bl = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rpwbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                repwerr.Background = imbrash;
            }
            else if (((PasswordBox)sender).Password != txt_pass.Password)
            {
                imbrash.ImageSource = errbitmap;
                repwerr.Background = imbrash;
            }
            else
            {
                imbrash.ImageSource = rightbitmap;
                repwerr.Background = imbrash;
                if (bl)
                {
                    admindat.addpassword(txt_pass_Copy.Password.Trim());
                }
            }
        }

        /// <summary>
        /// 返回管理员界面  没用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void return_admin_Click(object sender, RoutedEventArgs e)
        {
            Add_Admin.Visibility = Visibility.Hidden;
        }
        private void Hyperlink2_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.casit.com.cn");
        }

        /// <summary>
        /// 单个查询学生信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)   //
        {
            CheckSearch();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            this.timer0.Text = DateTime.Now.ToLongTimeString();
            this.timer1.Text = DateTime.Now.ToLongTimeString();
        }
        private void LaunchTimer()
        {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);  //
            timer.Tick += new EventHandler(TimerTick);
            timer.Start();
        }

        /// <summary>
        /// 管理员获得焦点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uidbox_Loaded(object sender, RoutedEventArgs e)
        {
            uidbox.Focus();
        }

        /// <summary>
        /// 删除考试模式中用户的过程信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            dbc.DelExam(txt_idnum.Text, "ExamStore");
            MessageBox.Show("删除成功！", "提示");
        }


        /// <summary>
        /// 打开/关闭训练模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("TrainModel");
            if (xn1.Attributes["TM"].Value == "true")
            {
                trainmodel = true;
            }
            else
            {
                trainmodel = false;
            }

            if (trainmodel == false)
            {
                btn_train.Content = "点击关闭训练模式";
                trainmodel = true;
                xn1.Attributes["TM"].Value = "true";
                xd.Save("config.xml");
            }
            else if (trainmodel == true)
            {
                btn_train.Content = "点击打开训练模式";
                trainmodel = false;
                xn1.Attributes["TM"].Value = "false";
                xd.Save("config.xml");
            }
        }

        /// <summary>
        /// 打开/关闭考核模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("TestModel");
            if (xn1.Attributes["tm"].Value == "true")
            {
                testmodel = true;
            }
            else
            {
                testmodel = false;
            }

            if (testmodel == true)
            {
                btn_test.Content = "点击打开考核模式";
                testmodel = false;
                xn1.Attributes["tm"].Value = "false";
                xd.Save("config.xml");
            }
            else if (testmodel == false)
            {
                btn_test.Content = "点击关闭考核模式";
                testmodel = true;
                xn1.Attributes["tm"].Value = "true";
                xd.Save("config.xml");
            }
        }

        //更新dataset数据到数据库方法
        public DataSet Updatedata(DataSet _ds, string tableName)
        {
            string sqltext = "select * from " + tableName;
            SqlConnection con = new SqlConnection(dbc.Dbsource());
            SqlDataAdapter da = new SqlDataAdapter(sqltext, con);
            SqlCommandBuilder cmdb = new SqlCommandBuilder(da);
            try
            {
                if (_ds != null)
                {
                    con.Open();
                    da.Update(_ds, tableName);
                    _ds.AcceptChanges();
                }
                return _ds;
            }
            catch
            {
                MessageBox.Show("更新数据库失败", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                da.Fill(_ds, tableName);
                return _ds;
            }
            finally
            {
                con.Close();
            }
        }

        /// <summary>
        /// 删除用户数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DelUserData(object sender,ItemDeletedEventArgs e)    //删除用户
        {
            Updatedata(Studentds, "student");                
        }
        public void DeleAdminData(object sender,ItemDeletedEventArgs e)   //删除管理员
        {
            Updatedata(Adminds, "Administrator");
        }
    }
}
