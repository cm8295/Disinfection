using System.Collections.Generic;
using System.Windows;

namespace Disinfection_Fin
{
    enum Error : int
    {
        LeftToRight = 1,
        TopToBottom = 2,
        DisinfectionArea = 3,
        Pressure = 4
    }
    class DataSave
    {
        List<Point> SaveP = new List<Point>();
        List<double> Pressure = new List<double>();
        public void Add(List<Point> lp)
        {
            SaveP = lp;
        }
        public void Add(List<double> a)
        {
            Pressure = a;
        }
        public void Clear()
        {
            SaveP.Clear();
            Pressure.Clear();
        }
        public List<Point> Getline()
        {
            return SaveP;
        }
        public List<double> Getpressure()
        {
            return Pressure;
        }
    }
    class UserData
    {
        string idnum = null; bool idbl = false;
        string username = null; bool namebl = false;
        string password = null; bool pwbl = false;
        string userclass = null;
        string score = null;
        string time = null;
        public void addidnum(string a)
        {
            idnum = a; idbl = true;
        }
        public void addusername(string b)
        {
            username = b; namebl = true;
        }
        public void addpassword(string c)
        {
            password = c; pwbl = true;
        }
        public void addclass(string d)
        {
            userclass = d;
        }
        public void addscore(string e)
        {
            score = e;
        }
        public void addtime(string f)
        {
            time = f;
        }
        public bool notnull()
        {
            return (idbl && namebl && pwbl);
        }
        public bool notnull_admin()
        {
            return (idbl && pwbl);
        }
        public void cleardata()
        {
            idbl = false; namebl = false; pwbl = false; userclass = null;
        }
        public string getidnum()
        {
            return idnum;
        }
        public string getusername()
        {
            return username;
        }
        public string getpassword()
        {
            return password;
        }
        public string getclass()
        {
            return userclass;
        }
        public string getscore()
        {
            return score;
        }
        public string gettime()
        {
            return time;
        }
    }
    class ResultData
    {
        int LeftToRight = 0;
        int TopToBottom = 0;
        int DisinfectionArea = 0;
        int Pressure = 0;
        int err_num1 = 0,        //面积错误次数
            err_num2 = 0,        //上下消毒方向错误次数
            err_num3 = 0,         //左右消毒方向错误次数
            err_num4 = 0,          //返回清洁区域的错误数
            err_Allnum = 0,       //共画出的消毒痕迹
            err_middle = 0,        //记录没有从伤口处开始消毒
            err_range1 = 0,       //没有达到错误范围数
            err_range2 = 0,       //没有到达耻骨以下的数目
            right_mult = 0,         //两次消毒
            score = 100;                //成绩
        public void ResultDataUpdate(Error e)
        {
            if (e == Error.LeftToRight)
            {
                LeftToRight++;
            }
            if (e == Error.TopToBottom)
            {
                TopToBottom++;
            }
            if (e == Error.DisinfectionArea)
            {
                DisinfectionArea++;
            }
            if (e == Error.Pressure)
            {
                Pressure++;
            }
        }
        public int[] ResultDataReturn()
        {
            int[] Res = new int[] { LeftToRight, TopToBottom, DisinfectionArea, Pressure };
            return Res;
        }
        public void ResultDataClear()   //错误
        {
            LeftToRight = 0;
            TopToBottom = 0;
            DisinfectionArea = 0;
            Pressure = 0;
        }
        public int[] ErrorNum()
        {
            int[] err = new int[]
            {      err_num1 = 0,        //面积错误次数
                   err_num2 = 0,        //上下消毒方向错误次数
                   err_num3 = 0,         //左右消毒方向错误次数
                   err_num4 = 0,          //返回清洁区域的错误数
                   err_Allnum = 0,       //共画出的消毒痕迹
                   err_middle = 0,        //记录没有从伤口处开始消毒
                   err_range1 = 0,       //没有达到错误范围数
                   err_range2 = 0,       //没有到达耻骨以下的数目
                   right_mult = 0,         //两次消毒
                   score = 100                //成绩
            };
            return err;
        }
    }
}