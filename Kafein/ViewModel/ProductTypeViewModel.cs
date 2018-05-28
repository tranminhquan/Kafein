using Kafein.Model;
using Kafein.Model.List;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kafein.ViewModel
{
    public class ProductTypeViewModel: BaseViewModel
    {
        private ListProductTypeModel listProductTypeModel;

        public ProductTypeViewModel()
        {
            listProductTypeModel = new ListProductTypeModel();         
            listProductTypeModel.LoadAllProductType();
            listProductTypeModel.List.Add(new ProductTypeModel("LMH0", "Tất Cả"));
            SelectedType = listProductTypeModel.List[listProductTypeModel.List.Count - 1];
            NotifyChanged("SelectedType");
            TypeSelectionChangeCommand = new DelegateCommand<ProductTypeModel>(ProductTypeChange);
        }

        public ObservableCollection<ProductTypeModel> ListProductType
        {
            get { return listProductTypeModel.List; }
            set { listProductTypeModel.List = value; NotifyChanged("ListProductType"); }
        }
        public ProductTypeModel SelectedType { get; set; }
        public DelegateCommand<ProductTypeModel> TypeSelectionChangeCommand { get; set; }

        private void ProductTypeChange(ProductTypeModel item)
        {
            if (item.ID == "LMH0")
                ListProductModel.GetInstance().LoadAllProduct();
            else
                ListProductModel.GetInstance().LoadProductFromType(item.ID);
        }
    }
}
