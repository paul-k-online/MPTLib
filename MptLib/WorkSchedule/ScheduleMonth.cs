using System;
using System.Collections.Generic;

namespace MPT.WorkSchedule
{
    public class ScheduleMonth
    {
        /*
        public static int JANUARY = 0;
        public static int FEBRUARY = 1;
        public static int MARCH = 2;
        public static int APRIL = 3;
        public static int MAY = 4;
        public static int JUNE = 5;
        public static int JULY = 6;
        public static int AUGUST = 7;
        public static int SEPTEMBER = 8;
        public static int OCTOBER = 9;
        public static int NOVEMBER = 10;
        public static int DECEMBER = 11;
        */

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
            public int WorkDays;
            public int WorkTime;
            public int NightTime;
            public int HolidayTime;
        }

        /*
        public int OverTimeA = 0;
        public int OverTimeB = 0;
        public int OverTimeV = 0;
        public int OverTimeG = 0;

        public int WorkDaysD = 0;
        public int WorkDaysA = 0;
        public int WorkDaysB = 0;
        public int WorkDaysV = 0;
        public int WorkDaysG = 0;

        public int WorkTimeD = 0;
        public int WorkTimeA = 0;
        public int WorkTimeB = 0;
        public int WorkTimeV = 0;
        public int WorkTimeG = 0;

        public int NightTimeA = 0;
        public int NightTimeB = 0;
        public int NightTimeV = 0;
        public int NightTimeG = 0;

        public int HolidayTimeA = 0;
        public int HolidayTimeB = 0;
        public int HolidayTimeV = 0;
        public int HolidayTimeG = 0;
        */

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

            for(var day=0; day<=30; day++)
            {
                try
                {
                    var scheduleDay = new ScheduleDay(_monthFirstDay.AddDays(day));
                    if(scheduleDay.Month == month)
                        Days.Add(scheduleDay);
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

                /*
                WorkTimeD += day.SmenaList[(int)Smena.Day].Hours;
                WorkTimeA += day.SmenaList[(int)Smena.A].Hours;
                WorkTimeB += day.SmenaList[(int)Smena.B].Hours;
                WorkTimeV += day.SmenaList[(int)Smena.C].Hours;
                WorkTimeG += day.SmenaList[(int)Smena.D].Hours;
                


                if (day.SmenaList[(int)Smena.Day].Hours != 0)
                    WorkDaysD++;
                if (day.SmenaList[(int)Smena.A].Hours != 0)
                    WorkDaysA++;
                if (day.SmenaList[(int)Smena.B].Hours != 0)
                    WorkDaysB++;
                if (day.SmenaList[(int)Smena.C].Hours != 0)
                    WorkDaysV++;
                if (day.SmenaList[(int)Smena.D].Hours != 0)
                    WorkDaysG++;

                

                if (day.SmenaList[(int)Smena.A].IsNight)
                    NightTimeA += day.SmenaList[(int)Smena.A].Hours;
                if (day.SmenaList[(int)Smena.B].IsNight)
                    NightTimeB += day.SmenaList[(int)Smena.B].Hours;
                if (day.SmenaList[(int)Smena.C].IsNight)
                    NightTimeV += day.SmenaList[(int)Smena.C].Hours;
                if (day.SmenaList[(int)Smena.D].IsNight)
                    NightTimeG += day.SmenaList[(int)Smena.D].Hours;



                if(day.IsHoliday)
                {
                    HolidayTimeA += day.SmenaList[(int)Smena.A].Hours;
                    HolidayTimeB += day.SmenaList[(int)Smena.B].Hours;
                    HolidayTimeV += day.SmenaList[(int)Smena.C].Hours;
                    HolidayTimeG += day.SmenaList[(int)Smena.D].Hours;
                }
                 * */
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
