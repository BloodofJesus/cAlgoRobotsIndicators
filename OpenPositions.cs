using System;
using cAlgo.API;
using cAlgo.API.Internals;
using cAlgo.API.Indicators;
using cAlgo.Indicators;

namespace cAlgo
{
    [Indicator(IsOverlay = false, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class OpenPositions : Indicator
    {

        public override void Calculate(int index)
        {

            var text = "";
            foreach (var pos in Positions)
            {
                if (pos.TradeType == TradeType.Buy)
                    text = text + pos.Quantity + " Lot " + pos.SymbolCode + " " + pos.TradeType + "   " + pos.NetProfit + "\n";
            }
            var name = "Open buy";
            double PL = Math.Round(Account.Equity - Account.Balance);
            text = "P&L " + PL + "  Balance " + Math.Round(Account.Balance, 2) + "  Equity " + Math.Round(Account.Equity, 2) + "\n" + text;
            var staticPos = StaticPosition.TopLeft;
            var color = Colors.DodgerBlue;
            ChartObjects.DrawText(name, text, staticPos, color);

            var text1 = "";
            foreach (var pos in Positions)
            {
                if (pos.TradeType == TradeType.Sell)
                    text1 = text1 + pos.Quantity + " Lot " + pos.SymbolCode + " " + pos.TradeType + "   " + pos.NetProfit + "\n";
            }
            name = "Open Sell";
            staticPos = StaticPosition.TopRight;
            var color1 = Colors.Red;
            ChartObjects.DrawText(name, text1, staticPos, color1);
        }
    }
}
