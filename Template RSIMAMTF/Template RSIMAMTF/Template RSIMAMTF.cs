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
    public class TemplateRSIMAMTF : Indicator
    {
        [Parameter(DefaultValue = "Hour")]
        public TimeFrame TimeframeSelection { get; set; }
        [Parameter(DefaultValue = 14)]
        public int PeriodsRSI { get; set; }

        [Parameter(DefaultValue = 14)]
        public int PeriodsSignal { get; set; }
        [Parameter(DefaultValue = 1)]
        public MovingAverageType MaTypeSignal { get; set; }

        [Output("ResultRsi", LineColor = "Lime")]
        public IndicatorDataSeries ResultRsi { get; set; }
        [Output("ResultSignal", LineColor = "Red")]
        public IndicatorDataSeries ResultSignal { get; set; }


        private Bars barsTF;
        private RelativeStrengthIndex rsi;
        private MovingAverage signal;

        protected override void Initialize()
        {
            barsTF = MarketData.GetBars(TimeframeSelection);
            if (!IsBacktesting)
            {
                while (barsTF.OpenTimes[0] > Bars.OpenTimes[0])
                    barsTF.LoadMoreHistory();
            }
            rsi = Indicators.RelativeStrengthIndex(barsTF.ClosePrices, PeriodsRSI);
            signal = Indicators.MovingAverage(rsi.Result, PeriodsSignal, MaTypeSignal);
        }

        public override void Calculate(int index)
        {
            int idx1 = TimeframeSelection == Chart.TimeFrame ? index : barsTF.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

            ResultRsi[index] = rsi.Result[idx1];
            ResultSignal[index] = signal.Result[idx1];
        }
    }
}

