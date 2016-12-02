using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MPT.WorkSchedule
{
    public enum MonthOfYear
    {
        [Display(Name = "День")]
        NotSet = 0,
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }

    public class ScheduleMonth
    {
        public string MonthName
        {
            get{ return _monthFirstDay.ToString("MMMM"); }
        }

        public string FullName
        {
            get { return _monthFirstDay.ToString("yyyy MMMM"); }

        }

        public List<ScheduleDay> Days;

        public Dictionary<Smena, SmenaMonth> SmenaList;
        
        public class SmenaMonth
        {
            public int OverTime;

            [Display(Name = "День")]
            public int WorkDays;
            public int WorkTime;
            public int NightTime;
            public int HolidayTime;
        }

        private DateTime _monthFirstDay;

        public SmenaMonth this[Smena key]
        {
            get { return SmenaList[key]; }
        }


        public ScheduleMonth(int month, int year)
        {           
            _monthFirstDay = new DateTime(year, month, 1);
            Days = new List<ScheduleDay>();
            SmenaList = new Dictionary<Smena, SmenaMonth>()
                        {
                            {Smena.Day, new SmenaMonth()},
                            {Smena.A, new SmenaMonth()},
                            {Smena.B, new SmenaMonth()},
                            {Smena.C, new SmenaMonth()},
                            {Smena.D, new SmenaMonth()},
                        };

            for(var day=0; day<=31; day++)
            {
                try
                {
                    var scheduleDay = new ScheduleDay(_monthFirstDay.AddDays(day));
                    if (scheduleDay.Month == month)
                    {
                        Days.Add(scheduleDay);
                    }
                }
                catch (Exception)
                {
                    //throw;
                }
            }
          
            foreach (var day in Days)
            {
                foreach (var smenaMonthKv in SmenaList)
                {
                    var key = smenaMonthKv.Key;
                    var smenaMonth = smenaMonthKv.Value;
                    
                    smenaMonth.WorkTime += day[key].Hours;

                    if (day[key].Hours != 0)
                        smenaMonth.WorkDays++;
                    
                    if (key == (int)Smena.Day)
                        continue;

                    if (day[key].IsNight)
                        smenaMonth.NightTime += day[key].Hours;

                    if (day.IsHoliday)
                        smenaMonth.HolidayTime += day[key].Hours;
                }
            }



            foreach (var smenaMonthKv in SmenaList)
            {
                var key = smenaMonthKv.Key;
                var smenaMonth = smenaMonthKv.Value;

                if (key == (int) Smena.Day) 
                    continue;

                var smenaMonthDay = SmenaList[(int) Smena.Day];
                if (smenaMonth.WorkTime > smenaMonthDay.WorkTime)
                    smenaMonth.OverTime = smenaMonth.WorkTime - smenaMonthDay.WorkTime;
            }

            /*
            if(WorkTimeA > WorkTimeD) 
                OverTimeA = WorkTimeA-WorkTimeD;
            if(WorkTimeB > WorkTimeD) 
                OverTimeB = WorkTimeB-WorkTimeD;
            if(WorkTimeV > WorkTimeD) 
                OverTimeV = WorkTimeV-WorkTimeD;
            if(WorkTimeG > WorkTimeD) 
                OverTimeG = WorkTimeG-WorkTimeD;
             * */
        }


        public override string ToString()
        {
            return FullName;
        }
    }
}
