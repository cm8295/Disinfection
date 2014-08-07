/******************************************************************/
/*************  考核模式  ******************/
/*************  中科信息  *****************/
/*                                       */
/*                                       */
/*                                       */
/*******************************************************************/
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
using System.Xml;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using Disinfection_Fin;

namespace ExamWin
{
    /// <summary>
    /// Interaction logic for TrainingWindow.xaml
    /// </summary>
public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialization();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this.Cursor = Cursors.Arrow;
            this.PreviewMouseDown += new MouseButtonEventHandler(Window_MouseDown);
            this.PreviewMouseMove += new MouseEventHandler(Window_MouseMove);
            this.PreviewMouseUp += new MouseButtonEventHandler(Window_MouseUp);           
            this.Closed += new EventHandler(close_run);
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("LastUserName");
            username = xn1.Attributes["name"].Value;
            Thread td = new Thread(new ThreadStart(TimerStart));
            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start();
        }
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern double COMStart();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern double COMStop();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern double GetFinPressure();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern bool InitCOM(int com);
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void UpdateData();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void CloseSPort();
        #region parameter
        string username;               //记录学生登陆的ID
        ResultData rd = new ResultData();
        DispatcherTimer Timer = new DispatcherTimer();
        DateTime StartTime, EndTime;
        double screeni = 1;        
        List<Point> Linep = new List<Point>();      //存储说有点坐标
        List<Point> LLine = new List<Point>();    //存储左边的线坐标
        List<Point> RLine = new List<Point>();     //存储右边的线坐标
        List<Point> LPoint = new List<Point>();     //存储消毒区域左边的点坐标
        List<Point> RPoint = new List<Point>();     //存储消毒区域右边的点坐标
        List<double> pressurelist;
        List<DataSave> AllData = new List<DataSave>();        
        Point startp;
        bool EnableDraw = false, DiviceReady = false, StartRun = false; int strokethick;
        double CurrentPres = 0,
               PyTemp = 0;            //暂存keyDown后的Y坐标
        int L_num = 310, R_num = 950;    //中间画布边界的X坐标
        string Use_time,         //考试用时
               times;         //用时
        DatabaseControl dbcon = new DatabaseControl();
        UserData userdate = new UserData();
        ResultData rdata = new ResultData();    
        static int err_num1 = 0,        //面积错误次数
                   err_num2 = 0,        //上下消毒方向错误次数
                   err_num3 = 0,         //左右消毒方向错误次数
                   err_num4 = 0,          //返回清洁区域的错误数
                   err_Allnum = 0,       //共画出的消毒痕迹
                   err_middle = 0,        //记录没有从伤口处开始消毒
                   err_range1 = 0,       //没有达到错误范围数
                   err_range2 = 0,       //没有到达耻骨以下的数目
                   right_mult = 0,         //两次消毒
                   score = 100;                //成绩
        string err_num5 = "正确";            //初始时默认第二次消毒范围小于第一次的范围
        string[] ExamErrRate = new string[20];       //考试每项的错误率x%y
        string[] ExamResult = new string[20];        //考试每项评估结果正确/错误        
        
        #endregion
        enum ScreenMode
        {
            NScreen = 1,
            WScreen = 2,
            Screen1 = 3,/*1280*1024  ,  5:4的屏幕   */
            Screen2 = 4,/*1024*768  , 4:3           */
            Screen3 = 5,/*1280*720 ,16:9            */
            Screen4 = 6,/*1920*1080 , 16:9          */
            Screen5 = 7,/*1366*768 ,16:10           */
            Unknow = 0
        }
        private ScreenMode GetScreenResolution(ref double i)//判断屏幕分辨率
        {
            double ScreenWidth = SystemParameters.PrimaryScreenWidth;
            double ScreenHight = SystemParameters.PrimaryScreenHeight;

            if (ScreenHight / 9 == ScreenWidth / 16)
            {
                i = ScreenWidth / 1920;
                return (ScreenMode.WScreen);
            }
            else if (ScreenHight / 3 == ScreenWidth / 4)
            {
                i = ScreenHight / 1200;
                return (ScreenMode.NScreen);
            }
            else if (ScreenHight / 1024 == ScreenWidth / 1280)
            {
                return ScreenMode.Screen1;
            }
            else if (ScreenHight / 768 == ScreenWidth / 1024)
            {
                return ScreenMode.Screen2;
            }
            else if (ScreenHight / 720 == ScreenWidth / 1280)
            {
                return ScreenMode.Screen3;
            }
            else if (ScreenHight / 1080 == ScreenWidth / 1920)
            {
                return ScreenMode.Screen4;
            }
            else if (ScreenHight / 768 == ScreenWidth / 1366)
            {
                return ScreenMode.Screen5;
            }
            else
            {
                return (ScreenMode.Unknow);
            }
        }

        private void Initialization()
        {
            BitmapImage bti = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22.jpg"));
            ImageBrush ib = new ImageBrush(bti);
            BitmapImage btt = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22_1.png"));
            ImageBrush tt = new ImageBrush(btt);
            if (GetScreenResolution(ref screeni) == ScreenMode.NScreen)
            {
                backg.Width = 1000 * screeni; backg.Height = 1000 * screeni;
                backg.Background = ib;
                c1.Height = 534 * screeni; c1.Width = 400 * screeni;
                c1g.Height = 534 * screeni; c1g.Width = 400 * screeni;
                c1g.Margin = new Thickness(506 * screeni, 482 * screeni, 556 * screeni, 289 * screeni);
            }
            else if (GetScreenResolution(ref screeni) == ScreenMode.WScreen)
            {
                backg.Width = 1200 * screeni; backg.Height = 1100 * screeni;
                backg.Background = ib;
                c1.Width = 640 * screeni; c1.Height = 360 * screeni;
                c1g.Width = 640 * screeni; c1g.Height = 360 * screeni;
                c1g.Margin = new Thickness(607 * screeni, 436 * screeni, 667 * screeni, 255 * screeni);
            }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen1)
            {
                backg.Width = 1000; backg.Height = 750;
                backg.Background = ib;
                backg2.Width = 1000; backg2.Height = 750;
                backg2.Background = tt;
                c1.Height = 470; c1.Width = 470;
                c1g.Height = 470; c1g.Width = 470;
                c1g.Margin = new Thickness(0, 0, 0, 0);
                c2.Height = 470; c2.Width = 470;
                c2g.Height = 470; c2g.Width = 470;
                c2g.Margin = new Thickness(0, 0, 0, 0);
                c3.Height = 470; c3.Width = 470;
                c3g.Height = 470; c3g.Width = 470;
            }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen2)
            { }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen3)
            { }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen4)
            { }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen5)   
            {
                backg.Width = 1000; backg.Height = 750;
                backg.Background = ib;
                backg2.Width = 1000; backg2.Height = 750;
                backg2.Background = tt;
                c1.Height = 470; c1.Width = 470;
                c1g.Height = 470; c1g.Width = 470;
                c1g.Margin = new Thickness(0, 0, 0, 0);
                c2.Height = 470; c2.Width = 470;
                c2g.Height = 470; c2g.Width = 470;
                c2g.Margin = new Thickness(0, 0, 0, 0);
                c3.Height = 470; c3.Width = 470;
                c3g.Height = 470; c3g.Width = 470;
            }
            else
            {
                backg.Width = 1000; backg.Height = 750;
                backg.Background = ib;
                backg2.Width = 1000; backg2.Height = 750;
                backg2.Background = tt;
                c1.Height = 470; c1.Width = 470;
                c1g.Height = 470; c1g.Width = 470;
                c1g.Margin = new Thickness(0, 0, 0, 0);
                c2.Height = 470; c2.Width = 470;
                c2g.Height = 470; c2g.Width = 470;
                c2g.Margin = new Thickness(0, 0, 0, 0);
                c3.Height = 470; c3.Width = 470;
                c3g.Height = 470; c3g.Width = 470;
                //c3g.Margin = new Thickness(0, 0, 0, 0);
            }
        }
        private void initdriver()
        {
            XmlDocument xd = new XmlDocument();
            xd.Load("config.xml");
            XmlNode xn = xd.SelectSingleNode("SysConfig");
            XmlNode xn1 = xn.SelectSingleNode("COM");
            XmlNode xn2 = xn.SelectSingleNode("StrokeThickness");
            strokethick = Convert.ToInt32(xn2.Attributes["st"].Value);
            int com = Convert.ToInt32(xn1.Attributes["comnum"].Value);
            if (InitCOM(com))
            {
                DiviceReady = true;
            }
            else
            {
                DiviceReady = false;
                MessageBox.Show("请检查COM口是否配置正确", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }
        double re, rf, min = SystemParameters.PrimaryScreenWidth - 150, max = 0, 
                       minx = SystemParameters.PrimaryScreenWidth - 150, maxx = 0,//存第一次消毒单边的坐标
                       minxx = SystemParameters.PrimaryScreenWidth - 150, maxxx = 0;//存第二次消毒单边的坐标
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //判断纱球是否返回到已消毒的清洁区域
            Point pointxy = Mouse.GetPosition(this);
            re = Mouse.GetPosition(this).Y;
            rf = Mouse.GetPosition(this).X;
            if ((LPoint.Count == 0) && (RPoint.Count == 0))
            {
                LPoint.Add(pointxy);
                RPoint.Add(pointxy);
            }
            else if (rf < LPoint[0].X)
            {
                LPoint.Add(pointxy);
                if (LPoint[LPoint.Count - 1].X < minx)
                {
                    minx = LPoint[LPoint.Count - 1].X;
                    if (right_mult >= 0 && minx > LPoint[LPoint.Count - 1].X)
                    {
                        err_num5 = "错误";
                    }
                }
                if (LPoint[LPoint.Count - 1].X > LPoint[LPoint.Count - 2].X)
                { err_num4++; }
            }
            else if (rf > RPoint[0].X)
            {
                RPoint.Add(pointxy);
                if (RPoint[RPoint.Count - 1].X > maxx)
                {
                    maxx = RPoint[RPoint.Count - 1].X;
                    if (right_mult >= 0 && maxx < RPoint[RPoint.Count - 1].X)
                    { err_num5 = "错误"; }
                }
                if (RPoint[RPoint.Count - 1].X < RPoint[RPoint.Count - 2].X)
                { err_num4++; }
            }
            //判断是两次以上消毒
            if (err_Allnum > 5)
            {
                if ((Math.Abs(LPoint[0].X - LPoint[LPoint.Count - 1].X) < 70 && Math.Abs(LPoint[0].X - minx) > 350) ||
                    (Math.Abs(RPoint[0].X - RPoint[RPoint.Count - 1].X) < 70 && Math.Abs(RPoint[0].X - maxx) > 350))
                {
                    right_mult++;
                    LPoint[0] = pointxy;
                    RPoint[0] = pointxy;
                    //ExamTest();
                    ClearAllData();
                }                  
            }

            txt_err_explain.Text = "";                
            this.PyTemp = re;
            if (re > 70.0 && re < 730)
            {
                err_range1++;
                if (rf > SystemParameters.PrimaryScreenWidth - 150)
                {
                    err_range1--;
                }
            }
            if (rf < min)
            {
                min = rf;
            }
            else if (rf > max)
            {
                max = rf;
            }
            try
            {
                COMStart();
                if ((Mouse.GetPosition(this).X > R_num && Mouse.GetPosition(this).X < SystemParameters.PrimaryScreenWidth - 130) || Mouse.GetPosition(this).X < L_num)
                {
                    BitmapImage bit = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_11.jpg"));
                    ImageBrush ib = new ImageBrush(bit);
                    BitmapImage btt = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_11_1.png"));
                    ImageBrush tt = new ImageBrush(btt);
                    backg.Background = ib;
                    backg2.Background = tt;
                    backg2.Visibility = Visibility.Hidden;
                    c1.Visibility = Visibility.Hidden;
                    c4.Visibility = Visibility.Hidden;
                    c2.Visibility = Visibility.Visible;
                    c3.Visibility = Visibility.Visible;
                }
                else if (Mouse.GetPosition(this).X < R_num && Mouse.GetPosition(this).X > L_num)
                {
                    BitmapImage bit = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22.jpg"));
                    ImageBrush ib = new ImageBrush(bit);
                    BitmapImage btt = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22_1.png"));
                    ImageBrush tt = new ImageBrush(btt);
                    backg.Background = ib;
                    backg2.Background = tt;
                    backg2.Visibility = Visibility.Visible;
                    c1.Visibility = Visibility.Visible;
                    c4.Visibility = Visibility.Visible;
                    c2.Visibility = Visibility.Hidden;
                    c3.Visibility = Visibility.Hidden;
                }
                else
                { }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            if (StartRun == false)
            {
                //记录开始时间
                StartTime = DateTime.Now;     
                StartRun = true;
            }
            Errtb.Text = "";
            Linep = new List<Point>();
            pressurelist = new List<double>();
            if (DiviceReady == true)
            {
                EnableDraw = true;
                startp = e.GetPosition(this);
                pressurelist.Add(GetFinPressure());
                Linep.Add(startp);
            }
        }

        //根据屏幕做偏移设置
        private Point pointcontrol(Point p)     
        {
            if (GetScreenResolution(ref screeni) == ScreenMode.Screen1)   //1280*1024
            {
                if (Mouse.GetPosition(this).X > L_num && Mouse.GetPosition(this).X < R_num)
                {
                    p.X = p.X / 1.45 - 185;
                    p.Y = p.Y / 1.6 + 20;
                }
                else if (Mouse.GetPosition(this).X < L_num)
                {
                    p.X = p.X / 1.4 + 180;
                    p.Y = p.Y / 1.8;
                }
                else if (Mouse.GetPosition(this).X > R_num && Mouse.GetPosition(this).X < SystemParameters.PrimaryScreenWidth - 130)
                {
                    p.X = p.X / 1.4 - 210;
                    p.Y = p.Y / 1.8;
                }
            }
            else if (GetScreenResolution(ref screeni) == ScreenMode.Screen5)  //1366*768
            {
                if (Mouse.GetPosition(this).X > L_num && Mouse.GetPosition(this).X < R_num)
                {
                    p.X = p.X / 1.45 - 185;
                    p.Y = p.Y / 1.6 + 20;
                }
                else if (Mouse.GetPosition(this).X < L_num)
                {
                    p.X = p.X / 1.4 + 180;
                    p.Y = p.Y / 1.8;
                }
                else if (Mouse.GetPosition(this).X > R_num && Mouse.GetPosition(this).X < SystemParameters.PrimaryScreenWidth - 130)
                {
                    p.X = p.X / 1.4 - 210;
                    p.Y = p.Y / 1.8;
                }
            }
            return (p);
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {

            test.Text = e.GetPosition(this).X + "  " + e.GetPosition(this).Y;
            if (EnableDraw == true)
            {
                if (err_Allnum == 0)
                {
                    if (Mouse.GetPosition(this).X < 530 || Mouse.GetPosition(this).X > 700)
                    {
                        err_middle = 100;
                    }
                }
                //UpdateData();
                Draw d = new Draw();
                if (Mouse.GetPosition(this).X > L_num && Mouse.GetPosition(this).X < R_num)
                {
                    if (Mouse.GetPosition(this).Y < 860)
                    {
                        d.DrawLine(c1, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                else if (Mouse.GetPosition(this).X > R_num && Mouse.GetPosition(this).X < SystemParameters.PrimaryScreenWidth - 130)
                {
                    if (Mouse.GetPosition(this).Y < 980)
                    {
                        d.DrawLine(c2, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                else if (Mouse.GetPosition(this).X < L_num)
                {
                    if (Mouse.GetPosition(this).Y < 980)
                    {
                        d.DrawLine(c3, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                else
                { }
                startp = e.GetPosition(this);
                Linep.Add(startp);
                pressurelist.Add(CurrentPres);
            }
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            re = Mouse.GetPosition(this).Y;
            rf = Mouse.GetPosition(this).X;
            if (re > 70.0 && re < 730)
            {
                err_range2++;
                if (rf > SystemParameters.PrimaryScreenWidth - 150)
                {
                    err_range2--;
                }
            }
            if (Mouse.GetPosition(this).X < SystemParameters.PrimaryScreenWidth - 150)
            {
                err_Allnum++;
            }
            try
            {
                COMStop();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            ActionJudge aj = new ActionJudge();
            DataSave ds = new DataSave();
            EnableDraw = false;            
            if (Linep.Count > 5 && AllData.Count == 1)
            {
                if (Linep.Count > 5 && AllData[0].Getline()[0].X > Linep[0].X)
                {
                    RLine = AllData[0].Getline();
                    LLine = Linep;
                }
                else
                {
                    RLine = Linep;
                    LLine = AllData[0].Getline();
                }
                if (!aj.AreaJudge(Linep, AllData[0].Getline(), strokethick))
                {                    
                    rd.ResultDataUpdate(Error.DisinfectionArea);
                    Errtb.Text = Errtb.Text + "面积错误";
                    string str = "注意：每次消毒请注意消毒同侧的上一条面积重合部分";
                    txt_err_explain.Text = str;
                    err_num1++;          
                }
            }
            if (Linep.Count > 5 && AllData.Count >= 2)
            {
                try
                {
                    if (!aj.TBSequenceJudge(Linep, AllData))
                    {                        
                        rd.ResultDataUpdate(Error.TopToBottom);
                        Errtb.Text = Errtb.Text + "上下错误";
                        string str = "注意：消毒方向只能从同一方向开始，请下一次以第一次消毒方向为准";
                        txt_err_explain.Text = str;
                        err_num2++;    
                    }

                    if (!aj.LRSequenceJudge(Linep, AllData))
                    {                        
                        rd.ResultDataUpdate(Error.LeftToRight);
                        Errtb.Text = Errtb.Text + "左右错误";
                        string str = "注意：消毒顺序只能是以第一次为准，一左一右，不能两次都在同一侧";
                        txt_err_explain.Text = str;
                        err_num3++;     
                    }

                    if (Linep[0].X > AllData[0].Getline()[0].X)
                    {
                        if (!aj.AreaJudge(Linep, RLine, strokethick))
                        {
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb.Text = Errtb.Text + "面积错误";
                            string str = "注意：每次消毒请与消毒同侧的上一条面积重合部分";
                            txt_err_explain.Text = str;
                            err_num1++;
                        }
                    }
                    else if (Linep[0].X < AllData[0].Getline()[0].X)
                    {
                        if (!aj.AreaJudge(Linep, LLine, strokethick))
                        {                            
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb.Text = Errtb.Text + "面积错误";
                            string str = "注意：每次消毒请与消毒同侧的上一条面积重合部分";
                            txt_err_explain.Text = str;
                            err_num1++;
                        }
                    }
                }
                finally
                {
                    if (Linep.Count > 5 && AllData[0].Getline()[0].X > Linep[0].X)
                    {
                        RLine = AllData[0].Getline();
                        LLine = Linep;
                    }
                    else
                    {
                        RLine = Linep;
                        LLine = AllData[0].Getline();
                    }
                }                
            }
            if (Linep.Count > 5)
            {
                ds.Add(Linep);
                AllData.Add(ds);
            }            
        }

        //正面面板
        private void c1_Loaded(object sender, RoutedEventArgs e)     
        {
            initdriver();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool bo = false;
            if (AllData.Count >= 1)
            {                
                if (e.Key == Key.Escape && bo == true)
                {
                    c1.Children.Clear();
                    c2.Children.Clear();
                    c3.Children.Clear();
                    AllData.Clear();
                    score = 100;
                    LLine.Clear();
                    RLine.Clear();
                    Linep.Clear();
                    Errtb.Text = "";
                    tb1.Text = "";
                    test.Text = "";
                    err_middle = 0;
                    err_Allnum = 0;
                    err_num1 = 0;
                    err_num2 = 0;
                    err_num3 = 0;
                    err_range1 = 0;
                    err_range2 = 0;
                    tt1.Content = null;
                    rd.ResultDataClear();
                }
            }
        }

        //退出
        private void Button_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            COMStop();            
            Application.Current.Shutdown();
        }
        void TimerStart()
        {
            Timer.Tick += new EventHandler(Timer_Tick);
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            Timer.Start();
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            UpdateData();
            CurrentPres = GetFinPressure();
            tb1.Text = "当前压力：" + CurrentPres.ToString();
        }
        
        //完成
        private void Finish_Button_Down(object sender, MouseButtonEventArgs e)   
        {
            right_mult++;
            DiviceReady = false;
            EnableDraw = false;
            EndTime = DateTime.Now;
            Use_time =(EndTime - StartTime).Hours.ToString() + "时" + (EndTime - StartTime).Minutes.ToString() + "分" + ((int)(EndTime - StartTime).Seconds).ToString() + "秒";
            //times = (EndTime - StartTime).Hours.ToString() + ":" + (EndTime - StartTime).Minutes.ToString() + ":" + ((int)(EndTime - StartTime).Seconds).ToString();
            txt_timer.Text = Use_time;
            userdate.addidnum(username);            
            userdate.addtime(Use_time);
            dbcon.updateexamdat(userdate);
	        StartRun = false;
            ExamTest();
            MessageBox.Show("你已成功提交考试数据", "提示");
        }
        void close_run(object sender, EventArgs e)
        {
            CloseSPort();
        }
        private void c2_Loaded(object sender, RoutedEventArgs e)    //侧面面板
        {
        }

        private void c3_Loaded(object sender, RoutedEventArgs e)
        {            
        }

        /// <summary>
        /// 学员考试过程记录的正误信息存入数据库中
        /// </summary>
        public void ExamTest()
        {            
            //初始化ExamErrRate字符串
            for (int i = 0; i < ExamErrRate.Length; i++)
            {
                ExamErrRate[i] = "空";
            }
            //传入学生编号和名称
            ExamErrRate[0] = username;

            EndTime = DateTime.Now;
            Use_time = (EndTime - StartTime).Hours.ToString() + "时" + (EndTime - StartTime).Minutes.ToString() + "分" + ((int)(EndTime - StartTime).Seconds).ToString() + "秒";            

            //计算没有消毒成功部分
            BlankCheck blank = new BlankCheck();
            string Pic_Path = AppDomain.CurrentDomain.BaseDirectory;
            tt1.Content =((blank.bc(Pic_Path, c3) + blank.bc(Pic_Path,c2) + blank.bc(Pic_Path,c1)) - 10).ToString() + "%";
            
            ExamErrRate[16] = Use_time;    //考试用时
            ExamErrRate[1] = (err_middle / 100).ToString();               //由手术中心向四周消毒         0:向四周消毒  1:错误
            ExamErrRate[2] = (err_num4 + "/" + err_Allnum).ToString();     //消毒纱球返回清洁区域
            ExamErrRate[3] = (err_num3 + "/" + err_Allnum).ToString();   //交替消毒
            ExamErrRate[4] = (err_num2 + "/" + err_Allnum).ToString();   //方向一致
            ExamErrRate[5] = (err_num1 + "/" + err_Allnum).ToString();   //叠瓦状消毒
            ExamErrRate[6] = tt1.Content.ToString();                       //消毒的空白区域
            ExamErrRate[7] = right_mult.ToString();                      //消毒的次数
            ExamErrRate[8] = (err_range1 + "/" + err_Allnum).ToString();  //上至乳头
            ExamErrRate[9] = (err_range2 + "/" + err_Allnum).ToString();   //下至耻骨
            ExamErrRate[12] = err_num5;                                     //两次消毒范围错误
            if (err_range1 > 0)
            {
                ExamResult[8] = "错误";
            }
            if (err_num1 > 0)
            {
                ExamResult[5] = "错误";
            }
            if (err_num2 > 0)
            {
                ExamResult[4] = "错误";
            }
            if (err_num3 > 0)
            {
                ExamResult[3] = "错误";
            }
            if (min > 90 && min < 500)
            {
                ExamErrRate[10] = "正确";      //记录左侧液中下
            }
            else
            {
                ExamResult[10] = "错误";
                ExamErrRate[10] = "100/100";
            }

            if (max > 950 && max < SystemParameters.PrimaryScreenWidth - 150)
            {
                ExamErrRate[11] = "正确";       //记录右侧液中下
            }
            else
            {
                ExamResult[11] = "错误";
                ExamErrRate[11] = "100/100";
            }
            for (int i = 0,j = 0; i < ExamErrRate.Length; i++)
            {
                if (ExamErrRate[i] != "空")
                {
                    j++;                                        
                }
                if (j > 5)
                {
                    ExamErrRate[17] = "不合格";
                    break;
                }
            }
            dbcon.StoreExamDataToSql(ExamErrRate);
        }

      //清除缓冲区中所有暂存数据
        public void ClearAllData()
        {
            c1.Children.Clear();
            c2.Children.Clear();
            c3.Children.Clear();
            AllData.Clear();
            score = 100;
            LLine.Clear();
            RLine.Clear();
            Linep.Clear();
            Errtb.Text = "";
            tb1.Text = "";
            test.Text = "";
            err_middle = 0;
            err_Allnum = 0;
            err_num1 = 0;
            err_num2 = 0;
            err_num3 = 0;
            err_num4 = 0;
            err_range1 = 0;
            err_range2 = 0;
            tt1.Content = null;
            rd.ResultDataClear();
        } 
    }
}

