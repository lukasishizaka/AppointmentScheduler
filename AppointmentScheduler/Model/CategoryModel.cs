using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointmentScheduler.Model
{
    public class CategoryModel:INotifyPropertyChanged
    {
        int categoryID;
        string categoryName;
        string categoryColor;
        string editColor;

        public int CategoryID { get => categoryID; set { categoryID = value; OnPropertyChanged("CategoryID"); } }
        public string CategoryName { get => categoryName; set { categoryName = value; OnPropertyChanged("CategoryName"); } }
        public string CategoryColor { get => categoryColor; set { categoryColor = value; OnPropertyChanged("CategoryColor"); } }
        public string EditColor { get => editColor; set { editColor = value; OnPropertyChanged("EditColor"); } }


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
