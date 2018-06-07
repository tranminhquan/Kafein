using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
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
using Font = System.Drawing.Font;
using FontStyle = System.Windows.FontStyle;
using MessageBox = System.Windows.MessageBox;
using PrintDialog = System.Windows.Controls.PrintDialog;

namespace Kafein.ViewModel
{
    public class ListProductViewModel: BaseViewModel
    {
        private ListProductModel listProductModel;
        private BillModel newBill;
        private ListDetailBillModel listDetailBill;
        int index = -1;
        public ListProductViewModel(): base()
        {

            listProductModel = ListProductModel.GetInstance();
            listProductModel.LoadAllProduct();
            //listDetailBill = ListDetailBillModel.GetInstance();
            listDetailBill = new ListDetailBillModel();

            // command init
            ProductSelectionChangeCommand = new DelegateCommand<ProductModel>(SelectedProductChange);
            ShortcutKeysCommand = new DelegateCommand<string>(HandleShortcutKeys);
            CreateBillCommand = new DelegateCommand(CreateBill);
            CheckoutBillCommand = new DelegateCommand(CheckoutBill);
            PrintBillCommand = new DelegateCommand(PrintBill);
            ClearBillCommand = new DelegateCommand(ClearBill);
            CancelCommand = new DelegateCommand(Cancel);
            DetailSelectionChangeCommand = new DelegateCommand<DetailBillItemViewModel>(SelectedDetailChange);
            RemoveItemCommand = new DelegateCommand<DetailBillItemViewModel>(RemoveDetailItem);


            // =============> !!!! [WARNING] DO NOT DELETE THIS CODE !!!! <==============
            SelectedIndex = 0;
            SelectedIndex = -1;
            NotifyChanged("SelectedIndex");
            // ==================================> <======================================
        }

        public ListProductViewModel(Action<object, object[]> navigate, object[] parameters): this()
        {
            this.navigate = navigate;

            if (parameters == null)
            {
                newBill = new BillModel();
                newBill.ID = BillModel.GenerateID(ListBillModel.GetInstace().List);
            }
            else
            {
                index = (int)parameters[0];
                newBill = (BillModel)ListGeneralBillModel.GetInstance().List[index].Bill;
                InputDeskNo = newBill.DeskNo;
                listDetailBill = (ListDetailBillModel)ListGeneralBillModel.GetInstance().List[index].ListDetailBill;
            }
        }

        // getter and setter
        public DelegateCommand<string> ShortcutKeysCommand { get; set; }
        public DelegateCommand<ProductModel> ProductSelectionChangeCommand { get; set; }
        public DelegateCommand CreateBillCommand { get; set; }
        public DelegateCommand CheckoutBillCommand { get; set; }
        public DelegateCommand PrintBillCommand { get; set; }
        public DelegateCommand ClearBillCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand<DetailBillItemViewModel> DetailSelectionChangeCommand { get; set; }
        public DelegateCommand<DetailBillItemViewModel> RemoveItemCommand { get; set; }
        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill.List; }
            set { listDetailBill.List = value; NotifyChanged("ListDetailBill"); }
        }
        public ObservableCollection<ProductModel> ListProduct
        {
            get { return listProductModel.List; }
            set { listProductModel.List = value; NotifyChanged("ListProduct"); }
        }

        public int InputDeskNo { get; set; }

        public int SelectedIndex { get; set; }

        public int SelectedIndexDetail { get; set; }

        public double SumPrice
        {
            get
            {
                double sum = 0;
                foreach (DetailBillItemViewModel item in ListDetailBill)
                {
                    sum += item.Price;
                }
                return sum;
            }
        }

        private void SelectedProductChange(ProductModel product)
        {
            if (SelectedIndex == -1)
                return;

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

                //Check if product was chosen, then update quantity
                foreach (DetailBillItemViewModel item in ListDetailBill)
                {
                    if (item.ProductName.Equals(product.Name))
                    {
                        //update quantity
                        item.Quantity++;
                        NotifyDetaillBillProperty();
                        return;
                    }
                }

                //Otherwise, create the bill
                // model related
                UnitModel unit = UnitModel.GetModelFromID(product.UnitID);

                // Generate id for detaill bill
                DetailBillModel detail = new DetailBillModel(DetailBillModel.GenerateID(listDetailBill.ListDetail), newBill.ID, product.ID, unit.ID, 1, product.Price);
                listDetailBill.Add(new DetailBillItemViewModel(product, unit, detail));

                NotifyDetaillBillProperty();
            }
            
            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                //Check if product was chosen, then update quantity
                foreach (DetailBillItemViewModel item in ListDetailBill)
                {
                    if (item.ProductName.Equals(product.Name))
                    {
                        //update quantity
                        item.Quantity--;
                        if (item.Quantity == 0)
                            RemoveItem(item);
                        NotifyDetaillBillProperty();
                        return;
                    }
                }
            }
        }

        private void SelectedDetailChange(DetailBillItemViewModel item)
        {
            //RemoveItem(item);
        }

        private void NotifyDetaillBillProperty()
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
                    CreateBill();                  
                    break;
                case "F2":
                    CheckoutBill();
                    break;
                case "F3":
                    PrintBill();
                    break;
                case "ESC":
                    Cancel();
                    break;
                case "DELETE":
                    ClearBill();
                    break;
            }
            
        }

        private void CreateBill()
        {
            InitBill();
            ListGeneralBillModel.GetInstance().List.Add(new GeneralBillModel(newBill, listDetailBill));
            navigate.Invoke("BillManagementViewModel", null);
        }

        private void InitBill()
        {
            // check if list detail is null
            if (ListDetailBill.Count == 0)
            {
                MessageBox.Show("Hóa đơn chưa chọn món", "Nhắc nhở", MessageBoxButton.OK);
                return;
            }
            
            // update property for general bill
            newBill.Date = DateTime.Now;
            newBill.DeskNo = InputDeskNo;
            newBill.Price = SumPrice;                   
        }

        private void CheckoutBill()
        {
            // create bill first
            InitBill();

            (new CheckoutDialog(navigate, newBill, ListDetailBill, index)).ShowDialog();

            // save to database
            //SaveToDatabase();

        }

        private void PrintBill()
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

        private void ClearBill()
        {
            ListDetailBill.Clear();
            NotifyChanged("SumPrice");
        }

        private void Cancel()
        {
            navigate.Invoke("BillManagementViewModel", null);
            ListGeneralBillModel.GetInstance().NotifyListChange();
        }

        private void RemoveItem(DetailBillItemViewModel item)
        {
            ListDetailBill.Remove(item);
        }

        private void SaveToDatabase()
        {
            // save bill
            BillModel.SaveToDatabase(newBill);

            // save detail bill
            foreach(DetailBillItemViewModel item in ListDetailBill)
            {
                DetailBillModel.SaveToDatabase(item.DetailBillModel);
            }
            
        }

        private void RemoveDetailItem(DetailBillItemViewModel item)
        {
            ListDetailBill.Remove(item);
            NotifyDetaillBillProperty();
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
            graphic.DrawString(InputDeskNo.ToString(), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + 40);

            graphic.DrawString("GIỜ BẮT ĐẦU".PadLeft(8), font, new SolidBrush(Color.Black), startX, startY + 65);
            graphic.DrawString(DateTime.Now.ToString(), font, new SolidBrush(Color.Black), startX + 70, startY + 65);

            string top = "Tên Mặt Hàng".PadRight(17) + "ĐVT".PadRight(7) + "SL".PadRight(6)  + "TT";
            graphic.DrawString(top, font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight; //make the spacing consistent
            graphic.DrawString("------------------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            offset = offset + (int)FontHeight + 5; //make the spacing consistent

            //Check if product was chosen, then update quantity
            foreach (DetailBillItemViewModel item in ListDetailBill)
            {
                graphic.DrawString(item.ProductName, font, new SolidBrush(Color.Black), startX, startY + offset);
                graphic.DrawString(item.UnitName, font, new SolidBrush(Color.Black), startX + 90, startY + offset);
                graphic.DrawString(item.Quantity.ToString(), font, new SolidBrush(Color.Black), startX + 125, startY + offset);
                graphic.DrawString((item.Price * item.Quantity).ToString(), font, new SolidBrush(Color.Black), startX + 148, startY + offset);
                offset = offset + (int)FontHeight + 5; //make the spacing consistent  
            }

            //offset = offset + 20; //make some room so that the total stands out.          

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("TỔNG CỘNG", new Font("Courier New", 8, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset - 10);
            graphic.DrawString(SumPrice.ToString(), font, new SolidBrush(Color.Black), startX + 80, startY + offset - 10);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("XIN CHÂN THÀNH CẢM ƠN QUÝ KHÁCH!".PadLeft(10), font, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("HẸN GẶP LẠI!".PadLeft(23), font, new SolidBrush(Color.Black), startX, startY + offset);
        }
    }
}
