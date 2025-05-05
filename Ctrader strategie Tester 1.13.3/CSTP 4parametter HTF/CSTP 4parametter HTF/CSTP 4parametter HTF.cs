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
    [Cloud("Result", "Signal", FirstColor = "Lime", SecondColor = "Red", Opacity = 0.25)]
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class CSTP_Indi_2ParametterHTF : Indicator
    {
        [Parameter("TimeFrame", DefaultValue = "Daily", Group = "Calculation Type")]
        public TimeFrame TF1 { get; set; }

        [Parameter(DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Relative_Strength_Index)]
        public CSTP_Indi_4Parametter.EnumIndiSelection IndicatorSelection { get; set; }

        [Parameter(DefaultValue = 14)]
        public double Value1 { get; set; }
        [Parameter(DefaultValue = 3)]
        public double Value2 { get; set; }
        [Parameter(DefaultValue = 9)]
        public double Value3 { get; set; }
        [Parameter(DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MaType { get; set; }


        [Output("Result", LineColor = "Green", LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Result { get; set; }
        [Output("Signal", LineColor = "Red", LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Signal { get; set; }

        private CSTP_Indi_4Parametter cstp;
        private Bars bars1;

        protected override void Initialize()
        {
            bars1 = MarketData.GetBars(TF1);

            if (!IsBacktesting)
            {
                while (bars1.OpenTimes[0] > Bars.OpenTimes[0])
                    bars1.LoadMoreHistory();
            }
            cstp = Indicators.GetIndicator<CSTP_Indi_4Parametter>(bars1, IndicatorSelection, Value1, Value2, Value3, MaType);
        }

        public override void Calculate(int index)
        {
            int idx1 = TF1 == Chart.TimeFrame ? index : bars1.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]);

            Result[index] = cstp.Result[idx1];

            Signal[index] = cstp.Signal[idx1];

        }
    }
}
