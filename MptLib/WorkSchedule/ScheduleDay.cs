using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MPT.WorkSchedule
{
    public enum Smena
    {
        [Description("День")]
        Day = 0,
        [Description("А")]
        A = 3,
        [Description("Б")]
        B = 5,
        [Description("В")]
        C = 1,
        [Description("Г")]
        D = 7,
    }


    public class ScheduleDay
    {
        public class SmenaDay
        {
            public bool IsNight;
            public int Hours = 8;
            public string BgColor = "#ffffff";
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

            var m = GetDiff(day, c, true);
            c = c.AddDays(m);

            var diff = GetDiff(day, c, false);
            return (diff >= 0) && (diff <= 7) ? diff : 8;
        }

        public static int GetDiff(DateTime day, DateTime firstDate, bool round)
        {
            var totalDays = (int)(day.Date - firstDate.Date).TotalDays;
            return round ? ((totalDays + 1) / 8) * 8 : totalDays;
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

        private static readonly List<DateTime> Holidays =
            new List<DateTime>
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
        
        
        private DateTime _day;

        private readonly Dictionary<Smena, SmenaDay> _smenaList =
            new Dictionary<Smena, SmenaDay>()
            {
                {Smena.Day, new SmenaDay()},
                {Smena.A, new SmenaDay()},
                {Smena.B, new SmenaDay()},
                {Smena.C, new SmenaDay()},
                {Smena.D, new SmenaDay()},
            };

        public SmenaDay this[Smena key]
        {
            get { return _smenaList[key]; }
        }
        
        public ScheduleDay(DateTime day)
        {
            _day = day;
            foreach (var smenaDay in _smenaList)
            {
                if (smenaDay.Key == (int)Smena.Day)
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
        }

        public ScheduleDay(int year, int month, int day) : 
            this(new DateTime(year, month, day))
        {
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
    }
}
