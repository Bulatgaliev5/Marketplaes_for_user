using CommunityToolkit.Mvvm.Messaging;
using Marketplaes02.Class;
using Mopups.PreBaked.Services;
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
        private float _OtPrice;
        private float _DoPrice;
        public float OtPrice
        {
            get => _OtPrice;
            set
            {
                _OtPrice = value;
                OnPropertyChanged("OtPrice");
            }
        }
        public float DoPrice
        {
            get => _DoPrice;
            set
            {
                _DoPrice = value;
                OnPropertyChanged("DoPrice");
            }
        }
        public ICommand FillterGoodsPriceCommand { get; }
        public ICommand SbrosCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            if (property == null)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        }
        private bool _isExecuting;


        public ViewModelFillterGoods(float otPrice, float doPrice)
        {
            OtPrice = otPrice;
            DoPrice = doPrice;
            FillterGoodsPriceCommand = new Command(async () => await FillterPriceGoods(), () => !_isExecuting);
            SbrosCommand = new Command(async () => await Sbros(), () => !_isExecuting);
        }

        public async Task FillterPriceGoods()
        {
            if (_isExecuting)
                return;

            _isExecuting = true;

            try
            {
                await Search();
                // Отправляем сообщение, что нужно отсортировать список по цене
                WeakReferenceMessenger.Default.Send(new UpdateFillter(OtPrice, DoPrice));
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

        public async Task Sbros()
        {
            if (_isExecuting)
                return;

            _isExecuting = true;

            try
            {
                await Search();
                // Отправляем сообщение, что нужно отсортировать список по цене
                WeakReferenceMessenger.Default.Send(new UpdateFillter(0, 3402823));
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
        public async Task Search()
        {
            List<string> message = ["Поиск.."];
            await PreBakedMopupService.GetInstance().WrapTaskInLoader(Task.Delay(3000), Color.FromRgb(0, 127, 255), Color.FromRgb(255, 255, 250), message, Color.FromRgb(0, 0, 0));
        }

    }
}
