using BudgetManager.DTO;
using BudgetManager.Models;
using BudgetManager.Views.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionPage : ContentPage
    {
        private FilterModel currentFilter;

        private IEnumerable<TransactionDTO> allTransactions;

        public TransactionPage()
        {
            InitializeComponent();
            currentFilter = new FilterModel();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            allTransactions = await App._transactionRepository.GetTransactionsDTO();
            ApplyFiltersAndSetItemsSource();
        }

        private void ApplyFiltersAndSetItemsSource()
        {
            var filteredTransactions = ApplyFilters(allTransactions, currentFilter);
            myCollectionView.ItemsSource = filteredTransactions.OrderByDescending(t => t.Date);
            var totalIncomesSum = filteredTransactions.Where(t => t.TransactionTypeName == "Incomes").Sum(t=>t.Amount);
            var totalExpenseSum = filteredTransactions.Where(t => t.TransactionTypeName == "Expenses").Sum(t => t.Amount);
            var profit = totalIncomesSum - totalExpenseSum;

            totalExpenseLabel.Text = totalExpenseSum.ToString();
            totalIncomesLabel.Text = totalIncomesSum.ToString();
            profitLabel.Text = profit.ToString();
        }

        private IEnumerable<TransactionDTO> ApplyFilters(IEnumerable<TransactionDTO> transactions, FilterModel filter)
        {
            var filteredList = transactions
                .Where(t =>
                    (filter.MinAmount == null || t.Amount >= filter.MinAmount) &&
                    (filter.MaxAmount == null || t.Amount <= filter.MaxAmount) &&
                    (filter.TransactionType == null || t.TransactionTypeName == filter.TransactionType) &&
                    (filter.Category == null || t.CategoryName == filter.Category) &&
                    (filter.PaymentMethod == null || t.PaymentMethodName == filter.PaymentMethod) &&
                     t.Date >= filter.StartDate &&
                     t.Date <= filter.EndDate
                );
            return filteredList;
        }

        private async void GoToAddOrEditTransaction(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditTransaction());
        }

        private async void SwipeEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var transaction = item.CommandParameter as TransactionDTO;
            await Navigation.PushAsync(new AddOrEditTransaction(transaction));
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var transaction = item.CommandParameter as TransactionDTO;


            var isTransactionInGoal = await App._transactionRepository.CheckGoalId(transaction.Id);

            if (isTransactionInGoal)
            {
                await DisplayAlert("Delete", $"Transaction {transaction.Amount} is present in Goal table and cannot be deleted", "Ok");
            }
            else
            {
                var result = await DisplayAlert("Delete", $"Delete '{transaction.Amount}' ?", "Yes", "No");
                if (result)
                {
                    await App._transactionRepository.DeleteTransactionById(transaction.Id);
                    OnAppearing();
                }
            }
        }

        private async void ShowFilterPopup(object sender, EventArgs e)
        {
            var filterPopupPage = new FilterPopupPage(ApplyFiltersAndRefresh, currentFilter);
            await Navigation.PushModalAsync(filterPopupPage);
        }

        private void ApplyFiltersAndRefresh(FilterModel newFilter)
        {
            currentFilter = newFilter;
            ApplyFiltersAndSetItemsSource();
        }
    }
}