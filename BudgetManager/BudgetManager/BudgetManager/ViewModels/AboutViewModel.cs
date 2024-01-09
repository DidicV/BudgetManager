using BudgetManager.Helpers;
using BudgetManager.Models;
using Microcharts;
using SkiaSharp;
using System.Collections.Generic;
using System.Linq;

namespace BudgetManager.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public int Year { get; set; }

        public AboutViewModel(int year)
        {
            Title = "About";
            Year = year;
            InitData();
        }

        private DonutChart donutChart;
        public DonutChart DonutChart
        {
            get => donutChart;
            set => SetProperty(ref donutChart, value);
        }

        private LineChart lineChart;
        public LineChart LineChart
        {
            get => lineChart;
            set => SetProperty(ref lineChart, value);
        }

        private LineChart lineChart2;
        public LineChart LineChart2
        {
            get => lineChart2;
            set => SetProperty(ref lineChart2, value);
        }

        private MultiLinesChart multiLinesChart;
        public MultiLinesChart MultiLinesChart
        {
            get => multiLinesChart;
            set => SetProperty(ref multiLinesChart, value);
        }

        private MultiBarChart multiBarChart;
        public MultiBarChart MultiBarChart
        {
            get => multiBarChart;
            set => SetProperty(ref multiBarChart, value);
        }


        private readonly string[] months = new string[] { "JAN", "FRB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };


        private readonly SKColor blueColor = SKColor.Parse("#09C");
        private readonly SKColor redColor = SKColor.Parse("#CC0000");


        private async void InitData()
        {
            var entries = new List<List<ChartEntry>>();
            var turnoverEntries = new List<ChartEntry>();
            var donutChartEntries = new List<ChartEntry>();

            var resultIncomes = await App._transactionRepository.GetTransactionsSummary(Year, "Expenses");
            var resultExpense = await App._transactionRepository.GetTransactionsSummary(Year, "Incomes");

            var incomes = resultIncomes.Select(t => (float)t.Sum);
            var expenses = resultExpense.Select(t => (float)t.Sum);


            var chargesEntries = new List<ChartEntry>();
            int i = 0;
            foreach (var data in expenses)
            {
                turnoverEntries.Add(new ChartEntry(data)
                {
                    Color = blueColor,
                    ValueLabel = $"{data}",
                    Label = months[i]
                });
                i++;
            }

            i = 0;
            foreach (var data in incomes)
            {
                chargesEntries.Add(new ChartEntry(data)
                {
                    Color = redColor,
                    ValueLabel = $"{data}",
                    Label = months[i]
                });
                i++;
            }





            donutChartEntries.Add(new ChartEntry(expenses.Sum())
            {
                Color = blueColor,
                ValueLabel = $"{expenses.Sum()}",
                Label = "Incomes",
                ValueLabelColor = blueColor
            });

            donutChartEntries.Add(new ChartEntry(incomes.Sum())
            {
                Color = redColor,
                ValueLabel = $"{incomes.Sum()}",
                Label = "Expenses",
                ValueLabelColor = redColor
            });

            List<string> predefinedColors = new List<string>
            {
                "#c6eac6",
                "#ffcc66",
                "#6699ff",
                "#ff6666",
                "#99cc99",
                "#cc99ff" 
            };

            var transactionTypesList = await App._transactionTypeRepository.GetTransactionTypes();

            var tramsactionTypes = transactionTypesList.Select(t=>t.Name).ToList();

            tramsactionTypes.Remove("Incomes");
            tramsactionTypes.Remove("Expenses");


            for(int j = 0; j < tramsactionTypes.Count; j++)
            {
                var result = await App._transactionRepository.GetTransactionsSummary(Year, tramsactionTypes[j]);
                var monthsSum = result.Select(t => (float)t.Sum);

                int colorIndex = j % predefinedColors.Count;


                donutChartEntries.Add(new ChartEntry(monthsSum.Sum())
                {
                    Color = SKColor.Parse(predefinedColors[colorIndex]),
                    ValueLabel = $"{monthsSum.Sum()}",
                    Label = tramsactionTypes[j],
                    ValueLabelColor = SKColor.Parse(predefinedColors[colorIndex])
                });
            }




            entries.Add(turnoverEntries);
            entries.Add(chargesEntries);

            DonutChart = new DonutChart { Entries = donutChartEntries, LabelTextSize = 30f };

            LineChart = new LineChart { Entries = turnoverEntries, LabelTextSize = 30f, LabelOrientation = Orientation.Horizontal };
            LineChart2 = new LineChart { Entries = chargesEntries, LabelTextSize = 30f, LabelOrientation = Orientation.Horizontal };

            MultiLinesChart = new MultiLinesChart
            {
                MultiLineEntires = entries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Horizontal,
                LineAreaAlpha = 50,
                PointAreaAlpha = 50,
                LegendNames = new List<string> { "Incomes", "Expenses" },
                IsAnimated = false

            };

            MultiBarChart = new MultiBarChart
            {
                MultiBarEntries = entries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Horizontal,
                PointAreaAlpha = 0,
                LegendNames = new List<string> { "Incomes", "Expenses" },
                IsAnimated = false
            };
        }
    }
}