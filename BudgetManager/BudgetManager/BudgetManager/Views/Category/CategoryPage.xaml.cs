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
            myCollectionView.ItemsSource = await App._categoryRepository.GetCategories();
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

            var isPresentCategory = await App._transactionRepository.CheckCategoryId(category.Id);

            if (isPresentCategory)
            {
                await DisplayAlert("Delete", $"Category {category.Name} is present in transaction table and cannot be deleted", "Ok");
            }
            else
            {
                var result = await DisplayAlert("Delete", $"Delete '{category.Name}' ?", "Yes", "No");

                if (result)
                {
                    await App._categoryRepository.DeleteCategory(category);
                    myCollectionView.ItemsSource = await App._categoryRepository.GetCategories();
                }
            }
        }

        private async void SeachBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            myCollectionView.ItemsSource = await App._categoryRepository.Seach(e.NewTextValue);
        }
    }
}