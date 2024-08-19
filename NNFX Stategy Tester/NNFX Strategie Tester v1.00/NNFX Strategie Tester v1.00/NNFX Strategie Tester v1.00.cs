using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.None)]
    public class NNFXStrategieTesterv100 : Robot
    {
        //-------------------------------- DAYS TO TRADE --------------------------------------------------------------//
        [Parameter("Monday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Mon { get; set; }
        [Parameter("Tuesday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Tue { get; set; }
        [Parameter("Wednesday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Wed { get; set; }
        [Parameter("Thuesday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Thu { get; set; }
        [Parameter("Friday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Fri { get; set; }
        [Parameter("Saturday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Sat { get; set; }
        [Parameter("Sunday", DefaultValue = true, Group = "===========================================================\nPERIOD SETTING\n===========================================================\nDay To Trade")]
        public bool Sun { get; set; }
        //-------------------------------- Time TO TRADE --------------------------------------------------------------//
        [Parameter("Trade Start Hour", DefaultValue = 0, Group = "Time To Trade")]
        public int StartHour { get; set; }
        [Parameter("Trade End Hour", DefaultValue = 24, Group = "Time To Trade")]
        public int EndHour { get; set; }

        [Parameter("Type", DefaultValue = EnumSelectionRiskType.Perentage_Risk, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public EnumSelectionRiskType SelectionRiskType { get; set; }
        public enum EnumSelectionRiskType
        {
            Fixed_Lot,
            Fixed_Volume,
            Perentage_Risk,
        }
        [Parameter("Value", DefaultValue = 1, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public double RiskValue { get; set; }
        [Parameter("Max Spread ", DefaultValue = 2, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public double SpreadMax { get; set; }

        [Parameter("Atr SL Period ", DefaultValue = 24, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public int AtrSLPeriod { get; set; }
        [Parameter("Atr SL Multiplier ", DefaultValue = 1.5, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public int AtrSLMultiplier { get; set; }
        [Parameter("AtrSL Ma Type", DefaultValue = MovingAverageType.Exponential, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public MovingAverageType AtrSLMaType { get; set; }

        //Baseline Output

        [Parameter("Time Frame", DefaultValue = "Hour", Group = "###########################################################\n###   STRATEGIE TESTER  #####################################\n###########################################################\n===========================================================\nBASELINE 1\n===========================================================")]
        public TimeFrame Baseline1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "  => Moving Average                         STRATEGIE 1")]
        public bool Baseline1MovingAverageUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "        => Moving Average                         STRATEGIE 1")]
        public int Baseline1MovingAveragePeriod { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Simple, Group = "        => Moving Average                         STRATEGIE 1")]
        public MovingAverageType Baseline1MovingAverageMaType { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "  => Moving Average                         STRATEGIE 1")]
        public EnumSignalOverUnderType Baseline1MovingAverageSignal { get; set; }
        public enum EnumSignalOverUnderType
        {
            Over_Positive_Under_Negative,
            Over_Negative_Under_Positive,
        }

        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nENTRY 1\n===========================================================")]
        public TimeFrame Entry1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "  => Rsi                         ENTRY 1")]
        public bool Entry1RsiUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "  => Rsi                         ENTRY 1")]
        public int Entry1RsiPeriod { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Level, Group = "  => Rsi                         ENTRY 1")]
        public EnumSignalCrossType Entry1RsiSignal { get; set; }
        public enum EnumSignalCrossType
        {
            Cross_On_Level,
            Cross_On_Signal,
            Cross_On_Signal_And_Level,
        }
        [Parameter("     Level Buy", DefaultValue = 30, Group = "  => Rsi                         ENTRY 1")]
        public double Entry1RsiLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "  => Rsi                         ENTRY 1")]
        public double Entry1RsiLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Rsi                         ENTRY 1")]
        public int Entry1RsiLookBackMin { get; set; }

        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nCONFIRMATION 1\n===========================================================")]
        public TimeFrame Confirmation1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "   => Macd                         CONFIRMATION 1")]
        public bool Confirmation1MacdUse { get; set; }
        [Parameter("     Period Slow", DefaultValue = 26, Group = "  => Macd                         CONFIRMATION 1")]
        public int Confirmation1MacdPeriodSlow { get; set; }
        [Parameter("     Period Fast", DefaultValue = 12, Group = "  => Macd                         CONFIRMATION 1")]
        public int Confirmation1MacdPeriodFast { get; set; }
        [Parameter("     Period Signal", DefaultValue = 9, Group = "  => Macd                         CONFIRMATION 1")]
        public int Confirmation1MacdPeriodSignal { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Signal_And_Level, Group = "  => Macd                         CONFIRMATION 1")]
        public EnumSignalCrossType Confirmation1MacdSignal { get; set; }

        [Parameter("     Level Buy", DefaultValue = 30, Group = "  => Macd                         CONFIRMATION 1")]
        public double Confirmation1MacdLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "  => Macd                         CONFIRMATION 1")]
        public double Confirmation1MacdLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Macd                         CONFIRMATION 1")]
        public int Confirmation1MacdLookBackMin { get; set; }


        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nVOLUME 1\n===========================================================")]
        public TimeFrame Volume1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "  => Mfi                         VOLUME 1")]
        public bool Volume1MfiUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "   => Mfi                         VOLUME 1")]
        public int Volume1MfiPeriod { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Signal_And_Level, Group = "   => Mfi                         VOLUME 1")]
        public EnumSignalCrossType Volume1MfiSignal { get; set; }


        [Parameter("     Level Buy", DefaultValue = 30, Group = "  => Mfi                         VOLUME 1")]
        public double Volume1MfiLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "  => Mfi                         VOLUME 1")]
        public double Volume1MfiLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Mfi                         VOLUME 1")]
        public int Volume1MfiLookBackMin { get; set; }


        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nEXIT 1\n===========================================================")]
        public TimeFrame ExitTimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "  => SuperTrend                         EXIT 1")]
        public bool Exit1SuperTrendUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "  => SuperTrend                         EXIT 1")]
        public int Exit1SuperTrendPeriod { get; set; }
        [Parameter("     Period Multi", DefaultValue = 1, Group = "  => SuperTrend                         EXIT 1")]
        public int Exit1SuperTrendPeriodMulti { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "  => SuperTrend                         EXIT 1")]
        public EnumSignalOverUnderType Exit1SuperTrendSignal { get; set; }

        private MovingAverage baseline1MovingAverage;
        private RelativeStrengthIndex entry1Rsi;
        private MacdCrossOver confirmation1Macd;
        private MoneyFlowIndex volume1Mfi;
        private Supertrend exit1Supertrend;
        private AverageTrueRange atr;

        private string botNameLabelBuy;
        private string botNameLabelSell;

        protected override void OnStart()
        {
            baseline1MovingAverage = Indicators.MovingAverage(Bars.ClosePrices, Baseline1MovingAveragePeriod, Baseline1MovingAverageMaType);
            entry1Rsi = Indicators.RelativeStrengthIndex(Bars.ClosePrices, Entry1RsiPeriod);
            confirmation1Macd = Indicators.MacdCrossOver(Confirmation1MacdPeriodSlow, Confirmation1MacdPeriodFast, Confirmation1MacdPeriodSignal);
            volume1Mfi = Indicators.MoneyFlowIndex(Volume1MfiPeriod);
            exit1Supertrend = Indicators.Supertrend(Exit1SuperTrendPeriod, Exit1SuperTrendPeriodMulti);
            atr = Indicators.AverageTrueRange(AtrSLPeriod, AtrSLMaType);

            botNameLabelBuy = SymbolName + " " + "ENTER BUY " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
            botNameLabelSell = SymbolName + " " + "ENTER SELL " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
        }
        protected override void OnBar()
        {
            if ((Positions.FindAll(botNameLabelBuy, SymbolName).Length == 0 && Positions.FindAll(botNameLabelSell, SymbolName).Length == 0) && Server.Time.ToLocalTime().Hour >= StartHour && Server.Time.ToLocalTime().Hour < EndHour)
            {
                if (CheckEntryConditions() && GetSignal(TradeType.Buy) == 1)
                    Open(TradeType.Buy, botNameLabelBuy);
                if (CheckEntryConditions() && GetSignal(TradeType.Sell) == -1)
                    Open(TradeType.Sell, botNameLabelSell);

                if (Positions.FindAll(botNameLabelBuy, SymbolName, TradeType.Buy).Length > 0)
                {
                    if (GetExit1(TradeType.Buy) == -1)
                        Close(TradeType.Buy, botNameLabelBuy);
                }

                //Check short exit
                if (Positions.FindAll(botNameLabelSell, SymbolName, TradeType.Sell).Length > 0)
                {
                    if (GetExit1(TradeType.Buy) == 1)
                        Close(TradeType.Sell, botNameLabelSell);
                }
            }
        }
        private int GetSignal(TradeType tradeType)
        {
            if ((GetBasline1(tradeType) == 1) && (GetEntry1(tradeType) == 1) && (GetCondirmation1(tradeType) == 1) && (GetVolume1(tradeType) == 1))
                return 1;

            else if ((GetBasline1(tradeType) == -1) && (GetEntry1(tradeType) == -1) && (GetCondirmation1(tradeType) == -1) && (GetVolume1(tradeType) == -1))
                return -1;

            else
                return 0;
        }
        private int GetBasline1(TradeType tradeType)
        {

            var result = GetMovingAverage(baseline1MovingAverage, Baseline1MovingAverageSignal);

            if (!Baseline1MovingAverageUse && tradeType == TradeType.Buy) // without baseline
                return 1;
            else if (!Baseline1MovingAverageUse && tradeType == TradeType.Sell) // without baseline
                return -1;
            else if ((!Baseline1MovingAverageUse || result == 1) && tradeType == TradeType.Buy)
                return 1;
            else if ((!Baseline1MovingAverageUse || result == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }
        private int GetEntry1(TradeType tradeType)
        {
            var result = GetRsi(entry1Rsi, Entry1RsiSignal, Entry1RsiLevelBuy, Entry1RsiLevelSell, Entry1RsiLookBackMin);

            if (!Entry1RsiUse && tradeType == TradeType.Buy) // without baseline
                return 1;
            else if (!Entry1RsiUse && tradeType == TradeType.Sell) // without baseline
                return -1;
            else if ((!Entry1RsiUse || result == 1) && tradeType == TradeType.Buy)
                return 1;
            else if ((!Entry1RsiUse || result == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetCondirmation1(TradeType tradeType)
        {
            var result = GetMacd(confirmation1Macd, Confirmation1MacdSignal, Confirmation1MacdLevelBuy, Confirmation1MacdLevelSell, Confirmation1MacdLookBackMin);

            if (!Confirmation1MacdUse && tradeType == TradeType.Buy) // without baseline
                return 1;
            else if (!Confirmation1MacdUse && tradeType == TradeType.Sell) // without baseline
                return -1;
            else if ((!Confirmation1MacdUse || result == 1) && tradeType == TradeType.Buy)
                return 1;
            else if ((!Confirmation1MacdUse || result == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetVolume1(TradeType tradeType)
        {
            var result = GetMfi(volume1Mfi, Volume1MfiSignal, Volume1MfiLevelBuy, Volume1MfiLevelSell, Volume1MfiLookBackMin);

            if (!Volume1MfiUse && tradeType == TradeType.Buy) // without baseline
                return 1;
            else if (!Volume1MfiUse && tradeType == TradeType.Sell) // without baseline
                return -1;
            else if ((!Volume1MfiUse || result == 1) && tradeType == TradeType.Buy)
                return 1;
            else if ((!Volume1MfiUse || result == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetExit1(TradeType tradeType)
        {

            var result = GetSupertrend(exit1Supertrend);

            if (!Exit1SuperTrendUse && tradeType == TradeType.Buy) // without baseline
                return 1;
            else if (!Exit1SuperTrendUse && tradeType == TradeType.Sell) // without baseline
                return -1;
            else if ((!Exit1SuperTrendUse || result == 1) && tradeType == TradeType.Sell)
                return 1;
            else if ((!Exit1SuperTrendUse || result == -1) && tradeType == TradeType.Buy)
                return -1;
            else
                return 0;
        }

        private int GetSupertrend(Supertrend supertrend)
        {
            //if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            //{

            //Print("GetRsi   " + rsi.Result.Last(1));

            if (Bars.ClosePrices.Last(1) < supertrend.DownTrend.Last(1) && Bars.ClosePrices.Last(0) >= supertrend.DownTrend.Last(1))
                return 1;
            else if (Bars.ClosePrices.Last(1) > supertrend.UpTrend.Last(1) && Bars.ClosePrices.Last(0) <= supertrend.UpTrend.Last(1))
                return -1;
            else
                return 0;
            // }
            /*else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }
            else
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }*/
        }


        private int GetMacd(MacdCrossOver macd, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
        {
            if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            {
                if (macd.MACD.Last(1) > levelBuy)
                    return 1;
                else if (macd.MACD.Last(1) < levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                {
                    if (macd.MACD.Last(0) > macd.Signal.Last(0))
                        return 1;
                    else if (macd.MACD.Last(0) < macd.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }
            else
            {
                {
                    if (macd.MACD.Last(0) > macd.Signal.Last(0) && macd.MACD.Last(0) > levelBuy)
                        return 1;
                    else if (macd.MACD.Last(0) < macd.Signal.Last(0) && macd.MACD.Last(0) < levelSell)
                        return 1;
                    else
                        return 0;
                }
            }
        }


        private int GetMfi(MoneyFlowIndex mfi, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
        {
            //if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            //{

            Print("GetRsi   " + mfi.Result.Last(1));

            if (mfi.Result.Last(1) < levelBuy)
                return 1;
            else if (mfi.Result.Last(1) > levelSell)
                return -1;
            else
                return 0;
            // }
            /*else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }
            else
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }*/
        }





        private int GetMovingAverage(MovingAverage ma, EnumSignalOverUnderType signalOverUnder)
        {

            Print("Getma   " + ma.Result.Last(1));

            if (signalOverUnder == EnumSignalOverUnderType.Over_Positive_Under_Negative)
            {
                if (Bars.ClosePrices.Last(0) > ma.Result.Last(0))
                    return 1;
                else if (Bars.ClosePrices.Last(0) < ma.Result.Last(0))
                    return -1;
                else
                    return 0;
            }
            else
            {
                if (Bars.ClosePrices.Last(0) < ma.Result.Last(0))
                    return 1;
                else if (Bars.ClosePrices.Last(0) > ma.Result.Last(0))
                    return -1;
                else
                    return 0;
            }
        }



        private int GetRsi(RelativeStrengthIndex rsi, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
        {
            //if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            //{

            Print("GetRsi   " + rsi.Result.Last(1));

            if (rsi.Result.Minimum(Entry1RsiLookBackMin + 1) <= levelBuy && rsi.Result.Last(1) > levelBuy)
                return 1;
            else if (rsi.Result.Maximum(Entry1RsiLookBackMin + 1) >= levelSell && rsi.Result.Last(1) < levelSell)
                return -1;
            else
                return 0;
            // }
            /*else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }
            else
            {
                {
                    if (rsi.Result.Minimum(lookBack + 1) < rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) > rsi.Signal.Last(0) && rsi.Result.Last(0) < rsi.Signal.Last(0))
                        return 1;
                    else if (rsi.Result.Minimum(lookBack + 1) > rsi.Signal.Minimum(lookBack + 1) && rsi.Result.Last(0) < rsi.Signal.Last(0) && rsi.Result.Last(0) > rsi.Signal.Last(0))
                        return 1;
                    else
                        return 0;
                }
            }*/
        }

        private bool CheckEntryConditions()
        {
            bool chkDayofWeek = ((Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Monday && Mon == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Tuesday && Tue == true)
            || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Wednesday && Wed == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Thursday && Thu == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Friday && Fri == true)
            || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Saturday && Sat == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Sunday && Sun == true));

            if ((Bars.OpenTimes.Last(0).ToLocalTime().Hour >= StartHour) && (Bars.OpenTimes.Last(0).ToLocalTime().Hour <= EndHour) && (chkDayofWeek == true) && (Symbol.Spread < (SpreadMax * Symbol.PipSize)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private double GetSL(TradeType tradeType)
        {
            if (tradeType == TradeType.Buy)
            {
                //Print((Bars.ClosePrices.Last(0) - Bars.LowPrices.Last(1) + atr.Result.Last(1)) * Symbol.PipSize);
                return (Bars.ClosePrices.Last(1) - Bars.LowPrices.Last(1) + atr.Result.Last(1)) / Symbol.PipSize;
            }

            else
            {
                Print((Bars.LowPrices.Last(1) - atr.Result.Last(1)) * Symbol.PipSize);
                return (Bars.HighPrices.Last(1) - Bars.ClosePrices.Last(1) + atr.Result.Last(1)) / Symbol.PipSize;
            }

        }
        private void Open(TradeType tradeType, string label)
        {
            var tradeAmount = 0.0;
            var sl = GetSL(tradeType);

            if (SelectionRiskType == EnumSelectionRiskType.Fixed_Lot)
                tradeAmount = Symbol.QuantityToVolumeInUnits(RiskValue);
            else if (SelectionRiskType == EnumSelectionRiskType.Fixed_Volume)
                tradeAmount = Symbol.NormalizeVolumeInUnits(RiskValue);
            else if (SelectionRiskType == EnumSelectionRiskType.Perentage_Risk)
                tradeAmount = Symbol.VolumeForFixedRisk(Account.Balance * RiskValue / 100, sl, RoundingMode.Down);

            TradeResult result = ExecuteMarketOrder(tradeType, SymbolName, tradeAmount, label, sl, null);
        }

        //Function for closing trades - EP10-Functions and Parameters
        private void Close(TradeType tradeType, string label)
        {
            foreach (var position in Positions.FindAll(label, SymbolName, tradeType))
                ClosePosition(position);
        }
        protected override void OnStop()
        {
            // Handle cBot stop here
        }
    }
}