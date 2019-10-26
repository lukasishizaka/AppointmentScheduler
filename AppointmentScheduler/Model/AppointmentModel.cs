using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Model
{
    public class AppointmentModel:INotifyPropertyChanged
    {
        int iD;
        string name;
        DateTime startTime;
        DateTime endTime;
        CategoryModel category;
        double dayviewposition;
        double dayviewheight;

        public int ID { get => iD; set { iD = value; OnPropertyChanged("ShiftScheduleID"); } }
        public DateTime StartTime { get => startTime; set { startTime = value; OnPropertyChanged("StartTime"); } }
        public DateTime EndTime { get => endTime; set { endTime = value; OnPropertyChanged("EndTime"); } }
        public double DayViewPosition { get => dayviewposition; set { dayviewposition = value; OnPropertyChanged("DayViewPosition"); } }
        public double DayViewHeight { get => dayviewheight; set { dayviewheight = value; OnPropertyChanged("DayViewHeight"); } }
        public CategoryModel Category { get => category; set { category = value; OnPropertyChanged("Category"); } }
        public string DisplayName { get => string.Format("{0:t}", StartTime) + "  " + Name; }
        public string Name { get => name; set { name = value; OnPropertyChanged("ShiftScheduleName"); } }
        public string DisplayStartTime { get => startTime.ToString("dddd, dd MMMM yyyy HH:mm:ss"); set => startTime = DateTime.Parse(value); }


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
