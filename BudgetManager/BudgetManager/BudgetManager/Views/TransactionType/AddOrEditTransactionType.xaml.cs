using BudgetManager.Helpers;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditTransactionType : ContentPage
    {
        private Models.TransactionType _transactionType;
        public AddOrEditTransactionType()
        {
            InitializeComponent();
            Title = "Add";
        }

        public AddOrEditTransactionType(Models.TransactionType transactionType)
        {
            InitializeComponent();
            Title = "Edit";
            _transactionType = transactionType;
            nameEntry.Text = _transactionType.Name;
            nameEntry.Focus();
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else if (nameEntry.Text.IsOneOf("Incomes", "Expenses"))
            {
                await DisplayAlert("Invalid", $"{nameEntry.Text} cannot be added or modified!", "Ok");
            }
            else if (_transactionType != null)
            {
                if (_transactionType.Name.IsOneOf("Incomes", "Expenses"))
                {
                    await DisplayAlert("Invalid", $"{_transactionType.Name} cannot be added or modified!", "Ok");
                }
                else
                {
                    EditTransactionType();
                }
            }
            else
            {
                AddTransactionType();
            }
        }

        private async void AddTransactionType()
        {
            await App._transactionTypeRepository.AddTransactionType(
                new Models.TransactionType
                {
                    Name = nameEntry.Text,
                });
            await Navigation.PopAsync();
        }

        private async void EditTransactionType()
        {
            _transactionType.Name = nameEntry.Text;
            await App._transactionTypeRepository.UpdateTransactionType(_transactionType);
            await Navigation.PopAsync();
        }
    }
}