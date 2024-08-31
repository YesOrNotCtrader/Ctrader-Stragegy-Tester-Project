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
    public class CSTP_Indi_1Parametter : Indicator
    {
        [Parameter(DefaultValue = EnumIndiSelection.Exponetial_Moving_Average)]
        public EnumIndiSelection IndicatorSelection { get; set; }
        public enum EnumIndiSelection
        {
            Exponetial_Moving_Average,
            Hull_Moving_Average,
            Simple_Moving_Average,
            TimesSeries_Moving_Average,
            Triangular_Moving_Average,
            VIDYA_Moving_Average,
            Weigthed_Moving_Average,
            WilderSmoothing_Moving_Average,
            Donchian_Middle,
            Linear_Regression_Intercept,
            Linear_Regression_Forecast,
            Aroon,
            Adxr,
        }

        [Parameter(DefaultValue = 13)]
        public int Period { get; set; }

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
                case (EnumIndiSelection.Exponetial_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Exponential).Result[index];
                    break;
                case (EnumIndiSelection.Hull_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Hull).Result[index];
                    break;
                case (EnumIndiSelection.Simple_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Simple).Result[index];
                    break;
                case (EnumIndiSelection.TimesSeries_Moving_Average):
                    Result[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.TimeSeries).Result[index];
                    Signal[index] = Bars.ClosePrices[index];
                    break;
                case (EnumIndiSelection.Triangular_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Triangular).Result[index];
                    break;
                case (EnumIndiSelection.VIDYA_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.VIDYA).Result[index];
                    break;
                case (EnumIndiSelection.Weigthed_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.Weighted).Result[index];
                    break;
                case (EnumIndiSelection.WilderSmoothing_Moving_Average):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.MovingAverage(Bars.ClosePrices, Period, MovingAverageType.WilderSmoothing).Result[index];
                    break;
                case (EnumIndiSelection.Donchian_Middle):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.DonchianChannel(Period).Middle[index];
                    break;
                case (EnumIndiSelection.Linear_Regression_Intercept):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.LinearRegressionIntercept(Bars.ClosePrices, Period).Result[index];
                    break;
                case (EnumIndiSelection.Linear_Regression_Forecast):
                    Result[index] = Bars.ClosePrices[index];
                    Signal[index] = Indicators.LinearRegressionForecast(Bars.ClosePrices, Period).Result[index];
                    break;
                case (EnumIndiSelection.Aroon):
                    Result[index] = Indicators.Aroon(Period).Up[index];
                    Signal[index] = Indicators.Aroon(Period).Down[index];
                    break;
                case (EnumIndiSelection.Adxr):
                    Result[index] = Indicators.AverageDirectionalMovementIndexRating(Period).DIPlus[index];
                    Signal[index] = Indicators.AverageDirectionalMovementIndexRating(Period).DIMinus[index];

                    break;
            }

        }
    }
}
