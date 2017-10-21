using GymProgFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GymProgUI.ViewModels
{
    public abstract class BaseSelectionViewModel<DTOType> : BaseViewModel where DTOType : FiterableDTO
    {
        private ICollection<DTOType> _items { get; set; }

        public ICollection<DTOType> Items
        {
            get
            {
                if (_searchText != null && _searchText != "")
                {
                    return _items.Where(currItem => currItem.Name.Contains(_searchText)).ToList();
                }
                else
                {
                    return _items;
                }
            }
            set
            {
                _items = value;
            }
        }

        public DTOType SelectedItem { get; set; }

        public Action<DTOType> ExectueAfterAdd { get; set; }

        private String _searchText { get; set; }

        public String SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                OnPropertyChanged("Items");
            }

        }

        public BaseSelectionViewModel(Action<DTOType> exectueAfterAdd) :  base(true)
        {
            ExectueAfterAdd = exectueAfterAdd;
        }

        public ICommand SelectItem
        {
            get
            {
                return new Command(() =>
                {
                    ExectueAfterAdd(SelectedItem);
                });
            }
        }
    }
}
