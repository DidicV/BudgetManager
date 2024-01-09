using SkiaSharp;

namespace Microcharts
{
    internal class Entry : ChartEntry
    {
        public Entry(float value) : base(value)
        {
        }

        public string Label { get; set; }
        public string ValueLabel { get; set; }
        public SKColor Color { get; set; }
    }
}