using BudgetManager.Models;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views.Transaction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FilterPopupPage : ContentPage
    {
        private readonly Action<FilterModel> filterCallback;

        private decimal? savedMinAmount;
        private decimal? savedMaxAmount;
        private string savedTransactionType;
        private string savedCategory;
        private string savedpaymentMethod;
        private DateTime savedStartDate;
        private DateTime savedEndDate;

        public FilterPopupPage(Action<FilterModel> filterCallback, FilterModel initialFilter)
        {
            InitializeComponent();
            this.filterCallback = filterCallback;

            // Save the initial filter values
            savedMinAmount = initialFilter.MinAmount;
            savedMaxAmount = initialFilter.MaxAmount;
            savedTransactionType = initialFilter.TransactionType;
            savedCategory = initialFilter.Category;
            savedpaymentMethod = initialFilter.PaymentMethod;
            savedStartDate = initialFilter.StartDate;
            savedEndDate = initialFilter.EndDate;

            // Initialize input fields with saved values
            minAmountEntry.Text = savedMinAmount?.ToString();
            maxAmountEntry.Text = savedMaxAmount?.ToString();
            SetPickers();
        }

        private async void SetPickers()
        {
            var paymentMethods = await App._paymentMethodRepository.GetPaymentMethods();
            var categories = await App._categoryRepository.GetCategories();
            var transactiontypes = await App._transactionTypeRepository.GetTransactionTypes();

            paymentMethodPicker.ItemsSource = paymentMethods;
            categoryPicker.ItemsSource = categories;
            transactionTypePicker.ItemsSource = transactiontypes;

            transactionTypePicker.SelectedItem = transactiontypes.FirstOrDefault(t => t.Name == savedTransactionType);
            paymentMethodPicker.SelectedItem = paymentMethods.FirstOrDefault(t => t.Name == savedpaymentMethod);
            categoryPicker.SelectedItem = categories.FirstOrDefault(t => t.Name == savedCategory);
            startDatePicker.Date = savedStartDate;
            endDatePicker.Date = savedEndDate;
        }

        private async void ClosePopup(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void ApplyFiltersAndClosePopup(object sender, EventArgs e)
        {
            var transactionType = (Models.TransactionType)transactionTypePicker.SelectedItem;
            var category = (Category)categoryPicker.SelectedItem;
            var paymentMethod = (PaymentMethod)paymentMethodPicker.SelectedItem;

            var newFilter = new FilterModel
            {
                MinAmount = string.IsNullOrEmpty(minAmountEntry.Text) ? null : (decimal?)Convert.ToDecimal(minAmountEntry.Text),
                MaxAmount = string.IsNullOrEmpty(maxAmountEntry.Text) ? null : (decimal?)Convert.ToDecimal(maxAmountEntry.Text),
            };

            if (transactionType != null)
            {
                newFilter.TransactionType = transactionType.Name;
            }

            if (category != null)
            {
                newFilter.Category = category.Name;
            }

            if (paymentMethod != null)
            {
                newFilter.PaymentMethod = paymentMethod.Name;
            }

            newFilter.StartDate = startDatePicker.Date;
            newFilter.EndDate = endDatePicker.Date;

            // Save the entered values for the next time the pop-up is opened
            savedMinAmount = newFilter.MinAmount;
            savedMaxAmount = newFilter.MaxAmount;
            savedTransactionType = newFilter.TransactionType;
            savedCategory = newFilter.Category;
            savedpaymentMethod = newFilter.PaymentMethod;

            // Invoke the callback with the new filter values
            filterCallback?.Invoke(newFilter);

            // Close the pop-up
            await Navigation.PopModalAsync();
        }

        private void ClearAllEntries(object sender, EventArgs e)
        {
            minAmountEntry.Text = string.Empty;
            maxAmountEntry.Text = string.Empty;
            transactionTypePicker.SelectedItem = null;
            categoryPicker.SelectedItem = null;
            paymentMethodPicker.SelectedItem = null;
            startDatePicker.Date = DateTime.Now.AddYears(-5);
            endDatePicker.Date = DateTime.Now;
        }
    }
}