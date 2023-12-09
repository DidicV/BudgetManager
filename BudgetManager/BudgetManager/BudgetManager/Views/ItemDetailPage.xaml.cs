using BudgetManager.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace BudgetManager.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}