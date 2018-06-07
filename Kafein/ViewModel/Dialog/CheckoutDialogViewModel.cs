using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kafein.ViewModel.Dialog
{
    public class CheckoutDialogViewModel : BaseViewModel
    {
        private BillModel bill;
        private ObservableCollection<DetailBillItemViewModel> listDetailBill;

        public CheckoutDialogViewModel() : base()
        {
            TextChangeCommand = new DelegateCommand<TextBox>(OnReceivedMoneyChange);

            CancelCommand = new DelegateCommand(Cancel);
            PrintAndCheckoutCommand = new DelegateCommand(PrintAndCheckout);
            CheckoutCommand = new DelegateCommand(Checkout);
        }

        private CheckoutDialogViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        // getter and setter
        public DelegateCommand<TextBox> TextChangeCommand { get; set; }
        public int Index { get; set; }

        public BillModel Bill
        {
            get { return bill; }
            set
            {
                bill = value;
                NotifyChanged("Bill");
                NotifyChanged("Title");
                NotifyChanged("SumPrice");
                NotifyChanged("Change");
                NotifyChanged("DeskNo");
            }
        }

        public ObservableCollection<DetailBillItemViewModel> ListDetailBill
        {
            get { return listDetailBill; }
            set
            {
                listDetailBill = value;
                NotifyChanged("ListDetailBill");
            }
        }

        public Action<object, object[]> Navigate
        {
            get { return this.navigate; }
            set
            {
                this.navigate = value;
                NotifyChanged("Navigate");
            }
        }

        public int DeskNo
        {
            get { return bill.DeskNo; }
        }

        public double SumPrice
        {
            get { return bill.Price; }
        }

        public double MoneyReceived { get; set; }
        public double Change { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand PrintAndCheckoutCommand { get; set; }
        public DelegateCommand CheckoutCommand { get; set; }

        public void OnReceivedMoneyChange(TextBox textBox)
        {
            try
            {
                Change = Convert.ToDouble(textBox.Text) - SumPrice;
                NotifyChanged("Change");
            }
            catch (FormatException)
            {
                return;
            }
        }

        private void SaveToDatabase()
        {
            // save bill
            BillModel.SaveToDatabase(bill);

            // save detail bill
            foreach (DetailBillItemViewModel item in listDetailBill)
            {
                DetailBillModel.SaveToDatabase(item.DetailBillModel);
            }

            // if it a awaiting bill
            if (Index >= 0)
            {
                ListGeneralBillModel.GetInstance().List.Remove(ListGeneralBillModel.GetInstance().List[Index]);
                ListGeneralBillModel.GetInstance().NotifyListChange();
            }
        }

        private void Cancel()
        {
            foreach (Window window in Application.Current.Windows)
                if (window.Title == "CheckoutDialog")
                    window.Close();
        }

        private void PrintAndCheckout()
        {
            SaveToDatabase();
            Print();
            Cancel();
        }

        private void Checkout()
        {
            SaveToDatabase();
            navigate.Invoke("BillManagementViewModel", null);
            Cancel();
        }

        private void Print()
        {
            try
            {
                PrintDialog printDialog = new PrintDialog();
                PrintDocument printDocument = new PrintDocument();

                printDocument.PrintPage +=
                    new System.Drawing.Printing.PrintPageEventHandler(
                        _CreateReceipt); //add an event handler that will do the printing

                if (printDialog.ShowDialog() == true)
                {
                    printDocument.Print();
                }
            }
            catch (Exception)
            {

            }
        }

        private void _CreateReceipt(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;
            Font font = new Font("Courier New", 6);
            float FontHeight = font.GetHeight();
            int startX = 10;
            int startY = 10;
            int offset = 90;

            graphic.DrawString("CÀ PHÊ TRI ÂN".PadLeft(16), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY);

            graphic.DrawString("252 Sông Lu, xã Trung An".PadLeft(25), font, new SolidBrush(Color.Black), startX + 22, startY + 15);
            graphic.DrawString("huyện Củ Chi, TP.HCM".PadLeft(27), font, new SolidBrush(Color.Black), startX, startY + 25);

            graphic.DrawString("HÓA ĐƠN BÀN SỐ".PadLeft(16), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + 40);
            graphic.DrawString(bill.DeskNo.ToString(), new Font("Courier New", 10, System.Drawing.FontStyle.Bold), new SolidBrush(Color.Black), startX + 150, startY + 40);

            graphic.DrawString("GIỜ BẮT ĐẦU".PadLeft(8), font, new SolidBrush(Color.Black), startX, startY + 65);
            graphic.DrawString(bill.OrderTime, font, new SolidBrush(Color.Black), startX + 70, startY + 65);

            string top = "Tên Mặt Hàng".PadRight(17) + "ĐVT".PadRight(7) + "SL".PadRight(6) + "TT";
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

            graphic.DrawString("Tiền nhận", font, new SolidBrush(Color.Black), startX, startY + offset + 5);
            graphic.DrawString(MoneyReceived.ToString(), font, new SolidBrush(Color.Black), startX + 80, startY + offset + 5);

            graphic.DrawString("Tiền thối", font, new SolidBrush(Color.Black), startX, startY + offset + 15);
            graphic.DrawString(Change.ToString(), font, new SolidBrush(Color.Black), startX + 80, startY + offset + 15);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("XIN CHÂN THÀNH CẢM ƠN QUÝ KHÁCH!".PadLeft(12), font, new SolidBrush(Color.Black), startX, startY + offset + 30);

            offset = offset + (int)FontHeight + 5; //make the spacing consistent              
            graphic.DrawString("HẸN GẶP LẠI!".PadLeft(23), font, new SolidBrush(Color.Black), startX, startY + offset + 35);
        }
    }
}
