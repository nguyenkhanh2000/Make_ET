using Make_ET.DataModels;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET.Redis
{
    public class S5G_ET_QUOTE
    {
        public FULL_ROW_QUOTE[] m_arrsttFullRowQuote;

        public FULL_ROW_QUOTE[] UpdateFullRowQuote(CGlobal.SECURITY[] arrsttSECURITY, CGlobal.LS[] arrsttLS, CGlobal.OS[] arrsttOS, CGlobal.FROOM[] arrsttFROOM, FULL_ROW_INDEX m_arrsttFullRowIndex)
        {
            int intCe = 0, intFl = 0;           //count tổng số mã có giá khớp = > < so vs tc
            try
            {
                //init
                if (m_arrsttFullRowQuote == null && arrsttSECURITY != null)
                {
                    Array.Resize(ref m_arrsttFullRowQuote, arrsttSECURITY.Length);
                }
                //chưa init, exit
                if (m_arrsttFullRowQuote == null) return m_arrsttFullRowQuote;
                //update SECURITY
                if (arrsttSECURITY != null)
                {
                    for (int i = 0; i < arrsttSECURITY.Length; i++)
                    {
                        m_arrsttFullRowQuote[i].Co = CConfig.Char2String(arrsttSECURITY[i].StockSymbol);            // 00 - code
                        //m_arrsttFullRowQuote[i].Co = new string(arrsttSECURITY[i].StockSymbol).Trim();
                        m_arrsttFullRowQuote[i].Re = arrsttSECURITY[i].PriorClosePrice.ToString();                  // 01 - ref
                        m_arrsttFullRowQuote[i].Ce = arrsttSECURITY[i].Ceiling.ToString();                          // 02 - ceiling
                        m_arrsttFullRowQuote[i].Fl = arrsttSECURITY[i].Floor.ToString();                            // 03 - floor

                        m_arrsttFullRowQuote[i].BP3 = arrsttSECURITY[i].Best3Bid.ToString();                        // 05 - buy price 3
                        m_arrsttFullRowQuote[i].BQ3 = arrsttSECURITY[i].Best3BidVolume.ToString();                  // 06 - buy quantity 3
                        m_arrsttFullRowQuote[i].BP2 = arrsttSECURITY[i].Best2Bid.ToString();                        // 07 - buy price 2
                        m_arrsttFullRowQuote[i].BQ2 = arrsttSECURITY[i].Best2BidVolume.ToString();                  // 08 - buy quantity 2
                        m_arrsttFullRowQuote[i].BP1 = arrsttSECURITY[i].Best1Bid.ToString(); /*Get Price(arrsttSECURITY[i].Best1Bid, arrsttSECURITY[i].Best1BidVolume, m_sttFullRowIndex.STAT_ControlCode);*///09 - buy price 1 (arrsttSECURITY[i].Best1Bid.ToString();)
                        m_arrsttFullRowQuote[i].BQ1 = arrsttSECURITY[i].Best1BidVolume.ToString();                  // 10 - buy quantity 1

                        // phien ATO/ATC
                        if (m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATO || m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATC)
                        {
                            if (m_arrsttFullRowQuote[i].MP != null && arrsttSECURITY[i].ProjectOpen == 0)
                                if (Convert.ToDouble(m_arrsttFullRowQuote[i].MP) > 0)
                                    m_arrsttFullRowQuote[i].MPO = m_arrsttFullRowQuote[i].MP;
                            if (m_arrsttFullRowQuote[i].MQ != null && arrsttSECURITY[i].ProjectOpen == 0)
                                if (Convert.ToDouble(m_arrsttFullRowQuote[i].MQ) > 0)
                                    m_arrsttFullRowQuote[i].MQO = m_arrsttFullRowQuote[i].MQ;

                            // chi gan TQ vao TQO 1 lan vao luc bat dau phien ATC
                            if (m_arrsttFullRowQuote[i].TQO == "0" && m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATC)
                                m_arrsttFullRowQuote[i].TQO = m_arrsttFullRowQuote[i].TQ;

                            m_arrsttFullRowQuote[i].MP = arrsttSECURITY[i].ProjectOpen.ToString();          // 11 - match price [LS]
                            m_arrsttFullRowQuote[i].MQ = "0";     // 12 - match quantity  [LS]
                            m_arrsttFullRowQuote[i].MC = m_arrsttFullRowQuote[i].MP == null || m_arrsttFullRowQuote[i].MP == "0" ? "0" : (Convert.ToSingle(m_arrsttFullRowQuote[i].MP) - Convert.ToSingle(m_arrsttFullRowQuote[i].Re)).ToString();    // 13 - match change [calculate]

                            if (Convert.ToDouble(m_arrsttFullRowQuote[i].MP) == Convert.ToDouble(m_arrsttFullRowQuote[i].Ce)) ++intCe;
                            if (Convert.ToDouble(m_arrsttFullRowQuote[i].MP) == Convert.ToDouble(m_arrsttFullRowQuote[i].Fl)) ++intFl;
                        }
                        // phien lien tuc + phien thoa thuan [14:45-15:00]
                        if (m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CON || m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO)
                        {
                            //if (m_sttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO && arrsttSECURITY[i].ProjectOpen.ToString()=="0")
                            if (m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO)
                            {
                                // 2015-06-29 17:00:11 huyNQ 2015-06-30 15:07:08 ngocta2
                                if (m_arrsttFullRowQuote[i].MPO != null)
                                    m_arrsttFullRowQuote[i].MP = m_arrsttFullRowQuote[i].MPO;
                                if (m_arrsttFullRowQuote[i].MQO != null)
                                    m_arrsttFullRowQuote[i].MQ = m_arrsttFullRowQuote[i].MQO;
                                m_arrsttFullRowQuote[i].MC = m_arrsttFullRowQuote[i].MP == null || m_arrsttFullRowQuote[i].MP == "0" ? "0" : (Convert.ToSingle(m_arrsttFullRowQuote[i].MP) - Convert.ToSingle(m_arrsttFullRowQuote[i].Re)).ToString();    // 13 - match change [calculate]
                            }
                            if (Convert.ToInt32(arrsttSECURITY[i].Last) == Convert.ToInt32(m_arrsttFullRowQuote[i].Ce)) ++intCe;
                            if (Convert.ToInt32(arrsttSECURITY[i].Last) == Convert.ToInt32(m_arrsttFullRowQuote[i].Fl)) ++intFl;
                        }

                        //m_arrsttFullRowQuote[i].SP1 = GetPrice(arrsttSECURITY[i].Best1Offer, arrsttSECURITY[i].Best1OfferVolume, m_sttFullRowIndex.STAT_ControlCode);// 14 - sell price 1 (arrsttSECURITY[i].Best1Offer.ToString())
                        m_arrsttFullRowQuote[i].SP1 = arrsttSECURITY[i].Best1Offer.ToString();
                        m_arrsttFullRowQuote[i].SQ1 = arrsttSECURITY[i].Best1OfferVolume.ToString();                // 15 - sell quantity 1
                        m_arrsttFullRowQuote[i].SP2 = arrsttSECURITY[i].Best2Offer.ToString();                      // 16 - sell price 2
                        m_arrsttFullRowQuote[i].SQ2 = arrsttSECURITY[i].Best2OfferVolume.ToString();                // 17 - sell quantity 2
                        m_arrsttFullRowQuote[i].SP3 = arrsttSECURITY[i].Best3Offer.ToString();                      // 18 - sell price 3
                        m_arrsttFullRowQuote[i].SQ3 = arrsttSECURITY[i].Best3OfferVolume.ToString();
                        //BQ4 = KL mua 4+ = TotalBidQtty - BQ1 - BQ2 - BQ3 [tu tinh]
                        //m_arrsttFullRowQuote[i].BQ4 = (Convert.ToSingle(m_arrsttFullRowQuote[i].TQ) -
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].BQ3) -
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].BQ2) -
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].BQ1)).ToString();
                        //m_arrsttFullRowQuote[i].SQ4 = (Convert.ToSingle(m_arrsttFullRowQuote[i].TQ) +
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].SQ3) +
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].SQ2) +
                        //            Convert.ToSingle(m_arrsttFullRowQuote[i].SQ1)).ToString();
                        if (m_arrsttFullRowQuote[i].BQ4 == null) m_arrsttFullRowQuote[i].BQ4 = "0";
                        if (m_arrsttFullRowQuote[i].SQ4 == null) m_arrsttFullRowQuote[i].SQ4 = "0";

                        m_arrsttFullRowQuote[i].TQ = arrsttSECURITY[i].LastVol.ToString();                          // 21 - total quantity (NM)
                        if (m_arrsttFullRowQuote[i].Op == "" || m_arrsttFullRowQuote[i].Op == "0" || m_arrsttFullRowQuote[i].Op == null)            //2015-12-21 09:38:04 ngocta2
                            m_arrsttFullRowQuote[i].Op = arrsttSECURITY[i].OpenPrice.ToString(); 						 // 22 - open (NM)   [OS=>SECURITY]
                        m_arrsttFullRowQuote[i].Hi = arrsttSECURITY[i].Highest.ToString();                          // 23 - highest (NM)
                        m_arrsttFullRowQuote[i].Lo = arrsttSECURITY[i].Lowest.ToString();
                        m_arrsttFullRowQuote[i].Av = arrsttSECURITY[i].AvrPrice.ToString();
                        m_arrsttFullRowQuote[i].SN = arrsttSECURITY[i].StockNo;                                     // 29 - hidden - StockNo
                        m_arrsttFullRowQuote[i].ST = CConfig.Char2String(arrsttSECURITY[i].StockType);             // 30 - hidden - StockType
                        //m_arrsttFullRowQuote[i].ST = new string(arrsttSECURITY[i].StockType).Trim();
                        m_arrsttFullRowQuote[i].PO = arrsttSECURITY[i].ProjectOpen.ToString();                      // 31 - hidden - ProjectOpen         
                        m_arrsttFullRowQuote[i].Ri = GetRightStatus(arrsttSECURITY[i]);                      // 32 - hidden - Rights 
                        //if (m_arrsttFullRowQuote[i].TQO == null) m_arrsttFullRowQuote[i].TQO = "0";
                        
                    }

                }
                ///UPDATE LS (NEW)
                if (m_arrsttFullRowIndex.STAT_ControlCode != CGlobal.MARKET_STAT_ATO && m_arrsttFullRowIndex.STAT_ControlCode != CGlobal.MARKET_STAT_ATC)
                {
                    int[] stockNoLS;
                    if (arrsttLS != null)
                    {
                        stockNoLS = new int[arrsttLS.Length];
                        for (int k = 0; k < arrsttLS.Length; k++)
                        {
                            stockNoLS[k] = arrsttLS[k].StockNo;
                        }
                    }
                    else
                        stockNoLS = new int[0];

                    for (int j = 0; j < m_arrsttFullRowQuote.Length; j++)
                    {
                        int indexLS = Array.LastIndexOf(stockNoLS, m_arrsttFullRowQuote[j].SN);
                        if (indexLS != -1)
                        {
                            if (m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CON) //phien lien tuc
                            {
                                m_arrsttFullRowQuote[j].MPO = arrsttLS[indexLS].Price.ToString();              // 11 - match price [LS]
                                m_arrsttFullRowQuote[j].MQO = arrsttLS[indexLS].MatchedVol.ToString();         // 12 - match quantity  [LS]
                            }
                            if (m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO) //phien thoa thuan 14h45-15h00
                            {
                                m_arrsttFullRowQuote[j].MPO = arrsttLS[indexLS].Price.ToString();                  // 11 - match price [LS]
                                m_arrsttFullRowQuote[j].MQO = arrsttLS[indexLS].MatchedVol.ToString();             //12 - match quantity [LS]
                            }
                            m_arrsttFullRowQuote[j].MPO = arrsttLS[indexLS].Price.ToString();                  // 11 - match price [LS]
                            m_arrsttFullRowQuote[j].MQO = arrsttLS[indexLS].MatchedVol.ToString();

                            m_arrsttFullRowQuote[j].MP = arrsttLS[indexLS].Price.ToString();
                            m_arrsttFullRowQuote[j].MQ = arrsttLS[indexLS].MatchedVol.ToString();

                            m_arrsttFullRowQuote[j].MC = (Convert.ToSingle(m_arrsttFullRowQuote[j].MP) - Convert.ToSingle(m_arrsttFullRowQuote[j].Re)).ToString(); //13-match change[calculate]                                                                                                                                                                                //break; //exit for j

                            m_arrsttFullRowQuote[j].TQO = (Convert.ToSingle(m_arrsttFullRowQuote[j].TQ) - Convert.ToSingle(m_arrsttFullRowQuote[j].MQO)).ToString();
                        }
                    }
                }
                //UPDATE FROOM (NEW)
                if (true)
                {
                    int[] stockNoFROOM;
                    if (arrsttFROOM != null)
                    {
                        stockNoFROOM = new int[arrsttFROOM.Length];
                        for (int k = 0; k < arrsttFROOM.Length; k++)
                        {
                            stockNoFROOM[k] = arrsttFROOM[k].StockNo;
                        }
                    }
                    else
                        stockNoFROOM = new int[0];
                    for (int j = 0; j < m_arrsttFullRowQuote.Length; j++)
                    {
                        int indexFROOM = Array.LastIndexOf(stockNoFROOM, m_arrsttFullRowQuote[j].SN);
                        if (indexFROOM != -1)
                        {
                            //lock(m_crfOS) { }
                            m_arrsttFullRowQuote[j].FB = arrsttFROOM[indexFROOM].BuyVolume.ToString();                 //26 - foreign buy (quantity) [FROOM]
                            m_arrsttFullRowQuote[j].FS = arrsttFROOM[indexFROOM].SellVolume.ToString();                //27 - foreign sell (quantiry) [FROOM]
                            m_arrsttFullRowQuote[j].FR = arrsttFROOM[indexFROOM].CurrentRoom.ToString();               //28 - foreign room (current) [FROOM]
                        }
                    }
                }
            }
            catch (Exception ex)
            { throw ex; }

            return m_arrsttFullRowQuote;
        }
        private string GetRightStatus(CGlobal.SECURITY stt)
        {
            try
            {
                string c = "", v = "";
                c = CConfig.Char2String(stt.SUSPENSION).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_SU + CConfig.SEP2 + c;
                c = CConfig.Char2String(stt.Delist).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_DE + CConfig.SEP2 + c;

                c = CConfig.Char2String(stt.HaltResumeFlag).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_HA + CConfig.SEP2 + c;

                c = CConfig.Char2String(stt.SPLIT).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_SP + CConfig.SEP2 + c;

                c = CConfig.Char2String(stt.Benefit).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_BE + CConfig.SEP2 + c;

                c = CConfig.Char2String(stt.Meeting).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_ME + CConfig.SEP2 + c;

                c = CConfig.Char2String(stt.Notice).Trim();
                if (c != "") v += CConfig.SEP1 + CConfig.KEY_NO + CConfig.SEP2 + c;
                if (v.Length > 0)
                    v = v.Remove(0, 1); // error if length=0
                return v;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
