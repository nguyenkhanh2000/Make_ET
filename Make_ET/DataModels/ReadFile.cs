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
        public CreaderAll<CGlobal.SECURITY> m_crfSECURITY;      // groupQUOTE: da read => build full row => update redis chart (ZSet)
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
            string resultjson = jsonValue.Substring(1,jsonValue.Length - 2);
            string convertedJsonString = Regex.Unescape(resultjson);
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
        public static string Unescape(string input)
        {
            return System.Text.RegularExpressions.Regex.Unescape(System.Text.RegularExpressions.Regex.Unescape(input));
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
