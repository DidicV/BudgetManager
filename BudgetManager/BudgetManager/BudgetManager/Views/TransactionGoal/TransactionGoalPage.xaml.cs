using BudgetManager.DTO;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionGoalPage : ContentPage
    {
        public TransactionGoalPage()
        {
            InitializeComponent();
        }

        private async void GoToAddOrEditTransactionGoal(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditTransactionGoal());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            myCollectionView.ItemsSource = await App._transactionGoalRepository.GetTransactionGoalsDTO();
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var transactionGoal = item.CommandParameter as GoalDTO;
            var result = await DisplayAlert("Delete", $"Delete '{transactionGoal.Id}' ?", "Yes", "No");
            if (result)
            {
                await App._transactionGoalRepository.DeleteTransactionGoalById(transactionGoal.Id);
                myCollectionView.ItemsSource = await App._transactionGoalRepository.GetTransactionGoalsDTO();
            }
        }
    }
}