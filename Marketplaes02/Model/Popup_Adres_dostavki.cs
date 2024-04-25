using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using Marketplaes02.View;

namespace Marketplaes02.Model
{
    public class Popup_Adres_dostavki : INotifyPropertyChanged
    {
        private readonly IPopupService popupService;

        public Popup_Adres_dostavki(IPopupService popupService)
        {
            this.popupService = popupService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void DisplayPopup()
        {
            this.popupService.ShowPopup<ViewAddAdres_dostavki>();
        }

    }
}
