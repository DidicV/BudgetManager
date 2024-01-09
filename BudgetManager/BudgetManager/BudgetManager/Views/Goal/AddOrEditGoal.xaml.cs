using BudgetManager.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditGoal : ContentPage
    {
        private Goal _goal;

        public AddOrEditGoal()
        {
            InitializeComponent();
            Title = "Add new goal";
            currentAmountLabel.IsVisible = false;
            currentAmountEntry.IsVisible = false;
        }

        public AddOrEditGoal(Goal goal)
        {
            InitializeComponent();
            currentAmountLabel.IsVisible = true;
            currentAmountEntry.IsVisible = true;
            Title = "Edit";
            _goal = goal;
            nameEntry.Text = _goal.Name;
            descriptionEntry.Text = _goal.Description;
            targetEntry.Text = _goal.TargetAmount.ToString();
            currentAmountEntry.Text = _goal.CurrentAmount.ToString();
            nameEntry.Focus();
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntry.Text) ||
                string.IsNullOrEmpty(descriptionEntry.Text) ||
                string.IsNullOrEmpty(targetEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else if (_goal != null)
            {
                EditGoal();
            }
            else
            {
                AddGoal();
            }
        }

        private async void AddGoal()
        {
            await App._goalRepository.AddGoal(
                new Goal
                {
                    Name = nameEntry.Text,
                    Description = descriptionEntry.Text,
                    TargetAmount = Convert.ToDecimal(targetEntry.Text)
                });
            await Navigation.PopAsync();
        }

        private async void EditGoal()
        {
            _goal.Name = nameEntry.Text;
            _goal.Description = descriptionEntry.Text;
            _goal.TargetAmount = Convert.ToDecimal(targetEntry.Text);
            await App._goalRepository.UpdateGoal(_goal);
            await Navigation.PopAsync();
        }
    }
}