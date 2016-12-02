using System;
using System.Collections.Generic;
using System.Linq;

namespace MPT.WorkSchedule
{
    public enum Smena
    {
        Day = 0,
        A = 3,
        B = 5,
        C = 1,
        D = 7,
    }

    public class ScheduleDay
    {
        public class SmenaDay
        {
            public bool IsNight = false;
            public int Hours = 8;
            public string BgColor = "#ffffff";
        }



        DateTime _day;
        
        /*
        public bool IsNightA;
        public bool IsNightB;
        public bool IsNightV;
        public bool IsNightG;

        public int hour_D;
        public int hour_A;
        public int hour_B;
        public int hour_V;
        public int hour_G;

        public string bgColor = "#ffffff";
        public string bgColor_A = "#ffffff";
        public string bgColor_B = "#ffffff";
        public string bgColor_V = "#ffffff";
        public string bgColor_G = "#ffffff";
        */

        readonly Dictionary<Smena, SmenaDay> _smenaList = new Dictionary<Smena, SmenaDay>()
                        {
                            {Smena.Day, new SmenaDay()},
                            {Smena.A,   new SmenaDay()},
                            {Smena.B,   new SmenaDay()},
                            {Smena.C,   new SmenaDay()},
                            {Smena.D,   new SmenaDay()},
                        };

        public SmenaDay this[Smena key]
        {
            get { return _smenaList[key]; }
        }

        public ScheduleDay(DateTime day)
        {
            _day = day;
            Init(); 
        }

        public ScheduleDay(int year, int month, int day) : this(new DateTime(year, month, day))
        {
        }

        public void Init()
        {
            //this.dayNum = this.day.get(Calendar.DAY_OF_MONTH);
            //this.dayName = arrayDN[this.day.get(Calendar.DAY_OF_WEEK)];
            //this.holiday =   CheckHoliday(this.day.get(Calendar.DAY_OF_MONTH),this.day.get(Calendar.MONTH));

            /*this.restday = (this.day.get(Calendar.DAY_OF_WEEK)==Calendar.SUNDAY)||
                    (this.day.get(Calendar.DAY_OF_WEEK)==Calendar.SATURDAY);
            */

            /*
            Calendar c = Calendar.getInstance();
            c.setTime(this.day.getTime());
            c.add(Calendar.DAY_OF_MONTH, 1);
            this.preHoliday =   CheckHoliday(c.get(Calendar.DAY_OF_MONTH),c.get(Calendar.MONTH));
            */

            foreach (var smenaDay in _smenaList)
            {
                if (smenaDay.Key == (int) Smena.Day)
                {
                    if (IsPreHoliday)
                        smenaDay.Value.Hours = 7;
                    if (IsRestday || IsHoliday)
                        smenaDay.Value.Hours = 0;

                    if (IsRestday)
                        smenaDay.Value.BgColor = "#f08080";
                    if (IsHoliday)
                        smenaDay.Value.BgColor = "#ff69b4";
                    if (IsPreHoliday && !IsRestday)
                        smenaDay.Value.BgColor = "#f0e68c";

                    continue;
                }

                var diff1 = GetDiffSh(_day, (int)smenaDay.Key);
                smenaDay.Value.Hours = SmenaHoursArray[diff1];
                smenaDay.Value.IsNight = SmenaNightArray[diff1];
                if (smenaDay.Value.IsNight)
                    smenaDay.Value.BgColor = "#d3d3d3";
            }

            


            /*
            hour_D = 8;
            if (IsRestday)
                hour_D = 0;
            else if (IsPreHoliday)
                hour_D = 7;
            else if (IsHoliday)
                hour_D = 0;
            
            if (IsRestday)
                bgColor = "#f08080";
            if (IsHoliday)
                bgColor = "#ff69b4";
            if (IsPreHoliday && !IsRestday)
                bgColor = "#f0e68c";
            */
            

            /*
            var diff = GetDiffSh(3);
            hour_A = ArraySmenaHours[diff];

            IsNightA = ArraySmenaNight[diff];
            if (IsNightA)
                bgColor_A = "#d3d3d3";

            diff = GetDiffSh(5);
            hour_B = ArraySmenaHours[diff];
            IsNightB = ArraySmenaNight[diff];
            if (IsNightB)
                bgColor_B = "#d3d3d3";

            diff = GetDiffSh(1);
            hour_V = ArraySmenaHours[diff];
            IsNightV = ArraySmenaNight[diff];
            if (IsNightV)
                bgColor_V = "#d3d3d3";

            diff = GetDiffSh(7);
            hour_G = ArraySmenaHours[diff];
            IsNightV = ArraySmenaNight[diff];
            if (IsNightG)
                bgColor_G = "#d3d3d3";
             * */
        }

        public int Day
        {
            get { return _day.Day; }
        }

        public string DayName
        {
            get { return _day.ToString("ddd"); }
        }

        public int Month
        {
            get { return _day.Month; }
        }

        public bool IsHoliday
        {
            get { return CheckHoliday(_day); }
        }

        public bool IsRestday
        {
            get { return _day.DayOfWeek == DayOfWeek.Saturday || _day.DayOfWeek == DayOfWeek.Sunday; }
        }

        public bool IsPreHoliday
        {
            get { return CheckHoliday(_day.AddDays(1)); }
        }

        public override string ToString()
        {
            return _day.ToShortDateString();
        }


        public static readonly int[] SmenaHoursArray = { 12, 12, 0, 4, 12, 8, 0, 0, 0 };
        public static readonly bool[] SmenaNightArray = { false, false, false, true, true, true, false, false, false };

        public static int GetDiffSh(DateTime day, int firstDay)
        {
            /*
            установка часов для смен
            . начало октета для смены А: 03.01.2011:
            . начало октета для смены Б: 05.01.2011:
            . начало октета для смены В: 01.01.2011:
            . начало октета для смены Г: 07.01.2011:
             */

            var c = new DateTime(2011, 1, firstDay);

            //c.add(Calendar.DAY_OF_MONTH, getDiff(c, true));
            var m = GetDiff(day, c, true);
            c = c.AddDays(m);

            int diff = GetDiff(day, c, false);
            return (diff >= 0) && (diff <= 7) ? diff : 8;
        }

        public static int GetDiff(DateTime day, DateTime firstDate, bool round)
        {
            // long diff =  this.day.getTime().getTime() - firstDate.getTime().getTime();
            // int dd = (int)(diff/(1000 * 60 * 60 * 24));

            var dd = (int)(day.Date - firstDate.Date).TotalDays;
            
            return round ? ((dd+1) / 8) * 8 : dd;
        }


        /*
        * 1 января – Новый год;
        * 7 января – Рождество Христово (православное Рождество);
        * 8 марта – День женщин;
        * 24 апреля – Радуница (по календарю православной конфессии);
        * 1 мая – Праздник труда;
        * 9 мая – День Победы;
        * 3 июля – День Независимости Республики Беларусь (День Республики);
        * 7 ноября – День Октябрьской революции;
        * 25 декабря – Рождество Христово (католическое Рождество).
        */
        static readonly List<DateTime> Holidays = new List<DateTime>()
                        {
                            new DateTime(2000, 1, 1), // 1 января – Новый год;
                            new DateTime(2000, 1, 7), //* 7 января – Рождество Христово (православное Рождество);
                            new DateTime(2000, 3, 8), //* 8 марта – День женщин;
                            new DateTime(2000, 4, 24), //* 24 апреля – Радуница (по календарю православной конфессии);
                            new DateTime(2000, 5, 1), //* 1 мая – Праздник труда;
                            new DateTime(2000, 5, 9), //* 9 мая – День Победы;
                            new DateTime(2000, 7, 3), //* 3 июля – День Независимости Республики Беларусь (День Республики);
                            new DateTime(2000, 11, 7), //* 7 ноября – День Октябрьской революции;
                            new DateTime(2000, 12, 25), //* 25 декабря – Рождество Христово (католическое Рождество).
                        };

        public static bool CheckHoliday(DateTime day)
        {
            return Holidays.Any(x => x.Month == day.Month && x.Day == day.Day);
        }
    }
}
