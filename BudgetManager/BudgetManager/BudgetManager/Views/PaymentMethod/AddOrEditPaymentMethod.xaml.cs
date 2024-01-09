using BudgetManager.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditPaymentMethod : ContentPage
    {

        private PaymentMethod _paymentMethod;

        public AddOrEditPaymentMethod()
        {
            InitializeComponent();
            Title = "Add new transaction method";
        }

        public AddOrEditPaymentMethod(PaymentMethod paymentMethod)
        {
            InitializeComponent();
            Title = "Edit";
            _paymentMethod = paymentMethod;
            nameEntry.Text = _paymentMethod.Name;
            nameEntry.Focus();
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else if (_paymentMethod != null)
            {
                EditPaymentMethod();
            }
            else
            {
                AddPaymentMethod();
            }
        }

        private async void AddPaymentMethod()
        {
            await App._paymentMethodRepository.AddPaymentMethod(
                new PaymentMethod
                {
                    Name = nameEntry.Text,
                });
            await Navigation.PopAsync();
        }

        private async void EditPaymentMethod()
        {
            _paymentMethod.Name = nameEntry.Text;
            await App._paymentMethodRepository.UpdatePaymentMethod(_paymentMethod);
            await Navigation.PopAsync();
        }
    }
}