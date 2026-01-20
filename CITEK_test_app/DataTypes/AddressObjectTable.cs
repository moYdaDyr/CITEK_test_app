using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CITEK_test_app
{
    public class AddressObjectTable
    {
        public string CategoryName { get; private set; }


        ObservableCollection<AddressObjectInfoPair> _addressObjects;

        public ObservableCollection<AddressObjectInfoPair> AddressObjects
        {
            get
            {
                return _addressObjects;
            }
            private set
            {
                _addressObjects = value;
            }
        }

        public AddressObjectTable(string addressObjectCategoryName)
        {
            CategoryName = addressObjectCategoryName;

            _addressObjects = new ObservableCollection<AddressObjectInfoPair>();
        }

        public void SortByName()
        {
            var sortedList = from obj in AddressObjects orderby obj.Name select obj;

            AddressObjects = new ObservableCollection<AddressObjectInfoPair>(sortedList);
        }
    }
}
