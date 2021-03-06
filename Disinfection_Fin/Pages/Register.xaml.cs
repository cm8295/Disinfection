﻿using System;
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

namespace Disinfection_Fin.Pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : UserControl
    {
        public Register()
        {
            InitializeComponent();
            registerbt.Click += new RoutedEventHandler(register_click);
            IDbox.LostFocus += new RoutedEventHandler(IDbox_LostFocus);
            Namebox.LostFocus += new RoutedEventHandler(Namebox_LostFocus);
            pwbox.LostFocus += new RoutedEventHandler(pwbox_LostFocus);
            rpwbox.LostFocus += new RoutedEventHandler(rpwbox_LostFocus);
            otherbox.LostFocus += new RoutedEventHandler(otherbox_LostFocus);
        }
        DatabaseControl dbcon = new DatabaseControl();
        BitmapImage errbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\err.png"));
        BitmapImage rightbitmap = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\icon\right.png"));
        UserData userdat = new UserData();
        bool bl = false; Canregeister canregister;
        struct Canregeister
        {
            public bool IDcanregister;
            public bool PWcanregister;
            public bool Namecanregister;
            public bool rpcanregister;
            public bool canregister()
            {
                return (IDcanregister && PWcanregister && Namecanregister && rpcanregister);
            }
        }
        private void register_click(object sender, RoutedEventArgs e)
        {
            if (canregister.canregister() == true)
            {
                if (userdat.notnull())
                {
                    dbcon.AddUser(userdat);
                    userdat.cleardata();
                }
                clear();
            }
            else
            {
                clear();
            }
        }
        private void clear()
        {
            IDbox.Text = "";
            Namebox.Text = "";
            pwbox.Clear();
            rpwbox.Clear();
            otherbox.Text = "";
            iderr.Background = null;
            nameerr.Background = null;
            pwerr.Background = null;
            rpwerr.Background = null;
        }
        private void IDbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((TextBox)sender).Text.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                canregister.IDcanregister = false;
                iderr.Background = imbrash;
            }
            else if (dbcon.CheckSameID(((TextBox)sender).Text, "IDnumber", "student"))
            {
                imbrash.ImageSource = errbitmap;
                canregister.IDcanregister = false;
                iderr.Background = imbrash;
            }
            else
            {
                canregister.IDcanregister = true;
                imbrash.ImageSource = rightbitmap;
                iderr.Background = imbrash;
                userdat.addidnum(IDbox.Text.Trim());
            }

        }

        private void Namebox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((TextBox)sender).Text.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                canregister.Namecanregister = false;
                nameerr.Background = imbrash;
            }
            else
            {
                canregister.Namecanregister = true;
                imbrash.ImageSource = rightbitmap;
                nameerr.Background = imbrash;
                userdat.addusername(Namebox.Text.Trim());
            }
        }

        private void pwbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                canregister.PWcanregister = false;
                pwerr.Background = imbrash;
                bl = false;
            }
            else
            {
                canregister.PWcanregister = true;
                imbrash.ImageSource = rightbitmap;
                pwerr.Background = imbrash;
                bl = true;
            }
        }

        private void rpwbox_LostFocus(object sender, RoutedEventArgs e)
        {
            ImageBrush imbrash = new ImageBrush();

            if (((PasswordBox)sender).Password.Trim().Length < 1)
            {
                imbrash.ImageSource = errbitmap;
                canregister.rpcanregister = false;
                rpwerr.Background = imbrash;
            }
            else if (((PasswordBox)sender).Password != pwbox.Password)
            {
                imbrash.ImageSource = errbitmap;
                canregister.rpcanregister = false;
                rpwerr.Background = imbrash;
            }
            else
            {
                canregister.rpcanregister = true;
                imbrash.ImageSource = rightbitmap;
                rpwerr.Background = imbrash;
                if (bl)
                {
                    userdat.addpassword(rpwbox.Password.Trim());
                }
            }
        }

        private void otherbox_LostFocus(object sender, RoutedEventArgs e)
        {
            userdat.addclass(((TextBox)sender).Text.Trim());
        }


    }
}
