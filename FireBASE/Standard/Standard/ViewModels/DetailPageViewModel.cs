using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Standard.ViewModels
{
    using System.ComponentModel;
    using Prism.Events;
    using Prism.Navigation;
    using Prism.Services;
    using Standard.Models;
    using Firebase.Database.Query;
    using Standard.Helpers;

    public class DetailPageViewModel : INotifyPropertyChanged, INavigationAware
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public InvoiceVM InvoiceSelected { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsAdd { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }

        private readonly INavigationService _navigationService;

        public DetailPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            SaveCommand = new DelegateCommand(OnSaveCommand);
            DeleteCommand = new DelegateCommand(OnDeleteCommand);
        }

        private async void OnDeleteCommand()
        {
            var child = MainHelper.FirebaseClient.Child("Invoices");
            await child.Child(InvoiceSelected.Key).DeleteAsync();

            var fooPara = new NavigationParameters();
            fooPara.Add("NeedRefresh", "");
            _navigationService.GoBackAsync(fooPara);
        }

        private async void OnSaveCommand()
        {
            if (IsUpdate == false)
            {
                var child = MainHelper.FirebaseClient.Child("Invoices");
                await child.PostAsync<Invoice>(new Invoice()
                {
                    InvoiceNo = InvoiceSelected.InvoiceNo,
                    Amount = InvoiceSelected.Amount,
                    Date = InvoiceSelected.Date,
                    Memo = InvoiceSelected.Memo,
                    Title = InvoiceSelected.Title,
                });
            }
            else
            {
                var child = MainHelper.FirebaseClient.Child("Invoices");
                await child.Child(InvoiceSelected.Key).PutAsync(new Invoice()
                {
                    InvoiceNo = InvoiceSelected.InvoiceNo,
                    Amount = InvoiceSelected.Amount,
                    Date = InvoiceSelected.Date,
                    Memo = InvoiceSelected.Memo,
                    Title = InvoiceSelected.Title,
                });
            }

            var fooPara = new NavigationParameters();
            fooPara.Add("NeedRefresh", "");
            _navigationService.GoBackAsync(fooPara);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("Action"))
            {
                var action = parameters["Action"] as string;
                if (action == "Update")
                {
                    if (parameters.ContainsKey("Item"))
                    {
                        var key = parameters["Item"] as string;
                        var fooItem = parameters["Item"] as Firebase.Database.FirebaseObject<Invoice>;
                        InvoiceSelected = new InvoiceVM()
                        {
                            Key = fooItem.Key,
                            Title = fooItem.Object.Title,
                            Amount = fooItem.Object.Amount,
                            Date = fooItem.Object.Date,
                            InvoiceNo = fooItem.Object.InvoiceNo,
                            Memo = fooItem.Object.Memo,
                        };
                        IsUpdate = true;
                        IsAdd = false;
                    }
                }
                else
                {
                    InvoiceSelected = new InvoiceVM()
                    {
                        Title = "新增發票要輸入的名稱",
                    };
                    IsUpdate = false;
                    IsAdd = true;
                }
            }
        }

    }
}
