using Kafein.Model.List;
using Kafein.Model.SalesNPay;
using Kafein.Utilities;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ReportManagementViewModel: BaseViewModel
    {
        private ListBillModel listBillModel;

        private ObservableCollection<double> revenue;
        private ObservableCollection<double> expenditure;

        private ObservableCollection<int> revenue_month;
        private ObservableCollection<int> revenue_year;
        private ObservableCollection<int> expenditure_month;
        private ObservableCollection<int> expenditure_year;

        
        //private ObservableCollection<String> timeline;
        //private ObservableCollection<DateTime> listDate;

        public ReportManagementViewModel(): base()
        {
            listBillModel = ListBillModel.GetInstance();
            ReportCollection = new SeriesCollection();
            MonthRevenueLabels = new ObservableCollection<String>();
            MonthExpenditureLabels = new ObservableCollection<String>();
            listBillModel.LoadAllBill();
            revenue_month = new ObservableCollection<int>();
            revenue_year = new ObservableCollection<int>();
            expenditure_month = new ObservableCollection<int>();
            expenditure_year = new ObservableCollection<int>();

            // Calculate revenue
            revenue = new ObservableCollection<double>();
            ObservableCollection<String[]> result_revenue = ListBillModel.GetRevenueByDayAndMonth();
            for(int i=0; i< result_revenue.Count;i++)
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
        }

        public ReportManagementViewModel(Action<object, object[]> navigate, object[] parameters) : this()
        {
            this.navigate = navigate;
        }

        public SeriesCollection ReportCollection { get; set; }
        public ObservableCollection<String> MonthRevenueLabels { get; set; }
        public ObservableCollection<String> MonthExpenditureLabels { get; set; }
        public Func<double, string> Formatter { get; set; }

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


    }
}
