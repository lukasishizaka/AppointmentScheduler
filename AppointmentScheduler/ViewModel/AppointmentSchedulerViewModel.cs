using AppointmentScheduler.Model;
using AppointmentScheduler.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AppointmentScheduler.EF;
using System.Windows.Media;

namespace AppointmentScheduler.ViewModel
{
    public class AppointmentSchedulerViewModel
    {
        ScheduleConfig siConfig;
        LKSEntities lks;


        #region Lifecycle

        public AppointmentSchedulerViewModel()
        {
            SiConfig = ScheduleConfig.GetInstance();
            lks = new LKSEntities();

            GetCategories();

            addAppointmentCommand = new BaseCommand(AddAppointment);
            editAppointmentCommand = new BaseCommand(EditAppointment);
            setCurrentDayCommand = new BaseCommand(SetCurrentDay);
            monthViewCommand = new BaseCommand(GetMonthView);
            nextDayCommand = new BaseCommand(NextDay);
            previousDayCommand = new BaseCommand(PreviousDay);
            saveEditedAppointmentCommand = new BaseCommand(SaveAppointment);
            deleteAppointmentCommand = new BaseCommand(DeleteAppointment);
            exitEditCommand = new BaseCommand(ExitEdit);
            executeDeleteCommand = new BaseCommand(ExecuteDelete);
            canceldeleteCommand = new BaseCommand(CancelDelete);

            nextMonthCommand = new BaseCommand(GetNextMonth);
            previousMonthCommand = new BaseCommand(getPrevMonth);

            SiConfig.DayNames = new ObservableCollection<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            GetDays();
        }

        #endregion

        #region ICommand
        public ICommand HideDialogUICommand
        {
            get { return hideDialogUICommand; }
        }
        private ICommand hideDialogUICommand;
        public ICommand AddAppointmentCommand
        {
            get { return addAppointmentCommand; }
        }
        ICommand addAppointmentCommand;

        public ICommand EditAppointmentCommand
        {
            get { return editAppointmentCommand; }
        }
        ICommand editAppointmentCommand;

        public ICommand ExitEditCommand
        {
            get { return exitEditCommand; }
        }
        ICommand exitEditCommand;

        public ICommand SaveEditedAppointmentCommand
        {
            get { return saveEditedAppointmentCommand; }
        }
        ICommand saveEditedAppointmentCommand;

        public ICommand DeleteAppointmentCommand
        {
            get { return deleteAppointmentCommand; }
        }
        ICommand deleteAppointmentCommand;
        public ICommand ExecuteDeleteCommand
        {
            get { return executeDeleteCommand; }
        }
        ICommand executeDeleteCommand;

        public ICommand CancelDeleteCommand
        {
            get { return canceldeleteCommand; }
        }
        ICommand canceldeleteCommand;

        public ICommand AddShiftCommand
        {
            get { return addShiftCommand; }
        }
        ICommand addShiftCommand;

        public ICommand PreviousMonthCommand
        {
            get { return previousMonthCommand; }
        }
        ICommand previousMonthCommand;

        public ICommand NextMonthCommand
        {
            get { return nextMonthCommand; }
        }
        ICommand nextMonthCommand;

        public ICommand AddMultiplierCommand
        {
            get { return addMultiplierCommand; }
        }
        ICommand addMultiplierCommand;

        public ICommand SavePartTimeCommand
        {
            get { return savePartTimeCommand; }
        }
        ICommand savePartTimeCommand;

        public ICommand SetCurrentDayCommand
        {
            get { return setCurrentDayCommand; }
        }
        ICommand setCurrentDayCommand;

        public ICommand MonthViewCommand
        {
            get { return monthViewCommand; }
        }
        ICommand monthViewCommand;

        public ICommand NextDayCommand
        {
            get { return nextDayCommand; }
        }
        ICommand nextDayCommand;

        public ICommand PreviousDayCommand
        {
            get { return previousDayCommand; }
        }
        ICommand previousDayCommand;
        #endregion

        public ScheduleConfig SiConfig { get => siConfig; set { siConfig = value; } }


        #region Appointments
        private void GetCategories()
        {
            SiConfig.Category = new ObservableCollection<CategoryModel>();
            var categories = (from c in lks.Categories select c);

            int temp = categories.Count();
            foreach (var category in categories)
            {
                SiConfig.Category.Add(new CategoryModel()
                {
                    CategoryID = category.CategoryID,
                    CategoryName = category.Name,
                    CategoryColor = category.Color,
                    EditColor = GetDarkerColor(category.Color)
                });
            }
        }
        private string GetDarkerColor(string hexcolor)
        {
            double coef = 0.85;
            Color color = (Color)ColorConverter.ConvertFromString(hexcolor);

            string temp = Color.FromArgb((byte)(color.A), (byte)(color.R * coef), (byte)(color.G * coef),
        (byte)(color.B * coef)).ToString().Remove(1, 2);

            return temp;
        }
        public void AddAppointment(object parameter)
        {
            if (SiConfig.Name != null && SiConfig.SelectedCategory != null)
            {
                Appointments dhs = new Appointments()
                {
                    Name = SiConfig.Name,
                    StartTime = SiConfig.StartTime,
                    EndTime = SiConfig.EndTime,
                    CategoryID = siConfig.SelectedCategory.CategoryID
                };

                var insert = lks.Appointments.Add(dhs);

                lks.SaveChanges();



                SiConfig.Name = string.Empty;
                SiConfig.StartTime = DateTime.Now;
                SiConfig.EndTime = DateTime.Now;
            }
            else
            {
                //DialogUISimpleMessages.ShowSimpleInfoMessage(DecisionModel, "Please fill in the remaining Items", HideDialogUICommand, TransMan.Translations["General.Info"].Translation);
            }

            GetDays();
        }
        private void EditAppointment(object parameter)
        {
            SiConfig.AppointmentToEdit = parameter as AppointmentModel;

            SiConfig.EditViewV = true;
        }
        private void SaveAppointment(object parameter)
        {
            Appointments app = (from u in lks.Appointments where u.ID == SiConfig.AppointmentToEdit.ID select u).First();

            app.Name = SiConfig.AppointmentToEdit.Name;
            app.StartTime = SiConfig.AppointmentToEdit.StartTime;
            app.EndTime = SiConfig.AppointmentToEdit.EndTime;
            app.CategoryID = SiConfig.AppointmentToEdit.Category.CategoryID;

            lks.SaveChanges();

            GetDays();

            SiConfig.EditViewV = false;
        }
        private void CancelDelete(object parameter)
        {
            //DecisionModel.Visible = false;
            SiConfig.EditViewV = true;
        }
        private void ExecuteDelete(object parameter)
        {
            SiConfig.EditViewV = false;

            //ShowSimpleDeleteMessage(DecisionModel, "Do you want to Delete the Current Appointment?", deleteAppointmentCommand, canceldeleteCommand, TransMan.Translations["General.Warning"].Translation);
        }
        private void DeleteAppointment(object parameter)
        {
            Appointments app = (from d in lks.Appointments where d.ID == SiConfig.AppointmentToEdit.ID select d).First();
            lks.Appointments.Remove(app);
            lks.SaveChanges();

            GetDays();

            //DecisionModel.Visible = false;
        }
        private void ExitEdit(object parameter)
        {
            SiConfig.EditViewV = false;
        }
        #endregion

        #region Calendar
        public void SetCurrentDay(object parameter)
        {
            SiConfig.SelectedDay = parameter as CalendarModel;

            if (SiConfig.SelectedDay != null)
            {
                SiConfig.DayViewV = true;
                SiConfig.MonthViewV = false;

                foreach (AppointmentModel hm in SiConfig.SelectedDay.ListOfAppointments)
                {
                    hm.DayViewHeight = SiConfig.CalculateHeight(hm.StartTime, hm.EndTime, 1000.0);
                    hm.DayViewPosition = SiConfig.CalculatePosition(hm.StartTime, 1000.0);
                }
            }
        }
        private void CheckIfDayHasAppointments()
        {
            var appointment = (from d in lks.Appointments select d);

            foreach (var item in appointment)
            {
                var category = from cat in lks.Categories where cat.CategoryID == item.CategoryID select cat;

                AppointmentModel app = new AppointmentModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    Category = new CategoryModel
                    {
                        CategoryID = category.FirstOrDefault().CategoryID,
                        CategoryName = category.FirstOrDefault().Name,
                        CategoryColor = category.FirstOrDefault().Color,
                        EditColor = GetDarkerColor(category.FirstOrDefault().Color)
                    },
                };

                foreach (CalendarModel cm in SiConfig.ListOfDays)
                {
                    if (cm.Date.Date == item.StartTime.Date)
                    {
                        if (!cm.ListOfAppointments.Any(p => p.ID == app.ID))
                        {
                            cm.ListOfAppointments.Add(app);
                            cm.ListOfAppointments = new ObservableCollection<AppointmentModel>(cm.ListOfAppointments);
                        }
                    }
                }
            }
        }
        public void getPrevMonth(object parameter)
        {
            SiConfig.ListOfDays = new ObservableCollection<CalendarModel>();
            SiConfig.Cm.Date = new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, SiConfig.Cm.Date.Day).AddMonths(-1);

            DateTime firstdayofweek = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, 1), CultureInfo.CurrentCulture);
            int daysinprevMonth = SiConfig.Cm.MyCal.GetDaysInMonth(firstdayofweek.Year, firstdayofweek.Month);

            if (firstdayofweek.Day != 1)
            {
                for (int i = firstdayofweek.Day; i <= daysinprevMonth; i++)
                {
                    SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(firstdayofweek.Year, firstdayofweek.Month, i), true));
                }
            }



            int daysInMonth = SiConfig.Cm.MyCal.GetDaysInMonth(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, i)));
            }

            DateTime firstdayofweeke = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, daysInMonth), CultureInfo.CurrentCulture);
            DateTime lastdayofweek = getLastDayOfWeek(firstdayofweeke);
            for (int i = 1; i <= lastdayofweek.Day; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(lastdayofweek.Year, lastdayofweek.Month, i), true));
            }

            CheckIfDayHasAppointments();
        }
        public void GetNextMonth(object parameter)
        {
            SiConfig.ListOfDays = new System.Collections.ObjectModel.ObservableCollection<CalendarModel>();
            SiConfig.Cm.Date = new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, SiConfig.Cm.Date.Day).AddMonths(1);

            DateTime firstdayofweek = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, 1), CultureInfo.CurrentCulture);
            int daysinprevMonth = SiConfig.Cm.MyCal.GetDaysInMonth(firstdayofweek.Year, firstdayofweek.Month);

            if (firstdayofweek.Day != 1)
            {
                for (int i = firstdayofweek.Day; i <= daysinprevMonth; i++)
                {
                    SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(firstdayofweek.Year, firstdayofweek.Month, i), true));
                }
            }



            int daysInMonth = SiConfig.Cm.MyCal.GetDaysInMonth(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, i)));
            }

            DateTime firstdayofweeke = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, daysInMonth), CultureInfo.CurrentCulture);
            DateTime lastdayofweek = getLastDayOfWeek(firstdayofweeke);
            for (int i = 1; i <= lastdayofweek.Day; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(lastdayofweek.Year, lastdayofweek.Month, i), true));
            }

            CheckIfDayHasAppointments();
        }
        public void NextDay(object parameter)
        {
            DateTime nextDay = SiConfig.SelectedDay.Date.AddDays(1);
            int daysInMonth = SiConfig.Cm.MyCal.GetDaysInMonth(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month);

            DateTime firstdayofweeke = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, daysInMonth), CultureInfo.CurrentCulture);
            DateTime lastdayofweek = getLastDayOfWeek(firstdayofweeke);

            if (SiConfig.SelectedDay.Date == lastdayofweek.Date)
            {
                GetNextMonth(null);
            }

            foreach (CalendarModel day in SiConfig.ListOfDays)
            {
                if (day.Date == nextDay.Date)
                {
                    SiConfig.SelectedDay = day;

                    foreach (AppointmentModel hm in SiConfig.SelectedDay.ListOfAppointments)
                    {
                        hm.DayViewHeight = SiConfig.CalculateHeight(hm.StartTime, hm.EndTime, 1000.0);
                        hm.DayViewPosition = SiConfig.CalculatePosition(hm.StartTime, 1000.0);
                    }
                }
            }
        }
        public void PreviousDay(object parameter)
        {


            DateTime previousDay = SiConfig.SelectedDay.Date.AddDays(-1);
            DateTime firstdayofweek = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, 1), CultureInfo.CurrentCulture);

            if (SiConfig.SelectedDay.Date == firstdayofweek.Date)
            {
                getPrevMonth(null);
            }

            foreach (CalendarModel day in SiConfig.ListOfDays)
            {
                if (day.Date == previousDay.Date)
                {
                    SiConfig.SelectedDay = day;

                    foreach (AppointmentModel hm in SiConfig.SelectedDay.ListOfAppointments)
                    {
                        hm.DayViewHeight = SiConfig.CalculateHeight(hm.StartTime, hm.EndTime, 1000.0);
                        hm.DayViewPosition = SiConfig.CalculatePosition(hm.StartTime, 1000.0);
                    }
                }
            }
        }
        public void GetMonthView(object parameter)
        {
            SiConfig.MonthViewV = true;
            SiConfig.DayViewV = false;
        }
        public void GetDays()
        {
            DateTime firstdayofweek = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, 1), CultureInfo.CurrentCulture);
            int daysinprevMonth = SiConfig.Cm.MyCal.GetDaysInMonth(firstdayofweek.Year, firstdayofweek.Month);
            SiConfig.ListOfDays = new ObservableCollection<CalendarModel>();

            if (firstdayofweek.Day != 1)
            {
                for (int i = firstdayofweek.Day; i <= daysinprevMonth; i++)
                {
                    SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(firstdayofweek.Year, firstdayofweek.Month, i), true));
                }
            }



            int daysInMonth = SiConfig.Cm.MyCal.GetDaysInMonth(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, i)));
            }

            DateTime firstdayofweeke = GetFirstDayOfWeek(new DateTime(SiConfig.Cm.Date.Year, SiConfig.Cm.Date.Month, daysInMonth), CultureInfo.CurrentCulture);
            DateTime lastdayofweek = getLastDayOfWeek(firstdayofweeke);
            for (int i = 1; i <= lastdayofweek.Day; i++)
            {
                SiConfig.ListOfDays.Add(new CalendarModel(new DateTime(lastdayofweek.Year, lastdayofweek.Month, i), true));
            }

            CheckIfDayHasAppointments();

        }
        private DateTime GetFirstDayOfWeek(DateTime dayInWeek, CultureInfo cultureInfo)
        {
            DayOfWeek firstDay = cultureInfo.DateTimeFormat.FirstDayOfWeek;
            DateTime firstDayInWeek = dayInWeek.Date;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            return firstDayInWeek;
        }
        private DateTime getLastDayOfWeek(DateTime dayInWeek)
        {
            DateTime lastday = GetFirstDayOfWeek(dayInWeek, CultureInfo.CurrentCulture).AddDays(6);
            return lastday;
        }
        #endregion
    }
}
