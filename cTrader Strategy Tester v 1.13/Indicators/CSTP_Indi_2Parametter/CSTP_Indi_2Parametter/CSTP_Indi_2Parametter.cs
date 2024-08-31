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
    [Indicator(IsOverlay = false, AccessRights = AccessRights.None)]
    public class CSTP_Indi_2Parametter : Indicator
    {
        [Parameter(DefaultValue = EnumIndiSelection.SuperTrend)]
        public EnumIndiSelection IndicatorSelection { get; set; }
        public enum EnumIndiSelection
        {
            SuperTrend,
            Parabolic_Sar,
            Bars_Vs_Vidya,

        }

        [Parameter(DefaultValue = 13)]
        public double Value1 { get; set; }
        [Parameter(DefaultValue = 3)]
        public double Value2 { get; set; }

        [Output("Result", IsHistogram = false, LineColor = "Green", LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Result { get; set; }
        [Output("Signal", IsHistogram = false, LineColor = "Red", LineStyle = LineStyle.Solid, PlotType = PlotType.Line, Thickness = 1)]
        public IndicatorDataSeries Signal { get; set; }

        protected override void Initialize()
        {

        }
        public override void Calculate(int index)
        {
            switch (IndicatorSelection)
            {
                case (EnumIndiSelection.SuperTrend):
                    Result[index] = Indicators.ParabolicSAR((int)Value1, (int)Value2).Result[index];
                    Signal[index] = !double.IsNaN(Indicators.Supertrend((int)Value1, (int)Value2).UpTrend[index]) ? Indicators.Supertrend((int)Value1, (int)Value2).UpTrend[index] : Indicators.Supertrend((int)Value1, (int)Value2).DownTrend[index];
                    break;
                case (EnumIndiSelection.Parabolic_Sar):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.ParabolicSAR((int)Value1, (int)Value2).Result[index];
                    break;
                case (EnumIndiSelection.Bars_Vs_Vidya):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.Vidya(Bars.ClosePrices, (int)Value1, Value2).Result[index];
                    break;
            }
        }
    }
}
