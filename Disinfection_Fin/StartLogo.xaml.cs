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
using System.Windows.Threading;
using System.Threading;
using System.Data;

namespace Disinfection_Fin
{
    /// <summary>
    /// Interaction logic for StartLogo.xaml
    /// </summary>
    public partial class StartLogo : Window
    {
        public StartLogo()
        {
            InitializeComponent();
            this.AllowsTransparency = true;
            probar.IsIndeterminate = true;
            logoimage.Source =new BitmapImage( new Uri(Environment.CurrentDirectory + @"/icon/logo.png"));
            td = new Thread(timerStart);
            td.SetApartmentState(ApartmentState.STA);
            td.IsBackground = true;
            td.Start();

        }
        DispatcherTimer timer = new DispatcherTimer();
        DatabaseControl dbc = new DatabaseControl();
        DataSet ds;
        Thread td;

        void timerStart()
        {
            try
            {
                ds = dbc.GetAllAdminInfor();
                timer.Interval = new TimeSpan(0, 0, 2);
                timer.Tick += new EventHandler(timer_tick);
                timer.Start();
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
                Environment.Exit(0);
            }
        }
        void timer_tick(object sender,EventArgs e)
        {
            if (ds.Tables[0].Columns.Count > 0)
            {
                timer.Stop();
                td.Abort();
                MainWindow mwin = new MainWindow();
                this.Close();
                mwin.Show();
            }
        }
    }
}
