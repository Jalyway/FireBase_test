using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Standard.ViewModels
{
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Standard.Helpers;
    using Standard.Models;
    using Firebase.Database.Query;

    public class MainPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<InvoiceVM> InvoiceSource { get; set; } = new ObservableCollection<InvoiceVM>();
        public InvoiceVM InvoiceSelected { get; set; }
        public bool IsRefreshing { get; set; }
        public DelegateCommand RefreshCommand { get; set; }
        private readonly INavigationService _navigationService;
        public DelegateCommand TestCommand { get; set; }
        public DelegateCommand TapCommand { get; set; }
        public DelegateCommand AddCommand { get; set; }
        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            RefreshCommand = new DelegateCommand(OnRefreshCommand);
            TestCommand = new DelegateCommand(OnTestCommand);
            TapCommand = new DelegateCommand(OnTapCommand);
            AddCommand = new DelegateCommand(OnAddCommand);
        }

        private async void OnAddCommand()
        {
            var fooItem = await GetInvoice(InvoiceSelected.Key);
            var fooPara = new NavigationParameters();
            fooPara.Add("Action", "Add");
            await _navigationService.NavigateAsync("DetailPage", fooPara);
        }

        private async void OnTapCommand()
        {
            var fooItem = await GetInvoice(InvoiceSelected.Key);
            var fooPara = new NavigationParameters();
            fooPara.Add("Action", "Update");
            fooPara.Add("Item", fooItem);
            await _navigationService.NavigateAsync("DetailPage", fooPara);
        }

        async Task<Firebase.Database.FirebaseObject<Invoice>> GetInvoice(string key)
        {
            var child = MainHelper.FirebaseClient.Child("Invoices");
            var fooPosts = await child.OnceAsync<Invoice>();
            var fooObject = fooPosts.FirstOrDefault(x => x.Key == key);
            return fooObject;
        }

        private async void OnTestCommand()
        {
            var child = MainHelper.FirebaseClient.Child("Invoices");
            await child.DeleteAsync();
            for (int i = 1; i < 5; i++)
            {
                await child.PostAsync<Invoice>(new Invoice()
                {
                    InvoiceNo = i.ToString(),
                    Amount = 100 + i * 5,
                    Date = DateTime.Now.AddDays(-1 * i),
                    Memo = $"Memo{i}",
                    Title = $"Invoice {i}"
                });
            }
        }

        private async void OnRefreshCommand()
        {
            await RefreshInvoice();
            IsRefreshing = false;

        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("NeedRefresh"))
            {
                IsRefreshing = true;
                await RefreshInvoice();
                IsRefreshing = false;
            }
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {

        }
        
        public async Task RefreshInvoice()
        {
            InvoiceSource.Clear();
            var child = MainHelper.FirebaseClient.Child("Invoices");
            var fooPosts = await child.OnceAsync<Invoice>();
            foreach(var item in fooPosts)
            {
                var fooItem = new InvoiceVM()
                {
                    Amount = item.Object.Amount,
                    Date = item.Object.Date,
                    InvoiceNo = item.Object.InvoiceNo,
                    Memo = item.Object.Title,
                    Key = item.Key,
                };
                InvoiceSource.Add(fooItem);
            }
        }
    }
}
