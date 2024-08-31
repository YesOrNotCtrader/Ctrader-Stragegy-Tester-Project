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
    public class CSTP_Indi_4Parametter : Indicator
    {
        [Parameter(DefaultValue = EnumIndiSelection.Relative_Strength_Index)]
        public EnumIndiSelection IndicatorSelection { get; set; }
        public enum EnumIndiSelection
        {
            oO_Oscilator_Oo,
            ________________,
            Accumulative_Swing_Index,
            Center_Of_Gravity,
            Commodity_Channel_Index,
            Cyber_Cycle,
            Detrended_Price_Oscillator,
            Linear_Regression_RSquared,
            Linear_Regression_Slope,
            Macd_Cross_Over,
            Mass_Index,
            Momentum_Oscillator,
            Price_Oscillator,
            Price_ROC,
            Rainbow_Oscillator,
            Relative_Strength_Index,
            Stochastic_Oscillator,
            Swing_Index,
            Trix,
            Vertical_Horizontal_Filter,
            Vidya,
            Williams_Pct_R,
            Williams_Accumulation_Distribution,

            _________________,
            oO_Volatility_Oo,
            __________________,
            Average_True_Range,
            Chaikin_Volatility,
            Historical_Volatility,
            Standard_Deviation,
            ADX,
            ADXR,
            oO_Volume_Oo,
            ____________________,
            Chaikin_Money_Flow,
            Ease_Of_Movement,
            Money_Flow_Index,
            Negative_Volume_Index,
            Positive_Volume_Index,
            On_Balance_Volume,
            Price_Volume_Trend,
            Tick_Volume,
            Trade_Volume_Index,
            Volume_Oscillator,
            Volume_ROC

        }
        [Parameter(DefaultValue = 14)]
        public double Value1 { get; set; }
        [Parameter(DefaultValue = 3)]
        public double Value2 { get; set; }
        [Parameter(DefaultValue = 9)]
        public double Value3 { get; set; }
        [Parameter(DefaultValue = MovingAverageType.Exponential)]
        public MovingAverageType MaType { get; set; }

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

                // Oscillator

                case (EnumIndiSelection.Macd_Cross_Over):
                    Result[index] = Indicators.MovingAverage(Bars.ClosePrices, (int)Value2, MaType).Result[index] - Indicators.MovingAverage(Bars.ClosePrices, (int)Value1, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Commodity_Channel_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.CommodityChannelIndex(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Cyber_Cycle):
                    Result[index] = Indicators.MovingAverage(Indicators.CyberCycle(Value1).Cycle, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Detrended_Price_Oscillator):
                    Result[index] = Indicators.MovingAverage(Indicators.DetrendedPriceOscillator(Bars.ClosePrices, (int)Value1, MaType).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index]; break;
                case (EnumIndiSelection.Mass_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.MassIndex(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Momentum_Oscillator):
                    Result[index] = Indicators.MovingAverage(Indicators.MomentumOscillator(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Price_Oscillator):
                    Result[index] = Indicators.PriceOscillator(Bars.ClosePrices, (int)Value1, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Price_ROC):
                    Result[index] = Indicators.MovingAverage(Indicators.PriceROC(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Rainbow_Oscillator):
                    Result[index] = Indicators.MovingAverage(Indicators.RainbowOscillator(Bars.ClosePrices, (int)Value1, MaType).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Vertical_Horizontal_Filter):
                    Result[index] = Indicators.MovingAverage(Indicators.VerticalHorizontalFilter(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Williams_Pct_R):
                    Result[index] = Indicators.MovingAverage(Indicators.WilliamsPctR(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Accumulative_Swing_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.AccumulativeSwingIndex(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Swing_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.SwingIndex(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Relative_Strength_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.RelativeStrengthIndex(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Trix):
                    Result[index] = Indicators.MovingAverage(Indicators.Trix(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Stochastic_Oscillator):
                    Result[index] = Indicators.StochasticOscillator(Bars, (int)Value1, (int)Value2, (int)Value3, MaType).PercentK[index];
                    Signal[index] = Indicators.StochasticOscillator(Bars, (int)Value1, (int)Value2, (int)Value3, MaType).PercentD[index];
                    break;
                case (EnumIndiSelection.Center_Of_Gravity):
                    Result[index] = Indicators.MovingAverage(Indicators.CenterOfGravity((int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Williams_Accumulation_Distribution):
                    Result[index] = Indicators.MovingAverage(Indicators.WilliamsAccumulationDistribution(Bars).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Linear_Regression_RSquared):
                    Result[index] = Indicators.MovingAverage(Indicators.LinearRegressionRSquared(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Linear_Regression_Slope):
                    Result[index] = Indicators.MovingAverage(Indicators.LinearRegressionSlope(Bars.ClosePrices, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Vidya):
                    Result[index] = Indicators.Vidya(Bars.ClosePrices, (int)Value1, Value2).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;

                // Volatility

                case (EnumIndiSelection.Average_True_Range):
                    Result[index] = Indicators.MovingAverage(Indicators.AverageTrueRange((int)Value1, MaType).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;

                case (EnumIndiSelection.Chaikin_Volatility):
                    Result[index] = Indicators.ChaikinVolatility((int)Value1, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Historical_Volatility):
                    Result[index] = Indicators.HistoricalVolatility(Bars.ClosePrices, (int)Value1, (int)Value2).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Standard_Deviation):
                    Result[index] = Indicators.MovingAverage(Indicators.StandardDeviation(Bars.ClosePrices, (int)Value1, MaType).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.ADX):
                    Result[index] = Indicators.MovingAverage(Indicators.AverageDirectionalMovementIndexRating((int)Value1).ADX, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.ADXR):
                    Result[index] = Indicators.MovingAverage(Indicators.AverageDirectionalMovementIndexRating((int)Value1).ADXR, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;

                // Volume
                case (EnumIndiSelection.Chaikin_Money_Flow):
                    Result[index] = Indicators.MovingAverage(Indicators.ChaikinMoneyFlow(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Ease_Of_Movement):
                    Result[index] = Indicators.MovingAverage(Indicators.EaseOfMovement((int)Value1, MaType).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Money_Flow_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.MoneyFlowIndex(Bars, (int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Negative_Volume_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.NegativeVolumeIndex(Bars.ClosePrices).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Positive_Volume_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.PositiveVolumeIndex(Bars.ClosePrices).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.On_Balance_Volume):
                    Result[index] = Indicators.MovingAverage(Indicators.OnBalanceVolume(Bars.ClosePrices).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Price_Volume_Trend):
                    Result[index] = Indicators.MovingAverage(Indicators.PriceVolumeTrend(Bars.ClosePrices).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Tick_Volume):
                    Result[index] = Indicators.MovingAverage(Indicators.TickVolume().Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Trade_Volume_Index):
                    Result[index] = Indicators.MovingAverage(Indicators.TradeVolumeIndex(Bars.ClosePrices).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Volume_Oscillator):
                    Result[index] = Indicators.VolumeOscillator((int)Value1, (int)Value2).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;
                case (EnumIndiSelection.Volume_ROC):
                    Result[index] = Indicators.MovingAverage(Indicators.VolumeROC((int)Value1).Result, (int)Value2, MaType).Result[index];
                    Signal[index] = Indicators.MovingAverage(Result, (int)Value3, MaType).Result[index];
                    break;

            }

            //Result[index] = result[index];
        }
    }
}
