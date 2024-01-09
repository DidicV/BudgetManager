using BudgetManager.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditTransactionGoal : ContentPage
    {
        private int selectedTransactionId;
        private int selectedGoalId;

        public AddOrEditTransactionGoal()
        {
            InitializeComponent();
            Title = "Add new goal";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            goalPicker.ItemsSource = await App._goalRepository.GetGoals();

            // exclude existing transactions
            var transactionGoals = await App._transactionGoalRepository.GetTransactionGoals();
            var excludedIds = transactionGoals.Select(t => t.TransactionId);

            var transactions = await App._transactionRepository.GetTransactions();

            foreach (var id in excludedIds)
            {
                var transaction = transactions.FirstOrDefault(t => t.Id == id);
                transactions.Remove(transaction);
            }

            transactionPicker.ItemsSource = transactions;
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            selectedTransactionId = ((Models.Transaction)transactionPicker.SelectedItem)?.Id ?? 0;
            selectedGoalId = ((Goal)goalPicker.SelectedItem)?.Id ?? 0;

            if (selectedTransactionId == 0 ||
                selectedGoalId == 0)
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else
            {
                AddGoal();
            }
        }

        private async void AddGoal()
        {
            try
            {
                await App._transactionGoalRepository.AddTransactionGoal(
                new TransactionGoal
                {
                    TransactionId = selectedTransactionId,
                    GoalId = selectedGoalId
                });
            }
            catch
            {
                await DisplayAlert("Invalid", "Only one transaction can be assigned to goal", "Ok");
            }

            await Navigation.PopAsync();
        }
    }
}