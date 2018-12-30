using Kafein.Model;
using Kafein.Model.List;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Kafein.Model.Importation;
using Font = System.Drawing.Font;
using FontStyle = System.Windows.FontStyle;
using MessageBox = System.Windows.MessageBox;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace Kafein.ViewModel
{
    public class ListIngridientViewModel: BaseViewModel
    {
        private ListIngridientModel listIngridientModel;
        private ImportationModel newImportation;
        private ListDetailImportationModel listDetailImportation;
        int index = -1;
        public ListIngridientViewModel(): base()
        {

            listIngridientModel = ListIngridientModel.GetInstance();
            listIngridientModel.LoadAllIngridient();
            //listDetailImportation = ListDetailImportationModel.GetInstance();
            listDetailImportation = new ListDetailImportationModel();

            // command init
            IngridientSelectionChangeCommand = new DelegateCommand<IngridientModel>(SelectedIngridientChange);
            ShortcutKeysCommand = new DelegateCommand<string>(HandleShortcutKeys);
            CreateImportationCommand = new DelegateCommand(CreateImportation);
            CheckoutImportationCommand = new DelegateCommand(CheckoutImportation);
            PrintImportationCommand = new DelegateCommand(PrintImportation);
            ClearImportationCommand = new DelegateCommand(ClearImportation);
            CancelCommand = new DelegateCommand(Cancel);
            DetailSelectionChangeCommand = new DelegateCommand<DetailImportationItemViewModel>(SelectedDetailChange);
            RemoveItemCommand = new DelegateCommand<DetailImportationItemViewModel>(RemoveDetailItem);


            // =============> !!!! [WARNING] DO NOT DELETE THIS CODE !!!! <==============
            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            // ==================================> <======================================
        }

        public ListIngridientViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;

            if (parameters == null)
            {
                newImportation = new ImportationModel();
                newImportation.ID = ImportationModel.GenerateID(ListImportationModel.GetInstance().List);
            }
            else
            {
                index = (int)parameters[0];
                newImportation = (ImportationModel)ListGeneralImportationModel.GetInstance().List[index].Importation;
                listDetailImportation = (ListDetailImportationModel)ListGeneralImportationModel.GetInstance().List[index].ListDetailImportation;
            }
        }

        // getter and setter
        public DelegateCommand<string> ShortcutKeysCommand { get; set; }
        public DelegateCommand<IngridientModel> IngridientSelectionChangeCommand { get; set; }
        public DelegateCommand CreateImportationCommand { get; set; }
        public DelegateCommand CheckoutImportationCommand { get; set; }
        public DelegateCommand PrintImportationCommand { get; set; }
        public DelegateCommand ClearImportationCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand<DetailImportationItemViewModel> DetailSelectionChangeCommand { get; set; }
        public DelegateCommand<DetailImportationItemViewModel> RemoveItemCommand { get; set; }
        public ObservableCollection<DetailImportationItemViewModel> ListDetailImportation
        {
            get { return listDetailImportation.List; }
            set { listDetailImportation.List = value; NotifyChanged("ListDetailImportation"); }
        }
        public ObservableCollection<IngridientModel> ListIngridient
        {
            get { return listIngridientModel.List; }
            set { listIngridientModel.List = value; NotifyChanged("ListIngridient"); }
        }

        public int SelectedIndex { get; set; }

        public int SelectedIndexDetail { get; set; }

        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    sum += item.Price;
                }
                return sum;
            }
        }

        private void SelectedIngridientChange(IngridientModel ingridient)
        {
            if (SelectedIndex == -1)
                return;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                //Check if ingridient was chosen, then update quantity
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    if (item.IngridientName.Equals(ingridient.Name))
                    {
                        //update quantity
                        item.Quantity++;
                        NotifyDetaillImportationProperty();
                        return;
                    }
                }

                //Otherwise, create the importation
                // model related
                UnitModel unit = UnitModel.GetModelFromID(ingridient.UnitID);

                // Generate id for detaill importation
                DetailImportationModel detail = new DetailImportationModel(DetailImportationModel.GenerateID(listDetailImportation.ListDetail), newImportation.ID, ingridient.ID, unit.ID, 1, ingridient.Price);
                listDetailImportation.Add(new DetailImportationItemViewModel(ingridient, unit, detail));

                NotifyDetaillImportationProperty();
            }
            
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                //Check if ingridient was chosen, then update quantity
                foreach (DetailImportationItemViewModel item in ListDetailImportation)
                {
                    if (item.IngridientName.Equals(ingridient.Name))
                    {
                        //update quantity
                        item.Quantity--;
                        if (item.Quantity == 0)
                            RemoveItem(item);
                        NotifyDetaillImportationProperty();
                        return;
                    }
                }
            }
        }

        private void SelectedDetailChange(DetailImportationItemViewModel item)
        {
            //RemoveItem(item);
        }

        private void NotifyDetaillImportationProperty()
        {
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            NotifyChanged("SumPrice");
        }

        private void HandleShortcutKeys(string key)
        {   
            switch(key)
            {
                case "F1":
                    CreateImportation();                  
                    break;
                case "F2":
                    CheckoutImportation();
                    break;
                case "F3":
                    PrintImportation();
                    break;
                case "ESC":
                    Cancel();
                    break;
                case "DELETE":
                    ClearImportation();
                    break;
            }
            
        }

        private void CreateImportation()
        {
            InitImportation();
            if (index >= 0)
            {
                ListGeneralImportationModel.GetInstance().List[index].Importation = newImportation;
                ListGeneralImportationModel.GetInstance().List[index].ListDetailImportation = listDetailImportation;
            }
            else
                ListGeneralImportationModel.GetInstance().List.Add(new GeneralImportationModel(newImportation, listDetailImportation));
            navigate.Invoke("ImportationManagementViewModel", null);
        }

        private void InitImportation()
        {
            // check if list detail is null
            if (ListDetailImportation.Count == 0)
            {
                MessageBox.Show("Phiếu nhập hàng chưa chọn nguyên liệu", "Nhắc nhở", MessageBoxButton.OK);
                return;
            }
            
            // update property for general importation
            newImportation.Date = DateTime.Now;
            newImportation.Price = SumPrice;                   
        }

        private void CheckoutImportation()
        {
            // create importation first
            InitImportation();

            //(new CheckoutDialog(navigate, newImportation, ListDetailImportation, index)).ShowDialog();

            // save to database
            //SaveToDatabase();

        }

        private void PrintImportation()
        {
            // print
            try
            {
                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();

                printDocument.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(_CreateReceipt); //add an event handler that will do the printing

                if (printDialog.ShowDialog() == true)
                {
                    printDocument.Print();
                }
            }
            catch (Exception)
            {

            }
        }

        private void ClearImportation()
        {
            (new ConfirmDialog("XÁC NHẬN", "Phiếu nhập hàng đang tạo hoặc đang chờ thanh toán. Xác nhận xóa?", (Action)delegate
            {
                ListDetailImportation.Clear();
                if (index >= 0)
                {
                    ListGeneralImportationModel.GetInstance().List.Remove(ListGeneralImportationModel.GetInstance().List[index]);
                    ListGeneralImportationModel.GetInstance().NotifyListChange();
                }
                NotifyChanged("SumPrice");
            })).ShowDialog();
            
        }

        private void Cancel()
        {
            navigate.Invoke("ImportationManagementViewModel", null);
        }

        private void RemoveItem(DetailImportationItemViewModel item)
        {
            ListDetailImportation.Remove(item);
        }

        private void SaveToDatabase()
        {
            // save importation
            ImportationModel.SaveToDatabase(newImportation);

            // save detail importation
            foreach(DetailImportationItemViewModel item in ListDetailImportation)
            {
                DetailImportationModel.SaveToDatabase(item.DetailImportationModel);
            }
            
        }

        private void RemoveDetailItem(DetailImportationItemViewModel item)
        {
            ListDetailImportation.Remove(item);
            NotifyDetaillImportationProperty();
        }

        private void _CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 6);
            float FontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 90;

            graphic.DrawString("CÀ PHÊ TRI ÂN".PadLeft(17), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY);

            graphic.DrawString("252 Sông Lu, xã Trung An".PadLeft(25), font, new SolidBrush(Color.Black), startX + 22, startY + 15);
            graphic.DrawString("huyện Củ Chi, TP.HCM".PadLeft(27), font, new SolidBrush(Color.Black), startX, startY + 25);

            graphic.DrawString("HÓA ĐƠN BÀN SỐ".PadLeft(16), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + 40);
            //graphic.DrawString(InputDeskNo.ToString(), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + 40);

            graphic.DrawString("GIỜ BẮT ĐẦU".PadLeft(8), font, new SolidBrush(Color.Black), startX, startY + 65);
            graphic.DrawString(DateTime.Now.ToString(), font, new SolidBrush(Color.Black), startX + 70, startY + 65);

            string top = "Tên Mặt Hàng".PadRight(17) + "ĐVT".PadRight(7) + "SL".PadRight(6)  + "TT";
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight; //make the spacing consistent
            graphic.DrawString("------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 5; //make the spacing consistent

            //Check if ingridient was chosen, then update quantity
            foreach (DetailImportationItemViewModel item in ListDetailImportation)
            {
                graphic.DrawString(item.IngridientName, font, new SolidBrush(Color.Black), startX, startY + offset);
                graphic.DrawString(item.UnitName, font, new SolidBrush(Color.Black), startX + 90, startY + offset);
                graphic.DrawString(item.Quantity.ToString(), font, new SolidBrush(Color.Black), startX + 125, startY + offset);
                graphic.DrawString((item.Price * item.Quantity).ToString(), font, new SolidBrush(Color.Black), startX + 148, startY + offset);
                offset = offset + (int)FontHeight + 5; //make the spacing consistent  
            }

            graphic.DrawString("------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("TỔNG CỘNG", new Font("Courier New", 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);
            graphic.DrawString(SumPrice.ToString(), font, new SolidBrush(Color.Black), startX + 80, startY + offset);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("XIN CHÂN THÀNH CẢM ƠN QUÝ KHÁCH!".PadLeft(10), font, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("HẸN GẶP LẠI!".PadLeft(23), font, new SolidBrush(Color.Black), startX, startY + offset);
        }
    }
}
