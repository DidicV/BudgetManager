using BudgetManager.DTO;
using BudgetManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditTransaction : ContentPage
    {
        public List<PaymentMethod> _paymentMethods;
        public List<Category> _categories;
        public List<Models.TransactionType> _transactionTypes;

        private int selectedPaymentMethodId;
        private int selectedCategoryId;
        private int selectedTransactionTypeId;

        private Models.Transaction _transaction;

        public AddOrEditTransaction()
        {
            InitializeComponent();
            Title = "Add new transaction";
        }

        public AddOrEditTransaction(TransactionDTO transaction)
        {
            InitializeComponent();
            Title = "Edit transaction";
            numericEntry.Text = transaction.Amount.ToString();
            date.Date = transaction.Date;
            descriptionEntry.Text = transaction.Description;
            numericEntry.Focus();

            _transaction = new Models.Transaction
            {
                Id = transaction.Id
            };

            InitializeData(transaction);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            _paymentMethods = await App._paymentMethodRepository.GetPaymentMethods();
            _categories = await App._categoryRepository.GetCategories();
            _transactionTypes = await App._transactionTypeRepository.GetTransactionTypes();

            if (_transaction == null)
            {
                paymentMethodPicker.ItemsSource = _paymentMethods;
                categoryPicker.ItemsSource = _categories;
                transactionTypePicker.ItemsSource = _transactionTypes;
            }
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            selectedPaymentMethodId = ((PaymentMethod)paymentMethodPicker.SelectedItem)?.Id ?? 0;
            selectedCategoryId = ((Category)categoryPicker.SelectedItem)?.Id ?? 0;
            selectedTransactionTypeId = ((Models.TransactionType)transactionTypePicker.SelectedItem)?.Id ?? 0;

            if (string.IsNullOrEmpty(numericEntry.Text) ||
                string.IsNullOrEmpty(descriptionEntry.Text) ||
                selectedPaymentMethodId == 0 ||
                selectedCategoryId == 0 ||
                selectedTransactionTypeId == 0)
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else if (_transaction != null)
            {
                EditTransaction();
            }
            else
            {
                AddTransaction();
            }
        }

        private async void AddTransaction()
        {
            await App._transactionRepository.AddTransaction(
                new Models.Transaction
                {
                    Amount = Convert.ToDecimal(numericEntry.Text),
                    Date = date.Date,
                    Description = descriptionEntry.Text,
                    CategoryId = selectedCategoryId,
                    TransactionTypeId = selectedTransactionTypeId,
                    PaymentMethodId = selectedPaymentMethodId
                });
            await Navigation.PopAsync();
        }

        private async void EditTransaction()
        {
            _transaction.Amount = Convert.ToDecimal(numericEntry.Text);
            _transaction.Date = date.Date;
            _transaction.Description = descriptionEntry.Text;
            _transaction.CategoryId = selectedCategoryId;
            _transaction.TransactionTypeId = selectedTransactionTypeId;
            _transaction.PaymentMethodId = selectedPaymentMethodId;
            await App._transactionRepository.UpdateTransaction(_transaction);
            await Navigation.PopAsync();
        }

        private async void InitializeData(TransactionDTO transaction)
        {
            _paymentMethods = await App._paymentMethodRepository.GetPaymentMethods();
            _categories = await App._categoryRepository.GetCategories();
            _transactionTypes = await App._transactionTypeRepository.GetTransactionTypes();

            paymentMethodPicker.ItemsSource = _paymentMethods;
            categoryPicker.ItemsSource = _categories;
            transactionTypePicker.ItemsSource = _transactionTypes;

            PaymentMethod selectedPaymentMethod = _paymentMethods.FirstOrDefault(p => p.Name == transaction.PaymentMethodName);
            Category selectedCategory = _categories.FirstOrDefault(c => c.Name == transaction.CategoryName);
            Models.TransactionType selectedTransactionType = _transactionTypes.FirstOrDefault(t => t.Name == transaction.TransactionTypeName);

            paymentMethodPicker.SelectedItem = selectedPaymentMethod;
            categoryPicker.SelectedItem = selectedCategory;
            transactionTypePicker.SelectedItem = selectedTransactionType;
        }
    }
}