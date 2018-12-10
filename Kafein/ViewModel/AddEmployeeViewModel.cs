using Kafein.Model;
using Kafein.Model.List;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kafein.ViewModel
{
    public class AddEmployeeViewModel : BaseViewModel
    {
        private ListPositionModel listPositionModel;
        private EmployeeModel updateEmployee;
        private string relativePath = null;
        public AddEmployeeViewModel() : base()
        {
            listPositionModel = new ListPositionModel();
            listPositionModel.LoadAllPosition();

            CancelCommand = new DelegateCommand(Cancel);
            AddEmployeeCommand = new DelegateCommand(AddEmployee);
        }

        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string CardID { get; set; }
        public string Phone { get; set; }
        public DateTime StartDate { get; set; }
        public string Shift { get; set; }
        public EmployeeModel UpdateEmployee
        {
            get { return updateEmployee; }
            set
            {
                updateEmployee = value;
                Name = value.Name;
                Birthday = value.Birthday;
                CardID = value.CardID;
                Phone = value.Phone;
                StartDate = value.StartDate;
                Shift = value.Shift;
                SelectedIndexPosition = listPositionModel.GetIndexByValue("ID", value.PositionID);

                NotifyChanged("UpdateEmployee");
                NotifyChanged("Name");
                NotifyChanged("Birthday");
                NotifyChanged("CardID");
                NotifyChanged("Phone");
                NotifyChanged("StartDate");
                NotifyChanged("Shift");
                NotifyChanged("SelectedIndexPosition");
            }
        }
        public ObservableCollection<object> ListPosition
        {
            get { return listPositionModel.ListName; }
        }
        public int SelectedIndexPosition { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand AddEmployeeCommand { get; set; }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "AddIngridientDialog")
                    window.Close();
        }

        private void AddEmployee()
        {
            //check null
            if (Name.Trim().Length == 0)
                return;
            if (CardID.Trim().Length < 9 || CardID.Trim().Length > 12)
                return;
            if (Phone.Trim().Length == 0)
                return;
            if (Shift.Trim().Length == 0)
                return;

            // create mode
            if (updateEmployee == null)
            {
                //add to database
                EmployeeModel employee = new EmployeeModel(EmployeeModel.GenerateID(), Name, Birthday, CardID, Phone, listPositionModel.List[SelectedIndexPosition].ID, StartDate, Shift);
                employee.SaveToDatabase();
            }
            else //update mode
            {
                EmployeeModel employee = new EmployeeModel(updateEmployee.ID, Name, Birthday, CardID, Phone, listPositionModel.List[SelectedIndexPosition].ID, StartDate, Shift);
                EmployeeModel.UpdateDatabase(employee);
            }

            Cancel();
        }
    }
}
