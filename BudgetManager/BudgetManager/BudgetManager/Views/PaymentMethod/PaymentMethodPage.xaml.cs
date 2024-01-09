using BudgetManager.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaymentMethodPage : ContentPage
    {
        public PaymentMethodPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            myCollectionView.ItemsSource = await App._paymentMethodRepository.GetPaymentMethods();
        }

        private async void GoToAddOrEditPaymentMethod(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditPaymentMethod());
        }

        private async void SwipeEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var paymentMethod = item.CommandParameter as PaymentMethod;
            await Navigation.PushAsync(new AddOrEditPaymentMethod(paymentMethod));
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var paymentMethod = item.CommandParameter as PaymentMethod;

            var isPaymentMethod = await App._transactionRepository.CheckPaymentMethodId(paymentMethod.Id);

            if (isPaymentMethod)
            {
                await DisplayAlert("Delete", $"Payment method {paymentMethod.Name} is present in transaction table and cannot be deleted", "Ok");
            }
            else
            {
                var result = await DisplayAlert("Delete", $"Delete '{paymentMethod.Name}' ?", "Yes", "No");
                if (result)
                {
                    await App._paymentMethodRepository.DeletePaymentMethod(paymentMethod);
                    myCollectionView.ItemsSource = await App._paymentMethodRepository.GetPaymentMethods();
                }
            }
        }

        private async void SeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App._paymentMethodRepository.Seach(e.NewTextValue);
        }
    }
}