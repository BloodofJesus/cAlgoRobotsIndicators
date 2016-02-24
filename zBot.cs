using System;
using System.Linq;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;
using cAlgo.Indicators;

namespace cAlgo
{
    [Robot(TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class zBot : Robot
    {
        [Parameter(DefaultValue = 0.0)]
        public double Parameter { get; set; }

        [Parameter("MA Type")]
        public MovingAverageType MAType { get; set; }

        [Parameter("Source")]
        public DataSeries SourceSeries { get; set; }

        [Parameter("Slow Periods", DefaultValue = 50)]
        public int SlowPeriods { get; set; }

        [Parameter("Fast Periods", DefaultValue = 60)]
        public int FastPeriods { get; set; }

        [Parameter("Quantity (Lots)", DefaultValue = 1, MinValue = 0.01, Step = 0.01)]
        public double Quantity { get; set; }

        private MovingAverage slowMa;
        private MovingAverage fastMa;
        private const string label = "Sample Trend cBot";

        private double open, close, low, high;

        Symbol Symbol1;

        protected override void OnStart()
        {
            // Put your initialization logic here
            fastMa = Indicators.MovingAverage(SourceSeries, FastPeriods, MAType);
            slowMa = Indicators.MovingAverage(SourceSeries, SlowPeriods, MAType);
        }

        protected override void OnBar()
        {

            MarketSeries data = MarketData.GetSeries(Symbol, TimeFrame.Minute15);
            DataSeries series = data.Close;
            int index = series.Count - 1;

            close = data.Close[index];
            high = data.High[index];
            low = data.Low[index];
            open = data.Open[index];

            //    Int32 opentime = (Int32)(data.OpenTime[index].Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            var longPosition = Positions.Find(label, Symbol, TradeType.Buy);
            var shortPosition = Positions.Find(label, Symbol, TradeType.Sell);

            var currentSlowMa = slowMa.Result.Last(0);
            var currentFastMa = fastMa.Result.Last(0);
            var previousSlowMa = slowMa.Result.Last(1);
            var previousFastMa = fastMa.Result.Last(1);

            Symbol1 = MarketData.GetSymbol("EURUSD");





            if (shortPosition == null && longPosition == null && previousFastMa < open && previousFastMa < close)
            {
                Print("BUY MA " + previousFastMa + " O " + open + " C " + close);

                Position position = Positions.Find(label, Symbol, TradeType.Sell);
                double vol = Quantity;
                if (position.NetProfit > 0)
                    vol = vol * 2;
                long Volume = Symbol.QuantityToVolume(vol);

                ExecuteMarketOrder(TradeType.Buy, Symbol, Volume, label, 222, 50);
                //ExecuteMarketOrder(TradeType.Sell, Symbol1, VolumeInUnitsBuy, label, 111, 50);
            }

            if (longPosition == null && shortPosition == null && previousFastMa > open && previousFastMa > close)
            {
                Print("SELL MA " + previousFastMa + " O " + open + " C " + close);

                Position position = Positions.Find(label, Symbol, TradeType.Buy);
                double vol = Quantity;

                if (position.NetProfit > 0)
                    vol = vol * 2;
                long Volume = Symbol.QuantityToVolume(vol);

                ExecuteMarketOrder(TradeType.Sell, Symbol, Volume, label, 222, 50);
                // ExecuteMarketOrder(TradeType.Buy, Symbol1, VolumeInUnitsBuy, label, 111, 50);
            }

        }



        protected override void OnStop()
        {
            // Put your deinitialization logic here
        }



    }
}
