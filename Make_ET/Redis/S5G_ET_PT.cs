using Make_ET.DataModels;
using Make_ET.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET.Redis
{
    public class S5G_ET_PT
    {
        //CGlobal.FULL_ROW_QUOTE[] m_arrsttFullRowQuote;
        CGlobal.FULL_PUT_AD[] m_arrstt_PUT_AD_BUY;
        CGlobal.FULL_PUT_AD[] m_arrstt_PUT_AD_SELL;
        CGlobal.FULL_PUT_EXEC[] m_arrstt_PUT_EXEC;
        CGlobal.FULL_PUT_EXEC[] m_arrstt_PUT_DC;
        CGlobal.FULL_ROW_PT m_arrstt_ROWPT;       
        private enum PUT_EXEC_SUM
        {
            TOTAL_QUANTITY,
            TOTAL_VALUE
        }
        private long GetSum(int intStockNo, CGlobal.PUT_EXEC[] arrsttPUT_EXEC, PUT_EXEC_SUM PES)
        {
            //Logger.LogInfo("GetSum");
            try
            {
                long intSum = 0;
                for (int i = 0; i < arrsttPUT_EXEC.Length; i++)
                {
                    if (PES == PUT_EXEC_SUM.TOTAL_QUANTITY)
                        intSum += Convert.ToInt64(arrsttPUT_EXEC[i].Vol);
                    if (PES == PUT_EXEC_SUM.TOTAL_VALUE)
                        intSum += Convert.ToInt64(arrsttPUT_EXEC[i].Vol) * Convert.ToInt64(arrsttPUT_EXEC[i].Price) * 10;
                }
                return intSum;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //param:(m_arrsttFullRowQuote,m_crfSECURITY.DataUpdate,m_crfPUT_AD.DataUpdate,m_crfPUT_EXEC.DataUpdate,m_crfPUT_DC.DataUpdate,m_arrstt_ROWPT,)
        public FULL_ROW_PT UpdateFULL_ROW_PT(FULL_ROW_QUOTE[] m_arrsttFullRowQuote, CGlobal.SECURITY[] arrsttSECURITY, CGlobal.PUT_AD[] arrsttPUT_AD, CGlobal.PUT_EXEC[] arrsttPUT_EXEC, CGlobal.PUT_DC[] arrsttPUT_DC)
        {
            Logger.LogInfo("UpdateFULL_ROW_PT");
            try
            {
                //UPDATE PUT_AD
                if (arrsttPUT_AD != null)
                {
                    var buyList = new List<CGlobal.FULL_PUT_AD>();
                    var sellList = new List<CGlobal.FULL_PUT_AD>();
                    int intAutoBuy = 0;
                    int intAutoSell = 0;
                    string Codes = "";
                    for (int i = 0; i < arrsttPUT_AD.Length; i++)
                    {
                        string side = new string(arrsttPUT_AD[i].Side).Trim();
                        if (side == "B")
                        {
                            intAutoBuy++;
                            for (int j = 0; j < arrsttSECURITY.Length; j++)
                            {
                                if (arrsttSECURITY[j].StockNo == arrsttPUT_AD[i].StockNo)
                                {
                                    Codes = new string(arrsttSECURITY[j].StockSymbol).Trim();
                                    break;
                                }
                            }
                            buyList.Add(new CGlobal.FULL_PUT_AD
                            {
                                Auto = intAutoBuy.ToString(),
                                Code = Codes,
                                Price = arrsttPUT_AD[i].Price.ToString(),
                                Quantity = arrsttPUT_AD[i].Vol.ToString(),
                                Time = arrsttPUT_AD[i].Time.ToString(),
                                TradeID = arrsttPUT_AD[i].TradeID.ToString()
                            });
                        }
                        else if (side == "S")
                        {
                            intAutoSell++;
                            for (int j = 0; j < arrsttSECURITY.Length; j++)
                            {
                                if (arrsttSECURITY[j].StockNo == arrsttPUT_AD[i].StockNo)
                                {
                                    Codes = new string(arrsttSECURITY[j].StockSymbol).Trim();
                                    break;
                                }
                            }
                            sellList.Add(new CGlobal.FULL_PUT_AD
                            {
                                Auto = intAutoSell.ToString(),
                                Code = Codes,
                                Price = arrsttPUT_AD[i].Price.ToString(),
                                Quantity = arrsttPUT_AD[i].Vol.ToString(),
                                Time = arrsttPUT_AD[i].Time.ToString(),
                                TradeID = arrsttPUT_AD[i].TradeID.ToString()
                            });
                        }
                        m_arrstt_PUT_AD_BUY = buyList.ToArray();
                        m_arrstt_PUT_AD_SELL = sellList.ToArray();
                        Array.Sort(m_arrstt_PUT_AD_BUY, (a, b) => a.Code.CompareTo(b.Code));
                        Array.Sort(m_arrstt_PUT_AD_SELL, (a, b) => a.Code.CompareTo(b.Code));
                    }
                }
                int[] stockNoSECURITY;
                stockNoSECURITY = new int[arrsttSECURITY.Length];
                for (int k = 0; k < arrsttSECURITY.Length; k++)
                {
                    stockNoSECURITY[k] = arrsttSECURITY[k].StockNo;
                }
                //UPDATE PUT_EXEC                              
                if (arrsttPUT_EXEC != null)
                {
                    if (m_arrstt_PUT_EXEC == null && arrsttPUT_EXEC != null)
                    {
                        Array.Resize(ref m_arrstt_PUT_EXEC, arrsttPUT_EXEC.Length);
                    }
                    if (m_arrstt_PUT_EXEC == null) return m_arrstt_ROWPT;
                    for (int i = 0; i < arrsttPUT_EXEC.Length; i++)
                    {
                        //int indexPUT_EXEC = Array.LastIndexOf(stockNoSECURITY, arrsttPUT_EXEC[i].StockNo);
                        int indexPUT_EXEC = Array.FindIndex(m_arrsttFullRowQuote,row => row.SN == arrsttPUT_EXEC[i].StockNo);
                        m_arrstt_PUT_EXEC[i].Auto = (i + 1).ToString();
                        m_arrstt_PUT_EXEC[i].Code = m_arrsttFullRowQuote[indexPUT_EXEC].Co.ToString();
                        m_arrstt_PUT_EXEC[i].Re = m_arrsttFullRowQuote[indexPUT_EXEC].Re.ToString();
                        m_arrstt_PUT_EXEC[i].Ce = m_arrsttFullRowQuote[indexPUT_EXEC].Ce.ToString();
                        m_arrstt_PUT_EXEC[i].Fl = m_arrsttFullRowQuote[indexPUT_EXEC].Fl.ToString();
                        m_arrstt_PUT_EXEC[i].PTPrice = arrsttPUT_EXEC[i].Price.ToString();
                        m_arrstt_PUT_EXEC[i].PTQuantity = arrsttPUT_EXEC[i].Vol.ToString();
                        m_arrstt_PUT_EXEC[i].PTTotalQuantity = this.GetSum(arrsttPUT_EXEC[i].StockNo, arrsttPUT_EXEC, PUT_EXEC_SUM.TOTAL_QUANTITY).ToString();
                        m_arrstt_PUT_EXEC[i].PTTotalValue = this.GetSum(arrsttPUT_EXEC[i].StockNo, arrsttPUT_EXEC, PUT_EXEC_SUM.TOTAL_VALUE).ToString();
                        m_arrstt_PUT_EXEC[i].NMTotalQuantity = m_arrsttFullRowQuote[indexPUT_EXEC].TQ;
                        m_arrstt_PUT_EXEC[i].NMPTTotalQuantity = (Convert.ToInt64(m_arrstt_PUT_EXEC[i].PTTotalQuantity) + Convert.ToInt64(m_arrstt_PUT_EXEC[i].NMTotalQuantity)).ToString();
                        m_arrstt_PUT_EXEC[i].ListingQuantity = "0";
                        m_arrstt_PUT_EXEC[i].Time = "";
                        m_arrstt_PUT_EXEC[i].ConfirmNo = arrsttPUT_EXEC[i].ConfirmNo.ToString();
                    }
                    Array.Sort(m_arrstt_PUT_EXEC,(a,b) => a.Code.CompareTo(b.Code));
                }
                //UPDATE PUT_DC
                if (arrsttPUT_DC != null)
                {
                    if (m_arrstt_PUT_DC == null && arrsttPUT_DC != null)
                    {
                        Array.Resize(ref m_arrstt_PUT_DC, arrsttPUT_DC.Length);
                    }
                    if (m_arrstt_PUT_DC == null) return m_arrstt_ROWPT;
                    for (int i = 0; i < arrsttPUT_DC.Length; i++)
                    {
                        // tim index cua Element trong this.m_arrsttFullRowQuote co SN=arrsttPUT_DC[i].StockNo
                        //int indexPUT_DC = Array.FindIndex(m_arrsttFullRowQuote, row => row.SN == arrsttPUT_DC[i].StockNo);
                        int indexPUT_DC = Array.LastIndexOf(stockNoSECURITY, arrsttPUT_DC[i].StockNo);
                        if (indexPUT_DC >= 0 && indexPUT_DC < m_arrsttFullRowQuote.Length)
                        {
                            m_arrstt_PUT_DC[i].Auto = (i + 1).ToString();
                            m_arrstt_PUT_DC[i].Code = m_arrsttFullRowQuote[indexPUT_DC].Co;
                            m_arrstt_PUT_DC[i].Re = "0";
                            m_arrstt_PUT_DC[i].Ce = "0";
                            m_arrstt_PUT_DC[i].Fl = "0";
                            m_arrstt_PUT_DC[i].PTPrice = arrsttPUT_DC[i].Price.ToString();
                            m_arrstt_PUT_DC[i].PTQuantity = arrsttPUT_DC[i].Vol.ToString();
                            m_arrstt_PUT_DC[i].PTTotalQuantity = "0";
                            m_arrstt_PUT_DC[i].PTTotalValue = "0";
                            m_arrstt_PUT_DC[i].NMTotalQuantity = "0";
                            m_arrstt_PUT_DC[i].NMPTTotalQuantity = "0";
                            m_arrstt_PUT_DC[i].ListingQuantity = "0";
                            m_arrstt_PUT_DC[i].Time = "";
                            m_arrstt_PUT_DC[i].ConfirmNo = arrsttPUT_DC[i].ConfirmNo.ToString();
                            /*Array.Resize(ref m_arrstt_PUT_DC, arrsttPUT_DC.Length - 1);*/
                        }
                    }
                    Array.Sort(m_arrstt_PUT_DC, (a, b) => a.Code.CompareTo(b.Code));
                }
                else arrsttPUT_DC = null;
                m_arrstt_ROWPT.PUT_AD_BUY = m_arrstt_PUT_AD_BUY;
                m_arrstt_ROWPT.PUT_AD_SELL = m_arrstt_PUT_AD_SELL;
                m_arrstt_ROWPT.PUT_EXEC = m_arrstt_PUT_EXEC;
                m_arrstt_ROWPT.PUT_DC = m_arrstt_PUT_DC;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
            return m_arrstt_ROWPT;
        }
    }
}
