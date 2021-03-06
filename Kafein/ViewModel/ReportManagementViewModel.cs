﻿using Kafein.Model;
using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Office.Interop.Excel;

namespace Kafein.ViewModel
{
    public class ReportManagementViewModel : BaseViewModel
    {
        private ListBillModel listBillModel;
        private ListRevenueModel listRevenueModel;
        private ListExpenditureModel listExpenditureModel;

        private ObservableCollection<double> revenue;
        private ObservableCollection<double> expenditure;

        private ObservableCollection<int> revenue_month;
        private ObservableCollection<int> revenue_year;
        private ObservableCollection<int> expenditure_month;
        private ObservableCollection<int> expenditure_year;
        private ObservableCollection<int> overview_month;
        private ObservableCollection<int> overview_year;


        //private ObservableCollection<String> timeline;
        //private ObservableCollection<DateTime> listDate;

        public ReportManagementViewModel() : base()
        {
            listBillModel = ListBillModel.GetInstance();
            listBillModel.LoadAllBill();

            listRevenueModel = ListRevenueModel.GetInstance();
            listExpenditureModel = ListExpenditureModel.GetInstance();

            ReportCollection = new LiveCharts.SeriesCollection();

            MonthRevenueLabels = new ObservableCollection<String>();
            MonthExpenditureLabels = new ObservableCollection<String>();
            MonthReportLabels = new ObservableCollection<string>();

            revenue_month = new ObservableCollection<int>();
            revenue_year = new ObservableCollection<int>();
            expenditure_month = new ObservableCollection<int>();
            expenditure_year = new ObservableCollection<int>();
            overview_month = new ObservableCollection<int>();
            overview_year = new ObservableCollection<int>();

            // Calculate revenue
            revenue = new ObservableCollection<double>();
            ObservableCollection<String[]> result_revenue = ListBillModel.GetRevenueByDayAndMonth();
            for (int i = 0; i < result_revenue.Count; i++)
            {
                //Debug.LogOutput(result_revenue[i][0]);
                String item_timeline = result_revenue[i][0].ToString();
                double item_revenue = Double.Parse(result_revenue[i][1].ToString());

                MonthRevenueLabels.Add(item_timeline);
                revenue_month.Add(Int32.Parse(result_revenue[i][2].ToString()));
                revenue_year.Add(Int32.Parse(result_revenue[i][3].ToString()));
                revenue.Add(item_revenue);
            }

            ReportCollection.Add(new ColumnSeries()
            {
                Title = "Tổng thu",
                Values = new ChartValues<double>(revenue)
            });

            // Calculate expenditure
            expenditure = new ObservableCollection<double>();
            ObservableCollection<String[]> result_expenditure = ListImportationModel.GetExpenditureByDayAndMonth();
            for (int i = 0; i < result_expenditure.Count; i++)
            {
                Debug.LogOutput(result_expenditure[i][0]);
                String item_timeline = result_expenditure[i][0].ToString();
                double item_expenditure = Double.Parse(result_expenditure[i][1].ToString());

                MonthExpenditureLabels.Add(item_timeline);
                expenditure_month.Add(Int32.Parse(result_expenditure[i][2].ToString()));
                expenditure_year.Add(Int32.Parse(result_expenditure[i][3].ToString()));
                expenditure.Add(item_expenditure);
            }

            ReportCollection.Add(new ColumnSeries()
            {
                Title = "Tổng chi",
                Values = new ChartValues<double>(expenditure)
            });

            Formatter = value => value.ToString("N");

            // Calculate for MonthReportLabels: find min and max timeline
            ObservableCollection<string[]> overview_temp = new ObservableCollection<string[]>();
            overview_temp = AdvancedQuery.GetAllTimeline();
            for (int i = 0; i < overview_temp.Count; i++)
            {
                MonthReportLabels.Add(overview_temp[i][0]);
                overview_month.Add(Int32.Parse(overview_temp[i][1]));
                overview_year.Add(Int32.Parse(overview_temp[i][2]));
            }

            // Export Excel
            ExportExcelCommand = new DelegateCommand(ExportExcel);

            // Month report combobox
            MonthReportChangeCommand = new DelegateCommand<string>(MonthReportChange);

            // Month product combobox
            NotifyChanged("SelectedMonthProduct");
            MonthProductChangeCommand = new DelegateCommand<string>(MonthProductChange);

            // init product chart
            ProductSeries = new LiveCharts.SeriesCollection();

            // Month product combobox
            NotifyChanged("SelectedMonthIngredient");
            MonthIngredientChangeCommand = new DelegateCommand<string>(MonthIngredientChange);

            // init ingredient chart
            IngredientSeries = new LiveCharts.SeriesCollection();

        }

        public ReportManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        public ObservableCollection<RevenueModel> ListRevenue
        {
            get { return listRevenueModel.List; }
            set { listRevenueModel.List = value; NotifyChanged("ListRevenue"); }
        }

        public double TotalRevenue
        {
            get
            {
                return listRevenueModel.List.Sum(a => a.Value * a.Quantity);
            }
        }

        public ObservableCollection<ExpenditureModel> ListExpenditure
        {
            get { return listExpenditureModel.List; }
            set { listExpenditureModel.List = value; NotifyChanged("ListExpanditure"); }
        }

        public double TotalExpenditure
        {
            get
            {
                return listExpenditureModel.List.Sum(a => a.Value * a.Quantity);
            }
        }

        public double Profit
        {
            get
            {
                return TotalRevenue - TotalExpenditure;
            }
        }

        public LiveCharts.SeriesCollection ReportCollection { get; set; }
        public ObservableCollection<String> MonthRevenueLabels { get; set; }
        public ObservableCollection<String> MonthExpenditureLabels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public DelegateCommand ExportExcelCommand { get; set; }

        // Combo box for overview report
        public string SelectedMonthReport { get; set; }
        public DelegateCommand<string> MonthReportChangeCommand { get; set; }
        public ObservableCollection<string> MonthReportLabels { get; set; }

        // Combox for product
        public string SelectedMonthProduct { get; set; }
        public DelegateCommand<string> MonthProductChangeCommand { get; set; }

        // Product chart
        public LiveCharts.SeriesCollection ProductSeries { get; set; }

        // Combox for ingredient
        public string SelectedMonthIngredient { get; set; }
        public DelegateCommand<string> MonthIngredientChangeCommand { get; set; }

        // Ingredient chart
        public LiveCharts.SeriesCollection IngredientSeries { get; set; }

        //// getter and setter
        //public ObservableCollection<BillModel> ListBill
        //{
        //    get { return listBillModel.List; }
        //    set
        //    {
        //        listBillModel.List = value;
        //        NotifyChanged("ListBillModel");
        //        //NotifyBillChange();
        //    }
        //}

        public void ReportTypeChange(string type)
        {
            Debug.LogOutput(type);
        }

        public void MonthProductChange(string item)
        {
            int index = MonthRevenueLabels.IndexOf(item);
            ObservableCollection<string[]> result = AdvancedQuery.GetProductRevenue(revenue_month[index], revenue_year[index]);
            ProductSeries.Clear();
            // Debug log
            for (int i = 0; i < result.Count; i++)
            {
                ProductSeries.Add
                    (
                        new PieSeries
                        {
                            Title = result[i][1] + " (" + result[i][2] + ")",
                            Values = new ChartValues<ObservableValue> { new ObservableValue(Double.Parse(result[i][3])) },
                            DataLabels = true
                        }
                    );

            }
        }

        public void MonthIngredientChange(string item)
        {
            int index = MonthExpenditureLabels.IndexOf(item);
            ObservableCollection<string[]> result = AdvancedQuery.GetIngredientExpenditure(expenditure_month[index], expenditure_year[index]);
            IngredientSeries.Clear();
            // Debug log
            for (int i = 0; i < result.Count; i++)
            {
                IngredientSeries.Add
                    (
                        new PieSeries
                        {
                            Title = result[i][1] + " (" + result[i][2] + ")",
                            Values = new ChartValues<ObservableValue> { new ObservableValue(Double.Parse(result[i][3])) },
                            DataLabels = true
                        }
                    );

            }
        }

        public void MonthReportChange(string time)
        {
            listRevenueModel.List.Clear();
            listExpenditureModel.List.Clear();

            // temp
            int index = MonthReportLabels.IndexOf(time);
            listRevenueModel.LoadRevenueReport(overview_month[index], overview_year[index]);
            listExpenditureModel.LoadExpenditureReport(overview_month[index], overview_year[index]);

            NotifyChanged("TotalRevenue");
            NotifyChanged("TotalExpenditure");
            NotifyChanged("Profit");
        }

        public void CalculateProfit()
        {

        }

        public void CalculateProduct()
        {

        }

        public void CalculateIngredient()
        {

        }

        private void ExportExcel()
        {
            Application app = new Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlNormal;
            //app.DisplayAlerts = false;

            Workbook wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            Worksheet ws = wb.Worksheets[1];

            ws.Range["A1"].Value = "Ngày lập báo cáo: " + DateTime.Now;
            ws.Range["A2"].Value = "Quán cà phê Tri Ân - Địa chỉ: 252 Sông Lu, xã Trung An";
            ws.Range["D4"].Value = "BÁO CÁO LỢI NHUẬN THÁNG " + SelectedMonthReport;
            ws.Range["D4"].Font.Bold = true;
            ws.Range["D4"].Font.Size = 20;

            ws.Range["B5"].Value = "Tổng thu";
            ws.Range["B5"].Font.Bold = true;
            ws.Range["C5"].Value = TotalRevenue;
            ws.Range["C5"].Font.Bold = true;
            ws.Range["D5"].Value = "Tổng chi";
            ws.Range["D5"].Font.Bold = true;
            ws.Range["E5"].Value = TotalExpenditure;
            ws.Range["E5"].Font.Bold = true;
            ws.Range["F5"].Value = "Lợi nhuận";
            ws.Range["F5"].Font.Bold = true;
            ws.Range["G5"].Value = Profit;
            ws.Range["G5"].Font.Bold = true;

            ws.Range["A7"].Value = "Ma CT Hoa Don";
            ws.Range["B7"].Value = "Ma HD";
            ws.Range["C7"].Value = "Ten mat hang";
            ws.Range["D7"].Value = "Ngay lap";
            ws.Range["E7"].Value = "So luong";
            ws.Range["F7"].Value = "Don gia";
            ws.Range["G7"].Value = "Thanh tien";
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["A" + i].Value = ListRevenue[i - 8].BillDetailID;
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["B" + i].Value = ListRevenue[i - 8].BillID;
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["C" + i].Value = ListRevenue[i - 8].ProductName;
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["D" + i].Value = ListRevenue[i - 8].Date.ToShortDateString();
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["E" + i].Value = ListRevenue[i - 8].Quantity;
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["F" + i].Value = ListRevenue[i - 8].Value;
            for (int i = 8; i < ListRevenue.Count + 8; i++)
                ws.Range["G" + i].Value = ListRevenue[i - 8].Price;

            ws.Range["I7"].Value = "Ma CT Phieu xuat";
            ws.Range["J7"].Value = "Ma Phieu xuat";
            ws.Range["K7"].Value = "Ten nguyen lieu";
            ws.Range["L7"].Value = "Ngay lap";
            ws.Range["M7"].Value = "So luong";
            ws.Range["N7"].Value = "Don gia";
            ws.Range["O7"].Value = "Thanh tien";

            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["I" + i].Value = ListExpenditure[i - 8].ImportationDetaillID;
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["J" + i].Value = ListExpenditure[i - 8].ImportationID;
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["K" + i].Value = ListExpenditure[i - 8].IngredientName;
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["L" + i].Value = ListExpenditure[i - 8].Date.ToShortDateString();
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["M" + i].Value = ListExpenditure[i - 8].Quantity;
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["N" + i].Value = ListExpenditure[i - 8].Value;
            for (int i = 8; i < ListExpenditure.Count + 8; i++)
                ws.Range["O" + i].Value = ListExpenditure[i - 8].Price;
        }
      
    }
}
