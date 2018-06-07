﻿using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using Kafein.View.Dialog;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Kafein.ViewModel
{
    public class ListProductViewModel: BaseViewModel
    {
        private ListProductModel listProductModel;
        private BillModel newBill;
        private ListDetailBillModel listDetailBill;
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
                newBill = (BillModel)parameters[0];
                listDetailBill = (ListDetailBillModel)parameters[1];
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

            (new CheckoutDialog(navigate, newBill, ListDetailBill)).ShowDialog();

            // save to database
            //SaveToDatabase();

        }

        private void PrintBill()
        {
            // print
        }

        private void ClearBill()
        {
            ListDetailBill.Clear();
            NotifyChanged("SumPrice");
        }

        private void Cancel()
        {
            navigate.Invoke("BillManagementViewModel", null);
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
    }
}
