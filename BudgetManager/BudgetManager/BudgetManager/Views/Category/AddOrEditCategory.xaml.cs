using BudgetManager.Models;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BudgetManager.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddOrEditCategory : ContentPage
    {

        private Category _category;

        public AddOrEditCategory()
        {
            InitializeComponent();
            Title = "Add";
        }

        public AddOrEditCategory(Category category)
        {
            InitializeComponent();
            Title = "Edit";
            _category = category;
            nameEntry.Text = _category.Name;
            nameEntry.Focus();
        }

        private async void AddOrEdit(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(nameEntry.Text))
            {
                await DisplayAlert("Invalid", "Blank or WhiteSpace value", "Ok");
            }
            else if (_category != null)
            {
                EditCategory();
            }
            else
            {
                AddCategory();
            }
        }

        private async void AddCategory()
        {
            await App._categoryRepository.AddCategory(
                new Category
                {
                    Name = nameEntry.Text,
                });
            await Navigation.PopAsync();
        }

        private async void EditCategory()
        {
            _category.Name = nameEntry.Text;
            await App._categoryRepository.UpdateCategory(_category);
            await Navigation.PopAsync();
        }
    }
}