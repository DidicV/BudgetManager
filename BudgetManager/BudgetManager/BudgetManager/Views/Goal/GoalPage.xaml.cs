using BudgetManager.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GoalPage : ContentPage
    {
        public GoalPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var goals = await App._goalRepository.GetGoals();
            var transactionGoals = await App._transactionGoalRepository.GetTransactionGoals();
            var transactions = await App._transactionRepository.GetTransactions();

            foreach (var goal in goals)
            {
                var trans_goals = transactionGoals.Where(t => t.GoalId == goal.Id).ToList();
                decimal sum = 0;

                foreach (var tg in trans_goals)
                {
                    sum += transactions.FirstOrDefault(t => t.Id == tg.TransactionId).Amount;
                }

                goal.CurrentAmount = sum;
                goal.Progress = (double)(goal.CurrentAmount / goal.TargetAmount);
            }
            myCollectionView.ItemsSource = goals;
        }

        private async void GoToAddOrEditGoal(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditGoal());
        }

        private async void SwipeEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var goal = item.CommandParameter as Goal;
            await Navigation.PushAsync(new AddOrEditGoal(goal));
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var goal = item.CommandParameter as Goal;
            var result = await DisplayAlert("Delete", $"Delete '{goal.Name}' ?", "Yes", "No");
            if (result)
            {
                await App._goalRepository.DeleteGoal(goal);
                await App._transactionGoalRepository.DeleteGoalsById(goal.Id);
                myCollectionView.ItemsSource = await App._goalRepository.GetGoals();
            }
        }
    }
}