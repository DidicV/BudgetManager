using BudgetManager.Repository;
using BudgetManager.Services;
using SQLite;
using System;
using System.IO;
using Xamarin.Forms;

namespace BudgetManager
{
    public partial class App : Application
    {
        private static readonly string dbPath = Path.Combine(Environment.GetFolderPath(
                                                             Environment.SpecialFolder.LocalApplicationData),
                                                             "budgetmanager.db");

        private static readonly SQLiteAsyncConnection database = new AppDatabase(dbPath).GetConnection();

        public static CategoryRepository _customerRepository = new CategoryRepository(database);

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
