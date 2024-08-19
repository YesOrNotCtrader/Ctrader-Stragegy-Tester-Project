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
    [Indicator(AccessRights = AccessRights.None)]
    public class ST_RsiMa : Indicator
    {

        [Parameter(DefaultValue = 14)]
        public int PeriodsRSI { get; set; }
        [Parameter(DefaultValue = 14)]
        public int PeriodsSignal { get; set; }
        [Parameter(DefaultValue = 1)]
        public MovingAverageType MaTypeSignal { get; set; }

        [Output("ResultRsi", LineColor = "Lime")]
        public IndicatorDataSeries Result { get; set; }
        [Output("ResultSignal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private RelativeStrengthIndex rsi;
        private MovingAverage signal;

        protected override void Initialize()
        {
            rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, PeriodsRSI);
            signal = Indicators.MovingAverage(rsi.Result, PeriodsSignal, MaTypeSignal);
        }

        public override void Calculate(int index)
        {
            Result[index] = rsi.Result[index];
            Signal[index] = signal.Result[index];
        }
    }
}

