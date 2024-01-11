using BudgetManager.Repository;
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

        public static TransactionTypeRepository _transactionTypeRepository = new TransactionTypeRepository(database);
        public static TransactionGoalRepository _transactionGoalRepository = new TransactionGoalRepository(database);
        public static PaymentMethodRepository _paymentMethodRepository = new PaymentMethodRepository(database);
        public static TransactionRepository _transactionRepository = new TransactionRepository(database);
        public static CategoryRepository _categoryRepository = new CategoryRepository(database);
        public static GoalRepository _goalRepository = new GoalRepository(database);

        public App()
        {
            InitializeComponent();

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
