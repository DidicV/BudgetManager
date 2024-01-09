using BudgetManager.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionTypePage : ContentPage
    {
        public TransactionTypePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            myCollectionView.ItemsSource = await App._transactionTypeRepository.GetTransactionTypes();
        }

        private async void GoToAddOrEditTransactionType(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditTransactionType());
        }

        private async void SwipeEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var transactionType = item.CommandParameter as Models.TransactionType;
            await Navigation.PushAsync(new AddOrEditTransactionType(transactionType));
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var transactionType = item.CommandParameter as Models.TransactionType;

            var isTransactionType = await App._transactionRepository.CheckTransactionTypeId(transactionType.Id);

            if (isTransactionType)
            {
                await DisplayAlert("Delete", $"Payment method {transactionType.Name} is present in transaction table and cannot be deleted", "Ok");
            }
            else if (transactionType.Name.IsOneOf("Incomes", "Expenses"))
            {
                await DisplayAlert("Invalid", $"{transactionType.Name} cannot be added or modified!", "Ok");
            }
            else
            {
                var result = await DisplayAlert("Delete", $"Delete '{transactionType.Name}' ?", "Yes", "No");
                if (result)
                {
                    await App._transactionTypeRepository.DeleteTransactionType(transactionType);
                    myCollectionView.ItemsSource = await App._transactionTypeRepository.GetTransactionTypes();
                }
            }
        }

        private async void SeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App._transactionTypeRepository.Seach(e.NewTextValue);
        }
    }
}