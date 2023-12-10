using BudgetManager.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public CategoryPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            myCollectionView.ItemsSource = await App._customerRepository.GetCategories();
        }

        private async void GoToAddOrEditCategory(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddOrEditCategory());
        }

        private async void SwipeEdit(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var category = item.CommandParameter as Category;
            await Navigation.PushAsync(new AddOrEditCategory(category));
        }

        private async void SwipeDelete(object sender, EventArgs e)
        {
            var item = sender as SwipeItem;
            var category = item.CommandParameter as Category;
            var result = await DisplayAlert("Delete", $"Delete {category.Name}", "Yes", "No");
            if (result)
            {
                await App._customerRepository.DeleteCategory(category);
                myCollectionView.ItemsSource = await App._customerRepository.GetCategories();
            }
        }

        private async void SeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App._customerRepository.Seach(e.NewTextValue);
        }
    }
}