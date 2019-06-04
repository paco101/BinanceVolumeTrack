//using Binance.Api;
//using Binance.Market;

using Binance.Net;
using Binance.Net.Logging;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trady.Core;

namespace Trading
{
    public partial class VolumeInfo : Form
    {
        private bool enableTrading = false;
        //private IBinanceApiUser iuser;
        public VolumeInfo()
        {
            InitializeComponent();
            enableTrading = chkLiveTrading.Checked;
            BinanceDefaults.SetDefaultApiCredentials("jfjGYilh6zE3EN0k4ENvFHPfun1MmIhvZsYvOBPgmqExCKyYVb1gjyE2DyAo4Ks8", "eUYGrWr30TDy8iIrI3SVUXg1D7Jo4ysj1wvFuUCaLZlE3ZqcqqjLl08zcIcNr2xc");
            BinanceDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);
            //BinanceDefaults.SetDefaultLogOutput(Console.Out);
            using (var client = new BinanceClient())
            {
                BinanceAccountInfo accountInfo = client.GetAccountInfo().Data;
                IEnumerable<BinanceBalance> candlearray = accountInfo.Balances.Select(e => e).Where(e => (e.Total + e.Locked)>0); //
            }
            if (!System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory +"\\Database.sqlite")) SQLiteConnection.CreateFile("Database.sqlite");
            _conn = new SQLiteConnection("Data Source=Database.sqlite;Version=3;");
            _conn.Open();
            try
            {
                string sql = "CREATE TABLE IF NOT EXISTS TICKERS (Symbol VARCHAR(20), active INT)";
                SQLiteCommand command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
                sql = "CREATE TABLE IF NOT EXISTS ORDERS (Symbol VARCHAR(20),Scanned DATE, Price decimal, Volume decimal, TakerBuyBaseAssetVolume decimal, TakerBuyQuoteAssetVolume decimal, NumberOfTrades INT, Hr8Av INT, NowPercent decimal, Min15Percent decimal, GreenCandles decimal)";
                command = new SQLiteCommand(sql, _conn);
                command.ExecuteNonQuery();
                sql = "CREATE VIEW IF NOT EXISTS [V_Symbols] AS SELECT COUNT(Symbol), Symbol FROM ORDERS GROUP BY Symbol ORDER By COUNT(Symbol) DESC;";
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //_conn.Close();
            }
            TableInit();
            //RefreshButton.Visible = false;
            FontSet();
            this.Show();
            GetVolumes();
            
        }
        //1200 Requests a minute
        //10 Orders a second
        //100,000 Orders a day
        DataTable CoinData = new DataTable();
        DataTable CoinData2 = new DataTable();
        private IEnumerable<BinancePrice> coinprices;
        private IEnumerable<BinancePrice> _pricesArray;
        private System.Data.SQLite.SQLiteConnection _conn;
        string sql = string.Empty;
        public void TableInit()
        {
            CoinData.Columns.Add("Symbol", typeof(string));
            CoinData.Columns.Add("Scanned", typeof(DateTime));
            CoinData.Columns.Add("Price", typeof(decimal));
            CoinData.Columns.Add("8HrAv", typeof(int));
            CoinData.Columns.Add("NowPercent", typeof(decimal));
            CoinData.Columns.Add("15MinPercent", typeof(decimal));
            CoinData.Columns.Add("GreenCandles", typeof(decimal));
            //CoinData.Columns.Add("Url", typeof(string));
            CoinData2.Columns.Add("Symbol", typeof(string));
            CoinData2.Columns.Add("Scanned", typeof(DateTime));
            CoinData2.Columns.Add("Price", typeof(decimal));
            CoinData2.Columns.Add("8HrAv", typeof(int));
            CoinData2.Columns.Add("NowPercent", typeof(decimal));
            CoinData2.Columns.Add("15MinPercent", typeof(decimal));
            CoinData2.Columns.Add("GreenCandles", typeof(decimal));
            //CoinData2.Columns.Add("Url", typeof(string));
        }

        private async Task<DataTable> SetupCoinData()
        {
            if (CoinData.Rows.Count == 0)
            {
                int iFound = 0;
                sql = string.Empty;
                SQLiteCommand command = new SQLiteCommand();
                command.CommandText = "SELECT * FROM TICKERS";
                command.Connection = _conn;
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            iFound = reader.StepCount;
                            DataRow row = CoinData.NewRow();
                            row[0] = reader.GetValue(0);
                            row[1] = DateTime.UtcNow;
                            row[2] = 0;
                            CoinData.Rows.Add(row);
                        }
                        return CoinData;
                    }
                }
                if (iFound == 0)
                {
                    using (var client = new BinanceClient())
                    {
                        var allBookPrices = client.GetAllPrices();
                        _pricesArray = allBookPrices.Data.ToArray().Where(o => //o.Symbol.EndsWith("ETH"));
                        //'CNDETH', 'ADAETH', 'LTCETH', 'POEETH', 'STRATETH', 'XVGETH', 'TRXETH', 'VENETH', 'LENDETH', 'XRPETH', 'XLMETH'
                        o.Symbol == "CNDETH" || o.Symbol == "ADAETH" || o.Symbol == "LTCETH" || o.Symbol == "POEETH" || o.Symbol == "STRATETH" || o.Symbol == "XVGETH" || o.Symbol == "TRXETH" || o.Symbol == "VENETH" || o.Symbol == "LENDETH" || o.Symbol == "XRPETH" || o.Symbol == "XLMETH");
                        //o.Symbol == "BTCUSDT" || o.Symbol == "ETHBTC" || o.Symbol.EndsWith("ETH"));
                    }
                    foreach (BinancePrice price in _pricesArray)
                    {
                        sql = sql +  "INSERT INTO TICKERS (Symbol, active) VALUES ('" + price.Symbol + "',0);";
                        DataRow row = CoinData.NewRow();
                        row[0] = price.Symbol;
                        row[1] = DateTime.UtcNow;
                        row[2] = price.Price;
                        CoinData.Rows.Add(row);
                    }
                    sql = sql + "UPDATE TICKERS set active = 1 where Symbol in ('CNDETH','ADAETH','LTCETH','POEETH','STRATETH','XVGETH','TRXETH','VENETH','LENDETH','XRPETH','XLMETH');";
                    //command = new SQLiteCommand(sql, _conn);

                    command.CommandText = sql;
                    command.ExecuteNonQuery();
                }
            }

            return CoinData;
        }

        
        public async Task<DataTable> GetVolumes()
        {
                    
            await SetupCoinData();
            using (var client = new BinanceClient())
            {
                while (true)
                {
                    foreach (DataRow pricer in CoinData.Rows)
                    {
                        IEnumerable<BinanceKline> eighthourcandles = client.GetKlines(pricer[0].ToString(), KlineInterval.FiveMinutes, startTime: DateTime.UtcNow.AddHours(-8), endTime: DateTime.UtcNow, limit: 100).Data.ToList();
                        IOrderedEnumerable<BinanceKline> candlearray = eighthourcandles.OrderByDescending(e => e.CloseTime);
                        decimal averagevolumeeighthours = candlearray.Average(candlestick => candlestick.Volume);
                        decimal fifteenminavg = (candlearray.ElementAt(1).Volume + candlearray.ElementAt(2).Volume +
                                                 candlearray.ElementAt(0).Volume) / 3;
                        BinanceOrderBook orderBook = client.GetOrderBook(pricer[0].ToString(), 10).Data;
                        IOrderedEnumerable<BinanceOrderBookEntry> orderAsks = orderBook.Asks.OrderByDescending(e => e.Quantity);
                        IOrderedEnumerable<BinanceOrderBookEntry> orderBids = orderBook.Bids.OrderByDescending(e => e.Quantity);
                        decimal maxAsk = orderAsks.First().Price;
                        decimal maxBid = orderBids.First().Price;
                        IEnumerable<BinanceAggregatedTrades> aggTrades = client.GetAggregatedTrades(pricer[0].ToString(), startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10).Data.ToList();
                        DataRow row = CoinData.Rows.Cast<DataRow>().First(o => o[0].ToString() == pricer[0].ToString());
                        row[1] = DateTime.UtcNow;
                        row[2] = eighthourcandles.Last().Close;
                        row[3] = Decimal.Round(averagevolumeeighthours, MidpointRounding.AwayFromZero);
                        row[4] = PercentGive(candlearray.First().Volume, averagevolumeeighthours);
                        row[5] = PercentGive(fifteenminavg, averagevolumeeighthours);
                        int i = 0;
                        while (i != 20)
                        {
                            if (candlearray.ElementAt(i).Close < candlearray.ElementAt(i + 1).Close) break;
                            i++;
                        }
                        row[6] = i;
                        //if (enableTrading)
                        {
                            IEnumerable<Candle> candls = candlearray.Select(p => new Candle(p.CloseTime, p.Open, p.High, p.Low, p.Close, p.Volume));
                            Trady.Analysis.Indicator.AverageDirectionalIndexRating avgDirIdx = new Trady.Analysis.Indicator.AverageDirectionalIndexRating(candls, 12, 12);
                            Trady.Analysis.Indicator.ModifiedMovingAverage mavgFast = new Trady.Analysis.Indicator.ModifiedMovingAverage(candls, 9);
                            Trady.Analysis.Indicator.ModifiedMovingAverage mavgSlow = new Trady.Analysis.Indicator.ModifiedMovingAverage(candls, 50);
                            Trady.Analysis.Indicator.IchimokuCloud ich = new Trady.Analysis.Indicator.IchimokuCloud(candls, 3, 9, 50);
                            /*if (ich.ShortPeriodCount > ich.MiddlePeriodCount)
                            {
                                Binance.Account.Orders.LimitOrder lmtOrder = new Binance.Account.Orders.LimitOrder(iuser);
                                lmtOrder.Symbol = pricer[0].ToString();
                                lmtOrder.Id = "1230";
                                lmtOrder.Price = candlearray.First().Close;
                                lmtOrder.Quantity = 100000;
                                lmtOrder.IcebergQuantity = 12;
                                api.PlaceAsync(lmtOrder, 0, CancellationToken.None);
                            }*/
                        }
                        sql = "INSERT INTO ORDERS (Symbol, Scanned, Price, Volume, TakerBuyBaseAssetVolume, TakerBuyQuoteAssetVolume, NumberOfTrades, Hr8Av, NowPercent, Min15Percent, GreenCandles) VALUES ('" + pricer[0].ToString() + "','" + row[1].ToString() + "', '" + row[2].ToString() + "', '"+ candlearray.First().Volume.ToString()+ "', '" + candlearray.First().TakerBuyBaseAssetVolume.ToString() + "', '" + candlearray.First().TakerBuyQuoteAssetVolume.ToString() + "', '" + candlearray.First().Trades.ToString() + "', '" + row[3].ToString() + "', '" + row[4].ToString() + "', '" + row[5].ToString() + "', '" + row[6].ToString() + "');";
                        SQLiteCommand command = new SQLiteCommand(sql, _conn);
                        command.ExecuteNonQuery();
                        if (chkShowNowPercent.Checked) CoinData.DefaultView.RowFilter = "NowPercent > 0";
                        //var filtered = CoinData.Select().Where(o => Convert.ToDecimal(o[3].ToString()) > Convert.ToDecimal(o[4].ToString()));
                        if (this.WindowState != FormWindowState.Minimized)
                        {
                            DataViewColours();
                            coindatagridview.DataSource = CoinData;
                            //coindatagridview.Sort(coindatagridview.Columns[3], ListSortDirection.Descending);
                        }
                    }
                }
            }
            
            return CoinData;
        }


        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            //RefreshButton.Visible = false;
            await GetVolumes();
        }

        private decimal PercentGive(decimal newvolume, decimal oldvolume)
        {
            return Decimal.Round(100 * (newvolume - oldvolume) / oldvolume, MidpointRounding.AwayFromZero);
        }

        private void DataViewColours()
        {
            if (coindatagridview.DataSource != null)
            {
                
                foreach (DataGridViewRow viewRow in coindatagridview.Rows)
                {
                    //CellCheck(viewRow, 3);
                    //CellCheck(viewRow, 4);
                    CellCheck(viewRow, 6);
                }
                
            }
        }

        


        private decimal CellCheck(DataGridViewRow viewRow, int i)
        {
            if (viewRow.Cells[i].Value.ToString() == "")
            {
                return 0;
            }
            else
            {
                if (Convert.ToDecimal(viewRow.Cells[4].Value) > Convert.ToDecimal(viewRow.Cells[5].Value) || Convert.ToDecimal(viewRow.Cells[3].Value) > 1000)
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[coindatagridview.DataSource];
                    currencyManager1.SuspendBinding();
                    coindatagridview.Rows[viewRow.Index].Visible = true;
                    currencyManager1.ResumeBinding();
                }
               if(Convert.ToDecimal(viewRow.Cells[3].Value) < 1000 || Convert.ToDecimal(viewRow.Cells[4].Value) < Convert.ToDecimal(viewRow.Cells[5].Value))
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[coindatagridview.DataSource];
                    currencyManager1.SuspendBinding();
                    coindatagridview.Rows[viewRow.Index].Visible = false;
                    currencyManager1.ResumeBinding();
                }
                if (i == 6)
                {
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) >= 4)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.LightGreen;
                        viewRow.Cells[i].Style.ForeColor = Color.Black;
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) >= 6)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.Green;
                        viewRow.Cells[i].Style.ForeColor = Color.White;
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) < 4)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.White;
                        viewRow.Cells[i].Style.ForeColor = Color.Black;
                    }
                }
                else
                {
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) >= 0)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.LightGreen;
                        viewRow.Cells[i].Style.ForeColor = Color.Black;
                  
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) >= 5)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.Green;
                        viewRow.Cells[i].Style.ForeColor = Color.White;
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) < 0)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.LightCoral;
                        viewRow.Cells[i].Style.ForeColor = Color.Black;
                       
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) <= -10)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.Red;
                        viewRow.Cells[i].Style.ForeColor = Color.Black;
                    }
                    if (Convert.ToDecimal(viewRow.Cells[i].Value) <= -30)
                    {
                        viewRow.Cells[i].Style.BackColor = Color.DarkRed;
                        viewRow.Cells[i].Style.ForeColor = Color.White;
                    }

                }
                return Convert.ToDecimal(viewRow.Cells[i].Value);
            }
        }

        private void coindatagridview_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in coindatagridview.SelectedCells)
            {
   
            }
        }

        private void FontSet()
        {
            foreach (Control control in Controls)
            {
                {
                    control.Font = new Font("Arial", 10, FontStyle.Bold);
                }
            }
        }

        private void coindatagridview_Sorted(object sender, EventArgs e)
        {
            
          
            DataViewColours();
        }



        private ListSortDirection SortOrder(SortOrder sortOrder)
        {
            if (sortOrder.ToString() == "Descending")
            {
                return ListSortDirection.Descending;
            }
            else
            {
                return ListSortDirection.Ascending;
            }
        }


        private void chkShowNowPercent_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RefreshButton_Click_1(object sender, EventArgs e)
        {

        }

        private void VolumeInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            _conn.Close();
        }

        private void coindatagridview_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            DataGridViewCell cell = (DataGridViewCell)coindatagridview.Rows[e.RowIndex].Cells[0];
            string sUrl = "https://www.binance.com/tradeDetail.html?symbol=";
            string sSymbol = string.Empty;
            if (cell.Value.ToString().EndsWith("ETH")) sSymbol = cell.Value.ToString().Replace("ETH", "_ETH");
            if (cell.Value.ToString().EndsWith("BTC")) sSymbol = cell.Value.ToString().Replace("BTC", "_BTC");
            if (cell.Value.ToString().EndsWith("BNB")) sSymbol = cell.Value.ToString().Replace("BNB", "_BNB");
            if (cell.Value.ToString().EndsWith("USDT")) sSymbol = cell.Value.ToString().Replace("USDT", "_USDT");
            Process.Start(sUrl + sSymbol);
        }

        private void chkLiveTrading_CheckedChanged(object sender, EventArgs e)
        {
            enableTrading = chkLiveTrading.Checked;
        }
    }

    /*
     *     class Program
    {
        static void Main(string[] args)
        {
            BinanceDefaults.SetDefaultApiCredentials("APIKEY", "APISECRET");
            BinanceDefaults.SetDefaultLogVerbosity(LogVerbosity.Debug);
            BinanceDefaults.SetDefaultLogOutput(Console.Out);

            using (var client = new BinanceClient())
            using (var socketClient = new BinanceSocketClient())
            {
                // Public
                var ping = client.Ping();
                var serverTime = client.GetServerTime();
                var orderBook = client.GetOrderBook("BNBBTC", 50);
                var aggTrades = client.GetAggregatedTrades("BNBBTC", startTime: DateTime.UtcNow.AddMinutes(-2), endTime: DateTime.UtcNow, limit: 10);
                var klines = client.GetKlines("BNBBTC", KlineInterval.OneHour, startTime: DateTime.UtcNow.AddHours(-10), endTime: DateTime.UtcNow, limit: 10);
                var prices24h = client.Get24HPrice("BNBBTC");
                var allPrices = client.GetAllPrices();
                var allBookPrices = client.GetAllBookPrices();

                // Private
                var openOrders = client.GetOpenOrders("BNBBTC");
                var allOrders = client.GetAllOrders("BNBBTC");
                var testOrderResult = client.PlaceTestOrder("BNBBTC", OrderSide.Buy, OrderType.Limit, 1, price: 1, timeInForce: TimeInForce.GoodTillCancel);
                var queryOrder = client.QueryOrder("BNBBTC", allOrders.Data[0].OrderId);
                var orderResult = client.PlaceOrder("BNBBTC", OrderSide.Sell, OrderType.Limit, 10, price: 0.0002m, timeInForce: TimeInForce.GoodTillCancel);
                var cancelResult = client.CancelOrder("BNBBTC", orderResult.Data.OrderId);
                var accountInfo = client.GetAccountInfo();
                var myTrades = client.GetMyTrades("BNBBTC");


                // Withdrawal/deposit
                var withdrawalHistory = client.GetWithdrawHistory();
                var depositHistory = client.GetDepositHistory();
                var withdraw = client.Withdraw("ASSET", "ADDRESS", 0);


                // Streams
                var successDepth = socketClient.SubscribeToDepthStream("bnbbtc", (data) =>
                {
                    // handle data
                });
                var successTrades = socketClient.SubscribeToTradesStream("bnbbtc", (data) =>
                {
                    // handle data
                });
                var successKline = socketClient.SubscribeToKlineStream("bnbbtc", KlineInterval.OneMinute, (data) =>
                {
                    // handle data
                });

                var successStart = client.StartUserStream();
                var successAccount = socketClient.SubscribeToAccountUpdateStream(successStart.Data.ListenKey, (data) =>
                {
                    // handle data
                });
                var successOrder = socketClient.SubscribeToOrderUpdateStream(successStart.Data.ListenKey, (data) =>
                {
                    // handle data
                });

                socketClient.UnsubscribeFromStream(successDepth.Data);
                socketClient.UnsubscribeFromAccountUpdateStream();
                socketClient.UnsubscribeAllStreams();
            }

            Console.ReadLine();
        }
    }*/
}
