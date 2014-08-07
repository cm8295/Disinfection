/******************************************************************/
/*************  训练模式  ******************/
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

namespace TrainingWin
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
        [DllImport(@"SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetParPressure1();
        [DllImport(@"SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetParPressure2();
        [DllImport(@"SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetParPressure3();
        [DllImport(@"SensorDataGet.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern double GetParPressure4();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern bool InitCOM(int com);
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void UpdateData();
        [DllImport(@"SensorDataGet.dll",CallingConvention=CallingConvention.Cdecl)]
        public static extern void CloseSPort();
        #region parameter
        string username;    //记录学生登陆ID
        ResultData rd = new ResultData();
        DispatcherTimer Timer = new DispatcherTimer();
        DateTime StartTime, EndTime;
        double screeni = 1;        
        List<Point> Linep = new List<Point>();        //存储消毒区域点坐标
        List<Point> LLine = new List<Point>();         //存储左边线坐标
        List<Point> RLine = new List<Point>();         //存储右边线坐标
        List<Point> Pointxy = new List<Point>();         //存储多次消毒中的点坐标
        List<Point> LPoint = new List<Point>();     //存储消毒区域左边的点坐标
        List<Point> RPoint = new List<Point>();     //存储消毒区域右边的点坐标
        List<double> pressurelist, pressurelist1, pressurelist2, pressurelist3, pressurelist4;    //store pressure
        List<DataSave> AllData = new List<DataSave>();
        Queue<double> preLine = new Queue<double>();
        Point startp, Temp_StarpPoint; //暂存开始点;
        bool EnableDraw = false, DiviceReady = false, StartRun = false; 
        int strokethick;
        double CurrentPres = 0,       //暂存分压力
               CurrentPres1 = 0,      //暂存分压力1
               CurrentPres2 = 0,       //暂存分压力2
               CurrentPres3 = 0,         //暂存分压力3
               CurrentPres4 = 0,               //暂存分压力4
               PyTemp = 0,        //暂存keyDown后的y坐标
               PreMax = 0,
               PreMin = 0;
        double re, rf,            //分别存当时的Y坐标和X坐标
               min = SystemParameters.PrimaryScreenWidth - 150, max = 0,
               minx = SystemParameters.PrimaryScreenWidth - 150, maxx = 0,//存第一次消毒单边的坐标
               PreMaxTemp = 0, PreMinTemp = 1000;                         //暂时记录最大压力值和最小压力值
        int L_num = 300, R_num = 930;    //中间画布边界的X坐标
        string Use_time;         //考试用时
        DatabaseControl dbcon = new DatabaseControl();
        UserData userdate = new UserData();
        bool bool_timer = true, timerSet = false;
        string err_num5;
        string err_middle = "",//记录没有从伤口处开始消毒
               err_Left = "",    //左至左侧腋中线
               err_Right = "";    //右至右侧腋中线       

        double w = SystemParameters.PrimaryScreenWidth;      //屏幕分辨率宽
        double h = SystemParameters.PrimaryScreenHeight;    //屏幕分辨率高
        Point navelPxy = new Point();        //存储肚脐坐标
        #endregion        

        private void Initialization()
        {
            //初始化窗体大小  
            /*算法：行：(第一行：h*3/20  第二行：h*17/20)    图片大小为1280*1024  
                   列：（第一列：5 * h * 17 / (4 * 20)）*********/
            double temp1Height = h * 3 / 20;
            double temp2Height = h * 17 / 20;
            double temp1Width = 5 * h * 17 / (4 * 20);
            double temp2Width = w - 5 * h * 17 / (4 * 20); 
            row1.Height = new GridLength(3, GridUnitType.Star);//把屏幕高度分成20份，消毒区域占17份，logo部分占3份 
            row2.Height = new GridLength(17, GridUnitType.Star);
            col1.Width = new GridLength(5 * h * 17 / (4 * 20));
            col2.Width = new GridLength(w - 5 * h * 17 / (4 * 20));
            backg.Width = 5 * h * 17 / (4 * 20);
            backg2.Width = 5 * h * 17 / (4 * 20);
            backg.Height = temp2Height;
            backg2.Height = temp2Height;
            gd11.Height = temp2Height;
            gd11.Width = temp2Width;
            bd1.Width = temp2Width - 15;    //样式的宽
            //bd1.Width = bd2.Width = temp2Width - 15;    //样式的宽
            //RadLine.Width = temp2Width;            //压力曲线宽
            txt_err_explain1.Width = txt_err_explain2.Width = txt_err_explain3.Width = temp2Width - 15;
            sp.Width = temp2Width - 15;
            ActionJudge aj = new ActionJudge();            
            navelPxy = aj.NavelJudge(h, temp1Width);        
            //
            BitmapImage bti = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22.jpg"));
            ImageBrush ib = new ImageBrush(bti);
            BitmapImage btt = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_22_1.png"));
            ImageBrush tt = new ImageBrush(btt);
            backg.Background = ib;
            backg2.Background = tt;            
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
            XmlNode xn3 = xn.SelectSingleNode("LastUserName");
            lbName.Content = xn3.Attributes["name"].Value;
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
        
                       
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //判断纱球是否返回到已消毒的清洁区域
            Point pointxy = Mouse.GetPosition(this);
            re = pointxy.Y;
            rf = pointxy.X;
            try
            {
                COMStart();
                if ((Mouse.GetPosition(this).X > R_num && Mouse.GetPosition(this).X < 1200) || Mouse.GetPosition(this).X < L_num)
                {
                    BitmapImage bit = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_11.jpg"));
                    ImageBrush ib = new ImageBrush(bit);
                    BitmapImage btt = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/Pic/demo_11_1.png"));
                    ImageBrush tt = new ImageBrush(btt);
                    backg.Background = ib;
                    backg2.Background = tt;
                    backg2.Visibility = Visibility.Visible;
                    //c1.Visibility = Visibility.Hidden;
                    c4.Visibility = Visibility.Visible;
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
                    //c2.Visibility = Visibility.Hidden;
                    //c3.Visibility = Visibility.Hidden;
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
            Linep = new List<Point>();
            pressurelist = new List<double>();
            pressurelist1 = new List<double>();
            pressurelist2 = new List<double>();
            pressurelist3 = new List<double>();
            pressurelist4 = new List<double>();            
            if (DiviceReady == true)
            {
                EnableDraw = true;
                startp = e.GetPosition(this);
                Temp_StarpPoint = startp;
                pressurelist.Add(GetFinPressure());
                pressurelist1.Add(GetParPressure1());
                pressurelist2.Add(GetParPressure2());
                pressurelist3.Add(GetParPressure3());
                pressurelist4.Add(GetParPressure4());                
                Linep.Add(startp);
            }
        }

        //根据屏幕做偏移算法
        private Point pointcontrol(Point p)
        {
            if (Temp_StarpPoint.X > L_num && Temp_StarpPoint.X < R_num)
            {
                p.X = (p.X - 240) / 1.3;      //减去相对位置240成为绝对位置
                if (p.Y < 650)             //肚挤坐标为基准，此为肚挤上方
                {
                    p.Y = p.Y / 650 * 426;        //426为肚挤在画布的相对位置，通过比例换算可以得到当前对应位置
                }
                else
                {
                    p.Y = (p.Y - 650) / 374 * 125 + 426;   //肚挤坐标为基准，此为肚挤下方
                }
            }
            else if (Temp_StarpPoint.X < L_num)
            {
                p.X = p.X;      //减去相对位置240成为绝对位置
                if (p.Y < 650)
                {
                    p.Y = p.Y / 650 * 426;
                }
                else
                {
                    p.Y = (p.Y - 650) / 374 * 125 + 426;
                }
            }
            else if (Temp_StarpPoint.X > R_num && Temp_StarpPoint.X < 1200)
            {
                p.X = (p.X - 720) - 170;      //减去相对位置720成为绝对位置      
                if (p.Y < 650)
                {
                    p.Y = p.Y / 650 * 426;
                }
                else
                {
                    p.Y = (p.Y - 650) / 374 * 125 + 426;
                }
            }                     
            return (p);
        }
    
    ///*压力移动获得*/
        public void PreMove()
        {
            if (preLine.Count() >= 200)
            {
                preLine.Dequeue();
                preLine.Enqueue(CurrentPres);
            }
            else
            {
                preLine.Enqueue(CurrentPres);
            }
        }
    /**/

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            PreMove();
            re = Mouse.GetPosition(this).X;
            rf = Mouse.GetPosition(this).Y;
            Positionxy.Text = e.GetPosition(this).X + "  " + e.GetPosition(this).Y;
            if (EnableDraw == true)
            {
                Draw d = new Draw();
                if (re > L_num && re < R_num + 50 && Temp_StarpPoint.X > L_num && Temp_StarpPoint.X < R_num)    //中间
                {
                    if (rf < 1024)
                    {
                        d.DrawLine(c1, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                if (re > R_num - 50 && re < 1200 && Temp_StarpPoint.X > R_num && Temp_StarpPoint.X < 1200)   //右边
                {
                    if (rf < 1024)
                    {
                        d.DrawLine(c2, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                if (re < L_num && Temp_StarpPoint.X < L_num)        //左边
                {
                    if (rf < 1024)
                    {
                        d.DrawLine(c3, strokethick, Colors.SaddleBrown, pointcontrol(startp), pointcontrol(e.GetPosition(this)));
                    }
                }
                else
                { }
                startp = e.GetPosition(this);
                Linep.Add(startp);
                pressurelist.Add(CurrentPres);
                //pressurelist1.Add(CurrentPres1);
                //pressurelist2.Add(CurrentPres2);
                //pressurelist3.Add(CurrentPres3);
                //pressurelist4.Add(CurrentPres4);
                //判断压力是否正确
                if (CurrentPres1 < 300 && CurrentPres3 < 50 && CurrentPres4 < 150)
                {
                }
                if (CurrentPres > PreMaxTemp)
                {
                    PreMaxTemp = CurrentPres;
                }
                else if (CurrentPres < PreMinTemp)
                {
                    PreMinTemp = CurrentPres;
                }
            }
        }

            
        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ActionJudge aj = new ActionJudge();
            DataSave ds = new DataSave();
            ///声音警告链接字符串
            string soundName = new Uri(Environment.CurrentDirectory + @"/TrainingWin/Music/warn.WAV").ToString();
            ///处理压力错误
            if (PreMaxTemp > 50 && PreMaxTemp < 150)
            {
                PreMax++;
            }
            if (PreMinTemp < 0)
            {
                PreMin++;
            }
            PreMaxTemp = 0;
            PreMinTemp = 100;
            ///
            int Canshushezhi = 5;       //
            try
            {
                COMStop();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            
            EnableDraw = false;
            if (Linep.Count > Canshushezhi && AllData.Count == 1)
            {
                if (Linep.Count > Canshushezhi && AllData[0].Getline()[0].X > Linep[0].X)
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
                    Errtb1.Text = "面积错误";
                    string str = "每次消毒请注意消毒同侧的上一条面积重合部分";
                    txt_err_explain1.Text = str;
                    aj.SoundWarningFun(soundName);   //警告
                }
                else
                {
                    Errtb1.Text = "";
                    txt_err_explain1.Text = "";
                }
            }
            if (Linep.Count > Canshushezhi && AllData.Count >= 2)
            {
                try
                {
                    if (!aj.TBSequenceJudge(Linep, AllData))
                    {                        
                        rd.ResultDataUpdate(Error.TopToBottom);
                        Errtb3.Text = "上下错误";
                        string str = "注意：消毒方向只能从同一方向开始，请下一次以第一次消毒方向为准";
                        txt_err_explain3.Text = str;
                        aj.SoundWarningFun(soundName);   //警告
                    }
                    else
                    {
                        Errtb3.Text = "";
                        txt_err_explain3.Text = "";
                    }

                    if (!aj.LRSequenceJudge(Linep, AllData))
                    {                        
                        rd.ResultDataUpdate(Error.LeftToRight);
                        Errtb2.Text = "左右错误";
                        string str = "注意：消毒顺序只能是以第一次为准，一左一右，不能两次都在同一侧";
                        txt_err_explain2.Text = str;
                        aj.SoundWarningFun(soundName);   //警告
                    }
                    else
                    {                        
                        Errtb2.Text = "";
                        txt_err_explain2.Text = "";
                    }

                    if (Linep[0].X > AllData[0].Getline()[0].X)
                    {
                        if (RPoint[RPoint.Count - 1].X - RPoint[RPoint.Count - 2].X > 0)
                        {
                            if (!aj.AreaJudge(Linep, RLine, strokethick))
                            {
                                rd.ResultDataUpdate(Error.DisinfectionArea);
                                Errtb1.Text = "面积错误";
                                string str = "注意：每次消毒请与消毒同侧的上一条面积重合部分";
                                txt_err_explain1.Text = str;
                                aj.SoundWarningFun(soundName);   //警告
                            }
                            else
                            {
                                Errtb1.Text = "";
                                txt_err_explain1.Text = "";
                            }
                        }
                        else
                        {
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb1.Text = "面积错误";
                            string str = "注意：消毒过的区域不能重复消毒";
                            txt_err_explain1.Text = str;
                            aj.SoundWarningFun(soundName);   //警告
                        }
                    }
                    else if (Linep[0].X < AllData[0].Getline()[0].X)
                    {
                        if (LPoint[LPoint.Count - 1].X - LPoint[LPoint.Count - 2].X < 0)
                        {
                            if (!aj.AreaJudge(Linep, LLine, strokethick))
                            {
                                rd.ResultDataUpdate(Error.DisinfectionArea);
                                Errtb1.Text = "面积错误";
                                string str = "注意：每次消毒请与消毒同侧的上一条面积重合部分";
                                txt_err_explain1.Text = str;
                                aj.SoundWarningFun(soundName);   //警告
                            }
                            else
                            {
                                Errtb1.Text = "";
                                txt_err_explain1.Text = "";
                            }
                        }
                        else
                        {
                            rd.ResultDataUpdate(Error.DisinfectionArea);
                            Errtb1.Text = "面积错误";
                            string str = "注意：消毒过的区域不能重复消毒";
                            txt_err_explain1.Text = str;
                            aj.SoundWarningFun(soundName);   //警告
                        }
                    }
                }
                finally
                {
                    if (Linep.Count > Canshushezhi && AllData[0].Getline()[0].X > Linep[0].X)
                    {
                        LLine = Linep;
                    }
                    else
                    {
                        RLine = Linep;
                    }
                }                
            }
            if (Linep.Count > Canshushezhi)
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
            if (AllData.Count >= 1)
            {
                if (e.Key == Key.Escape)
                {
                    timerSet = false;
                    ClearAllData();
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
            Timer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            Timer.Start();
        }

        void Timer_Tick(object sender, EventArgs e)
        {
            UpdateData();
            CurrentPres = GetFinPressure();
            //CurrentPres1 = GetParPressure1();
            //CurrentPres2 = GetParPressure2();
            //CurrentPres3 = GetParPressure3();
            //CurrentPres4 = GetParPressure4();
            tb1.Text = CurrentPres.ToString();
        }
        
        //完成
        private void Finish_Button_Down(object sender, MouseButtonEventArgs e)   
        {
            double dd1, dd2, dd3, dd;    //暂时存储空白部分   
            EndTime = DateTime.Now;
            Use_time =(EndTime - StartTime).Hours.ToString() + "时" + (EndTime - StartTime).Minutes.ToString() + "分" + ((int)(EndTime - StartTime).Seconds).ToString() + "秒";            
            txt_timer.Text = Use_time;
            userdate.addidnum(username);            
            userdate.addtime(Use_time);
            dbcon.updateexamdat(userdate);
            //计算没有消毒成功部分
            BlankCheck blank = new BlankCheck();
            string Pic_Path = AppDomain.CurrentDomain.BaseDirectory;                 
            dd1 = blank.bc_Center(Pic_Path, c1);
            dd2 = blank.bc_Right(Pic_Path, c2);
            dd3 = blank.bc_Left(Pic_Path, c3);
            dd = (dd1 + dd2 + dd3) / 3;            
            tt1.Content = Math.Round(dd, 1).ToString() + "%";
	        StartRun = false;
            //消毒评估结果
            ReportForms report = new ReportForms();   
            report.Show();
            report.txt_Use_times.Text = Use_time;      //考试用时
           
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

        //清除缓冲区中所有暂存数据
        public void ClearAllData()
        {
            c1.Children.Clear();
            c2.Children.Clear();
            c3.Children.Clear();
            AllData.Clear();
            LLine.Clear();
            RLine.Clear();
            Linep.Clear();
            tt1.DataContext = "";
            Errtb3.Text = "";
            Errtb1.Text = "";
            Errtb2.Text = "";
            Positionxy.Text = "";
            err_middle = null;
            err_Left = "";
            err_Right = "";
            tt1.Content = null;
            this.min = 0;
            this.max = 0;
            this.minx = 0;
            this.maxx = 0;
            txt_err_explain1.Text = null;
            txt_err_explain2.Text = null;
            txt_err_explain3.Text = null;
            rd.ResultDataClear();
        }
    }
}

