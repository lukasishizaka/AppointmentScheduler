using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace AppointmentScheduler.Model
{
    public class ScheduleConfig : INotifyPropertyChanged
    {
        private ObservableCollection<CategoryModel> _Category;
        private DateTime _EndTime = DateTime.Now;
        private ObservableCollection<CalendarModel> _listOfDays = new ObservableCollection<CalendarModel>();
        private string _Name;
        private CategoryModel _SelectedCategory;
        private DateTime _StartTime = DateTime.Now;
        private AppointmentModel appointmentToEdit;
        private CalendarModel cm = new CalendarModel();
        private ObservableCollection<string> dayNames = new ObservableCollection<string>();
        private bool dayViewV = false;
        private bool editViewV = false;
        private bool monthViewV = true;
        private CalendarModel selectedDay;
        public AppointmentModel AppointmentToEdit { get => appointmentToEdit; set { appointmentToEdit = value; OnPropertyChanged("AppointmentToEdit"); } }

        public ObservableCollection<CategoryModel> Category { get => _Category; set { _Category = value; OnPropertyChanged("Category"); } }

        public CalendarModel Cm { get => cm; set { cm = value; OnPropertyChanged("Cm"); } }

        public ObservableCollection<string> DayNames { get => dayNames; set { dayNames = value; OnPropertyChanged("DayNames"); } }

        public bool DayViewV { get => dayViewV; set { dayViewV = value; OnPropertyChanged("DayViewV"); } }

        public DateTime EndTime { get => _EndTime; set { _EndTime = value; OnPropertyChanged("EndTime"); } }

        public bool EditViewV { get => editViewV; set { editViewV = value; OnPropertyChanged("EditViewV"); } }

        public ObservableCollection<CalendarModel> ListOfDays { get => _listOfDays; set { _listOfDays = value; OnPropertyChanged("ListOfDays"); } }

        public bool MonthViewV { get => monthViewV; set { monthViewV = value; OnPropertyChanged("MonthViewV"); } }

        public string Name { get => _Name; set { _Name = value; OnPropertyChanged("Name"); } }

        public CategoryModel SelectedCategory
        {
            get => _SelectedCategory;
            set
            {
                _SelectedCategory = value;
                //if (_SelectedCategory.CategoryName == "Shift")
                //{
                //    OnShiftEnable = false;
                //    Day = false;
                //    Week = false;
                //    Month = false;
                //    Year = false;
                //}
                //else
                //{
                //    OnShiftEnable = true;
                //}
                OnPropertyChanged("SelectedCategory");
            }
        }

        public CalendarModel SelectedDay
        {
            get => selectedDay;
            set
            {
                selectedDay = value;
                OnPropertyChanged("SelectedDay");
            }
        }

        public DateTime StartTime { get => _StartTime; set { _StartTime = value; OnPropertyChanged("StartTime"); } }

        #region LifeCycle
        public static ScheduleConfig GetInstance()
        {
            if (_config == null)
            {
                _config = new ScheduleConfig();
            }
            return _config;
        }
        private static ScheduleConfig _config;
        #endregion


        #region Methods
        public double CalculateHeight(DateTime start, DateTime end, double borderheight)
        {
            return (((double)end.Hour + ((double)end.Minute / 60.0)) - ((double)start.Hour + ((double)start.Minute / 60.0))) * ((borderheight == 0 ? 207.0 : (borderheight)) / 24.0);
        }

        public double CalculatePosition(DateTime start, double borderheight)
        {
            return ((double)start.Hour + ((double)start.Minute / 60.0)) * ((borderheight == 0 ? 207.0 : (borderheight)) / 24.0);
        }
        #endregion


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}