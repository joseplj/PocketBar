﻿using PocketBar.Services;
using Prism;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PocketBar.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, IActiveAware, INavigatedAware, IInitialize
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler IsActiveChanged;
        public bool IsEmpty { get; set; }
        public bool IsRefreshing { get; set; } = false;

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set { _isActive = value; IsActiveChanged?.Invoke(this, EventArgs.Empty); }
        }
        public INavigationService NavigationService { get; set; }
        public IPageDialogService PageDialogService { get; set; }
        public DelegateCommand OnPressedBackCommand { get; set; }
        public bool IsLoading { get; set; }

        public BaseViewModel(PageDialogService pageDialogService, INavigationService navigationService)
        {
            this.PageDialogService = pageDialogService;
            this.NavigationService = navigationService;
            OnPressedBackCommand = new DelegateCommand(async() => { await GoBack(); });
        }

        public async Task<bool> HasInternetConnection(bool sendMessage = false)
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return true;
            }
            else
            {
                if (sendMessage)
                {
                    await App.Current.MainPage.DisplayAlert(Constants.ErrorMessages.NoInternet,Constants.ErrorMessages.NoInternetDescription, Constants.ErrorMessages.Ok);

                }
                return false;
            }
        }

        public async Task ShowMessage(string title, string message, string cancel, string accept = null)
        {
           await PageDialogService.DisplayAlertAsync(title, message, accept, cancel);
        }

        public async Task GoBack()
        {
            await NavigationService.GoBackAsync();
        }

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
            // do nothing, this is meant to be overriden
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
            // do nothing, this is meant to be overriden
        }

        public virtual void Initialize(INavigationParameters parameters)
        {
            // do nothing, this is meant to be overriden
        }
    }
}
