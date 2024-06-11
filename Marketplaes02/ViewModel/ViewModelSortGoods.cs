using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Class;
using Marketplaes02.Model;
using Mopups.PreBaked.AbstractClasses;
using Mopups.PreBaked.Interfaces;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Marketplaes02.ViewModel
{
    public class ViewModelSortGoods : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }

        public ICommand SortGoodsPriceCommand { get; }
        private string _SelectedSortOption;

        public string SelectedSortOption
        {
            get => _SelectedSortOption;
            set
            {
                _SelectedSortOption = value;
                OnPropertyChanged("SelectedSortOption");

            }
        }


        private IList<bool> _boolRadioButton;
        public IList<bool> BoolRadioButton
        {
            get => _boolRadioButton;
            set
            {
                _boolRadioButton = value;
                OnPropertyChanged("BoolRadioButton");
            }
        }
        private bool _isExecuting;


        public ViewModelSortGoods(IList<bool> boolRadioButton)
        {
            BoolRadioButton = boolRadioButton;
            SortGoodsPriceCommand = new Command<string>(async (sortOption) => await SortGoodsPrice(sortOption), (sortOption) => !_isExecuting);
        }

        public async Task SortGoodsPrice(string sortOption)
        {
            if (_isExecuting)
                return;

            _isExecuting = true;

            try
            {
                // Отправляем сообщение, что нужно отсортировать список по цене
                WeakReferenceMessenger.Default.Send(new UpdateSort(sortOption));
                await MopupService.Instance.PopAsync();
            }
            catch (InvalidOperationException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Уведомление", "Страница уже открыто или закрыто: " + ex.Message, "ОK");

            }
            finally
            {
                _isExecuting = false;
            }
        }


    }

}
