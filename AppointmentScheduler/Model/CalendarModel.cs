using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Model
{
    public class CalendarModel:INotifyPropertyChanged
    {
        CultureInfo _myCI = CultureInfo.CurrentCulture;
        CalendarWeekRule _myCWR;
        DayOfWeek _firstDOW;
        Calendar _myCal;


        int _weekNo;
        DayOfWeek _weekDay;
        DateTime _date = DateTime.Now;
        bool isToday = false;
        bool isNotCurrMonth = false;
        string month;
        int year;
        int day;

        ObservableCollection<AppointmentModel> _ListOfAppointments = new ObservableCollection<AppointmentModel>();

        public CalendarModel()
        {
            MyCal = _myCI.Calendar;
            _myCWR = _myCI.DateTimeFormat.CalendarWeekRule;
            _firstDOW = _myCI.DateTimeFormat.FirstDayOfWeek;

            WeekNo = MyCal.GetWeekOfYear(Date, _myCWR, _firstDOW);
            WeekDay = MyCal.GetDayOfWeek(Date);
            month = Date.ToString("MMMM", CultureInfo.CurrentCulture);
            year = Date.Year;
            day = Date.Day;
        }

        public CalendarModel(DateTime date)
        {
            MyCal = _myCI.Calendar;
            _myCWR = _myCI.DateTimeFormat.CalendarWeekRule;
            _firstDOW = _myCI.DateTimeFormat.FirstDayOfWeek;
            Date = date;

            if (date == DateTime.Today)
            {
                isToday = true;
            }
        }

        public CalendarModel(DateTime date, bool isnotcurrmonth)
        {
            MyCal = _myCI.Calendar;
            _myCWR = _myCI.DateTimeFormat.CalendarWeekRule;
            _firstDOW = _myCI.DateTimeFormat.FirstDayOfWeek;
            Date = date;
            IsNotCurrMonth = isnotcurrmonth;

            if (date == DateTime.Today)
            {
                isToday = true;
            }
        }

        public int WeekNo { get => _weekNo; set { _weekNo = value; OnPropertyChanged("WeekNo"); } }
        public DayOfWeek WeekDay { get => _weekDay; set { _weekDay = value; OnPropertyChanged("WeekDay"); } }
        public string Month { get => month; set { month = value; OnPropertyChanged("Month"); } }
        public DateTime Date
        {
            get => _date; set
            {
                _date = value;
                OnPropertyChanged("DateNow");

                WeekNo = MyCal.GetWeekOfYear(Date, _myCWR, _firstDOW);
                WeekDay = MyCal.GetDayOfWeek(Date);
                Month = Date.ToString("MMMM", CultureInfo.CurrentCulture);
                Year = Date.Year;
                Day = Date.Day;
            }
        }
        public Calendar MyCal { get => _myCal; set => _myCal = value; }
        public int Day { get => day; set { day = value; OnPropertyChanged("Day"); } }

        public bool IsToday { get => isToday; set { isToday = value; OnPropertyChanged("IsToday"); } }

        public bool IsNotCurrMonth { get => isNotCurrMonth; set { isNotCurrMonth = value; OnPropertyChanged("IsNotCurrMonth"); } }

        public ObservableCollection<AppointmentModel> ListOfAppointments
        {
            get
            {
                _ListOfAppointments = new ObservableCollection<AppointmentModel>(_ListOfAppointments.OrderBy(s => s.StartTime.Hour).ThenBy(s => s.StartTime.Minute));
                return _ListOfAppointments;
            }
            set
            {
                _ListOfAppointments = value;
                OnPropertyChanged("ListOfAppointments");
            }
        }


        public int Year { get => year; set { year = value; OnPropertyChanged("Year"); } }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}
