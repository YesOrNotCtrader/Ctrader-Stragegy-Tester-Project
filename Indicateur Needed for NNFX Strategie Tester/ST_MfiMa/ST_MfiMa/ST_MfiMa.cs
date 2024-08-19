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
    public class ST_MfiMa : Indicator
    {

        [Parameter(DefaultValue = 14)]
        public int PeriodsMfi { get; set; }
        [Parameter(DefaultValue = 14)]
        public int PeriodsSignal { get; set; }
        [Parameter(DefaultValue = 1)]
        public MovingAverageType MaTypeSignal { get; set; }

        [Output("ResultRsi", LineColor = "Lime")]
        public IndicatorDataSeries Result { get; set; }
        [Output("ResultSignal", LineColor = "Red")]
        public IndicatorDataSeries Signal { get; set; }

        private MoneyFlowIndex mfi;
        private MovingAverage signal;

        protected override void Initialize()
        {
            mfi = Indicators.MoneyFlowIndex(PeriodsMfi);
            signal = Indicators.MovingAverage(mfi.Result, PeriodsSignal, MaTypeSignal);
        }

        public override void Calculate(int index)
        {
            Result[index] = mfi.Result[index];
            Signal[index] = signal.Result[index];
        }
    }
}

