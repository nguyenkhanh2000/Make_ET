using Make_ET.Log;
using Make_ET.Mail;
using Make_ET.Oracle;
using Make_ET.Redis;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static Make_ET.DataModels.CGlobal;

namespace Make_ET.DataModels
{
    public class ReadFile
    {          
        CGlobal.FULL_ROW_QUOTE[] m_arrsttFullRowQuote;
        CGlobal.FULL_ROW_PT m_arrstt_ROWPT;
        CGlobal.FULL_ROW_INDEX m_arrsttFullRowIndex;
        CGlobal.LastIndexHO m_LIH;

        CGlobal.FULL_PUT_AD[] m_arrstt_PUT_AD_BUY;
        CGlobal.FULL_PUT_AD[] m_arrstt_PUT_AD_SELL;
        CGlobal.FULL_PUT_EXEC[] m_arrstt_PUT_EXEC;
        CGlobal.FULL_PUT_EXEC[] m_arrstt_PUT_DC;

        public CreaderAll<CGlobal.LS> arrsttLS;
        public CreaderAll<CGlobal.FROOM> arrsttFROOM;
        /*public CreaderF<CGlobal.SECURITY> m_crfSECURITY;*/        // groupQUOTE: da read => build full row => update redis chart (ZSet)
        public CreaderAll<CGlobal.SECURITY> m_crfSECURITY;
        public CreaderAll<CGlobal.SECURITYOL> m_crfSECURITYOL;
        public CreaderAll<CGlobal.LS> m_crfLS;
        public CreaderAll<CGlobal.LO> m_crfLO;
        public CreaderAll<CGlobal.FROOM> m_crfFROOM;
        public CreaderAll<CGlobal.LE> m_crfLE;
        public CreaderAll<CGlobal.OS> m_crfOS;
        public CreaderAll<CGlobal.MARKET_STAT> m_crfMARKET_STAT;

        public CreaderAll<CGlobal.PUT_AD> m_crfPUT_AD;
        public CreaderAll<CGlobal.PUT_EXEC> m_crfPUT_EXEC;
        public CreaderAll<CGlobal.PUT_DC> m_crfPUT_DC;

        public CreaderAll<CGlobal.INAV> m_crdINAV;
        public CreaderAll<CGlobal.IINDEX> m_crdIINDEX;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVN30List;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVNIndexList;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVN100List;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVNAllList;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVNMidList;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVNSmlList;
        public CreaderAll<CGlobal.VNX_MARKET_LIST> m_crdVNXAllList;

        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVN30Index;
        public CreaderAll<CGlobal.VNX_MARKET_VNINDEX> m_crdVNIndex;
        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVN100Index;
        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVNAllIndex;
        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVNMidIndex;
        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVNSmlIndex;
        public CreaderAll<CGlobal.VNX_MARKET_INDEX> m_crdVNXAllIndex;

        //public CreaderAll<CGlobal.VNX_MARKET_VNINDEX> m_crfVNX_MARKET_VNINDEX;

        private const int VALUE_LENGTH_MAX = 10000;
        public string path;

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,
                 string key, string def, StringBuilder retVal,
            int size, string filePath);
        public ReadFile()
        {
            IniConfig();

        }
        private void IniConfig()
        {
            Logger.LogInfo("IniConfig");
            this.m_crfSECURITY = new CreaderAll<CGlobal.SECURITY>("");
            this.m_crfSECURITYOL = new CreaderAll<CGlobal.SECURITYOL>("");
            //this.m_crfMARKET_STAT = new CreaderF<CGlobal.MARKET_STAT>("");
            this.m_crfLS = new CreaderAll<CGlobal.LS>("");
            this.m_crfLO = new CreaderAll<CGlobal.LO>("");
            this.m_crfLE = new CreaderAll<CGlobal.LE>("");
            this.m_crfOS = new CreaderAll<CGlobal.OS>("");
            this.m_crfFROOM = new CreaderAll<CGlobal.FROOM>("");
            this.m_crfPUT_EXEC = new CreaderAll<CGlobal.PUT_EXEC>("");
            this.m_crfPUT_AD = new CreaderAll<PUT_AD>("");
            this.m_crfPUT_DC = new CreaderAll<PUT_DC>("");
            //this.m_crfVNX_MARKET_VNINDEX = new CreaderAll<CGlobal.VNX_MARKET_VNINDEX>("");
            this.m_crfMARKET_STAT = new CreaderAll<CGlobal.MARKET_STAT>("");
            this.m_crdINAV = new CreaderAll<CGlobal.INAV>("");
            this.m_crdIINDEX = new CreaderAll<CGlobal.IINDEX>("");
            this.m_crdVN30List = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVNIndexList = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVN100List = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVNAllList = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVNMidList = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVNSmlList = new CreaderAll<VNX_MARKET_LIST>("");
            this.m_crdVNXAllList = new CreaderAll<VNX_MARKET_LIST>("");

            this.m_crdVN30Index = new CreaderAll<VNX_MARKET_INDEX>("");
            this.m_crdVNIndex = new CreaderAll<VNX_MARKET_VNINDEX>("");
            this.m_crdVN100Index = new CreaderAll<VNX_MARKET_INDEX>("");
            this.m_crdVNAllIndex = new CreaderAll<VNX_MARKET_INDEX>("");
            this.m_crdVNMidIndex = new CreaderAll<VNX_MARKET_INDEX>("");
            this.m_crdVNSmlIndex = new CreaderAll<VNX_MARKET_INDEX>("");
            this.m_crdVNXAllIndex = new CreaderAll<VNX_MARKET_INDEX>("");
            //MARKET_STAT
            this.m_crfMARKET_STAT.FileName = "MARKET_STAT";
            this.m_crfMARKET_STAT.FilePath = Path.Combine(CConfig.directoryPath, m_crfMARKET_STAT.FileName + ".dat"); //@"D:\FPTS_Test\BACKUP28\MARKET_STAT.dat"
            //LS
            this.m_crfLS.FileName = "LS";
            this.m_crfLS.FilePath = Path.Combine(CConfig.directoryPath, m_crfLS.FileName + ".dat"); //@"D:\FPTS_Test\BACKUP28\LS.dat"
            //LO
            this.m_crfLO.FileName = "LO";
            this.m_crfLO.FilePath = Path.Combine(CConfig.directoryPath, m_crfLO.FileName = "LO" + ".dat");               //@"D:\FPTS_Test\BACKUP28\LO.dat";
            //SECURITY
            this.m_crfSECURITY.FileName = /*this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_FILENAME);// SECURITY*/   "SECURITY";
            this.m_crfSECURITY.RedisKeyListCode = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_REDISKEYLISTCODE);// "S5G_OTHER_HO_LIST_CODE";            
            this.m_crfSECURITY.RedisKeyListCodeID = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_REDISKEYLISTCODEID);//"S5G_OTHER_HO_LIST_CODEID";         
            this.m_crfSECURITY.FilePath = Path.Combine(CConfig.directoryPath, m_crfSECURITY.FileName + ".dat");                                 /*this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_FILEPATH); */ //@"D:\FPTS_Test\BACKUP28\SECURITY.dat";
            this.m_crfSECURITY.SkipPropertyName = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_SKIPPROPERTYNAME);//"Ceiling";    // bo qua dong thua ETF
            this.m_crfSECURITY.ColumnListJSON = /*this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_COLUMNLISTJSON);//*/ "StockSymbol|1,Ceiling|100,Floor|100,PriorClosePrice|100,Best3Bid|100,Best3BidVolume|1,Best2Bid|100,Best2BidVolume|1,Best1Bid|100,Best1BidVolume|1,ProjectOpen|100,MatchedVol|1,MatchChange|1,Best1Offer|100,Best1OfferVolume|1,Best2Offer|100,Best2OfferVolume|1,Best3Offer|100,Best3OfferVolume|1,LastVol|1,OpenPrice|100,Highest|100,Lowest|100,CurrentRoom|1";
            this.m_crfSECURITY.ColumnListSQL = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_COLUMNLISTSQL);//"TranID,TranDate,TransDate,Stockno,StockSymbol,StockType,Ceiling,Floor,BigLotValue,SectorNo,Designated,SUSPENSION,Delist,HaltResumeFlag,SPLIT,Benefit,Meeting,Notice,ClientIDRequest,CouponRate,IssueDate,MatureDate,AvrPrice,ParValue,SDCFlag,PriorClosePrice,PriorCloseDate,ProjectOpen,OpenPrice,Last,LastVol,LastVal,Highest,Lowest,Totalshares,TotalValue,AccumulateDeal,BigDeal,BigVolume,BigValue,OddDeal,OddVolume,OddValue,Best1Bid,Best1BidVolume,Best2Bid,Best2BidVolume,Best3Bid,Best3BidVolume,Best1Offer,Best1OfferVolume,Best2Offer,Best2OfferVolume,Best3Offer,Best3OfferVolume,BoardLost,msrepl_tran_version,SecurityNumberOld,SecurityNumberNew,LastQtty,VWAP,Date";// bo qua SecurityName, ko truyen value
            this.m_crfSECURITY.PropertyNameOfSymbolField = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_PROPERTYNAMEOFSYMBOLFIELD);//"StockSymbol";
            this.m_crfSECURITY.PropertyNameOfSymbolIDField = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_PROPERTYNAMEOFSYMBOLIDFIELD);//"StockNo";
            this.m_crfSECURITY.IDPropertyName = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_IDPROPERTYNAME);//"StockNo";
            this.m_crfSECURITY.TemplateJsonFull = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_TEMPLATEJSONFULL);//"{\"RowID\":\"(CodeID)\",\"Info\":[(AllElements)]}";
            this.m_crfSECURITY.TemplateJsonElement = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_TEMPLATEJSONELEMENT);//"[(Index),(NewValue)]";
            this.m_crfSECURITY.TemplateSqlFull = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_TEMPLATESQLFULL);//"EXEC prc_5G_QUOTE_FEEDER_HO_PRS_UPDATE_SECURITY (AllElements)";
            this.m_crfSECURITY.TemplateSqlElement = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_TEMPLATESQLELEMENT);//"@(ColumnName)='(NewValue)'";
            this.m_crfSECURITY.JSONFilterPropertyName = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_JSONFILTERPROPERTYNAME);//"StockType";
            this.m_crfSECURITY.JSONFilterValidValue = this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_JSONFILTERVALIDVALUE);//"S,U,E";
            this.m_crfSECURITY.IsBuildUpdateJSON = /*Convert.ToBoolean(this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_ISBUILDUPDATEJSON));*/true;
            this.m_crfSECURITY.IsBuildUpdateSQL = /*Convert.ToBoolean(this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_ISBUILDUPDATESQL));//*/true;
            this.m_crfSECURITY.SkipValueBigger = /*Convert.ToInt32(this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_SKIPVALUEBIGGER));*/ 99999;        // bo qua dong thua ETF
            this.m_crfSECURITY.ThreadID = /*Convert.ToInt32(this.IniReadValue(CConfig.INI_SECTION_SECURITY, CConfig.INI_KEY_THREADID));*/ 1000;

            //SECURITYOL
            this.m_crfSECURITYOL.FileName = "SECURITYOL";
            this.m_crfSECURITYOL.FilePath = Path.Combine(CConfig.directoryPath, m_crfSECURITYOL.FileName + ".dat");                    //@"D:\FPTS_Test\BACKUP28\SECURITYOL.dat";

            //LE
            this.m_crfLE.FileName = "LE";
            this.m_crfLE.FilePath = Path.Combine(CConfig.directoryPath, m_crfLE.FileName + ".dat");                            //@"D:\FPTS_Test\BACKUP28\LE.dat";
            //OS
            this.m_crfOS.FileName = "OS";
            this.m_crfOS.FilePath = Path.Combine(CConfig.directoryPath, m_crfOS.FileName + ".dat");                         //@"D:\FPTS_Test\BACKUP28\OS.dat";
            //FROOM
            this.m_crfFROOM.FileName = "FROOM";
            this.m_crfFROOM.FilePath = Path.Combine(CConfig.directoryPath, m_crfFROOM.FileName + ".dat");                   //@"D:\FPTS_Test\BACKUP28\FROOM.dat";
            //PUT_AD
            this.m_crfPUT_AD.FileName = "PUT_AD";
            this.m_crfPUT_AD.FilePath = Path.Combine(CConfig.directoryPath, m_crfPUT_AD.FileName + ".dat");                 //@"D:\FPTS_Test\BACKUP28\PUT_AD.dat";
            //PUT_EXEC 
            this.m_crfPUT_EXEC.FileName = "PUT_EXEC";
            this.m_crfPUT_EXEC.FilePath = Path.Combine(CConfig.directoryPath, m_crfPUT_EXEC.FileName + ".dat");              //@"D:\FPTS_Test\BACKUP28\PUT_EXEC.dat";
            //PUT_DC
            this.m_crfPUT_DC.FileName = "PUT_DC";
            this.m_crfPUT_DC.FilePath = Path.Combine(CConfig.directoryPath, m_crfPUT_DC.FileName + ".dat");                  //@"D:\FPTS_Test\BACKUP28\PUT_DC.dat";       
            //INAV
            this.m_crdINAV.FileName = "(yyyy)(MM)(dd)_INAV";
            this.m_crdINAV.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdINAV.FileName + ".dat");                      //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_INAV.dat";
            //IINDEX
            this.m_crdIINDEX.FileName = "(yyyy)(MM)(dd)_IINDEX";
            this.m_crdIINDEX.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdIINDEX.FileName + ".dat");                   //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_IINDEX.dat";
            //YYYYMMDD_VN30
            this.m_crdVN30Index.FileName = "(yyyy)(MM)(dd)_VN30";
            this.m_crdVN30Index.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVN30Index.FileName + ".dat");               //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VN30.dat";
            //YYYYMMDD_VNINDEX
            this.m_crdVNIndex.FileName = "(yyyy)(MM)(dd)_VNINDEX";
            this.m_crdVNIndex.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVNIndex.FileName + ".dat");                   //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VNINDEX.dat";
            //YYYYMMDD_VN100
            this.m_crdVN100Index.FileName = "(yyyy)(MM)(dd)_VN100";
            this.m_crdVN100Index.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVN100Index.FileName + ".dat");                    //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VN100.dat";
            //YYYYMMDD_VNALL
            this.m_crdVNAllIndex.FileName = "(yyyy)(MM)(dd)_VNALL";
            this.m_crdVNAllIndex.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVNAllIndex.FileName + ".dat");                      //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VNALL.dat";
            //YYYYMMDD_VNMID
            this.m_crdVNMidIndex.FileName = "(yyyy)(MM)(dd)_VNMID";
            this.m_crdVNMidIndex.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVNMidIndex.FileName + ".dat");                      //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VNMID.dat";
            //YYYYMMDD_VNSML
            this.m_crdVNSmlIndex.FileName = "(yyyy)(MM)(dd)_VNSML";
            this.m_crdVNSmlIndex.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVNSmlIndex.FileName + ".dat");                   //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VNSML.dat";
            //YYYYMMDD_VNXALL
            this.m_crdVNXAllIndex.FileName = "(yyyy)(MM)(dd)_VNXALL";
            this.m_crdVNXAllIndex.FilePath = Path.Combine(CConfig.directoryPath_Index, m_crdVNXAllIndex.FileName + ".dat");                   //@"D:\FPTS_Test\FPT_VNX\(yyyy)(MM)(dd)_VNXALL.dat";
        }
        public void Update_FULL()
        {
            Logger.LogInfo("Update_FULL_Redis()");
            try
            {
                this.Read_LAST_INDEX_HO();

                S5G_ET_INDEX et_index = new S5G_ET_INDEX();
                m_arrsttFullRowIndex = et_index.UpdateFULL_ROW_INDEX(m_LIH, m_crfMARKET_STAT.DataUpdate, m_crdVNIndex.DataUpdate, m_crdVN30Index.DataUpdate,
                    m_crdVN100Index.DataUpdate, m_crdVNAllIndex.DataUpdate, m_crdVNXAllIndex.DataUpdate, m_crdVNMidIndex.DataUpdate,
                    m_crdVNSmlIndex.DataUpdate, m_crdINAV.DataUpdate, m_crdIINDEX.DataUpdate);

                S5G_ET_QUOTE et_quote = new S5G_ET_QUOTE();
                m_arrsttFullRowQuote = et_quote.UpdateFullRowQuote(this.m_crfSECURITY.DataUpdate, this.m_crfLS.DataUpdate, this.m_crfOS.DataUpdate, this.m_crfFROOM.DataUpdate , m_arrsttFullRowIndex);
                S5G_ET_PT et_pt = new S5G_ET_PT();
                m_arrstt_ROWPT = et_pt.UpdateFULL_ROW_PT(m_arrsttFullRowQuote,this.m_crfSECURITY.DataUpdate, this.m_crfPUT_AD.DataUpdate, this.m_crfPUT_EXEC.DataUpdate, this.m_crfPUT_DC.DataUpdate);

                //this.UpdateFULL_ROW_INDEX();
                //this.UpdateFullRowQuote(this.m_crfSECURITY.DataUpdate, this.m_crfLS.DataUpdate, this.m_crfOS.DataUpdate, this.m_crfFROOM.DataUpdate);
                //this.UpdateFULL_ROW_PT(this.m_crfPUT_AD.DataUpdate, this.m_crfSECURITY.DataUpdate, this.m_crfPUT_EXEC.DataUpdate, this.m_crfPUT_DC.DataUpdate);
                this.Oracle_STOCK_HCM();
                this.Redis_S5G_ET_PT();
                //this.S5G_ET_QUOTE();
                this.Redis_S5G_ET_QUOTE();
                this.Redis_S5G__ET_INDEX();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            finally
            {

            }
        }        
        //public void Thread_QUOTE_SECURITY()
        //{
        //    try
        //    {

        //        // read file
        //        //this.m_crfSECURITY.ReadFile();
        //        // update full row
        //        //this.Update(this.m_crfSECURITY.DataNew);
        //        //this.m_crfMARKET_STAT.ReadFile();

        //        this.UpdateFullRowQuote(this.m_crfSECURITY.DataNew, this.m_crfLS.DataNew, this.m_crfOS.DataNew, this.m_crfFROOM.DataNew);
        //        //this.m_crfSECURITYOL.ReadFileBig();

        //        //this.m_crfLE.ReadFileBig();
        //        //this.m_crfLS.ReadFileBig();
        //        //this.m_crfLO.ReadFileBig();
        //        //this.m_crfOS.ReadFileBig();
        //        //this.m_crfFROOM.ReadFileBig();
        //        //this.m_crfPUT_EXEC.ReadFileBig();
        //    }
        //    catch (Exception ex)
        //    {
        //        // CLog.LogEx(CLog.GetLogExFileName(this.m_crfSECURITY.ThreadID, CConfig.LOGEX_ERROR_TYPE, CConfig.LOGEX_ERROR_EXT), CBase.GetDeepCaller() + " => " + ex.Message);
        //        Logger.LogError("An error occurred: " + ex.Message);
        //    }
        //    finally
        //    {

        //    }

        //}
        
        public async Task Thread_QUOTE_LSAsync()
        {
            Logger.LogInfo("Thread_QUOTE_LSAsync");
            try
            {
                await Task.Run(() => this.m_crfLS.ReadFileBig());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task Thread_QUOTE_LOAsync()
        {
            Logger.LogInfo("Thread_QUOTE_LOAsync");
            try
            {
                await Task.Run(() => this.m_crfLO.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_SECURITYAsync()
        {
            Logger.LogInfo("Thread_QUOTE_SECURITYAsync");
            try
            {
                await Task.Run(() => this.m_crfSECURITY.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_MARKET_STATAsync()
        {
            Logger.LogInfo("Thread_QUOTE_MARKET_STATAsync");
            try
            {
                await Task.Run(() => this.m_crfMARKET_STAT.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_SECURITYOLAsync()
        {
            Logger.LogInfo("Thread_QUOTE_SECURITYOLAsync");
            try
            {
                await Task.Run(() => this.m_crfSECURITYOL.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_LEAsync()
        {
            Logger.LogInfo("Thread_QUOTE_LEAsync");
            try
            {
                await Task.Run(() => this.m_crfLE.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_OSAsync()
        {
            Logger.LogInfo("Thread_QUOTE_OSAsync");
            try
            {
                await Task.Run(() => this.m_crfOS.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_FROOMAsync()
        {
            Logger.LogInfo("Thread_QUOTE_FROOMAsync");
            try
            {
                await Task.Run(() => this.m_crfFROOM.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_PUT_ADAsync()
        {
            Logger.LogInfo("Thread_QUOTE_PUT_ADAsync");
            try
            {
                await Task.Run(() => this.m_crfPUT_AD.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_PUT_EXECAsync()
        {
            Logger.LogInfo("Thread_QUOTE_PUT_EXECAsync");
            try
            {
                await Task.Run(() => this.m_crfPUT_EXEC.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_QUOTE_PUT_DCAsync()
        {
            Logger.LogInfo("Thread_QUOTE_PUT_DCAsync");
            try
            {
                await Task.Run(() => this.m_crfPUT_DC.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        //public async Task Thread_VNX_MARKET_INDEXAsync()
        //{
        //    try
        //    {
        //        await Task.Run(() => this.m_crfVNX_MARKET_VNINDEX.ReadFileBig());
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public async Task Thread_VNX_INAVAsync()
        {
            Logger.LogInfo("Thread_VNX_INAVAsync");
            try
            {
                await Task.Run(() => this.m_crdINAV.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_IINDEXAsync()
        {
            Logger.LogInfo("Thread_VNX_IINDEXAsync");
            try
            {
                await Task.Run(() => this.m_crdIINDEX.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VN30Async()
        {
            Logger.LogInfo("Thread_VNX_VN30Async");
            try
            {
                await Task.Run(() => this.m_crdVN30Index.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VNINDEXAsync()
        {
            Logger.LogInfo("Thread_VNX_VNINDEXAsync");
            try
            {
                await Task.Run(() => this.m_crdVNIndex.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VN100Async()
        {
            Logger.LogInfo("Thread_VNX_VN100Async");
            try
            {
                await Task.Run(() => this.m_crdVN100Index.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VNALLAsync()
        {
            Logger.LogInfo("Thread_VNX_VNALLAsync");
            try
            {
                await Task.Run(() => this.m_crdVNAllIndex.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VNMIDAsync()
        {
            Logger.LogInfo("Thread_VNX_VNMIDAsync");
            try
            {
                await Task.Run(() => this.m_crdVNMidIndex.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VNSMLAsync()
        {
            Logger.LogInfo("Thread_VNX_VNSMLAsync");
            try
            {
                await Task.Run(() => this.m_crdVNSmlIndex.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
        public async Task Thread_VNX_VNXALLAsync()
        {
            Logger.LogInfo("Thread_VNX_VNXALLAsync");
            try
            {
                await Task.Run(() => this.m_crdVNXAllIndex.ReadFileBig());
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
        }
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
        private string GetRightStatus(CGlobal.SECURITY stt)
        {
            Logger.LogInfo("GetRightStatus");
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
        public bool Update(CGlobal.SECURITY[] arrsttSECURITY)
        {
            if (arrsttSECURITY != null)
            {
                //xử lý 
            }
            return true;
        }
        public bool UpdateFullRowQuote(CGlobal.SECURITY[] arrsttSECURITY, CGlobal.LS[] arrsttLS, CGlobal.OS[] arrsttOS, CGlobal.FROOM[] arrsttFROOM)
        {
            Logger.LogInfo("UpdateFullRowQuote");
            int intCe = 0, intFl = 0;           //count tổng số mã có giá khớp = > < so vs tc
            try
            {
                //init
                if (m_arrsttFullRowQuote == null && arrsttSECURITY != null)
                {
                    Array.Resize(ref this.m_arrsttFullRowQuote, arrsttSECURITY.Length);
                }
                //chưa init, exit
                if (this.m_arrsttFullRowQuote == null) return false;
                //update SECURITY   
                if (arrsttSECURITY != null)
                {
                    for (int i = 0; i < arrsttSECURITY.Length; i++)
                    {
                        this.m_arrsttFullRowQuote[i].Co = CConfig.Char2String(arrsttSECURITY[i].StockSymbol);            // 00 - code
                        //this.m_arrsttFullRowQuote[i].Co = new string(arrsttSECURITY[i].StockSymbol).Trim();
                        this.m_arrsttFullRowQuote[i].Re = arrsttSECURITY[i].PriorClosePrice.ToString();                  // 01 - ref
                        this.m_arrsttFullRowQuote[i].Ce = arrsttSECURITY[i].Ceiling.ToString();                          // 02 - ceiling
                        this.m_arrsttFullRowQuote[i].Fl = arrsttSECURITY[i].Floor.ToString();                            // 03 - floor

                        this.m_arrsttFullRowQuote[i].BP3 = arrsttSECURITY[i].Best3Bid.ToString();                        // 05 - buy price 3
                        this.m_arrsttFullRowQuote[i].BQ3 = arrsttSECURITY[i].Best3BidVolume.ToString();                  // 06 - buy quantity 3
                        this.m_arrsttFullRowQuote[i].BP2 = arrsttSECURITY[i].Best2Bid.ToString();                        // 07 - buy price 2
                        this.m_arrsttFullRowQuote[i].BQ2 = arrsttSECURITY[i].Best2BidVolume.ToString();                  // 08 - buy quantity 2
                        this.m_arrsttFullRowQuote[i].BP1 = arrsttSECURITY[i].Best1Bid.ToString(); /*this.Get Price(arrsttSECURITY[i].Best1Bid, arrsttSECURITY[i].Best1BidVolume, this.m_sttFullRowIndex.STAT_ControlCode);*///09 - buy price 1 (arrsttSECURITY[i].Best1Bid.ToString();)
                        this.m_arrsttFullRowQuote[i].BQ1 = arrsttSECURITY[i].Best1BidVolume.ToString();                  // 10 - buy quantity 1

                        // phien ATO/ATC
                        if (this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATO || this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATC)
                        {
                            if (this.m_arrsttFullRowQuote[i].MP != null && arrsttSECURITY[i].ProjectOpen == 0)
                                if (Convert.ToDouble(this.m_arrsttFullRowQuote[i].MP) > 0)
                                    this.m_arrsttFullRowQuote[i].MPO = this.m_arrsttFullRowQuote[i].MP;
                            if (this.m_arrsttFullRowQuote[i].MQ != null && arrsttSECURITY[i].ProjectOpen == 0)
                                if (Convert.ToDouble(this.m_arrsttFullRowQuote[i].MQ) > 0)
                                    this.m_arrsttFullRowQuote[i].MQO = this.m_arrsttFullRowQuote[i].MQ;

                            // chi gan TQ vao TQO 1 lan vao luc bat dau phien ATC
                            if (this.m_arrsttFullRowQuote[i].TQO == "0" && this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_ATC)
                                this.m_arrsttFullRowQuote[i].TQO = this.m_arrsttFullRowQuote[i].TQ;

                            this.m_arrsttFullRowQuote[i].MP = arrsttSECURITY[i].ProjectOpen.ToString();          // 11 - match price [LS]
                            this.m_arrsttFullRowQuote[i].MQ = "0";     // 12 - match quantity  [LS]
                            this.m_arrsttFullRowQuote[i].MC = this.m_arrsttFullRowQuote[i].MP == null || this.m_arrsttFullRowQuote[i].MP == "0" ? "0" : (Convert.ToSingle(this.m_arrsttFullRowQuote[i].MP) - Convert.ToSingle(this.m_arrsttFullRowQuote[i].Re)).ToString();    // 13 - match change [calculate]

                            if (Convert.ToDouble(this.m_arrsttFullRowQuote[i].MP) == Convert.ToDouble(this.m_arrsttFullRowQuote[i].Ce)) ++intCe;
                            if (Convert.ToDouble(this.m_arrsttFullRowQuote[i].MP) == Convert.ToDouble(this.m_arrsttFullRowQuote[i].Fl)) ++intFl;
                        }
                        // phien lien tuc + phien thoa thuan [14:45-15:00]
                        if (this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CON || this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO)
                        {
                            //if (this.m_sttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO && arrsttSECURITY[i].ProjectOpen.ToString()=="0")
                            if (this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO)
                            {
                                // 2015-06-29 17:00:11 huyNQ 2015-06-30 15:07:08 ngocta2
                                if (this.m_arrsttFullRowQuote[i].MPO != null)
                                    this.m_arrsttFullRowQuote[i].MP = this.m_arrsttFullRowQuote[i].MPO;
                                if (this.m_arrsttFullRowQuote[i].MQO != null)
                                    this.m_arrsttFullRowQuote[i].MQ = this.m_arrsttFullRowQuote[i].MQO;
                                this.m_arrsttFullRowQuote[i].MC = this.m_arrsttFullRowQuote[i].MP == null || this.m_arrsttFullRowQuote[i].MP == "0" ? "0" : (Convert.ToSingle(this.m_arrsttFullRowQuote[i].MP) - Convert.ToSingle(this.m_arrsttFullRowQuote[i].Re)).ToString();    // 13 - match change [calculate]
                            }
                            if (Convert.ToInt32(arrsttSECURITY[i].Last) == Convert.ToInt32(this.m_arrsttFullRowQuote[i].Ce)) ++intCe;
                            if (Convert.ToInt32(arrsttSECURITY[i].Last) == Convert.ToInt32(this.m_arrsttFullRowQuote[i].Fl)) ++intFl;
                        }

                        //this.m_arrsttFullRowQuote[i].SP1 = this.GetPrice(arrsttSECURITY[i].Best1Offer, arrsttSECURITY[i].Best1OfferVolume, this.m_sttFullRowIndex.STAT_ControlCode);// 14 - sell price 1 (arrsttSECURITY[i].Best1Offer.ToString())
                        this.m_arrsttFullRowQuote[i].SP1 = arrsttSECURITY[i].Best1Offer.ToString();
                        this.m_arrsttFullRowQuote[i].SQ1 = arrsttSECURITY[i].Best1OfferVolume.ToString();                // 15 - sell quantity 1
                        this.m_arrsttFullRowQuote[i].SP2 = arrsttSECURITY[i].Best2Offer.ToString();                      // 16 - sell price 2
                        this.m_arrsttFullRowQuote[i].SQ2 = arrsttSECURITY[i].Best2OfferVolume.ToString();                // 17 - sell quantity 2
                        this.m_arrsttFullRowQuote[i].SP3 = arrsttSECURITY[i].Best3Offer.ToString();                      // 18 - sell price 3
                        this.m_arrsttFullRowQuote[i].SQ3 = arrsttSECURITY[i].Best3OfferVolume.ToString();

                        this.m_arrsttFullRowQuote[i].TQ = arrsttSECURITY[i].LastVol.ToString();                          // 21 - total quantity (NM)
                        if (this.m_arrsttFullRowQuote[i].Op == "" || this.m_arrsttFullRowQuote[i].Op == "0" || this.m_arrsttFullRowQuote[i].Op == null)            //2015-12-21 09:38:04 ngocta2
                            this.m_arrsttFullRowQuote[i].Op = arrsttSECURITY[i].OpenPrice.ToString(); 						 // 22 - open (NM)   [OS=>SECURITY]
                        this.m_arrsttFullRowQuote[i].Hi = arrsttSECURITY[i].Highest.ToString();                          // 23 - highest (NM)
                        this.m_arrsttFullRowQuote[i].Lo = arrsttSECURITY[i].Lowest.ToString();
                        this.m_arrsttFullRowQuote[i].Av = arrsttSECURITY[i].AvrPrice.ToString();
                        this.m_arrsttFullRowQuote[i].SN = arrsttSECURITY[i].StockNo;                                     // 29 - hidden - StockNo
                        this.m_arrsttFullRowQuote[i].ST = CConfig.Char2String(arrsttSECURITY[i].StockType);             // 30 - hidden - StockType
                        //this.m_arrsttFullRowQuote[i].ST = new string(arrsttSECURITY[i].StockType).Trim();
                        this.m_arrsttFullRowQuote[i].PO = arrsttSECURITY[i].ProjectOpen.ToString();                      // 31 - hidden - ProjectOpen         
                        this.m_arrsttFullRowQuote[i].Ri = this.GetRightStatus(arrsttSECURITY[i]);                      // 32 - hidden - Rights 
                        if (this.m_arrsttFullRowQuote[i].TQO == null) this.m_arrsttFullRowQuote[i].TQO = "0";
                        
                        this.m_arrsttFullRowQuote[i].BQ4 = (Convert.ToSingle(this.m_arrsttFullRowQuote[i].TQ) -
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].BQ3) -
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].BQ2) -
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].BQ1)).ToString();
                        this.m_arrsttFullRowQuote[i].SQ4 = (Convert.ToSingle(this.m_arrsttFullRowQuote[i].TQ) +
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].SQ3) +
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].SQ2) +
                                    Convert.ToSingle(this.m_arrsttFullRowQuote[i].SQ1)).ToString();
                    }
                    
                }
                ///UPDATE LS (NEW)
                if (this.m_arrsttFullRowIndex.STAT_ControlCode != CGlobal.MARKET_STAT_ATO && this.m_arrsttFullRowIndex.STAT_ControlCode != CGlobal.MARKET_STAT_ATC)
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

                    for (int j = 0; j < this.m_arrsttFullRowQuote.Length; j++)
                    {
                        int indexLS = Array.LastIndexOf(stockNoLS, m_arrsttFullRowQuote[j].SN);
                        if (indexLS != -1)
                        {
                            if (this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CON) //phien lien tuc
                            {
                                this.m_arrsttFullRowQuote[j].MPO = arrsttLS[indexLS].Price.ToString();              // 11 - match price [LS]
                                this.m_arrsttFullRowQuote[j].MQO = arrsttLS[indexLS].MatchedVol.ToString();         // 12 - match quantity  [LS]
                            }
                            if (this.m_arrsttFullRowIndex.STAT_ControlCode == CGlobal.MARKET_STAT_CLO) //phien thoa thuan 14h45-15h00
                            {
                                this.m_arrsttFullRowQuote[j].MPO = arrsttLS[indexLS].Price.ToString();                  // 11 - match price [LS]
                                this.m_arrsttFullRowQuote[j].MQO = arrsttLS[indexLS].MatchedVol.ToString();             //12 - match quantity [LS]
                            }
                            this.m_arrsttFullRowQuote[j].MP = arrsttLS[indexLS].Price.ToString();
                            this.m_arrsttFullRowQuote[j].MQ = arrsttLS[indexLS].MatchedVol.ToString();

                            this.m_arrsttFullRowQuote[j].MC = (Convert.ToSingle(this.m_arrsttFullRowQuote[j].MP) - Convert.ToSingle(this.m_arrsttFullRowQuote[j].Re)).ToString(); //13-match change[calculate]                                                                                                                                                                                //break; //exit for j
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
                    for (int j = 0; j < this.m_arrsttFullRowQuote.Length; j++)
                    {
                        int indexFROOM = Array.LastIndexOf(stockNoFROOM, m_arrsttFullRowQuote[j].SN);
                        if (indexFROOM != -1)
                        {
                            //lock(m_crfOS) { }
                            this.m_arrsttFullRowQuote[j].FB = arrsttFROOM[indexFROOM].BuyVolume.ToString();                 //26 - foreign buy (quantity) [FROOM]
                            this.m_arrsttFullRowQuote[j].FS = arrsttFROOM[indexFROOM].SellVolume.ToString();                //27 - foreign sell (quantiry) [FROOM]
                            this.m_arrsttFullRowQuote[j].FR = arrsttFROOM[indexFROOM].CurrentRoom.ToString();               //28 - foreign room (current) [FROOM]
                        }
                    }
                }
            }
            catch (Exception ex)
            { 
                Logger.LogError("An error occurred: " + ex.Message); 
            }

            return true;
        }
        public bool UpdateFULL_ROW_PT(CGlobal.PUT_AD[] arrsttPUT_AD, CGlobal.SECURITY[] arrsttSECURITY, CGlobal.PUT_EXEC[] arrsttPUT_EXEC, CGlobal.PUT_DC[] arrsttPUT_DC)
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
                    if (m_arrstt_PUT_EXEC == null) return false;
                    for (int i = 0; i < arrsttPUT_EXEC.Length; i++)
                    {
                        int indexPUT_EXEC = Array.LastIndexOf(stockNoSECURITY, arrsttPUT_EXEC[i].StockNo);
                        m_arrstt_PUT_EXEC[i].Auto = (i + 1).ToString();
                        m_arrstt_PUT_EXEC[i].Code = m_arrsttFullRowQuote[indexPUT_EXEC].Co.ToString();
                        m_arrstt_PUT_EXEC[i].Re = m_arrsttFullRowQuote[indexPUT_EXEC].Re.ToString();
                        m_arrstt_PUT_EXEC[i].Ce = m_arrsttFullRowQuote[indexPUT_EXEC].Ce.ToString();
                        m_arrstt_PUT_EXEC[i].Fl = m_arrsttFullRowQuote[indexPUT_EXEC].Fl.ToString();
                        m_arrstt_PUT_EXEC[i].PTPrice = arrsttPUT_EXEC[i].Price.ToString();
                        m_arrstt_PUT_EXEC[i].PTQuantity = arrsttPUT_EXEC[i].Vol.ToString();
                        m_arrstt_PUT_EXEC[i].PTTotalQuantity = this.GetSum(arrsttPUT_EXEC[i].StockNo, arrsttPUT_EXEC, PUT_EXEC_SUM.TOTAL_QUANTITY).ToString();
                        m_arrstt_PUT_EXEC[i].PTTotalValue = this.GetSum(arrsttPUT_EXEC[i].StockNo, arrsttPUT_EXEC, PUT_EXEC_SUM.TOTAL_VALUE).ToString();
                        m_arrstt_PUT_EXEC[i].NMTotalQuantity = this.m_arrsttFullRowQuote[indexPUT_EXEC].TQ;
                        m_arrstt_PUT_EXEC[i].NMPTTotalQuantity = (Convert.ToInt64(m_arrstt_PUT_EXEC[i].PTTotalQuantity) + Convert.ToInt64(m_arrstt_PUT_EXEC[i].NMTotalQuantity)).ToString();
                        m_arrstt_PUT_EXEC[i].ListingQuantity = "0";
                        m_arrstt_PUT_EXEC[i].Time = "";
                        m_arrstt_PUT_EXEC[i].ConfirmNo = arrsttPUT_EXEC[i].ConfirmNo.ToString();
                    }
                }
                //UPDATE PUT_DC
                if (arrsttPUT_DC != null)
                {
                    if (m_arrstt_PUT_DC == null && arrsttPUT_DC != null)
                    {
                        Array.Resize(ref m_arrstt_PUT_DC, arrsttPUT_DC.Length);
                    }
                    if (m_arrstt_PUT_DC == null) return false;
                    for (int i = 0; i < arrsttPUT_DC.Length; i++)
                    {
                        // tim index cua Element trong this.m_arrsttFullRowQuote co SN=arrsttPUT_DC[i].StockNo
                        //int intIndex = Array.FindIndex(this.m_arrsttFullRowQuote, row => row.SN == arrsttPUT_DC[i].StockNo);
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
                }
                m_arrstt_ROWPT.PUT_AD_BUY = m_arrstt_PUT_AD_BUY;
                m_arrstt_ROWPT.PUT_AD_SELL = m_arrstt_PUT_AD_SELL;
                m_arrstt_ROWPT.PUT_EXEC = m_arrstt_PUT_EXEC;
                m_arrstt_ROWPT.PUT_DC = m_arrstt_PUT_DC;
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
            return true;
        }
        public bool UpdateFULL_ROW_INDEX()
        {
            Logger.LogInfo("UpdateFULL_ROW_INDEX");
            try
            {
                //MARKET_STAT
                MARKET_STAT mARKET_STAT = this.m_crfMARKET_STAT.DataUpdate[this.m_crfMARKET_STAT.DataUpdate.Length - 1];
                this.m_arrsttFullRowIndex.STAT_ControlCode = /*mARKET_STAT.ControlCode.ToString();*/new string(mARKET_STAT.ControlCode).Trim();
                this.m_arrsttFullRowIndex.STAT_Time = mARKET_STAT.Time.ToString();
                //this.m_arrsttFullRowIndex.STAT_Date = DateTime.Now.ToString();
                VNX_MARKET_VNINDEX VNINDEX = this.m_crdVNIndex.DataUpdate[this.m_crdVNIndex.DataUpdate.Length - 1];
                this.m_arrsttFullRowIndex.VNI_Time = VNINDEX.Time.ToString();
                this.m_arrsttFullRowIndex.VNI_IndexValue = (VNINDEX.Index / 100.0).ToString();
                this.m_arrsttFullRowIndex.VNI_TotalTrade = VNINDEX.TotalTrade.ToString();
                this.m_arrsttFullRowIndex.VNI_TotalSharesAOM = VNINDEX.TotalShares.ToString();
                this.m_arrsttFullRowIndex.VNI_TotalValuesAOM = (VNINDEX.TotalValues / 1000.0).ToString();
                this.m_arrsttFullRowIndex.VNI_UpVolume = VNINDEX.UpVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_DownVolume = VNINDEX.DownVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_NoChangeVolume = VNINDEX.NoChangeVolume.ToString();
                this.m_arrsttFullRowIndex.VNI_Up = VNINDEX.Up.ToString();
                this.m_arrsttFullRowIndex.VNI_Down = VNINDEX.Down.ToString();
                this.m_arrsttFullRowIndex.VNI_NoChange = VNINDEX.NoChange.ToString();
                this.m_arrsttFullRowIndex.VNI_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNI_IndexValue) - this.m_LIH.Data[0].VNIndex).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNI_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNI_Change) / this.m_LIH.Data[0].VNIndex) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VN30 = this.m_crdVN30Index.DataUpdate[this.m_crdVN30Index.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VN30_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VN30_IndexValue) - this.m_LIH.Data[0].VN30).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VN30_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VN30_Change) / this.m_LIH.Data[0].VN30) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VN100 = this.m_crdVN100Index.DataUpdate[this.m_crdVN100Index.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VN100_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VN100_IndexValue) - this.m_LIH.Data[0].VN100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VN100_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VN100_Change) / this.m_LIH.Data[0].VN100) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNAll = this.m_crdVNAllIndex.DataUpdate[this.m_crdVN30Index.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VNALL_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNALL_IndexValue) - this.m_LIH.Data[0].VNALL).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNALL_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNALL_Change) / this.m_LIH.Data[0].VNALL) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNXAll = this.m_crdVNXAllIndex.DataUpdate[this.m_crdVNXAllIndex.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VNXALL_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNXALL_IndexValue) - this.m_LIH.Data[0].VNXALL).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNXALL_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNXALL_Change) / this.m_LIH.Data[0].VNXALL) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNMid = this.m_crdVNMidIndex.DataUpdate[this.m_crdVNMidIndex.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VNMID_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNMID_IndexValue) - this.m_LIH.Data[0].VNMID).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNMID_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNMID_Change) / this.m_LIH.Data[0].VNMID) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                VNX_MARKET_INDEX VNSml = this.m_crdVNSmlIndex.DataUpdate[this.m_crdVNSmlIndex.DataUpdate.Length - 1];
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
                this.m_arrsttFullRowIndex.VNSML_Change = (Convert.ToDouble(this.m_arrsttFullRowIndex.VNSML_IndexValue) - this.m_LIH.Data[0].VNSML).ToString(CConfig.FORMAT_INDEX_CHANGE);
                this.m_arrsttFullRowIndex.VNSML_ChangePercent = ((Convert.ToDouble(this.m_arrsttFullRowIndex.VNSML_Change) / this.m_LIH.Data[0].VNSML) * 100).ToString(CConfig.FORMAT_INDEX_CHANGE);
                CGlobal.INAV INAV = this.m_crdINAV.DataUpdate[this.m_crdINAV.DataUpdate.Length - 1];
                this.m_arrsttFullRowIndex.INAV_iNAV = INAV.iNAV.ToString();
                this.m_arrsttFullRowIndex.INAV_StockNo = INAV.StockNo.ToString();
                this.m_arrsttFullRowIndex.INAV_StockSymbol = new string(INAV.StockSymbol).Trim();/*this.m_crdINAV.Char2String(INAV.StockSymbol);*/
                this.m_arrsttFullRowIndex.INAV_Time = INAV.Time.ToString();

                CGlobal.IINDEX IINDEX = this.m_crdIINDEX.DataUpdate[this.m_crdIINDEX.DataUpdate.Length - 1];
                this.m_arrsttFullRowIndex.IINDEX_ETFSymbol = /*IINDEX.ETFSymbol.ToString();*/ new string(IINDEX.ETFSymbol).Trim();
                this.m_arrsttFullRowIndex.IINDEX_iIndex = IINDEX.iIndex.ToString();
                this.m_arrsttFullRowIndex.IINDEX_IndexSymbol = /*IINDEX.IndexSymbol.ToString();*/ new string(IINDEX.IndexSymbol).Trim();
                this.m_arrsttFullRowIndex.IINDEX_iIndexSymbol = IINDEX.Time.ToString();
                this.m_arrsttFullRowIndex.IINDEX_Time = IINDEX.Time.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
            return true;
        }
        public string IniReadValue(string Section, string Key)
        {
            Logger.LogInfo("IniReadValue");
            try
            {
                StringBuilder temp = new StringBuilder(VALUE_LENGTH_MAX);
                int i = GetPrivateProfileString(Section, Key, "", temp, VALUE_LENGTH_MAX, this.path);
                return temp.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }
        public void Redis_S5G_ET_QUOTE()
        {
            Logger.LogInfo("Redis_S5G_ET_QUOTE");
            string Redis_message = "<p>S5G_ET_QUOTE saved successfully</p>";
            Connection.ConnectionRedis();
            IDatabase db = Connection.GetRedisDatabase();
            try
            {
                double dblScore = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string jsonData = JsonConvert.SerializeObject(this.m_arrsttFullRowQuote);
                db.SortedSetAdd(CConfig.KEY_ET_QUOTE, jsonData, dblScore);
                Connection.RedisClose();
            }
            catch(Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
            finally
            {
                Send_Mail sent_mail = new Send_Mail();
                sent_mail.Send_Message(Redis_message);
            }           
        }
        public void Redis_S5G_ET_PT()
        {           
            Logger.LogInfo("Redis_S5G_ET_PT");
            string Redis_message = "<p>S5G_ET_PT saved successfully</p>";
            Connection.ConnectionRedis();
            IDatabase db = Connection.GetRedisDatabase();
            try
            {
                double dblScore = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string jsonData = JsonConvert.SerializeObject(this.m_arrstt_ROWPT);
                db.SortedSetAdd(CConfig.KEY_ET_PT, jsonData, dblScore);
                Connection.RedisClose();
            }
            catch(Exception ex)
            {                
                Logger.LogError("An error occurred: " + ex.Message);
            }
            finally
            {
                Send_Mail sent_mail = new Send_Mail();
                sent_mail.Send_Message(Redis_message);
            }
        }
        public void Redis_S5G__ET_INDEX()
        {
            Logger.LogInfo("Redis_S5G__ET_INDEX");
            string Redis_message = "<p>S5G__ET_INDEX saved successfully</p>";
            Connection.ConnectionRedis();
            IDatabase db = Connection.GetRedisDatabase();
            try
            {
                double dblScore = Convert.ToDouble(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                string jsonData = JsonConvert.SerializeObject(this.m_arrsttFullRowIndex);
                db.SortedSetAdd(CConfig.KEY_ET_Index, jsonData, dblScore);
                Connection.RedisClose();
            }
            catch(Exception ex) { 
                Logger.LogError("An error occurred: " + ex.Message); 
            }
            finally
            {
                Send_Mail sent_mail = new Send_Mail();
                sent_mail.Send_Message(Redis_message);
            }
        }
        public void Read_LAST_INDEX_HO()
        {
            Logger.LogInfo("Read_LAST_INDEX_HO");
            Connection.ConnectionRedis();
            IDatabase db = Connection.GetRedisDatabase();

            string jsonValue = db.StringGet("S5G_LAST_INDEX_HO");
            string convertedJsonString = Regex.Unescape(jsonValue);
            //string jsonValue = "{\"Time\":\"2023-07-14 08:17:45.074\",\"Data\":[{\"TradingDate\":\"13-07-2023\",\"VNIndex\":1165.42,\"VN100\":1121.98,\"VN30\":1156.11,\"VNALL\":1136.56,\"VNMID\":1551.19,\"VNSML\":1379.93,\"VNXALL\":1820.30}]}";           
            if (!string.IsNullOrEmpty(convertedJsonString))
            {
                m_LIH = JsonConvert.DeserializeObject<LastIndexHO>(convertedJsonString);
                if (m_LIH != null && m_LIH.Data != null)
                {
                    foreach (LastIndexHODetail detail in m_LIH.Data)
                    {
                        string tradingDate = detail.TradingDate;
                        double vnIndex = detail.VNIndex;
                        double vnsml = detail.VNSML;
                        double vnmid = detail.VNMID;
                        double vnall = detail.VNALL;
                        double vn30 = detail.VN30;
                        double vn100 = detail.VN100;
                        double vnxall = detail.VNXALL;
                    }
                }
            }
        }
        public void SaveData()
        {
            Logger.LogInfo("SaveData");
            ConnectionOracle.ConnectOracle();
            OracleConnection conn = ConnectionOracle._oracleconnection;
            //string sqlQuery = "SELECT COUNT(*) FROM STOCK_HCM";
            int i = 1;
            conn.Open();
            try
            {               
                using (OracleCommand truncateCmd = new OracleCommand("TRUNCATE TABLE STOCK_HCM", conn))
                {
                    truncateCmd.ExecuteNonQuery();
                }
                foreach (CGlobal.SECURITY security in m_crfSECURITY.DataUpdate)
                {
                    using (OracleCommand cmd = new OracleCommand("INSERT_STOCK_HCM", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Add(":TRANID", OracleDbType.Decimal).Value = i;
                        cmd.Parameters.Add(":STOCKNO", OracleDbType.Decimal).Value = security.StockNo;
                        cmd.Parameters.Add(":STOCKSYMBOL", OracleDbType.NVarchar2, 16).Value = security.StockSymbol;
                        cmd.Parameters.Add(":STOCKTYPE", OracleDbType.NVarchar2, 2).Value = security.StockType;
                        cmd.Parameters.Add(":PRIORCLOSEPRICE", OracleDbType.Double).Value = security.PriorClosePrice;
                        cmd.Parameters.Add(":OPENPRICE", OracleDbType.Double).Value = security.OpenPrice;
                        cmd.Parameters.Add(":LAST", OracleDbType.Double).Value = security.Last;
                        cmd.Parameters.Add(":LASTVOL", OracleDbType.Decimal).Value = security.LastVol;
                        cmd.Parameters.Add(":HIGHEST", OracleDbType.Double).Value = security.Highest;
                        cmd.Parameters.Add(":LOWEST", OracleDbType.Double).Value = security.Lowest;
                        cmd.ExecuteNonQuery();
                    }
                    i++;
                }                                 
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
        public bool Oracle_STOCK_HCM()
        {
            Logger.LogInfo("Oracle_STOCK_HCM");
            STOCK_HCM data = new STOCK_HCM();
            data.Save(m_crfSECURITY);
            return true;
        }
        //public bool S5G_ET_QUOTE()
        //{
        //    CGlobal.SECURITY[] arrsttSECURITY = m_crfSECURITY.DataUpdate;
        //    CGlobal.LS[] arrsttLS = m_crfLS.DataUpdate;
        //    CGlobal.OS[] arrsttOS = m_crfOS.DataUpdate;
        //    CGlobal.FROOM[] arrsttFROOM = m_crfFROOM.DataUpdate;
        //    FULL_ROW_QUOTE[] arrsttFullRowQuote = m_arrsttFullRowQuote;
        //    FULL_ROW_INDEX arrsttFullRowIndex = m_arrsttFullRowIndex;
        //    S5G_ET_QUOTE et_quote = new S5G_ET_QUOTE();
        //    et_quote.UpdateFullRowQuote(arrsttSECURITY, arrsttLS, arrsttOS, arrsttFROOM, arrsttFullRowQuote, arrsttFullRowIndex);
        //    return true;
        //}
    }
}
