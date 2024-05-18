using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Class;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Marketplaes02.ViewModel
{
    public class ViewModelFillterGoods : INotifyPropertyChanged
    {
        private int _OtPrice;
        private int _DoPrice;
        public int OtPrice
        {
            get => _OtPrice;
            set
            {
                _OtPrice = value;
                OnPropertyChanged("OtPrice");
            }
        }
        public int DoPrice
        {
            get => _DoPrice;
            set
            {
                _DoPrice = value;
                OnPropertyChanged("DoPrice");
            }
        }
        public ICommand FillterGoodsPriceCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        public ViewModelFillterGoods()
        {
            FillterGoodsPriceCommand = new Command(FillterPriceGoods);
        }
        public async void FillterPriceGoods()
        {
            // Отправляем сообщение, что нужно отсортировать список по цене
            WeakReferenceMessenger.Default.Send(new UpdateFillter(OtPrice, DoPrice));
            await MopupService.Instance.PopAsync();
        }
    }
}
