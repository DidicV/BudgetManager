using BudgetManager.ViewModels;
using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace BudgetManager.Views
{
    public partial class AboutPage : ContentPage
    {
        public ObservableCollection<int> Years { get; set; }

        public AboutPage()
        {
            InitializeComponent();
            Years = new ObservableCollection<int>();
            for (int year = DateTime.Now.Year - 10; year <= DateTime.Now.Year + 10; year++)
            {
                Years.Add(year);
            }
        }

        protected override void OnAppearing()
        {
            yearPicker.ItemsSource = Years;
            yearPicker.SelectedIndex = 10;
            BindingContext = new AboutViewModel(DateTime.Now.Year);
        }

        private void OnSelectYearClicked(object sender, EventArgs e)
        {
            int selectedYear = (int)yearPicker.SelectedItem;
            BindingContext = new AboutViewModel(selectedYear);
        }
    }
}