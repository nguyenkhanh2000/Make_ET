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
    public class S5G_ET_INDEX
    {
        CGlobal.FULL_ROW_INDEX m_arrsttFullRowIndex;
        
        public FULL_ROW_INDEX UpdateFULL_ROW_INDEX(LastIndexHO m_LIH, CGlobal.MARKET_STAT[] arr_MARKET_STAT, CGlobal.VNX_MARKET_VNINDEX[] arr_VNIndex,
            CGlobal.VNX_MARKET_INDEX[] arr_VN30Index, CGlobal.VNX_MARKET_INDEX[] arr_VN100Index,CGlobal.VNX_MARKET_INDEX[] arr_VNAllIndex,
            CGlobal.VNX_MARKET_INDEX[] arr_VNXAllIndex,CGlobal.VNX_MARKET_INDEX[] arr_VNMidIndex,CGlobal.VNX_MARKET_INDEX[] arr_VNSmlIndex,CGlobal.INAV[] arr_INAV,CGlobal.IINDEX[] arr_IINDEX,int intCe, int intFl)
        {
            Logger.LogInfo("UpdateFULL_ROW_INDEX");
            try
            {
                //MARKET_STAT
                MARKET_STAT market_stat = arr_MARKET_STAT[arr_MARKET_STAT.Length - 2];
                this.m_arrsttFullRowIndex.STAT_ControlCode = /*market_stat.ControlCode.ToString();*/new string(market_stat.ControlCode).Trim();
                this.m_arrsttFullRowIndex.STAT_Time = market_stat.Time.ToString();
                DateTime datetime = new DateTime(2023, 7, 28);
                //DateTime datetime = DateTime.Now;
                string formattedDate = datetime.ToString("dd/MM/yyyy");
                this.m_arrsttFullRowIndex.STAT_Date = formattedDate;
                VNX_MARKET_VNINDEX VNINDEX = arr_VNIndex[arr_VNIndex.Length - 1];
                this.m_arrsttFullRowIndex.VNI_Time = VNINDEX.Time.ToString();
                this.m_arrsttFullRowIndex.VNI_IndexValue = (VNINDEX.Index / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNI_TotalTrade = VNINDEX.TotalTrade.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNI_TotalSharesAOM = VNINDEX.TotalShares.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNI_TotalValuesAOM = (VNINDEX.TotalValues / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNI_UpVolume = VNINDEX.UpVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_DownVolume = VNINDEX.DownVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_NoChangeVolume = VNINDEX.NoChangeVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_Up = VNINDEX.Up.ToString();
                this.m_arrsttFullRowIndex.VNI_Down = VNINDEX.Down.ToString();
                this.m_arrsttFullRowIndex.VNI_NoChange = VNINDEX.NoChange.ToString(); 
                this.m_arrsttFullRowIndex.VNI_Ceiling = intCe.ToString();
                this.m_arrsttFullRowIndex.VNI_Floor = intFl.ToString();
                this.m_arrsttFullRowIndex.VNI_TotalSharesOld = "0";
                this.m_arrsttFullRowIndex.VNI_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNI_IndexValue) - m_LIH.Data[0].VNIndex).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNI_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNI_Change) / m_LIH.Data[0].VNIndex) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VN30 = arr_VN30Index[arr_VN30Index.Length - 1];
                this.m_arrsttFullRowIndex.VN30_Time = VN30.Time.ToString();
                this.m_arrsttFullRowIndex.VN30_IndexValue = (VN30.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN30_TotalSharesAOM = VN30.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VN30_TotalSharesPT = VN30.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VN30_TotalValuesAOM = (VN30.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN30_TotalValuesPT = (VN30.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN30_Up = VN30.Up.ToString();
                this.m_arrsttFullRowIndex.VN30_Down = VN30.Down.ToString();
                this.m_arrsttFullRowIndex.VN30_NoChange = VN30.NoChange.ToString();
                this.m_arrsttFullRowIndex.VN30_Ceiling = VN30.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VN30_Floor = VN30.Floor.ToString();
                this.m_arrsttFullRowIndex.VN30_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VN30_IndexValue) - m_LIH.Data[0].VN30).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VN30_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VN30_Change) / m_LIH.Data[0].VN30) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VN100 = arr_VN100Index[arr_VN100Index.Length - 1];
                this.m_arrsttFullRowIndex.VN100_Time = VN100.Time.ToString();
                this.m_arrsttFullRowIndex.VN100_IndexValue = (VN100.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN100_TotalSharesAOM = VN100.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VN100_TotalSharesPT = VN100.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VN100_TotalValuesAOM = (VN100.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN100_TotalValuesPT = (VN100.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VN100_Up = VN100.Up.ToString();
                this.m_arrsttFullRowIndex.VN100_Down = VN100.Down.ToString();
                this.m_arrsttFullRowIndex.VN100_NoChange = VN100.NoChange.ToString();
                this.m_arrsttFullRowIndex.VN100_Ceiling = VN100.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VN100_Floor = VN100.Floor.ToString();
                this.m_arrsttFullRowIndex.VN100_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VN100_IndexValue) - m_LIH.Data[0].VN100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VN100_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VN100_Change) / m_LIH.Data[0].VN100) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNAll = arr_VNAllIndex[arr_VNAllIndex.Length - 1];
                this.m_arrsttFullRowIndex.VNALL_Time = VNAll.Time.ToString();
                this.m_arrsttFullRowIndex.VNALL_IndexValue = (VNAll.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNALL_TotalSharesAOM = VNAll.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNALL_TotalSharesPT = VNAll.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNALL_TotalValuesAOM = (VNAll.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNALL_TotalValuesPT = (VNAll.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNALL_Up = VNAll.Up.ToString();
                this.m_arrsttFullRowIndex.VNALL_Down = VNAll.Down.ToString();
                this.m_arrsttFullRowIndex.VNALL_NoChange = VNAll.NoChange.ToString();
                this.m_arrsttFullRowIndex.VNALL_Ceiling = VNAll.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VNALL_Floor = VNAll.Floor.ToString();
                this.m_arrsttFullRowIndex.VNALL_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNALL_IndexValue) - m_LIH.Data[0].VNALL).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNALL_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNALL_Change) / m_LIH.Data[0].VNALL) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNXAll = arr_VNXAllIndex[arr_VNXAllIndex.Length - 1];
                this.m_arrsttFullRowIndex.VNXALL_Time = VNXAll.Time.ToString();
                this.m_arrsttFullRowIndex.VNXALL_IndexValue = (VNXAll.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNXALL_TotalSharesAOM = VNXAll.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNXALL_TotalSharesPT = VNXAll.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNXALL_TotalValuesAOM = (VNXAll.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNXALL_TotalValuesPT = (VNXAll.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNXALL_Up = VNXAll.Up.ToString();
                this.m_arrsttFullRowIndex.VNXALL_Down = VNXAll.Down.ToString();
                this.m_arrsttFullRowIndex.VNXALL_NoChange = VNXAll.NoChange.ToString();
                this.m_arrsttFullRowIndex.VNXALL_Ceiling = VNXAll.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VNXALL_Floor = VNXAll.Floor.ToString();
                this.m_arrsttFullRowIndex.VNXALL_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNXALL_IndexValue) - m_LIH.Data[0].VNXALL).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNXALL_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNXALL_Change) / m_LIH.Data[0].VNXALL) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNMid = arr_VNMidIndex[arr_VNMidIndex.Length - 1];
                this.m_arrsttFullRowIndex.VNMID_Time = VNMid.Time.ToString();
                this.m_arrsttFullRowIndex.VNMID_IndexValue = (VNMid.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNMID_TotalSharesAOM = VNMid.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNMID_TotalSharesPT = VNMid.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNMID_TotalValuesAOM = (VNMid.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNMID_TotalValuesPT = (VNMid.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNMID_Up = VNMid.Up.ToString();
                this.m_arrsttFullRowIndex.VNMID_Down = VNMid.Down.ToString();
                this.m_arrsttFullRowIndex.VNMID_NoChange = VNMid.NoChange.ToString();
                this.m_arrsttFullRowIndex.VNMID_Ceiling = VNMid.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VNMID_Floor = VNMid.Floor.ToString();
                this.m_arrsttFullRowIndex.VNMID_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNMID_IndexValue) - m_LIH.Data[0].VNMID).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNMID_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNMID_Change) / m_LIH.Data[0].VNMID) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNSml = arr_VNSmlIndex[arr_VNSmlIndex.Length - 1];
                this.m_arrsttFullRowIndex.VNSML_Time = VNSml.Time.ToString();
                this.m_arrsttFullRowIndex.VNSML_IndexValue = (VNSml.IndexValue / 100.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNSML_TotalSharesAOM = VNSml.TotalSharesAOM.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNSML_TotalSharesPT = VNSml.TotalSharesPT.ToString(CConfig.FORMAT_INDEX_QTTY);
                this.m_arrsttFullRowIndex.VNSML_TotalValuesAOM = (VNSml.TotalValuesAOM / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNSML_TotalValuesPT = (VNSml.TotalValuesPT / 1000.0).ToString(CConfig.FORMAT_INDEX_VALUE);
                this.m_arrsttFullRowIndex.VNSML_Up = VNSml.Up.ToString();
                this.m_arrsttFullRowIndex.VNSML_Down = VNSml.Down.ToString();
                this.m_arrsttFullRowIndex.VNSML_NoChange = VNSml.NoChange.ToString();
                this.m_arrsttFullRowIndex.VNSML_Ceiling = VNSml.Ceiling.ToString();
                this.m_arrsttFullRowIndex.VNSML_Floor = VNSml.Floor.ToString();
                this.m_arrsttFullRowIndex.VNSML_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNSML_IndexValue) - m_LIH.Data[0].VNSML).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNSML_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNSML_Change) / m_LIH.Data[0].VNSML) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                CGlobal.INAV INAV = arr_INAV[arr_INAV.Length - 1];
                this.m_arrsttFullRowIndex.INAV_iNAV = INAV.iNAV.ToString();
                this.m_arrsttFullRowIndex.INAV_StockNo = INAV.StockNo.ToString();
                this.m_arrsttFullRowIndex.INAV_StockSymbol = /*new string(INAV.StockSymbol).Trim();*/CConfig.Char2String(INAV.StockSymbol);
                this.m_arrsttFullRowIndex.INAV_Time = INAV.Time.ToString();

                CGlobal.IINDEX IINDEX = arr_IINDEX[arr_IINDEX.Length - 1];
                this.m_arrsttFullRowIndex.IINDEX_ETFSymbol = IINDEX.ETFSymbol.ToString(); /*new string(IINDEX.ETFSymbol).Trim();*/
                this.m_arrsttFullRowIndex.IINDEX_iIndex = IINDEX.iIndex.ToString();
                this.m_arrsttFullRowIndex.IINDEX_IndexSymbol = IINDEX.IndexSymbol.ToString(); /*new string(IINDEX.IndexSymbol).Trim();*/
                this.m_arrsttFullRowIndex.IINDEX_iIndexSymbol = IINDEX.Time.ToString();
                this.m_arrsttFullRowIndex.IINDEX_Time = IINDEX.Time.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError(" S5G_ET_INDEX a error occurred: " + ex.Message);
            }
            return m_arrsttFullRowIndex;
        }
    }
}
