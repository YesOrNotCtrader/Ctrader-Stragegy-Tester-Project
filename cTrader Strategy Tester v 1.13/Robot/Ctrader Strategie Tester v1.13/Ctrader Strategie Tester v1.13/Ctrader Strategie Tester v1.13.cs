/* 
//// End backtest\\\

- Optimisation Fitness Check: X
- CSV Add winrate : X

*/
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cAlgo.API;
using cAlgo.API.Collections;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Robots
{
    [Robot(AccessRights = AccessRights.FullAccess)]
    public class CtraderStrategieTesterv11 : Robot
    {
        // DAYS TO TRADE 
        //--------------------------------Export--------------------------------------------------------------//
        [Parameter("Export Data to CSV ?", DefaultValue = true, Group = "===========================================================\nDATA EXPORT\n===========================================================")]
        public bool ExportData { get; set; }
        [Parameter("Data Folder", DefaultValue = "C:\\Users\\User\\Desktop", Group = "===========================================================\nDATA EXPORT\n===========================================================")]
        public string DataDir { get; set; }
        [Parameter("File Name", DefaultValue = EnumFileName.Account_Balance, Group = "===========================================================\nDATA EXPORT\n===========================================================")]
        public EnumFileName FileName { get; set; }
        public enum EnumFileName
        {
            Account_Balance,
            Profit_Factor,
            Drawdown,
            Drawup,
            Consecutive_Win,
            Consecutive_Loss,
        }
        [Parameter("Bars Or Ticks Method", DefaultValue = EnumTicksOrBarsCalculation.Bars, Group = "===========================================================\nMETHOD CALCULATION\n===========================================================")]
        public EnumTicksOrBarsCalculation TicksOrBarsMethod { get; set; }
        public enum EnumTicksOrBarsCalculation
        {
            Bars,
            Ticks
        }

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
        [Parameter("Value", DefaultValue = 1, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================", Step = 0.1)]
        public double RiskValue { get; set; }
        [Parameter("Max Spread ", DefaultValue = 2, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================", Step = 1)]
        public double SpreadMax { get; set; }
        [Parameter("Comission in Pips ", DefaultValue = 1, Group = "===========================================================\nMONNEY MANNAGEMENT\n===========================================================", Step = 0.5)]
        public double ComissionInPips { get; set; }

        [Parameter("Sleeping Strategie", DefaultValue = EnumSleepingStrategie.None, Group = "Sleeping Period")]
        public EnumSleepingStrategie SleepingStrategie { get; set; }
        public enum EnumSleepingStrategie
        {
            On_Loss,
            On_Win,
            On_All,
            None,
        }

        [Parameter("Time Frame Bars", DefaultValue = "Hour", Group = "Sleeping Period")]
        public TimeFrame SleepingPeriodTimeframe { get; set; }
        [Parameter("Numbers Bar To Open New Trade", DefaultValue = 1, Group = "Sleeping Period")]
        public int SleepingPeriodNbrsBars { get; set; }

        //INDICATOR 
        //Baseline Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "###########################################################\n###   STRATEGIE TESTER  #####################################\n###########################################################\n===========================================================\nBASELINE 1\n===========================================================")]
        public TimeFrame Baseline1TimeFrame { get; set; }
        [Parameter("Use it ?", DefaultValue = false, Group = "B1  => Indicator 1 parametter")]
        public bool Baseline1UseP1 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_1Parametter.EnumIndiSelection.Simple_Moving_Average, Group = "B1  => Indicator 1 parametter")]
        public CSTP_Indi_1Parametter.EnumIndiSelection Baseline1IndiSelectionP1 { get; set; }
        [Parameter("     Value1", DefaultValue = 34, Group = "B1  => Indicator 1 parametter", Step = 1)]
        public double Baseline1Value1P1 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "B1  => Indicator 1 parametter")]
        public EnumSignalOverUnderType Baseline1SignalTypeP1 { get; set; }
        public enum EnumSignalOverUnderType
        {
            Over_Positive_Under_Negative,
            Over_Negative_Under_Positive,
        }

        [Parameter("Use it ?", DefaultValue = true, Group = "B1  => Indicator 2 parametter")]
        public bool Baseline1UseP2 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_2Parametter.EnumIndiSelection.SuperTrend, Group = "B1  => Indicator 2 parametter")]
        public CSTP_Indi_2Parametter.EnumIndiSelection Baseline1IndiSelectionP2 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "B1  => Indicator 2 parametter", Step = 1)]
        public double Baseline1Value1P2 { get; set; }
        [Parameter("     Value2", DefaultValue = 10, Group = "B1  => Indicator 2 parametter", Step = 1)]
        public double Baseline1Value2P2 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "B1  => Indicator 2 parametter")]
        public EnumSignalOverUnderType Baseline1SignalTypeP2 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "B1  => Indicator 4 parametter")]
        public bool Baseline1UseP4 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Price_Volume_Trend, Group = "B1  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Baseline1IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "B1  => Indicator 4 parametter", Step = 1)]
        public double Baseline1Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 55, Group = "B1  => Indicator 4 parametter", Step = 1)]
        public double Baseline1Value2P4 { get; set; }
        [Parameter("     Value3", DefaultValue = 55, Group = "B1  => Indicator 4 parametter", Step = 1)]
        public double Baseline1Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Simple, Group = "B1  => Indicator 4 parametter")]
        public MovingAverageType Baseline1MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Signal_Positive_Or_Under_Negative, Group = "B1  => Indicator 4 parametter")]
        public EnumSignalDoubleOverUnderType Baseline1SignalTypeP4 { get; set; }
        public enum EnumSignalDoubleOverUnderType
        {
            oO_1_Condition_Oo,
            __________________,
            Over_Level_Positive_Or_Under_Negative,
            Over_Level_Negative_Or_Under_Positive,
            Over_Signal_Positive_Or_Under_Negative,
            Over_Signal_Negative_Or_Under_Positive,

            Over_Level_Valid_Or_Under_Invalid,
            Over_Level_Invalid_Or_Under_Valid,
            Over_Signal_Valid_Or_Under_Invalid,
            Over_Signal_Invalid_Or_Under_Valid,
            _________________,
            oO_2_Condition_Oo,
            ___________________,
            Over_Level_And_Signal_Positive_Or_Under_Negative,
            Over_Level_And_Signal_Negative_Or_Under_Positive,
            Over_Level_And_Under_Signal_Positive_Or_Under_Level_And_Over_Signal_Negative,
            Over_Level_And_Under_Signal_Negative_Or_Under_Level_And_Over_Signal_Positive,
            Over_Level_And_Signal_Valid_Or_Under_Invalid,
            Over_Level_And_Signal_Invalid_Or_Under_Valid,
        }
        [Parameter("     Level Buy", DefaultValue = 50, Group = "B1  => Indicator 4 parametter", Step = 1)]
        public double Baseline1LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 50, Group = "B1  => Indicator 4 parametter", Step = 1)]
        public double Baseline1LevelSellP4 { get; set; }

        //Entry Setting 
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nENTRY 1\n===========================================================")]
        public TimeFrame Entry1TimeFrame { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "E1  => Indicator 1 parametter")]
        public bool Entry1UseP1 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_1Parametter.EnumIndiSelection.Simple_Moving_Average, Group = "E1  => Indicator 1 parametter")]
        public CSTP_Indi_1Parametter.EnumIndiSelection Entry1IndiSelectionP1 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "E1  => Indicator 1 parametter", Step = 1)]
        public double Entry1Value1P1 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Signal, Group = "E1  => Indicator 1 parametter")]
        public EnumSignalCrossType Entry1SignalTypeP1 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 2, MinValue = 1, Group = "E1  => Indicator 1 parametter", Step = 1)]
        public int Entry1LookBackMinP1 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "E1  => Indicator 2 parametter")]
        public bool Entry1UseP2 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_2Parametter.EnumIndiSelection.SuperTrend, Group = "E1  => Indicator 2 parametter")]
        public CSTP_Indi_2Parametter.EnumIndiSelection Entry1IndiSelectionP2 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "E1  => Indicator 2 parametter", Step = 1)]
        public double Entry1Value1P2 { get; set; }
        [Parameter("     Value2", DefaultValue = 10, Group = "E1  => Indicator 2 parametter", Step = 1)]
        public double Entry1Value2P2 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Signal, Group = "E1  => Indicator 2 parametter")]
        public EnumSignalCrossType Entry1SignalTypeP2 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 2, MinValue = 1, Group = "E1  => Indicator 2 parametter", Step = 1)]
        public int Entry1LookBackMinP2 { get; set; }

        [Parameter("Use it ?", DefaultValue = true, Group = "E1  => Indicator 4 parametter")]
        public bool Entry1UseP4 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Relative_Strength_Index, Group = "E1  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Entry1IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public double Entry1Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 1, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public double Entry1Value2P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 9, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public double Entry1Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Simple, Group = "E1  => Indicator 4 parametter")]
        public MovingAverageType Entry1MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalCrossType.Cross_On_Level, Group = "E1  => Indicator 4 parametter")]
        public EnumSignalCrossType Entry1SignalTypeP4 { get; set; }
        public enum EnumSignalCrossType
        {
            Cross_On_Level,
            Cross_On_Signal,
            Cross_On_Signal_And_Level,
            Cross_On_Level_And_Signal,
        }
        [Parameter("     Level Buy", DefaultValue = 30, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public double Entry1LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 70, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public double Entry1LevelSellP4 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "E1  => Indicator 4 parametter", Step = 1)]
        public int Entry1LookBackMinP4 { get; set; }

        //Confirmation Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nConfirmation 1\n===========================================================")]
        public TimeFrame Confirmation1TimeFrame { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "C1  => Indicator 1 parametter")]
        public bool Confirmation1UseP1 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_1Parametter.EnumIndiSelection.Simple_Moving_Average, Group = "C1  => Indicator 1 parametter")]
        public CSTP_Indi_1Parametter.EnumIndiSelection Confirmation1IndiSelectionP1 { get; set; }
        [Parameter("     Value1", DefaultValue = 34, Group = "C1  => Indicator 1 parametter", Step = 1)]
        public double Confirmation1Value1P1 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "C1  => Indicator 1 parametter")]
        public EnumSignalOverUnderType Confirmation1SignalTypeP1 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "C1  => Indicator 2 parametter")]
        public bool Confirmation1UseP2 { get; set; }
        [Parameter("     BaseLine Indicator", DefaultValue = CSTP_Indi_2Parametter.EnumIndiSelection.SuperTrend, Group = "C1  => Indicator 2 parametter")]
        public CSTP_Indi_2Parametter.EnumIndiSelection Confirmation1IndiSelectionP2 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "C1  => Indicator 2 parametter", Step = 1)]
        public double Confirmation1Value1P2 { get; set; }
        [Parameter("     Value2", DefaultValue = 10, Group = "C1  => Indicator 2 parametter", Step = 1)]
        public double Confirmation1Value2P2 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalOverUnderType.Over_Positive_Under_Negative, Group = "C1  => Indicator 2 parametter")]
        public EnumSignalOverUnderType Confirmation1SignalTypeP2 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "C1  => Indicator 4 parametter")]
        public bool Confirmation1UseP4 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Price_Volume_Trend, Group = "C1  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Confirmation1IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 26, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public double Confirmation1Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 12, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public double Confirmation1Value2P4 { get; set; }
        [Parameter("     Value3", DefaultValue = 9, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public double Confirmation1Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Exponential, Group = "C1  => Indicator 4 parametter")]
        public MovingAverageType Confirmation1MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Positive_Or_Under_Negative, Group = "C1  => Indicator 4 parametter")]
        public EnumSignalDoubleOverUnderType Confirmation1SignalTypeP4 { get; set; }
        [Parameter("     Level Buy", DefaultValue = 50, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public double Confirmation1LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 50, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public double Confirmation1LevelSellP4 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "C1  => Indicator 4 parametter", Step = 1)]
        public int Confirmation1LookBackMinP4 { get; set; }

        //Volume Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nVolume 1\n===========================================================")]
        public TimeFrame Volume1TimeFrame { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "V1  => Indicator 4 parametter")]
        public bool Volume1UseP4 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Price_Volume_Trend, Group = "V1  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Volume1IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 26, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public double Volume1Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 12, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public double Volume1Value2P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 9, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public double Volume1Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Exponential, Group = "V1  => Indicator 4 parametter")]
        public MovingAverageType Volume1MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Signal_Positive_Or_Under_Negative, Group = "V1  => Indicator 4 parametter")]
        public EnumSignalDoubleOverUnderType Volume1SignalTypeP4 { get; set; }
        [Parameter("     Level Buy", DefaultValue = 50, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public double Volume1LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 50, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public double Volume1LevelSellP4 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "V1  => Indicator 4 parametter", Step = 1)]
        public int Volume1LookBackMinP4 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "V2  => Indicator 4 parametter")]
        public bool Volume2UseP4 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Price_Volume_Trend, Group = "V2  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Volume2IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 26, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public double Volume2Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 12, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public double Volume2Value2P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 9, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public double Volume2Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Exponential, Group = "V2  => Indicator 4 parametter")]
        public MovingAverageType Volume2MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Signal_Positive_Or_Under_Negative, Group = "V2  => Indicator 4 parametter")]
        public EnumSignalDoubleOverUnderType Volume2SignalTypeP4 { get; set; }
        [Parameter("     Level Buy", DefaultValue = 50, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public double Volume2LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 50, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public double Volume2LevelSellP4 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "V2  => Indicator 4 parametter", Step = 1)]
        public int Volume2LookBackMinP4 { get; set; }

        [Parameter("Use it ?", DefaultValue = false, Group = "V3  => Indicator 4 parametter")]
        public bool Volume3UseP4 { get; set; }
        [Parameter("     Entry Indicator", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Price_Volume_Trend, Group = "V3  => Indicator 4 parametter")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Volume3IndiSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 26, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public double Volume3Value1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 12, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public double Volume3Value2P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 9, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public double Volume3Value3P4 { get; set; }
        [Parameter("     Moving Average Type", DefaultValue = MovingAverageType.Exponential, Group = "V3  => Indicator 4 parametter")]
        public MovingAverageType Volume3MaTypeP4 { get; set; }
        [Parameter("     Signal Type", DefaultValue = EnumSignalDoubleOverUnderType.Over_Signal_Positive_Or_Under_Negative, Group = "V3  => Indicator 4 parametter")]
        public EnumSignalDoubleOverUnderType Volume3SignalTypeP4 { get; set; }
        [Parameter("     Level Buy", DefaultValue = 50, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public double Volume3LevelBuyP4 { get; set; }
        [Parameter("     Level Sell", DefaultValue = 50, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public double Volume3LevelSellP4 { get; set; }
        [Parameter("     LookBack Min", DefaultValue = 1, MinValue = 1, Group = "V3  => Indicator 4 parametter", Step = 1)]
        public int Volume3LookBackMinP4 { get; set; }

        //Exit Setting
        [Parameter("Time Frame", DefaultValue = "Hour", Group = "\n===========================================================\nExit 1\n===========================================================")]
        public TimeFrame Exit1TimeFrame { get; set; }
        [Parameter("Stop Loss Type", DefaultValue = EnumExit1IndiType.Atr, Group = "Ex1  => Setting")]
        public EnumExit1IndiType Exit1IndiType { get; set; }
        public enum EnumExit1IndiType
        {
            Sl_TP_Fixed,
            Swing,
            Atr,
            Indicator_1_Parametter,
            Indicator_2_Parametters,
            Indicator_4_Parametters,
        }
        [Parameter("     SL Fixed / Period (Swing/Indicators)", DefaultValue = 10, Group = "Ex1  => Setting", Step = 1)]
        public double Sl { get; set; }
        [Parameter("     Multiplier", DefaultValue = 1.5, Group = "Ex1  => Setting", Step = 0.1)]
        public double SlMultiplier { get; set; }
        [Parameter("     RRR (0 = Disable)", DefaultValue = 2, Group = "Ex1  => Setting", Step = 0.1)]
        public double RRR { get; set; }

        [Parameter("Indicator 1 Parametter", DefaultValue = CSTP_Indi_1Parametter.EnumIndiSelection.Donchian_Middle, Group = "Ex1  => Stop Loss Calculation")]
        public CSTP_Indi_1Parametter.EnumIndiSelection Exit1CalculationSelectionP1 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue1P1 { get; set; }
        [Parameter("Indicator 2 Parametter", DefaultValue = CSTP_Indi_2Parametter.EnumIndiSelection.SuperTrend, Group = "Ex1  => Stop Loss Calculation")]
        public CSTP_Indi_2Parametter.EnumIndiSelection Exit1CalculationSelectionP2 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue1P2 { get; set; }
        [Parameter("     Value2", DefaultValue = 3, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue2P2 { get; set; }
        [Parameter("Indicator 3 Parametter", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Average_True_Range, Group = "Ex1  => Stop Loss Calculation")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Exit1CalculationSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 1, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue2P4 { get; set; }
        [Parameter("     Value3", DefaultValue = 1, Group = "Ex1  => Stop Loss Calculation", Step = 1)]
        public double Exit1CalculationValue3P4 { get; set; }
        [Parameter("     MaType", DefaultValue = MovingAverageType.Simple, Group = "Ex1  => Stop Loss Calculation")]
        public MovingAverageType Exit1CalculationMaTypeP4 { get; set; }

        [Parameter("Trailling Stop Type", DefaultValue = EnumExit1IndiTraillingType.Indicator_1_Parametter, Group = "Ex1  => Trailling Stop Setting")]
        public EnumExit1IndiTraillingType Exit1TraillingType { get; set; }
        public enum EnumExit1IndiTraillingType
        {
            Indicator_1_Parametter,
            Indicator_2_Parametters,
            Indicator_4_Parametters,
            Standard_Trailling_Stop,
            None,
        }

        [Parameter("     Start on RRR", DefaultValue = 0.3, Group = "Ex1  => Trailling Stop Setting", Step = 0.1)]
        public double Exit1TraillingStart { get; set; }
        [Parameter("Indicator 1 Parametter", DefaultValue = CSTP_Indi_1Parametter.EnumIndiSelection.Donchian_Middle, Group = "Ex1  => Trailling Stop Setting")]
        public CSTP_Indi_1Parametter.EnumIndiSelection Exit1TraillingSelectionP1 { get; set; }
        [Parameter("     Value1", DefaultValue = 55, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue1P1 { get; set; }
        [Parameter("Indicator 2 Parametter", DefaultValue = CSTP_Indi_2Parametter.EnumIndiSelection.SuperTrend, Group = "Ex1  => Trailling Stop Setting")]
        public CSTP_Indi_2Parametter.EnumIndiSelection Exit1TraillingSelectionP2 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue1P2 { get; set; }
        [Parameter("     Value2", DefaultValue = 3, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue2P2 { get; set; }
        [Parameter("Indicator 3 Parametter", DefaultValue = CSTP_Indi_4Parametter.EnumIndiSelection.Average_True_Range, Group = "Ex1  => Trailling Stop Setting")]
        public CSTP_Indi_4Parametter.EnumIndiSelection Exit1TraillingSelectionP4 { get; set; }
        [Parameter("     Value1", DefaultValue = 13, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue1P4 { get; set; }
        [Parameter("     Value2", DefaultValue = 1, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue2P4 { get; set; }
        [Parameter("     Value3", DefaultValue = 1, Group = "Ex1  => Trailling Stop Setting", Step = 1)]
        public double Exit1TraillingValue3P4 { get; set; }
        [Parameter("     MaType", DefaultValue = MovingAverageType.Simple, Group = "Ex1  => Trailling Stop Setting")]
        public MovingAverageType Exit1TraillingMaTypeP4 { get; set; }


        //--------------------------------Optimisation--------------------------------------------------------------//
        [Parameter("Ratio Win / Loss", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool RatioWin { get; set; }
        [Parameter("Equity Drawdown Max", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool DrawdownEquityMax { get; set; }
        [Parameter("Balance Drawdown Max", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool DrawdownBalanceMax { get; set; }
        [Parameter("Ratio Profi Net", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool RatioProfiNet { get; set; }
        [Parameter("Ratio Tp/Sl", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool RatioTpSl { get; set; }
        [Parameter("Profit Factor", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool ProfitFactor { get; set; }
        [Parameter("K ratio Optimisation", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool Kratio { get; set; }
        [Parameter("PROM Optimisation", DefaultValue = false, Group = "============================================================================\nBACKTEST SETTING\n============================================================================\nGet Fitness Parametters")]
        public bool PROM { get; set; }




        //Init indicator
        private CSTP_Indi_1Parametter baseline1P1;
        private CSTP_Indi_2Parametter baseline1P2;
        private CSTP_Indi_4Parametter baseline1P4;

        private CSTP_Indi_1Parametter entry1P1;
        private CSTP_Indi_2Parametter entry1P2;
        private CSTP_Indi_4Parametter entry1P4;

        private CSTP_Indi_1Parametter confirmation1P1;
        private CSTP_Indi_2Parametter confirmation1P2;
        private CSTP_Indi_4Parametter confirmation1P4;

        private CSTP_Indi_4Parametter volume1P4;
        private CSTP_Indi_4Parametter volume2P4;
        private CSTP_Indi_4Parametter volume3P4;

        private CSTP_Indi_1Parametter exit1CalculationP1;
        private CSTP_Indi_2Parametter exit1CalculationP2;
        private CSTP_Indi_4Parametter exit1CalculationP4;

        private CSTP_Indi_1Parametter exit1TraillingP1;
        private CSTP_Indi_2Parametter exit1TraillingP2;
        private CSTP_Indi_4Parametter exit1TraillingP4;

        //SL Function
        private AverageTrueRange atr;
        private bool traillingSL;
        private double traillingSLStart;

        //Base Setting 
        private int barsBack;
        private string botNameLabelBuy;
        private string botNameLabelSell;


        //Sleep Setting 
        private Bars sleepBars;
        private DateTime startSleepPeriod;

        //Statistics Setting 
        private List<List<double>> positionStat;
        private double initialBalance;
        private double drawUpMax, drawDownMax;

        protected override void OnStart()
        {
            //Init indicator
            //baseline
            baseline1P1 = Indicators.GetIndicator<CSTP_Indi_1Parametter>(MarketData.GetBars(Baseline1TimeFrame), Baseline1IndiSelectionP1, Baseline1Value1P1);
            baseline1P2 = Indicators.GetIndicator<CSTP_Indi_2Parametter>(MarketData.GetBars(Baseline1TimeFrame), Baseline1IndiSelectionP2, Baseline1Value1P2, Baseline1Value2P2);
            baseline1P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Baseline1TimeFrame), Baseline1IndiSelectionP4, Baseline1Value1P4, Baseline1Value2P4, Baseline1Value3P4, Baseline1MaTypeP4);

            //entry                                                                                        
            entry1P1 = Indicators.GetIndicator<CSTP_Indi_1Parametter>(MarketData.GetBars(Entry1TimeFrame), Entry1IndiSelectionP1, Entry1Value1P1);
            entry1P2 = Indicators.GetIndicator<CSTP_Indi_2Parametter>(MarketData.GetBars(Entry1TimeFrame), Entry1IndiSelectionP2, Entry1Value1P2, Entry1Value2P2);
            entry1P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Entry1TimeFrame), Entry1IndiSelectionP4, Entry1Value1P4, Entry1Value2P4, Entry1Value3P4, Entry1MaTypeP4);

            //confirmation
            confirmation1P1 = Indicators.GetIndicator<CSTP_Indi_1Parametter>(MarketData.GetBars(Confirmation1TimeFrame), Confirmation1IndiSelectionP1, Confirmation1Value1P1);
            confirmation1P2 = Indicators.GetIndicator<CSTP_Indi_2Parametter>(MarketData.GetBars(Confirmation1TimeFrame), Confirmation1IndiSelectionP2, Confirmation1Value1P2, Confirmation1Value2P2);
            confirmation1P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Confirmation1TimeFrame), Confirmation1IndiSelectionP4, Confirmation1Value1P4, Confirmation1Value2P4, Confirmation1Value3P4, Confirmation1MaTypeP4);

            //volume
            volume1P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Volume1TimeFrame), Volume1IndiSelectionP4, Volume1Value1P4, Volume1Value2P4, Volume1Value3P4, Volume1MaTypeP4);
            volume2P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Volume1TimeFrame), Volume2IndiSelectionP4, Volume2Value1P4, Volume2Value2P4, Volume2Value3P4, Volume2MaTypeP4);
            volume3P4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Volume1TimeFrame), Volume3IndiSelectionP4, Volume3Value1P4, Volume3Value2P4, Volume3Value3P4, Volume3MaTypeP4);

            //exit
            exit1CalculationP1 = Indicators.GetIndicator<CSTP_Indi_1Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1CalculationSelectionP1, Exit1CalculationValue1P1);
            exit1CalculationP2 = Indicators.GetIndicator<CSTP_Indi_2Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1CalculationSelectionP2, Exit1CalculationValue1P2, Exit1CalculationValue2P2);
            exit1CalculationP4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1CalculationSelectionP4, Exit1CalculationValue1P4, Exit1CalculationValue2P4, Exit1CalculationValue3P4, Exit1CalculationMaTypeP4);

            exit1TraillingP1 = Indicators.GetIndicator<CSTP_Indi_1Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1TraillingSelectionP1, Exit1TraillingValue1P1);
            exit1TraillingP2 = Indicators.GetIndicator<CSTP_Indi_2Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1TraillingSelectionP2, Exit1TraillingValue1P2, Exit1TraillingValue2P2);
            exit1TraillingP4 = Indicators.GetIndicator<CSTP_Indi_4Parametter>(MarketData.GetBars(Exit1TimeFrame), Exit1TraillingSelectionP4, Exit1TraillingValue1P4, Exit1TraillingValue2P4, Exit1TraillingValue3P4, Exit1TraillingMaTypeP4);

            //exit1Supertrend = Indicators.GetIndicator<ST_SuperTrend>(MarketData.GetBars(Exit1TimeFrame), Exit1SuperTrendPeriod, Exit1SuperTrendPeriodMulti);

            //SL Function
            atr = Indicators.AverageTrueRange(MarketData.GetBars(Exit1TimeFrame), (int)Sl, MovingAverageType.Simple);
            traillingSL = false;
            traillingSLStart = 0.0;
            //Base Setting 
            barsBack = TicksOrBarsMethod == EnumTicksOrBarsCalculation.Ticks ? 0 : 1;
            botNameLabelBuy = SymbolName + " " + "ENTER BUY " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
            botNameLabelSell = SymbolName + " " + "ENTER SELL " + TimeFrame + " " + (Bars.OpenTimes.Last(0).ToLocalTime().ToString());
            Positions.Closed += OnPositionsClosed;


            //Sleep Setting 
            sleepBars = MarketData.GetBars(SleepingPeriodTimeframe); //SleepingPeriodNbrsBars
            startSleepPeriod = Bars.OpenTimes.Last(0).ToLocalTime();

            //Statistics Setting 
            initialBalance = Account.Balance;
            positionStat = new List<List<double>>();
            drawUpMax = 0;
            drawDownMax = 0;

        }
        protected override void OnTick()
        {
            if ((Positions.FindAll(botNameLabelBuy, SymbolName).Length != 0 || Positions.FindAll(botNameLabelSell, SymbolName).Length != 0))
                GetDrawUpDrawDown();
            if (TicksOrBarsMethod == EnumTicksOrBarsCalculation.Ticks)
                Calulate();
        }
        protected override void OnBar()
        {
            if (TicksOrBarsMethod == EnumTicksOrBarsCalculation.Bars)
                Calulate();
        }
        private void Calulate()
        {
            //Check For position before enter -> possible to add parametter with multi position enter


            if (CheckEntryConditions() && Signal(TradeType.Buy) == 1)
                Open(TradeType.Buy, botNameLabelBuy);
            if (CheckEntryConditions() && Signal(TradeType.Sell) == -1)
                Open(TradeType.Sell, botNameLabelSell);

            // Exit
            if (Exit1TraillingType != EnumExit1IndiTraillingType.None)
            {
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
        }
        private int Signal(TradeType tradeType)
        {
            // all function to get Signal Enter
            //Print((GetBasline1(tradeType)) + "    " + (GetEntry1(tradeType)) + "    " + (GetConfirmation1(tradeType)) + "    " + (GetVolume1(tradeType)));
            if ((GetBasline1(tradeType) == 1) && (GetEntry1(tradeType) == 1) && (GetConfirmation1(tradeType) == 1) && (GetVolume1(tradeType) == 1))
                return 1;

            else if ((GetBasline1(tradeType) == -1) && (GetEntry1(tradeType) == -1) && (GetConfirmation1(tradeType) == -1) && (GetVolume1(tradeType) == -1))
                return -1;

            else
                return 0;
        }
        private int GetBasline1(TradeType tradeType) // all indicator adding on baseline are implemented here
        {
            var resultBaseline1P = FunctionSignalUnderOver(tradeType, baseline1P1.Result, baseline1P1.Signal, Baseline1SignalTypeP1); // FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
            var resultBaseline2P = FunctionSignalUnderOver(tradeType, baseline1P2.Result, baseline1P2.Signal, Baseline1SignalTypeP2); // FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
            var resultBaseline4P = FunctionSignalDoubleSignalUnderOver(tradeType, baseline1P4.Result, baseline1P4.Signal, Baseline1SignalTypeP4, Baseline1LevelBuyP4, Baseline1LevelSellP4); // FunctionSignalDoubleSignalUnderOver(DataSeries result, DataSeries signal, EnumSignalDoubleOverUnderType signalOverUnder, double levelBuy, double levelSell)

            // without baseline
            if (!Baseline1UseP1 && !Baseline1UseP2 && !Baseline1UseP4 && tradeType == TradeType.Buy)
                return 1;
            else if (!Baseline1UseP1 && !Baseline1UseP2 && !Baseline1UseP4 && tradeType == TradeType.Sell)
                return -1;
            // with baseline
            else if ((!Baseline1UseP1 || resultBaseline1P == 1) && (!Baseline1UseP2 || resultBaseline2P == 1) && (!Baseline1UseP4 || resultBaseline4P == 1)) // add all variable with this sentence (exemple): (!Volume1MfiUse || resultMfi == 1)
                return 1;
            else if ((!Baseline1UseP1 || resultBaseline1P == -1) && (!Baseline1UseP2 || resultBaseline2P == -1) && (!Baseline1UseP4 || resultBaseline4P == -1))
                return -1;
            else
                return 0;
        }
        private int GetEntry1(TradeType tradeType) // all indicator adding on entry are implemented here
        {
            var resultEntry1P1 = FunctionSignalCrossEnter(tradeType, entry1P1.Result, entry1P1.Signal, Entry1SignalTypeP1, double.NaN, double.NaN, Entry1LookBackMinP1); //FunctionSignalCrossEnter(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
            var resultEntry1P2 = FunctionSignalCrossEnter(tradeType, entry1P2.Result, entry1P2.Signal, Entry1SignalTypeP2, double.NaN, double.NaN, Entry1LookBackMinP2); //FunctionSignalCrossEnter(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
            var resultEntry1P4 = FunctionSignalCrossEnter(tradeType, entry1P4.Result, entry1P4.Signal, Entry1SignalTypeP4, Entry1LevelBuyP4, Entry1LevelSellP4, Entry1LookBackMinP4); //FunctionSignalCrossEnter(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
                                                                                                                                                                                      //Print(resultEntry1P4);
                                                                                                                                                                                      // without Entry
            if (!Entry1UseP1 && !Entry1UseP2 && !Entry1UseP4 && tradeType == TradeType.Buy)
                return 1;
            else if (!Entry1UseP1 && !Entry1UseP2 && !Entry1UseP4 && tradeType == TradeType.Sell)
                return -1;
            // with Entry
            else if ((!Entry1UseP1 || resultEntry1P1 == 1) && (!Entry1UseP2 || resultEntry1P2 == 1) && (!Entry1UseP4 || resultEntry1P4 == 1)) // add all variable with this sentence (exemple): (!Entry1RsiUse || resultRsi == 1)
                return 1;
            else if ((!Entry1UseP1 || resultEntry1P1 == -1) && (!Entry1UseP2 || resultEntry1P2 == -1) && (!Entry1UseP4 || resultEntry1P4 == -1))
                return -1;
            else
                return 0;
        }

        private int GetConfirmation1(TradeType tradeType) // all indicator adding on confirmation are implemented here
        {
            var resultConfirmation1P1 = FunctionSignalUnderOver(tradeType, confirmation1P1.Result, confirmation1P1.Signal, Confirmation1SignalTypeP1); // FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
            var resultConfirmation1P2 = FunctionSignalUnderOver(tradeType, confirmation1P2.Result, confirmation1P2.Signal, Confirmation1SignalTypeP2); // FunctionSignalUnderOver(DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
            var resultConfirmation1P4 = FunctionSignalDoubleSignalUnderOver(tradeType, confirmation1P4.Result, confirmation1P4.Signal, Confirmation1SignalTypeP4, Confirmation1LevelBuyP4, Confirmation1LevelSellP4); //FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)

            // without Condirmation
            if (!Confirmation1UseP1 && !Confirmation1UseP2 && !Confirmation1UseP4 && tradeType == TradeType.Buy)
                return 1;
            else if (!Confirmation1UseP1 && !Confirmation1UseP2 && !Confirmation1UseP4 && tradeType == TradeType.Sell) // without Condirmation
                return -1;
            // with Condirmation
            else if ((!Confirmation1UseP1 || resultConfirmation1P1 == 1) && (!Confirmation1UseP2 || resultConfirmation1P2 == 1) && (!Confirmation1UseP4 || resultConfirmation1P4 == 1)) // add all variable with this sentence : (!Confirmation1MacdUse || resultMacd == 1)
                return 1;
            else if ((!Confirmation1UseP1 || resultConfirmation1P1 == -1) && (!Confirmation1UseP2 || resultConfirmation1P2 == -1) && (!Confirmation1UseP4 || resultConfirmation1P4 == -1))
                return -1;
            else
                return 0;
        }

        private int GetVolume1(TradeType tradeType) // all indicator adding on volume are implemented here
        {
            var resultVolume1P4 = FunctionSignalDoubleSignalUnderOver(tradeType, volume1P4.Result, volume1P4.Signal, Volume1SignalTypeP4, Volume1LevelBuyP4, Volume1LevelSellP4); //FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
            var resultVolume2P4 = FunctionSignalDoubleSignalUnderOver(tradeType, volume2P4.Result, volume2P4.Signal, Volume2SignalTypeP4, Volume2LevelBuyP4, Volume2LevelSellP4); //FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
            var resultVolume3P4 = FunctionSignalDoubleSignalUnderOver(tradeType, volume3P4.Result, volume3P4.Signal, Volume3SignalTypeP4, Volume3LevelBuyP4, Volume3LevelSellP4); //FunctionSignalCross(DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)

            // without Volume
            if (!Volume1UseP4 && !Volume2UseP4 && !Volume3UseP4 && tradeType == TradeType.Buy)
                return 1;
            else if (!Volume1UseP4 && !Volume2UseP4 && !Volume3UseP4 && tradeType == TradeType.Sell) // without Volume
                return -1;
            // with Volume
            else if ((!Volume1UseP4 || resultVolume1P4 == 1) && (!Volume2UseP4 || resultVolume2P4 == 1) && (!Volume3UseP4 || resultVolume3P4 == 1))  // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                return 1;
            else if ((!Volume1UseP4 || resultVolume1P4 == -1) && (!Volume2UseP4 || resultVolume2P4 == -1) && (!Volume3UseP4 || resultVolume3P4 == -1))
                return -1;
            else
                return 0;
        }

        private int GetExit1(TradeType tradeType) // all indicator adding on exit are implemented here
        {
            traillingSL = traillingSL ? traillingSL : GetTraillingSL(tradeType);
            if (traillingSL)
            {
                if (Exit1TraillingType == EnumExit1IndiTraillingType.Indicator_1_Parametter)
                {
                    var resultExit1P1 = FunctionCross(exit1TraillingP1.Result, exit1TraillingP1.Signal);

                    //Print(Bars.OpenTimes.Last(0).ToLocalTime() + "  " + traillingSL + " " + GetTraillingSL(tradeType) + "   " + resultExit1P1);

                    if ((resultExit1P1 == 1) && tradeType == TradeType.Sell) // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                        return 1;
                    else if ((resultExit1P1 == -1) && tradeType == TradeType.Buy)
                        return -1;
                    else
                        return 0;
                }
                if (Exit1TraillingType == EnumExit1IndiTraillingType.Indicator_2_Parametters)
                {
                    var resultExit1P2 = FunctionCross(exit1TraillingP2.Result, exit1TraillingP2.Signal);
                    if ((resultExit1P2 == 1) && tradeType == TradeType.Sell) // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                        return 1;
                    else if ((resultExit1P2 == -1) && tradeType == TradeType.Buy)
                        return -1;
                    else
                        return 0;
                }
                if (Exit1TraillingType == EnumExit1IndiTraillingType.Indicator_4_Parametters)
                {
                    var resultExit1P2 = FunctionCross(exit1TraillingP4.Result, exit1TraillingP4.Signal);
                    if ((resultExit1P2 == 1) && tradeType == TradeType.Sell) // add all variable with this sentence : (!Volume1MfiUse || resultMfi == 1)
                        return 1;
                    else if ((resultExit1P2 == -1) && tradeType == TradeType.Buy)
                        return -1;
                    else
                        return 0;
                }
                else
                    return 0;
            }
            else // Get SL() -> Fixed SL or Swing Sl
                return 0;

        }

        // FUNCTION FOR SIGNAL -> need backtest and finding if other conditions need to be add
        // Confirmation Type Function
        private int FunctionSignalUnderOver(TradeType tradeType, DataSeries result, DataSeries Signal, EnumSignalOverUnderType signalOverUnder)
        {
            if (signalOverUnder == EnumSignalOverUnderType.Over_Positive_Under_Negative)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > Signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < Signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else // Over_Negative_Under_Positive
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < Signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > Signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
        }
        // Confirmation Type Function
        private int FunctionSignalDoubleSignalUnderOver(TradeType tradeType, DataSeries result, DataSeries signal, EnumSignalDoubleOverUnderType signalOverUnder, double levelBuy, double levelSell)
        {
            if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_Positive_Or_Under_Negative)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_Negative_Or_Under_Positive)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Signal_Positive_Or_Under_Negative)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Signal_Negative_Or_Under_Positive)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_Valid_Or_Under_Invalid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_Invalid_Or_Under_Valid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Signal_Valid_Or_Under_Invalid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Signal_Invalid_Or_Under_Valid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Positive_Or_Under_Negative)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > levelBuy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < levelSell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Negative_Or_Under_Positive)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < levelBuy && result.Last(barsBack) < signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > levelSell && result.Last(barsBack) > signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Under_Signal_Positive_Or_Under_Level_And_Over_Signal_Negative)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > levelBuy && result.Last(barsBack) < signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < levelSell && result.Last(barsBack) > signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Under_Signal_Negative_Or_Under_Level_And_Over_Signal_Positive)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < levelBuy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > levelSell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Valid_Or_Under_Invalid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) > levelBuy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) > levelSell && result.Last(barsBack) > signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalDoubleOverUnderType.Over_Level_And_Signal_Invalid_Or_Under_Valid)
            {
                if (tradeType == TradeType.Buy && result.Last(barsBack) < levelBuy && result.Last(barsBack) < signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && result.Last(barsBack) < levelSell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else
                return 0;
        }
        // Real Enter Type Function -> min and max for having possibility to find range period with Multiple signal (Maybe need a special function) it work but it's not good When I think about it. (result.Minimum(lookBack + 1) <= signal.Last(2) && result.Last(1) > signal.Last(1))
        private int FunctionSignalCrossEnter(TradeType tradeType, DataSeries result, DataSeries signal, EnumSignalCrossType signalOverUnder, double levelBuy, double levelSell, int lookBack)
        {
            if (signalOverUnder == EnumSignalCrossType.Cross_On_Level)
            {
                //Print(result.Minimum((lookBack + barsBack)) + "             " + Bars.OpenTimes.Last(0));
                if (tradeType == TradeType.Buy && GetMinimum(result, (lookBack + barsBack)) <= levelBuy && result.Last(barsBack) > levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && GetMaximum(result, (lookBack + barsBack)) >= levelSell && result.Last(barsBack) < levelSell)
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalCrossType.Cross_On_Signal)
            {
                if (tradeType == TradeType.Buy && GetSignalLookbackBuy(result, signal, lookBack + barsBack) && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && GetSignalLookbackSell(result, signal, lookBack + barsBack) && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else if (signalOverUnder == EnumSignalCrossType.Cross_On_Level_And_Signal)
            {
                if (tradeType == TradeType.Buy && GetMinimum(result, (lookBack + barsBack)) <= levelBuy && result.Last(barsBack) > levelBuy && result.Last(barsBack) > signal.Last(barsBack))
                    return 1;
                else if (tradeType == TradeType.Sell && GetMaximum(result, (lookBack + barsBack)) >= levelSell && result.Last(barsBack) < levelSell && result.Last(barsBack) < signal.Last(barsBack))
                    return -1;
                else
                    return 0;
            }
            else // Cross_On_Signal_And_Level
            {
                if (tradeType == TradeType.Buy && GetSignalLookbackBuy(result, signal, lookBack + barsBack) && result.Last(barsBack) > signal.Last(barsBack) && result.Last(barsBack) < levelBuy)
                    return 1;
                else if (tradeType == TradeType.Sell && GetSignalLookbackSell(result, signal, lookBack + barsBack) && result.Last(barsBack) < signal.Last(barsBack) && result.Last(barsBack) > levelSell)
                    return -1;
                else
                    return 0;
            }
        }
        private bool GetSignalLookbackBuy(DataSeries result, DataSeries signal, int lookBack)  // -> test if more faster than result.Maximum(lookBack +1)
        {
            for (int i = 1; i <= lookBack; i++)
            {
                if (result.Last(i) < signal.Last(i))
                    return true;

            }
            return false;
        }
        private bool GetSignalLookbackSell(DataSeries result, DataSeries signal, int lookBack)  // -> test if more faster than result.Maximum(lookBack +1)
        {
            for (int i = 1; i <= lookBack; i++)
            {
                if (result.Last(i) > signal.Last(i))
                    return true;
            }
            return false;
        }
        private double GetMaximum(DataSeries result, int lookBack)  // -> test if more faster than result.Maximum(lookBack +1)
        {
            var max = double.MinValue;
            for (int i = 1; i <= lookBack; i++)
                max = Math.Max(result.Last(i), max);
            return max;
        }
        private double GetMinimum(DataSeries result, int lookBack)  // -> test if more faster than result.Minimum(lookBack +1)
        {
            var min = double.MaxValue;
            for (int i = 1; i <= lookBack; i++)
                min = Math.Min(result.Last(i), min);
            return min;
        }

        // Exit Type Function
        private int FunctionCross(DataSeries result, DataSeries signal)
        {
            //Print(Bars.OpenTimes.Last(0).ToLocalTime() + "  " + result.Last(2) + "  <= " + signal.Last(2) + "  &&  " + result.Last(1) + " > " + signal.Last(1) + "   " + (result.Last(2) <= signal.Last(2)) + "   " + (result.Last(1) > signal.Last(1)));

            if (result.Last(2) <= signal.Last(2) && result.Last(1) > signal.Last(1))
                return 1;
            else if (result.Last(2) >= signal.Last(2) && result.Last(1) < signal.Last(1))
                return -1;
            else
                return 0;
        }

        // FUNCTION BASE ROBOT
        // Function time to trade, days, and spred

        // Function Sl 
        private double GetSL(TradeType tradeType)
        {
            var result = 0.0;

            if (Exit1IndiType == EnumExit1IndiType.Indicator_1_Parametter)
            {
                if (tradeType == TradeType.Buy)
                    result = (Bars.ClosePrices.Last(1) - exit1CalculationP1.Result.Last(1)) / Symbol.PipSize;
                else
                    result = (exit1CalculationP1.Result.Last(1) - Bars.ClosePrices.Last(1)) / Symbol.PipSize;
            }
            else if (Exit1IndiType == EnumExit1IndiType.Indicator_2_Parametters)
            {
                if (tradeType == TradeType.Buy)
                    result = (Bars.ClosePrices.Last(1) - exit1CalculationP2.Result.Last(1)) / Symbol.PipSize;
                else
                    result = (exit1CalculationP2.Result.Last(1) - Bars.ClosePrices.Last(1)) / Symbol.PipSize;
            }

            else if (Exit1IndiType == EnumExit1IndiType.Indicator_4_Parametters)
            {
                if (tradeType == TradeType.Buy)
                    result = (Bars.ClosePrices.Last(1) - exit1CalculationP4.Result.Last(1)) / Symbol.PipSize;
                else
                    result = (exit1CalculationP4.Result.Last(1) - Bars.ClosePrices.Last(1)) / Symbol.PipSize;
            }

            else if (Exit1IndiType == EnumExit1IndiType.Swing)
            {
                if (tradeType == TradeType.Buy)
                    result = (Bars.ClosePrices.Last(1) - Bars.ClosePrices.Minimum((int)Sl)) / Symbol.PipSize;
                else
                    result = (Bars.ClosePrices.Maximum((int)Sl) - Bars.ClosePrices.Last(1)) / Symbol.PipSize;
            }
            else if (Exit1IndiType == EnumExit1IndiType.Sl_TP_Fixed)
            {
                result = (Sl);
            }

            if (result <= 1 || Exit1IndiType == EnumExit1IndiType.Atr)
            {
                if (tradeType == TradeType.Buy)
                    result = (atr.Result.Last(1) * SlMultiplier) / Symbol.PipSize;
                else
                    result = (atr.Result.Last(1) * SlMultiplier) / Symbol.PipSize;
            }
            return result;
        }

        private bool GetTraillingSL(TradeType tradeType)
        {

            if (tradeType == TradeType.Buy && Symbol.Bid > traillingSLStart)
                return true;
            else if (tradeType == TradeType.Sell && Symbol.Ask < traillingSLStart)
                return true;
            else
                return false;


        }
        private void Open(TradeType tradeType, string label)
        {
            var tradeAmount = 0.0;
            var sl = GetSL(tradeType);
            var tp = (sl + ComissionInPips) * RRR;


            if (SelectionRiskType == EnumSelectionRiskType.Fixed_Lot)
                tradeAmount = Symbol.QuantityToVolumeInUnits(RiskValue);
            else if (SelectionRiskType == EnumSelectionRiskType.Fixed_Volume)
                tradeAmount = Symbol.NormalizeVolumeInUnits(RiskValue);
            else if (SelectionRiskType == EnumSelectionRiskType.Perentage_Risk)
                tradeAmount = Symbol.VolumeForFixedRisk(Account.Balance * RiskValue / 100, sl, RoundingMode.Down);

            TradeResult result = ExecuteMarketOrder(tradeType, SymbolName, tradeAmount, label, sl, tp == 0 ? null : tp, null, Exit1TraillingType == EnumExit1IndiTraillingType.Standard_Trailling_Stop);

            if (Exit1TraillingType != EnumExit1IndiTraillingType.None)
                traillingSLStart = tradeType == TradeType.Buy ? Bars.ClosePrices.Last(1) + ((sl * Symbol.PipSize) * Exit1TraillingStart) : Bars.ClosePrices.Last(1) - ((sl * Symbol.PipSize) * Exit1TraillingStart);

            // Print(traillingSLStart);
            // Possible Pending Order for a waiting retest of signal
            //TradeResult resultPending = PlaceLimitOrder(tradeType, SymbolName, tradeAmount, Bars.ClosePrices.Last(0), label, sl, null);

        }
        //Function Closing trade on Signal SL
        private void Close(TradeType tradeType, string label)
        {
            var lastPosition = Positions.FindAll(label, SymbolName, tradeType).Last();
            foreach (var position in Positions.FindAll(label, SymbolName, tradeType))
                ClosePosition(position);
            traillingSL = false;
            traillingSLStart = double.NaN;
            //ReinitialisationOnClose(lastPosition); // Check if work without 2 Reinitialisation on > ctrader 4.8.30 previous need
        }
        //Function Closing trade on Base SL
        private void OnPositionsClosed(PositionClosedEventArgs args)
        {
            traillingSL = false;
            traillingSLStart = double.NaN;
            ReinitialisationOnClose(args.Position);
        }
        //Statistics Functions
        private void GetDrawUpDrawDown()
        {
            var positionSymbol = Positions.FindAll(botNameLabelBuy).Length + Positions.FindAll(botNameLabelSell).Length;
            var netProfit = Positions.Where(x => x.TradeType == TradeType.Buy || x.TradeType == TradeType.Sell).Sum(p => p.NetProfit);
            while (netProfit > drawUpMax)
                drawUpMax = netProfit;
            while (netProfit < drawDownMax)
                drawDownMax = netProfit;
        }
        private bool CheckEntryConditions()
        {
            bool chkDayofWeek = ((Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Monday && Mon == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Tuesday && Tue == true)
            || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Wednesday && Wed == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Thursday && Thu == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Friday && Fri == true)
            || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Saturday && Sat == true) || (Bars.OpenTimes.Last(0).ToLocalTime().DayOfWeek == DayOfWeek.Sunday && Sun == true));

            if ((Bars.OpenTimes.Last(0).ToLocalTime().Hour >= StartHour) && (Bars.OpenTimes.Last(0).ToLocalTime().Hour <= EndHour) && (chkDayofWeek == true) && (Symbol.Spread < (SpreadMax * Symbol.PipSize)) && !OnSleepPeriod() && (Positions.FindAll(botNameLabelBuy, SymbolName).Length == 0 && Positions.FindAll(botNameLabelSell, SymbolName).Length == 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool OnSleepPeriod()
        {
            TimeSpan nbrsBarsAfterLoss = sleepBars.OpenTimes.Last(0) - sleepBars.OpenTimes.Last(1);
            if (SleepingStrategie != EnumSleepingStrategie.None)
            {
                if (((startSleepPeriod + (nbrsBarsAfterLoss * SleepingPeriodNbrsBars) <= Bars.OpenTimes.Last(0).ToLocalTime()) == true))
                    return false;
                else
                    return true;
            }
            else
                return false;
        }
        private void ReinitialisationOnClose(Position position)
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
                accountBalance = Account.Balance;
                netProfit = position.NetProfit;
                commission = position.Commissions;
                profitFactor = position.TradeType == TradeType.Buy ? ((position.CurrentPrice - position.EntryPrice) / (position.EntryPrice - position.StopLoss.Value)) : ((position.EntryPrice - position.CurrentPrice) / (position.StopLoss.Value - position.EntryPrice));
                swap = position.Swap;
                drawdown = drawDownMax / Account.Balance * 100;
                drawup = drawUpMax / Account.Balance * 100;
                consecutiveWin = netProfit > 0 ? 1 : 0;
                consecutiveLoss = netProfit < 0 ? 1 : 0;

                positionStat.Add(new List<double> { positionStat.Count, accountBalance, netProfit, profitFactor, commission, swap, drawdown, drawup, consecutiveWin, consecutiveLoss });
                // average of all stat at the end or on close position ?
            }

            else
            {
                var lastPosition = positionStat.Count - 1;

                accountBalance = Account.Balance;
                netProfit = position.NetProfit;
                commission = position.Commissions;
                profitFactor = position.TradeType == TradeType.Buy ? ((position.CurrentPrice - position.EntryPrice) / (position.EntryPrice - position.StopLoss.Value)) : ((position.EntryPrice - position.CurrentPrice) / (position.StopLoss.Value - position.EntryPrice));
                swap = position.Swap;
                drawdown = drawDownMax / Account.Balance * 100;
                drawup = drawUpMax / Account.Balance * 100;
                consecutiveWin = netProfit > 0 ? positionStat[lastPosition][8] + 1 : 0;
                consecutiveLoss = netProfit < 0 ? positionStat[lastPosition][9] + 1 : 0;

                positionStat.Add(new List<double> { positionStat.Count, accountBalance, netProfit, profitFactor, commission, swap, drawdown, drawup, consecutiveWin, consecutiveLoss });
            }
            initialBalance = Account.Balance;
            drawUpMax = 0;
            drawDownMax = 0;

            //Sleep initialisation
            if (SleepingStrategie == EnumSleepingStrategie.On_Win && netProfit > 0.00)
            {
                startSleepPeriod = Bars.OpenTimes.Last(0).ToLocalTime();
            }
            else if (SleepingStrategie == EnumSleepingStrategie.On_Loss && netProfit < 0.00)
            {
                startSleepPeriod = Bars.OpenTimes.Last(0).ToLocalTime();
            }
            else if (SleepingStrategie == EnumSleepingStrategie.On_All)
            {
                startSleepPeriod = Bars.OpenTimes.Last(0).ToLocalTime();
            }
            else
            {
                startSleepPeriod = sleepBars.OpenTimes.Last((SleepingPeriodNbrsBars + 1)).ToLocalTime();
            }
        }

        protected override void OnStop()
        {
            if (ExportData)
            {
                string path = DataDir;
                string pathAndFilename = Path.Combine(path, "Ctrader Strategy Tester", Symbol.Name);

                if (!Directory.Exists(pathAndFilename))
                    Directory.CreateDirectory(pathAndFilename);
                string filename = "";
                switch (FileName)
                {
                    case (EnumFileName.Account_Balance):
                        filename = "Account_Balance " + positionStat[positionStat.Count - 1][1].ToString() + "$";
                        break;
                    case (EnumFileName.Profit_Factor):
                        filename = "Profit_Factor " + positionStat[positionStat.Count - 1][3].ToString() + "$";
                        break;
                    case (EnumFileName.Drawdown):
                        filename = "Drawdown " + positionStat[positionStat.Count - 1][6].ToString() + "$";
                        break;
                    case (EnumFileName.Drawup):
                        filename = "Drawup " + positionStat[positionStat.Count - 1][7].ToString() + "$";
                        break;
                    case (EnumFileName.Consecutive_Win):
                        filename = "Consecutive_Win " + positionStat[positionStat.Count - 1][8].ToString() + " Max";
                        break;
                    case (EnumFileName.Consecutive_Loss):
                        filename = "Consecutive_Loss " + positionStat[positionStat.Count - 1][9].ToString() + " Max";
                        break;
                }

                int i = 0;
                string pathAndFilename2 = Path.Combine(pathAndFilename, filename + " " + " pass " + i + ".csv");

                while (File.Exists(pathAndFilename2))
                {
                    pathAndFilename2 = Path.Combine(pathAndFilename, filename + " " + " pass " + i + ".csv");
                    i++;
                }
                File.WriteAllText(pathAndFilename2, "Position" + ";" + "Account Balance" + ";" + "Profit" + ";" + "Profit Factor" + ";" + "Commission" + ";" + "Swap" + ";" + "Drawdown" + ";" + "Drawup" + ";" + "ConsecutiveWin" + ";" + "ConsecutiveLoss" + ";" + "totalWin" + ";" + "totalLoss" + ";" + "winRate" + "\r\n");
                // Print on log
                double averageaccountBalance = 0.0;
                double averagenetProfit = 0.0;
                double averageprofitFactor = 0.0;
                double averagecommission = 0.0;
                double averageswap = 0.0;
                double averagedrawdown = 0.0;
                double averagedrawup = 0.0;
                double averageconsecutiveWin = 0.0;
                double averageconsecutiveLoss = 0.0;

                double maxaccountBalance = double.MinValue;
                double maxagenetProfit = double.MinValue;
                double maxprofitFactor = double.MinValue;
                double maxcommission = double.MinValue;
                double maxswap = double.MinValue;
                double maxdrawdown = double.MinValue;
                double maxdrawup = double.MinValue;
                double maxconsecutiveWin = double.MinValue;
                double maxconsecutiveLoss = double.MinValue;

                double minaccountBalance = double.MaxValue;
                double minagenetProfit = double.MaxValue;
                double minprofitFactor = double.MaxValue;
                double mincommission = double.MaxValue;
                double minswap = double.MaxValue;
                double mindrawdown = double.MaxValue;
                double mindrawup = double.MaxValue;
                double minconsecutiveWin = double.MaxValue;
                double minconsecutiveLoss = double.MaxValue;

                double totalWin = 0;
                double totalLoss = 0;
                double winRate = 0;

                for (i = 0; i < positionStat.Count; i++)
                {
                    totalWin = positionStat[i][2] > 0.000 ? totalWin + 1 : totalWin;
                    totalLoss = positionStat[i][2] < 0.000 ? totalLoss + 1 : totalLoss;
                    winRate = (totalWin / (totalWin + totalLoss)) * 100;

                    File.AppendAllText(pathAndFilename2, (positionStat[i][0] + 1) + ";" + positionStat[i][1].ToString("F2") + ";" + positionStat[i][2].ToString("F2") + ";" + positionStat[i][3].ToString("F2") + ";" + positionStat[i][4] + ";" + positionStat[i][5] + ";" + positionStat[i][6].ToString("F2") + ";" + positionStat[i][7].ToString("F2") + ";" + positionStat[i][8] + ";" + positionStat[i][9] + ";" + totalWin + ";" + totalLoss + ";" + winRate.ToString("F2") + "\r\n");

                    averageaccountBalance += positionStat[i][1];
                    averagenetProfit += positionStat[i][2];
                    averageprofitFactor += positionStat[i][3];
                    averagecommission += positionStat[i][4];
                    averageswap += positionStat[i][5];
                    averagedrawdown += positionStat[i][6];
                    averagedrawup += positionStat[i][7];
                    averageconsecutiveWin += positionStat[i][8];
                    averageconsecutiveLoss += positionStat[i][9];

                    maxaccountBalance = Math.Max(positionStat[i][1], maxaccountBalance);
                    maxagenetProfit = Math.Max(positionStat[i][2], maxagenetProfit);
                    maxprofitFactor = Math.Max(positionStat[i][3], maxprofitFactor);
                    maxcommission = Math.Max(positionStat[i][4], maxcommission);
                    maxswap = Math.Max(positionStat[i][5], maxswap);
                    maxdrawdown = Math.Max(positionStat[i][6], maxdrawdown);
                    maxdrawup = Math.Max(positionStat[i][7], maxdrawup);
                    maxconsecutiveWin = Math.Max(positionStat[i][8], maxconsecutiveWin);
                    maxconsecutiveLoss = Math.Max(positionStat[i][9], maxconsecutiveLoss);

                    minaccountBalance = Math.Min(positionStat[i][1], minaccountBalance);
                    minagenetProfit = Math.Min(positionStat[i][2], minagenetProfit);
                    minprofitFactor = Math.Min(positionStat[i][3], minprofitFactor);
                    mincommission = Math.Min(positionStat[i][4], mincommission);
                    minswap = Math.Min(positionStat[i][5], minswap);
                    mindrawdown = Math.Min(positionStat[i][6], mindrawdown);
                    mindrawup = Math.Min(positionStat[i][7], mindrawup);
                    minconsecutiveWin = Math.Min(positionStat[i][8], minconsecutiveWin);
                    minconsecutiveLoss = Math.Min(positionStat[i][9], minconsecutiveLoss);
                }

                double res0 = averageaccountBalance / positionStat.Count;
                double res1 = averagenetProfit / positionStat.Count;
                double res2 = averageprofitFactor / positionStat.Count;
                double res3 = averagecommission / positionStat.Count;
                double res4 = averageswap / positionStat.Count;
                double res5 = averagedrawdown / positionStat.Count;
                double res6 = averagedrawup / positionStat.Count;
                double res7 = averageconsecutiveWin / positionStat.Count;
                double res8 = averageconsecutiveLoss / positionStat.Count;

                File.AppendAllText(pathAndFilename2, "\r\n");
                File.AppendAllText(pathAndFilename2, "Average" + ";" + res0.ToString("F2") + ";" + res1.ToString("F2") + ";" + res2.ToString("F2") + ";" + res3.ToString("F2") + ";" + res4.ToString("F2") + ";" + res5.ToString("F2") + ";" + res6.ToString("F2") + ";" + res7.ToString("F2") + ";" + res8.ToString("F2") + "\r\n\r\n");
                File.AppendAllText(pathAndFilename2, "Max" + ";" + maxaccountBalance.ToString("F2") + ";" + maxagenetProfit.ToString("F2") + ";" + maxprofitFactor.ToString("F2") + ";" + maxcommission.ToString("F2") + ";" + maxswap.ToString("F2") + ";" + maxdrawdown.ToString("F2") + ";" + maxdrawup.ToString("F2") + ";" + maxconsecutiveWin.ToString("F2") + ";" + maxconsecutiveLoss.ToString("F2") + "\r\n\r\n");
                File.AppendAllText(pathAndFilename2, "Min" + ";" + minaccountBalance.ToString("F2") + ";" + minagenetProfit.ToString("F2") + ";" + minprofitFactor.ToString("F2") + ";" + mincommission.ToString("F2") + ";" + minswap.ToString("F2") + ";" + mindrawdown.ToString("F2") + ";" + mindrawup.ToString("F2") + ";" + minconsecutiveWin.ToString("F2") + ";" + minconsecutiveLoss.ToString("F2") + "\r\n\r\n");
                File.AppendAllText(pathAndFilename2, SymbolName + " " + Chart.TimeFrame.ToString() + "\r\n\r\n");


                var parameters = this.GetType().GetProperties()
                                .Where(p => p.IsDefined(typeof(ParameterAttribute), false));

                StringBuilder content = new StringBuilder();

                foreach (var parameter in parameters)
                {
                    var value = parameter.GetValue(this);
                    content.AppendLine($"{parameter.Name}: {value}");
                }

                File.AppendAllText(pathAndFilename2, content.ToString());
                Print("average account Balance :   " + res0 + " || average net Profit :    " + res1 + " || average profit Factor  :  " + res2 + " || average commission :  " + res3 + " || average swap :  " + res4 + " || average drawdown :  " + res5 + " || average drawup :  " + res6 + " || average consecutive Win :  " + res7 + " || average consecutive Loss :  " + res8);

            }  // Export to csv need Function need idea for good using (no csv and output result on chart) ?
        }
        protected override double GetFitness(GetFitnessArgs args)
        {
            // Ratio Win vs loss
            double losing = args.LosingTrades == 0 ? 1 : args.LosingTrades;
            double win = args.WinningTrades == 0 ? 1 : args.WinningTrades;

            double ratioWin = 1;
            double ratioProfiNet = 1;
            double ratioTpSl = 1;
            double profitFactor = 1;
            double prom = 1;
            double karatio = 1;
            double drawdownEquity = 1;
            double drawdownBalance = 1;

            if (RatioWin)
                ratioWin = args.TotalTrades == 0 ? 0 : win / losing;

            if (RatioProfiNet)
                ratioProfiNet = args.NetProfit / initialBalance * 100;

            if (RatioTpSl)
                ratioTpSl = args.TotalTrades == 0 ? 0 : Exit1IndiType == EnumExit1IndiType.Sl_TP_Fixed
                                                     || Exit1IndiType == EnumExit1IndiType.Atr
                                                     || Exit1IndiType == EnumExit1IndiType.Swing ? RRR : args.History.Max(x => x.NetProfit) / Math.Abs(args.History.Min(x => x.NetProfit));

            if (ProfitFactor)
                profitFactor = args.TotalTrades == 0 ? 1 : args.ProfitFactor == 0 ? 1 : args.ProfitFactor;

            if (DrawdownEquityMax)
            {
                drawdownEquity = args.MaxEquityDrawdownPercentages < 1 ? 5 :
                           args.MaxEquityDrawdownPercentages < 2 ? 3.8 :
                           args.MaxEquityDrawdownPercentages < 3 ? 2.9 :
                           args.MaxEquityDrawdownPercentages < 4 ? 2.22 :
                           args.MaxEquityDrawdownPercentages < 5 ? 1.7 :
                           args.MaxEquityDrawdownPercentages < 6 ? 1.43 :
                           args.MaxEquityDrawdownPercentages < 7 ? 1.26 :
                           args.MaxEquityDrawdownPercentages < 8 ? 1.16 :
                           args.MaxEquityDrawdownPercentages < 9 ? 1.10 :
                           args.MaxEquityDrawdownPercentages < 10 ? 1.06 :
                           args.MaxEquityDrawdownPercentages >= 10 ? 1 : 1;
            }
            if (DrawdownBalanceMax)
            {
                drawdownBalance = args.MaxBalanceDrawdownPercentages < 1 ? 5 :
                         args.MaxBalanceDrawdownPercentages < 2 ? 3.8 :
                         args.MaxBalanceDrawdownPercentages < 3 ? 2.9 :
                         args.MaxBalanceDrawdownPercentages < 4 ? 2.22 :
                         args.MaxBalanceDrawdownPercentages < 5 ? 1.7 :
                         args.MaxBalanceDrawdownPercentages < 6 ? 1.43 :
                         args.MaxBalanceDrawdownPercentages < 7 ? 1.26 :
                         args.MaxBalanceDrawdownPercentages < 8 ? 1.16 :
                         args.MaxBalanceDrawdownPercentages < 9 ? 1.10 :
                         args.MaxBalanceDrawdownPercentages < 10 ? 1.06 :
                         args.MaxBalanceDrawdownPercentages >= 10 ? 1 : 1;
            }

            // Strategie PROM
            // check for errors / wrong values
            if (Kratio)
            {
                double balance = initialBalance;
                double log_balance = 0;
                int size = History.Count + 1;
                double[] log_profit = new double[size];
                log_profit[0] = 0.0;
                int count = 1;

                double x_sum = 0;
                double y_sum = 0;

                foreach (var ticket in History)
                {
                    log_balance += Math.Log((balance + ticket.NetProfit) / balance);
                    balance += ticket.NetProfit;
                    log_profit[count] = log_balance;

                    x_sum += count;
                    y_sum += log_balance;
                    count++;
                }

                double x_mean = x_sum / size;
                double y_mean = y_sum / size;

                double slope_numerator = 0;
                double slope_denominator = 0;
                double x_sqr = 0;
                double y_sqr = 0;

                for (int i = 0; i < size; i++)
                {
                    slope_numerator += (i - x_mean) * (log_profit[i] - y_mean);
                    slope_denominator += Math.Pow(i - x_mean, 2);

                    y_sqr += Math.Pow(log_profit[i] - y_mean, 2);
                    x_sqr += Math.Pow(i - x_mean, 2);
                }

                double slope = slope_numerator / slope_denominator;
                double std_err = Math.Sqrt((y_sqr - (Math.Pow(slope_numerator, 2) / x_sqr)) / ((size - 2) * x_sqr));

                karatio = args.TotalTrades == 0 ? 0 : slope / std_err;
            }

            if (PROM)
            {
                double WinTradeTotal = 0.0;
                double LossTradeTotal = 0.0;
                int WinTradeCount = 0;
                int LossTradeCount = 0;
                double AvgWinningTrade = 0.0;
                double AvgLosingTrade = 0.0;

                foreach (HistoricalTrade trade in History)
                {
                    if (trade.NetProfit > 0.01)
                    {
                        WinTradeTotal = WinTradeTotal + trade.NetProfit;
                        WinTradeCount++;
                    }
                    else
                    {
                        LossTradeTotal = LossTradeTotal + trade.NetProfit;
                        LossTradeCount++;
                    }
                }

                AvgWinningTrade = Math.Round(WinTradeTotal / WinTradeCount, 2);
                AvgLosingTrade = (Math.Round(LossTradeTotal / LossTradeCount, 2) * -1);

                double sqrtWins = Math.Sqrt(win);
                double sqrtLosses = Math.Sqrt(losing);

                prom = args.TotalTrades == 0 ? 0 : (((AvgWinningTrade * (win - sqrtWins)) - (AvgLosingTrade * (losing - sqrtLosses))) / initialBalance) * 100;
            }

            //  if (args.NetProfit >= 0)
            Print(ratioWin + "     " + ratioProfiNet + "     " + ratioTpSl + "     " + profitFactor + "     " + prom + "     " + drawdownEquity + "     " + drawdownBalance + "     " + karatio);
            return (ratioWin * ratioProfiNet * ratioTpSl * profitFactor * prom * drawdownEquity * drawdownBalance * karatio);
            // else
            //   return (args.Equity - initialBalance);
        }
    }
}
