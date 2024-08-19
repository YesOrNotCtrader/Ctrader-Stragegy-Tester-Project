using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo
{
    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class ST_SuperTrend : Indicator
    {
        [Parameter(DefaultValue = 10)]
        public int Period { get; set; }
        [Parameter(DefaultValue = 3.0)]
        public double Multiplier { get; set; }

        [Output("Result", LineColor = "Lime", PlotType = PlotType.Points, Thickness = 3)]
        public IndicatorDataSeries Result { get; set; }


        private IndicatorDataSeries _upBuffer;
        private IndicatorDataSeries _downBuffer;
        private AverageTrueRange _averageTrueRange;
        private int[] _trend;

        protected override void Initialize()
        {
            _trend = new int[1];
            _upBuffer = CreateDataSeries();
            _downBuffer = CreateDataSeries();
            _averageTrueRange = Indicators.AverageTrueRange(Bars, Period, MovingAverageType.Simple);
        }

        public override void Calculate(int index)
        {
            // Init
            Result[index] = double.NaN;

            double median = (Bars.HighPrices[index] + Bars.LowPrices[index]) / 2;
            double atr = _averageTrueRange.Result[index];

            _upBuffer[index] = median + Multiplier * atr;
            _downBuffer[index] = median - Multiplier * atr;


            if (index < 1)
            {
                _trend[index] = 1;
                return;
            }

            Array.Resize(ref _trend, _trend.Length + 1);

            // Main Logic
            if (Bars.ClosePrices[index] > _upBuffer[index - 1])
            {
                _trend[index] = 1;
            }
            else if (Bars.ClosePrices[index] < _downBuffer[index - 1])
            {
                _trend[index] = -1;
            }
            else if (_trend[index - 1] == 1)
            {
                _trend[index] = 1;
            }
            else if (_trend[index - 1] == -1)
            {
                _trend[index] = -1;
            }

            if (_trend[index] < 0 && _trend[index - 1] > 0)
                _upBuffer[index] = median + (Multiplier * atr);
            else if (_trend[index] < 0 && _upBuffer[index] > _upBuffer[index - 1])
                _upBuffer[index] = _upBuffer[index - 1];

            if (_trend[index] > 0 && _trend[index - 1] < 0)
                _downBuffer[index] = median - (Multiplier * atr);
            else if (_trend[index] > 0 && _downBuffer[index] < _downBuffer[index - 1])
                _downBuffer[index] = _downBuffer[index - 1];


            Result[index] = _trend[index] == 1 ? _downBuffer[index] : _upBuffer[index];

        }
    }
}
