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
    public class NNFXStrategieTesterv101 : Robot
    {
        // DAYS TO TRADE 
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
        //Time TO TRADE 
        [Parameter("Trade Start Hour", DefaultValue = 0, Group = "Time To Trade")]
        public int StartHour { get; set; }
        [Parameter("Trade End Hour", DefaultValue = 24, Group = "Time To Trade")]
        public int EndHour { get; set; }

        //Risk TO TRADE 
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

        //SL Setting -> need add trailing stop, swing sl, fixed sl 
        [Parameter("Atr SL Period ", DefaultValue = 24, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public int AtrSLPeriod { get; set; }
        [Parameter("Atr SL Multiplier ", DefaultValue = 1.5, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public int AtrSLMultiplier { get; set; }
        [Parameter("AtrSL Ma Type", DefaultValue = MovingAverageType.Exponential, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================")]
        public MovingAverageType AtrSLMaType { get; set; }

        //Baseline Setting
        [Parameter("Time Frame", DefaultValue = "Daily", Group = "###########################################################\n###   STRATEGIE TESTER  #####################################\n###########################################################\n===========================================================\nBASELINE 1\n===========================================================")]
        public TimeFrame Baseline1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = true, Group = "  => Moving Average                         STRATEGIE 1")]
        public bool Baseline1MovingAverageUse { get; set; }
        [Parameter("     Period", DefaultValue = 34, Group = "        => Moving Average                         STRATEGIE 1")]
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
        //Entry Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nENTRY 1\n===========================================================")]
        public TimeFrame Entry1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = true, Group = "  => Rsi                         ENTRY 1")]
        public bool Entry1RsiUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "  => Rsi                         ENTRY 1")]
        public int Entry1RsiPeriod { get; set; }
        [Parameter("     Period Signal", DefaultValue = 9, Group = "  => Rsi                         ENTRY 1")]
        public int Entry1RsiPeriodSignal { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Simple, Group = "  => Rsi                         ENTRY 1")]
        public MovingAverageType Entry1RsiPeriodSignalMaType { get; set; }

        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Level, Group = "  => Rsi                         ENTRY 1")]
        public EnumSignalCrossType Entry1RsiSignal { get; set; }
        public enum EnumSignalCrossType
        {
            Cross_On_Level,
            Cross_On_Signal,
            Cross_On_Signal_And_Level,
            Cross_On_Level_And_Signal,
        }
        [Parameter("     Level Buy", DefaultValue = 30, Group = "  => Rsi                         ENTRY 1")]
        public double Entry1RsiLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "  => Rsi                         ENTRY 1")]
        public double Entry1RsiLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Rsi                         ENTRY 1")]
        public int Entry1RsiLookBackMin { get; set; }

        //Confirmation Setting
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
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Positive_Or_Under_Negative, Group = "  => Macd                         CONFIRMATION 1")]
        public EnumSignalDoubleOverUnderType Confirmation1MacdSignal { get; set; }
        public enum EnumSignalDoubleOverUnderType
        {
            Over_Level_And_Signal_Positive_Or_Under_Negative,
            Over_Level_And_Signal_Negative_Or_Under_Positive,

            Over_Level_And_Under_Signal_Positive_Or_Under_Level_And_Over_Signal_Negative,
            Over_Level_And_Under_Signal_Negative_Or_Under_Level_And_Over_Signal_Positive,
        }

        [Parameter("     Level Buy", DefaultValue = 0, Group = "  => Macd                         CONFIRMATION 1")]
        public double Confirmation1MacdLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 0, Group = "  => Macd                         CONFIRMATION 1")]
        public double Confirmation1MacdLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Macd                         CONFIRMATION 1")]
        public int Confirmation1MacdLookBackMin { get; set; }

        //Volume Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nVOLUME 1\n===========================================================")]
        public TimeFrame Volume1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "  => Mfi                         VOLUME 1")]
        public bool Volume1MfiUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "   => Mfi                         VOLUME 1")]
        public int Volume1MfiPeriod { get; set; }
        [Parameter("     Period Signal", DefaultValue = 9, Group = "  => Mfi                         VOLUME 1")]
        public int Volume1MfiPeriodSignal { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Simple, Group = "  => Mfi                         VOLUME 1")]
        public MovingAverageType Volume1MfiPeriodSignalMaType { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Negative_Under_Positive, Group = "   => Mfi                         VOLUME 1")]
        public EnumSignalOverUnderType Volume1MfiSignal { get; set; }
        [Parameter("     Level Buy", DefaultValue = 30, Group = "  => Mfi                         VOLUME 1")]
        public double Volume1MfiLevelBuy { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "  => Mfi                         VOLUME 1")]
        public double Volume1MfiLevelSell { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "  => Mfi                         VOLUME 1")]
        public int Volume1MfiLookBackMin { get; set; }

        //Exit Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nEXIT 1\n===========================================================")]
        public TimeFrame ExitTimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = true, Group = "  => SuperTrend                         EXIT 1")]
        public bool Exit1SuperTrendUse { get; set; }
        [Parameter("     Period", DefaultValue = 13, Group = "  => SuperTrend                         EXIT 1")]
        public int Exit1SuperTrendPeriod { get; set; }
        [Parameter("     Period Multi", DefaultValue = 1, Group = "  => SuperTrend                         EXIT 1")]
        public double Exit1SuperTrendPeriodMulti { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "  => SuperTrend                         EXIT 1")]
        public EnumSignalOverUnderType Exit1SuperTrendSignal
        { get; set; }

        //Init indicator
        private MovingAverage baseline1MovingAverage;
        private ST_RsiMa entry1Rsi;
        private MacdCrossOver confirmation1Macd;
        private ST_MfiMa volume1Mfi;
        private ST_SuperTrend exit1Supertrend;

        //SL Function
        private AverageTrueRange atr;

        //Base Setting 
        private string botNameLabelBuy;
        private string botNameLabelSell;

        //Statistics Setting 
        private List<List<double>> positionStat;
        private double initialBalance;
        private double drawUpMax;

        protected override void OnStart()
        {
            //Init indicator
            baseline1MovingAverage = Indicators.MovingAverage(MarketData.GetBars(Baseline1TimeFrame).ClosePrices, Baseline1MovingAveragePeriod, Baseline1MovingAverageMaType);
            entry1Rsi = Indicators.GetIndicator<ST_RsiMa>(MarketData.GetBars(Entry1TimeFrame), Entry1RsiPeriod, Entry1RsiPeriodSignal, Entry1RsiPeriodSignalMaType);
            confirmation1Macd = Indicators.MacdCrossOver(MarketData.GetBars(Confirmation1TimeFrame).ClosePrices, Confirmation1MacdPeriodSlow, Confirmation1MacdPeriodFast, Confirmation1MacdPeriodSignal);
            volume1Mfi = Indicators.GetIndicator<ST_MfiMa>(MarketData.GetBars(Volume1TimeFrame), Volume1MfiPeriod, Volume1MfiPeriodSignal, Volume1MfiPeriodSignalMaType);
            exit1Supertrend = Indicators.GetIndicator<ST_SuperTrend>(MarketData.GetBars(Exit1TimeFrame), Exit1SuperTrendPeriod, Exit1SuperTrendPeriodMulti);

            //SL Function
            atr = Indicators.AverageTrueRange(AtrSLPeriod, AtrSLMaType);

            //Base Setting 

            botNameLabelBuy = SymbolName + " " + "ENTER BUY " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
            botNameLabelSell = SymbolName + " " + "ENTER SELL " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
            Positions.Closed += OnPositionsClosed;

            //Statistics Setting 
            initialBalance = Account.Balance;
            positionStat = new List<List<double>>();
            drawUpMax = 0.0;

        }
        protected override void OnTick()
        {
            DrawUp();
        }

        protected override void OnBar()
        {
            //Check For position before enter -> possible to add parametter with multi position enter
            if ((Positions.FindAll(botNameLabelBuy, SymbolName).Length == 0 && Positions.FindAll(botNameLabelSell, SymbolName).Length == 0) && Server.Time.ToLocalTime().Hour >= StartHour && Server.Time.ToLocalTime().Hour < EndHour)
            {
                if (CheckEntryConditions() && GetSignal(TradeType.Buy) == 1)
                    Open(TradeType.Buy, botNameLabelBuy);
                if (CheckEntryConditions() && GetSignal(TradeType.Sell) == -1)
                    Open(TradeType.Sell, botNameLabelSell);
            }
            //Check For Buy Position before Exit
            if (Positions.FindAll(botNameLabelBuy, SymbolName, TradeType.Buy).Length > 0)
            {
                if (GetExit1(TradeType.Buy) == -1)
                    Close(TradeType.Buy, botNameLabelBuy);
            }
            //Check For Sell Position before Exit
            if (Positions.FindAll(botNameLabelSell, SymbolName, TradeType.Sell).Length > 0)
            {
                if (GetExit1(TradeType.Sell) == 1)
                    Close(TradeType.Sell, botNameLabelSell);
            }

        }
        private int GetSignal(TradeType tradeType)
        {
            // all function to get Signal Enter
            if ((GetBasline1(tradeType) == 1) && (GetEntry1(tradeType) == 1) && (GetCondirmation1(tradeType) == 1) && (GetVolume1(tradeType) == 1))
                return 1;

            else if ((GetBasline1(tradeType) == -1) && (GetEntry1(tradeType) == -1) && (GetCondirmation1(tradeType) == -1) && (GetVolume1(tradeType) == -1))
                return -1;

            else
                return 0;
        }
        private int GetBasline1(TradeType tradeType) // all indicator adding on baseline are implemented here
        {
            //int idx1 = Baseline1TimeFrame == Chart.TimeFrame ? index : barsTF.OpenTimes.GetIndexByTime(Bars.OpenTimes[index]); -> need to backtest for knowing if special index is needed for multiTimeframe

            var resultMovingAverage = FunctionSignalUnderOver(Bars.ClosePrices, baseline1MovingAverage.Result, Baseline1MovingAverageSignal); // FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)

            // without baseline
            if (!Baseline1MovingAverageUse && tradeType == TradeType.Buy)
                return 1;
            else if (!Baseline1MovingAverageUse && tradeType == TradeType.Sell)
                return -1;
            // with baseline
            else if ((!Baseline1MovingAverageUse || resultMovingAverage == 1) && tradeType == TradeType.Buy) // add all variable with this sentence (exemple): (!Volume1MfiUse || resultMfi == 1)
                return 1;
            else if ((!Baseline1MovingAverageUse || resultMovingAverage == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }
        private int GetEntry1(TradeType tradeType) // all indicator adding on entry are implemented here
        {
            var resultRsi = FunctionSignalCross(entry1Rsi.Result, entry1Rsi.Signal, Entry1RsiSignal, Entry1RsiLevelBuy, Entry1RsiLevelSell, Entry1RsiLookBackMin); //FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)

            // without Entry
            if (!Entry1RsiUse && tradeType == TradeType.Buy)
                return 1;
            else if (!Entry1RsiUse && tradeType == TradeType.Sell)
                return -1;
            // with Entry
            else if ((!Entry1RsiUse || resultRsi == 1) && tradeType == TradeType.Buy) // add all variable with this sentence (exemple): (!Entry1RsiUse || resultRsi == 1)
                return 1;
            else if ((!Entry1RsiUse || resultRsi == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetCondirmation1(TradeType tradeType) // all indicator adding on confirmation are implemented here
        {
            var resultMacd = FunctionSignalDoubleSignalUnderOver(confirmation1Macd.MACD, confirmation1Macd.Signal, Confirmation1MacdSignal, Confirmation1MacdLevelBuy, Confirmation1MacdLevelSell); // FunctionSignalDoubleSignalUnderOver(DataSeries result, DataSeries signal, EnumSignalDoubleOverUnderType signalOverUnder, double levelBuy, double levelSell)

            // without Condirmation
            if (!Confirmation1MacdUse && tradeType == TradeType.Buy)
                return 1;
            else if (!Confirmation1MacdUse && tradeType == TradeType.Sell) // without Condirmation
                return -1;
            // with Condirmation
            else if ((!Confirmation1MacdUse || resultMacd == 1) && tradeType == TradeType.Buy) // add all variable with this sentence : (!Confirmation1MacdUse || resultMacd == 1)
                return 1;
            else if ((!Confirmation1MacdUse || resultMacd == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetVolume1(TradeType tradeType) // all indicator adding on volume are implemented here
        {
            var resultMfi = FunctionSignalUnderOver(volume1Mfi.Result, volume1Mfi.Signal, Volume1MfiSignal); // FunctionignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)

            // without Volume
            if (!Volume1MfiUse && tradeType == TradeType.Buy)
                return 1;
            else if (!Volume1MfiUse && tradeType == TradeType.Sell) // without Volume
                return -1;
            // with Volume
            else if ((!Volume1MfiUse || resultMfi == 1) && tradeType == TradeType.Buy)  // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                return 1;
            else if ((!Volume1MfiUse || resultMfi == -1) && tradeType == TradeType.Sell)
                return -1;
            else
                return 0;
        }

        private int GetExit1(TradeType tradeType) // all indicator adding on exit are implemented here
        {
            var resultSuperTrend = FunctionCross(Bars.ClosePrices, exit1Supertrend.Result); // FunctionCross(DataSeries result, DataSeries signal)

            // without Exit
            if (!Exit1SuperTrendUse && tradeType == TradeType.Buy) // without Exit
                return 1;
            else if (!Exit1SuperTrendUse && tradeType == TradeType.Sell)
                return -1;
            // with Exit
            else if ((!Exit1SuperTrendUse || resultSuperTrend == 1) && tradeType == TradeType.Sell) // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                return 1;
            else if ((!Exit1SuperTrendUse || resultSuperTrend == -1) && tradeType == TradeType.Buy)
                return -1;
            else
                return 0;
        }

        // FUNCTION FOR SIGNAL -> need backtest and finding if other conditions need to be add
        // Confirmation Type Function
        private int FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
        {
            if (signalOverUnder == EnumSignalOverUnderType.Over_Positive_Under_Negative)
            {
                if (result.Last(0) > Signal.Last(0))
                    return 1;
                else if (result.Last(0) < Signal.Last(0))
                    return -1;
                else
                    return 0;
            }
            else // Over_Negative_Under_Positive
            {
                if (result.Last(0) < Signal.Last(0))
                    return 1;
                else if (result.Last(0) > Signal.Last(0))
                    return -1;
                else
                    return 0;
            }
        }
        // Confirmation Type Function
        private int FunctionSignalDoubleSignalUnderOver(DataSeries result, DataSeries signal, EnumSignalDoubleOverUnderType signalOverUnder, double levelBuy, double levelSell)
        {
            if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Positive_Or_Under_Negative)
            {
                if (result.Last(1) > levelBuy && result.Last(1) > signal.Last(1))
                    return 1;
                else if (result.Last(1) < levelSell && result.Last(1) < signal.Last(1))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Positive_Or_Under_Negative)
            {
                if (result.Last(1) < levelBuy && result.Last(1) < signal.Last(1))
                    return 1;
                else if (result.Last(1) > levelSell && result.Last(1) > signal.Last(1))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Under_Signal_Positive_Or_Under_Level_And_Over_Signal_Negative)
            {
                if (result.Last(1) > levelBuy && result.Last(1) < signal.Last(1))
                    return 1;
                else if (result.Last(1) < levelSell && result.Last(1) > signal.Last(1))
                    return -1;
                else
                    return 0;
            }

            else // Over_Level_And_Under_Signal_Negative_Or_Under_Level_And_Over_Signal_Positive
            {
                if (result.Last(1) < levelBuy && result.Last(1) > signal.Last(1))
                    return 1;
                else if (result.Last(1) > levelSell && result.Last(1) < signal.Last(1))
                    return -1;
                else
                    return 0;
            }
        }
        // Real Enter Type Function -> min and max for having possibility to find range period with Multiple signal (Maybe need a special function) it work but it's not good When I think about it. (result.Minimum(lookBack + 1) <= signal.Last(2) && result.Last(1) > signal.Last(1))
        private int FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
        {
            if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            {
                if (result.Minimum(lookBack + 1) <= levelBuy && result.Last(1) > levelBuy)
                    return 1;
                else if (result.Maximum(lookBack + 1) >= levelSell && result.Last(1) < levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalCrossType.Cross_On_Level_And_Signal)
            {
                if (result.Minimum(lookBack + 1) <= levelBuy && result.Last(1) > levelBuy && signal.Last(1) > signal.Last(1))
                    return 1;
                else if (result.Maximum(lookBack + 1) >= levelSell && result.Last(1) < levelSell && signal.Last(1) < signal.Last(1))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                if (result.Minimum(lookBack + 1) <= signal.Last(2) && result.Last(1) > signal.Last(1))
                    return 1;
                else if (result.Maximum(lookBack + 1) >= signal.Last(2) && result.Last(1) < signal.Last(1))
                    return -1;
                else
                    return 0;
            }

            else // Cross_On_Signal_And_Level
            {
                if (result.Minimum(lookBack + 1) <= signal.Last(2) && result.Last(1) > signal.Last(1) && result.Last(1) < levelBuy)
                    return 1;
                else if (result.Maximum(lookBack + 1) >= signal.Last(2) && result.Last(1) < signal.Last(1) && result.Last(1) > levelSell)
                    return -1;
                else
                    return 0;
            }
        }
        // Exit Type Function
        private int FunctionCross(DataSeries result, DataSeries signal)
        {
            if (result.Last(2) < signal.Last(2) && result.Last(1) >= signal.Last(1))
                return 1;
            else if (result.Last(2) > signal.Last(2) && result.Last(1) <= signal.Last(1))
                return -1;
            else
                return 0;
        }

        // FUNCTION BASE ROBOT
        // Function time to trade, days, and spred
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
        // Function Sl -> need to add swing, fixed sl, 
        private double GetSL(TradeType tradeType)
        {
            if (tradeType == TradeType.Buy)
                return (Bars.ClosePrices.Last(1) - Bars.LowPrices.Last(1) + atr.Result.Last(1)) / Symbol.PipSize;
            else
                return (Bars.HighPrices.Last(1) - Bars.ClosePrices.Last(1) + atr.Result.Last(1)) / Symbol.PipSize;
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

            TradeResult result = ExecuteMarketOrder(tradeType, SymbolName, tradeAmount, label, sl, null, null, false /*Trailing stop */ );

            // Possible Pending Order for a waiting retest of signal
            //TradeResult resultPending = PlaceLimitOrder(tradeType, SymbolName, tradeAmount, Bars.ClosePrices.Last(0), label, sl, null);

        }
        //Function Closing trade on Signal SL
        private void Close(TradeType tradeType, string label)
        {
            var lastPosition = Positions.FindAll(label, SymbolName, tradeType).Last();
            foreach (var position in Positions.FindAll(label, SymbolName, tradeType))
                ClosePosition(position);

            AddInfosPositionList(lastPosition);
        }
        //Function Closing trade on Base SL
        private void OnPositionsClosed(PositionClosedEventArgs args)
        {
            AddInfosPositionList(args.Position);
        }
        //Statistics Functions
        private void AddInfosPositionList(Position position)
        {
            double accountBalance = 0.0;          //[positionStat.count-1][0]
            double netProfit = 0.0;               //[positionStat.count-1][1]
            double profitFactor = 0.0;            //[positionStat.count-1][2]
            double commission = 0.0;              //[positionStat.count-1][3]
            double swap = 0.0;                    //[positionStat.count-1][4]
            double drawdown = 0.0;                //[positionStat.count-1][5]
            double drawup = 0.0;                  //[positionStat.count-1][6]
            double consecutiveWin = 0.0;          //[positionStat.count-1][7]
            double consecutiveLoss = 0.0;

            if (positionStat.Count == 0)
            {
                accountBalance = initialBalance;
                netProfit = position.NetProfit;
                commission = position.Commissions;
                profitFactor = position.TradeType == TradeType.Buy ? ((position.CurrentPrice - position.EntryPrice) / (position.EntryPrice - position.StopLoss.Value)) : ((position.EntryPrice - position.CurrentPrice) / (position.StopLoss.Value - position.EntryPrice));
                swap = position.Swap;
                drawdown = initialBalance - netProfit;
                drawup = drawUpMax;
                consecutiveWin = 0;
                consecutiveLoss = 0;

                positionStat.Add(new List<double> { accountBalance, netProfit, profitFactor, commission, swap, drawdown, drawup, consecutiveWin, consecutiveLoss });

                // average of all stat at the end or on close position ?
            }

            else
            {
                var lastPosition = positionStat.Count - 1;

                accountBalance = initialBalance;
                netProfit = position.NetProfit;
                commission = position.Commissions;
                profitFactor = position.TradeType == TradeType.Buy ? ((position.CurrentPrice - position.EntryPrice) / (position.EntryPrice - position.StopLoss.Value)) : ((position.EntryPrice - position.CurrentPrice) / (position.StopLoss.Value - position.EntryPrice));
                swap = position.Swap;
                drawdown = initialBalance - netProfit;
                drawup = drawUpMax;
                consecutiveWin = netProfit > 0 ? positionStat[lastPosition][7] + 1 : 0;
                consecutiveLoss = netProfit < 0 ? positionStat[lastPosition][8] + 1 : 0;

                positionStat.Add(new List<double> { accountBalance, netProfit, profitFactor, commission, swap, drawdown, drawup, consecutiveWin, consecutiveLoss });
            }
            initialBalance = Account.Balance;
            drawUpMax = 0.0;
        }
        private void DrawUp()
        {
            var positionSymbol = Positions.FindAll(botNameLabelBuy).Length + Positions.FindAll(botNameLabelSell).Length;

            if (positionSymbol == 0)
            {
                drawUpMax = 0.0;
            }
            else if (positionSymbol != 0)
            {
                var drawUp = Positions.Where(x => x.TradeType == TradeType.Buy || x.TradeType == TradeType.Sell).Sum(p => p.NetProfit);
                while (drawUp > drawUpMax)
                    drawUpMax = drawUp;
            }
        }

        protected override void OnStop()
        {
            // Print on log
            var averageaccountBalance = 0.0;
            var averagenetProfit = 0.0;
            var averageprofitFactor = 0.0;
            var averagecommission = 0.0;
            var averageswap = 0.0;
            var averagedrawdown = 0.0;
            var averagedrawup = 0.0;
            var averageconsecutiveWin = 0.0;
            var averageconsecutiveLoss = 0.0;

            initialBalance = Account.Balance;

            for (int i = 0; i < positionStat.Count; i++)
            {
                averageaccountBalance += positionStat[i][0];
                averagenetProfit += positionStat[i][1];
                averageprofitFactor += positionStat[i][2];
                averagecommission += positionStat[i][3];
                averageswap += positionStat[i][4];
                averagedrawdown += positionStat[i][5];
                averagedrawup += positionStat[i][6];
                averageconsecutiveWin += positionStat[i][7];
                averageconsecutiveLoss += positionStat[i][8];
            }

            var res0 = averageaccountBalance / positionStat.Count;
            var res1 = averagenetProfit / positionStat.Count;
            var res2 = averageprofitFactor / positionStat.Count;
            var res3 = averagecommission / positionStat.Count;
            var res4 = averageswap / positionStat.Count;
            var res5 = averagedrawdown / positionStat.Count;
            var res6 = averagedrawup / positionStat.Count;
            var res7 = averageconsecutiveWin / positionStat.Count;
            var res8 = averageconsecutiveLoss / positionStat.Count;

            Print("average account Balance :   " + res0 + " || average net Profit :    " + res1 + " || average profit Factor  :  " + res2 + " || average commission :  " + res3 + " || average swap :  " + res4 + " || average drawdown :  " + res5 + " || average drawup :  " + res6 + " || average consecutive Win :  " + res7 + " || average consecutive Loss :  " + res8);

            // Export to csv need Function need idea for good using
        }
    }
}
