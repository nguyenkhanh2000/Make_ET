using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CReaderBase<T>
    {
        protected byte[] GetBytes(string filePath)
        {
            byte[] bytes = null;
            try
            {
                bytes = File.ReadAllBytes(filePath);
            }catch (Exception ex) 
            {
                throw ex;
            }
            return bytes;
        }
        public delegate void fncCheckThreadGroupQUOTEINDEX();
        protected long m_lngTickCount = 0;

        protected const string FILE_PRS_OS = "OS.DAT";
        protected const string FILE_PRS_SECURITY = "SECURITY.DAT";
        protected const string FILE_PRS_LS = "LS.DAT";

        protected const string FILE_PRS_OS_FIELD_STOCKNO = "StockNo"; // cot StockNo trong file OS
        protected const string FILE_PRS_OS_FIELD_PRICE = "Price"; /// cot Price trong file OS

        private const string PRS_SECURITY_COL_BIGLOTVALUE = "BigLotValue";

        protected Object m_objLocker = new Object();
        protected bool m_blnInProces = false;   // Read+SendRedis xong thi moi reset m_flgInProces ve lai false [IMPORTANT]

        protected int m_intThreadStatus;
        //public int THREAD_STATUS_READY = 0;
        //public int THREAD_STATUS_READING = 1;
        //public int THREAD_STATUS_WAITING = 2;

        protected bool m_blnThreadStarted;      // tranh truong hop start 2 thread cung 1 luc, 2 thread nay se update val sai theo logic tinh toan ban dau la chi start 1 thread => qua trinh doc file bi ngung vi val sai

        protected int m_intPreReadLength;       // do dai (bytes) cua file da doc lan truoc
        protected int m_intPreReadTotalRecord;  // tong so record cua file da doc lan truoc
        protected int m_intNewLength;           // do dai (bytes) cua file dang doc hien tai
        protected int m_intNewTotalRecord;      // tong so record cua file dang doc hien tai
        protected string m_strReadTime;         // thoi diem doc xong du lieu tu file binary (FPTS server time)
        protected string m_strFilePath;         // full path cua file C:\HOSTC_IS\BACKUP31\TOTALMKT.DAT
        protected int m_intReadFileTotal;       // tong so lan doc du lieu file binary
        protected int m_intReadErrorTotal;      // tong so lan doc du lieu file binary bi error
        protected bool m_blnFirst;              // lan dau tien doc file thi can ReadAll
        protected int m_intRowCountDone;        // so row da doc xong (VD: co 199 rows, da doc xong 24 rows => m_intRowCountDone=24)
        
        protected double m_dblDuration;         // thoi gian doc xong data tu file vao array (ms)
        protected const string FORMAT_DATETIME_1 = "yyyy-MM-dd HH:mm:ss.fff"; //>> "HH:mm:ss" ko du du lieu debug, phai them ms => "HH:mm:ss.fff"
        protected string m_strFileName;         // TOTALMKT.DAT

        protected string m_strMonitorListenerURL;   // URL de send thong tin vao monitor
                                                    //
        protected string m_strRedisKeyListCode = "";            // S5G_OTHER_HO_LIST_CODE
        protected string m_strRedisKeyListCodeID = "";          // S5G_OTHER_HO_LIST_CODEID
        protected string m_strRedisTempateKeyFull = "";         // S5G_FULL_HO_SECURITY_(RowID)
        protected string m_strRedisTempateKeySnapshot = "";     // S5G_SNAPSHOT_HO_SECURITY_(SnapshotAutoIncrease)
        protected int m_intRedisFullDurationInMinutes = 9999;   // FULL = 99999; SNAPSHOT = 30
        protected int m_intRedisSnapshotDurationInMinutes = 30; // FULL = 99999; SNAPSHOT = 30
        protected int m_intSnapshotAutoIncrease = 10000;       // khi co snapshot thay doi data thi +1
        public class APP_DATA
        {
            public string RowID;
            public List<List<string>> Info;
        }

        //protected CRedisClient m_CRC = null;        //new CRedisClient("10.26.2.250", 6379);
        protected string m_strRedisPublishChannel = ""; // publish vao kenh nao (1 thread = read 1 file + publish data vao 1 channel)
                                                        

        // ======================= QuoteFeeder =======================
        protected string TYPE_SYSTEM_CHAR = "System.Char[]";
        protected string m_strTemplateJsonElement = "";   // "[(Index),(NewValue)]";
        protected string m_strTemplateJsonFull = "";   // "{\"Code\":\"(Code)\",\"CodeID\":\"(CodeID)\",\"Change\":[(AllElements)]}";
        protected string m_strTemplateSqlElement = "";   // "@(ColumnName)='(NewValue)'";
        protected string m_strTemplateSqlFull = "";   // "EXEC prc_5G_QUOTE_FEEDER_UPDATE_HO_PRS (AllElements)";
        public int m_intThreadID = 0; // dung de ghi logEx, do multi thread ko dc ghi chung vao cung 1 file 
                                      //protected string FIELD_STOCKNO = "StockNo";
                                      // ======================= /QuoteFeeder =======================

        // =============== advanced 2015-03-19 14:07:14 ngocta2 ================
        protected T[] m_arrsttOldData = null;               // luu tat ca data cua lan doc file truoc do (ReaderD.ReadFileBig, danh cho doc file size to se KHONG luu data nay)
        protected T[] m_arrsttNewData = null;               // luu tat ca data cua lan doc file moi nhat (ReaderD.ReadFileBig, danh cho doc file size to se KHONG luu data nay)
        protected T[] m_arrsttUpdateData = null;            // luu tat ca data row co update, so sanh tu file moi nhat voi file truoc do (ReaderD.ReadFileBig, danh cho doc file size to se CO luu data nay)
        protected int[] m_arrintDoneRowID = null;           // (OS) array luu cac StockNo da lay OK gia, lan sau ko lay nua



        protected string m_strIDPropertyName = "";  // su dung PropertyName nay de so sanh tim ra cac struct

        // [0] = ["StockSymbol|1", "Ceiling|100", "Floor|100",..."CurrentRoom|1"]
        // [1] = ["StockSymbol", "Ceiling", "Floor",..."CurrentRoom"]
        // [2] = ["1", "100", "100",..."1"]
        protected string[][] m_arrstrColumnListJSON = new string[3][];

        // [0]=StockSymbol, [1]=Ceiling
        protected string[] m_arrstrColumnListSQL = null;

        // 2015-03-10 15:26:36 ngocta2
        // can read SECURITY dau tien de lay ra array chua cac StockNo de gan vao CReaderD
        // [1,3,4,5,7,8,9,10,12,13,14,15,17,190,193,195,196,197....3835,3836,3837,3838,3839,3840]        
        protected string[] m_arrstrStockNo = null;
        protected string[] m_arrstrStockSymbol = null;

        // 2015-03-05 14:44:08 ngocta2
        // gia su SECURITY.dat luc 11h20 khac voi SECURITY.dat luc 11h21 tai 2 row
        // 1. row co code = ABT 
        //      => old = Best1OfferVolume: 47141        new = Best1OfferVolume: 47333
        // 2. row co code = AVF
        //      => old = Best3Bid: 4.4                  new = Best3Bid: 4.8
        //      => old = Best3BidVolume: 25144          new = Best3BidVolume: 1000
        // vay se tao ra 2 element trong array m_arrstrUpdateJSON
        // [0] = {"Code":"ABT","Change":[[14,47333]]}
        // [1] = {"Code":"AVF","Change":[[8,4.8],[9,1000]]}
        protected string[] m_arrstrUpdateJSON = null;

        // gia su co update tai row ABT, FPT
        // row CCL thay doi gia tri cot [9]
        // row DHM thay doi gia tri cot [14] va cot [19]
        // vay thi
        // m_arrstrUpdateJSON
        //      [0]: "{\"Code\":\"CCL\",\"Change\":[[9,4087]]}"
        //      [1]: "{\"Code\":\"DHM\",\"Change\":[[19,11887],[14,3482]]}"
        // m_arrstrUpdateJSONFull
        //      [0]: "{\"Code\":\"CCL\",\"Change\":[[0,\"CCL\"],[1,5.2],[2,4.6],[3,4.9],[20,4.9],[10,5],[19,18931],[21,5],[22,4.8],[8,4.9],[9,4087],[6,4.8],[7,11531],[4,4.7],[5,4526],[13,5],[14,5267],[15,5.1],[16,7104],[17,5.2],[18,9690]]}"
        //      [1]: "{\"Code\":\"DHM\",\"Change\":[[0,\"DHM\"],[1,7.2],[2,6.4],[3,6.8],[20,6.8],[10,6.8],[19,11887],[21,6.9],[22,6.7],[8,6.7],[9,5649],[6,6.6],[7,15760],[4,6.5],[5,16228],[13,6.8],[14,3482],[15,6.9],[16,10553],[17,7],[18,5853]]}"
        protected string[] m_arrstrUpdateJSONFull = null;

        // StockNo hoac StockSymbol : hien tai la StockNo
        protected string[] m_arrstrRowID = null;

        // tuong tu m_arrstrUpdateJSON 
        // bat buoc phai co StockNo
        // [0] = EXEC prc_5G_QUOTE_FEEDER_UPDATE_HO_PRS @StockNo='1', @StockSymbol='ABT', @Best1OfferVolume='47333'
        // [1] = EXEC prc_5G_QUOTE_FEEDER_UPDATE_HO_PRS @StockNo='2', @StockSymbol='AVF', @Best3Bid='4.8', @Best3BidVolume='1000'
        protected string[] m_arrstrUpdateSQL = null;

        // "StockSymbol,Ceiling,Floor,Reference,Best3Bid,Best3BidVolume,Best2Bid,Best2BidVolume,Best1Bid,Best1BidVolume,Last,MatchedVol,MatchChange,Best1Offer,Best1OfferVolume,Best2Offer,Best2OfferVolume,Best3Offer,Best3OfferVolume,LastVol,OpenPrice,Highest,Lowest,CurrentRoom"
        // => StockSymbol=0, Ceiling=1, Reference=3, Last=10 .... chi hien thi ra ngoai UI cua end-user
        protected string m_strColumnListJSON = "";

        // tuong tu m_strColumnListJSON nhung day du tat ca cot >> trong DB
        protected string m_strColumnListSQL = "";

        // luu ten property cua field chua ma ck = "StockSymbol"
        protected string m_strPropertyNameOfSymbolField = "";

        // luu ten property cua field chua ID ma ck = "StockNo" >>> DUNG DE SELECT DISTINCT TRONG ARRAY
        protected string m_strPropertyNameOfSymbolIDField = "";

        // true thi build array UpdateJSON
        protected bool m_blnIsBuildUpdateJSON = false;

        // true thi build array UpdateSQL
        protected bool m_blnIsBuildUpdateSQL = false;

        // filter khi xuat JSON, filter theo cot nao "StockType"
        protected string m_strJSONFilterPropertyName = "";

        // filter khi xuat JSON, filter theo gia tri hop le nao => "U,E,S" => ["U","E","S"]
        protected string m_strJSONFilterValidValue = "";
        protected string[] m_arrstrJSONFilterValidValue = null;

        // SQL,JSON bo qua khi gap PropertyName/value nay
        protected string m_strSkipPropertyName = "";

        // SQL,JSON bo qua khi gap PropertyName/value nay
        protected int m_intSkipValueBigger = 99999;

        public bool IsBuildUpdateJSON
        {
            get { return this.m_blnIsBuildUpdateJSON; }
            set { this.m_blnIsBuildUpdateJSON = value; }
        }

        public bool IsBuildUpdateSQL
        {
            get { return this.m_blnIsBuildUpdateSQL; }
            set { this.m_blnIsBuildUpdateSQL = value; }
        }

        public string PropertyNameOfSymbolField
        {
            get { return this.m_strPropertyNameOfSymbolField; }
            set { this.m_strPropertyNameOfSymbolField = value; }
        }

        public string PropertyNameOfSymbolIDField
        {
            get { return this.m_strPropertyNameOfSymbolIDField; }
            set { this.m_strPropertyNameOfSymbolIDField = value; }
        }

        public string ColumnListJSON
        {
            get { return this.m_strColumnListJSON; }
            set
            {
                this.m_strColumnListJSON = value;
                this.m_arrstrColumnListJSON[0] = this.m_strColumnListJSON.Split(',');
                this.m_arrstrColumnListJSON[1] = new string[this.m_arrstrColumnListJSON[0].Length];
                this.m_arrstrColumnListJSON[2] = new string[this.m_arrstrColumnListJSON[0].Length];
                for (var i = 0; i < m_arrstrColumnListJSON[0].Length; i++)
                {
                    string[] arr = m_arrstrColumnListJSON[0][i].Split('|');
                    this.m_arrstrColumnListJSON[1][i] = arr[0];
                    this.m_arrstrColumnListJSON[2][i] = arr[1];
                }
            }
        }

        public string ColumnListSQL
        {
            get { return this.m_strColumnListSQL; }
            set { this.m_strColumnListSQL = value; this.m_arrstrColumnListSQL = this.m_strColumnListSQL.Split(','); }
        }

        public string[] UpdateJSON
        {
            get { return this.m_arrstrUpdateJSON; }
        }
        public string[] UpdateSQL
        {
            get { return this.m_arrstrUpdateSQL; }
        }
        public string[] UpdateJSONFull
        {
            get { return this.m_arrstrUpdateJSONFull; }
        }
        public string[] UpdateRowID
        {
            get { return this.m_arrstrRowID; }
        }


        public string IDPropertyName
        {
            get { return this.m_strIDPropertyName; }
            set { this.m_strIDPropertyName = value; }
        }

        public string TemplateJsonFull
        {
            get { return this.m_strTemplateJsonFull; }
            set { this.m_strTemplateJsonFull = value; }
        }

        public string TemplateSqlFull
        {
            get { return this.m_strTemplateSqlFull; }
            set { this.m_strTemplateSqlFull = value; }
        }
        public string TemplateJsonElement
        {
            get { return this.m_strTemplateJsonElement; }
            set { this.m_strTemplateJsonElement = value; }
        }

        public string TemplateSqlElement
        {
            get { return this.m_strTemplateSqlElement; }
            set { this.m_strTemplateSqlElement = value; }
        }

        public string JSONFilterPropertyName
        {
            get { return this.m_strJSONFilterPropertyName; }
            set { this.m_strJSONFilterPropertyName = value; }
        }

        public string JSONFilterValidValue
        {
            get { return this.m_strJSONFilterValidValue; }
            set { this.m_strJSONFilterValidValue = value; this.m_arrstrJSONFilterValidValue = this.m_strJSONFilterValidValue.Split(','); }
        }

        public string SkipPropertyName
        {
            get { return this.m_strSkipPropertyName; }
            set { this.m_strSkipPropertyName = value; }
        }

        public int SkipValueBigger
        {
            get { return this.m_intSkipValueBigger; }
            set { this.m_intSkipValueBigger = value; }
        }
        public int ThreadStatus
        {
            get { return this.m_intThreadStatus; }
            set { this.m_intThreadStatus = value; }
        }
        public bool ThreadStarted //3:08 PM Sunday, April 10, 2016 ... 2 thread cung start la error ko reset dc cac flag, stop read file
        {
            get { return this.m_blnThreadStarted; }
            set { this.m_blnThreadStarted = value; }
        }

        protected enum CREATING_SCRIPT
        {
            NOTHING,
            JSON,
            SQL
        }

        public enum THREAD_FLAG
        {
            READY = 0,
            READING = 1,
            WAITING = 2
        }


        // =============== advanced 2015-03-19 14:07:14 ngocta2 ================

        /// <summary>
        /// constructor
        /// </summary>
        protected CReaderBase()
        {
            this.m_intPreReadLength = 0;
            this.m_intPreReadTotalRecord = 0;
            this.m_intNewLength = 0;
            this.m_intNewTotalRecord = 0;
            this.m_strReadTime = DateTime.Now.ToString(FORMAT_DATETIME_1);
            this.m_strFilePath = "";
            this.m_strFileName = "";
            this.m_intReadFileTotal = 0;
            this.m_intReadErrorTotal = 0;
            this.m_blnFirst = true;
            this.m_dblDuration = 0;
            this.m_intThreadStatus = 0;         // THREAD_FLAG.READY;
            this.m_blnThreadStarted = false;    // 9:32 AM Saturday, April 09, 2016
        }
        // properties
        public int FileLength
        {
            get { return this.m_intPreReadLength; }
        }
        public int TotalRecord
        {
            get { return this.m_intPreReadTotalRecord; }
        }
        public string ReadTime
        {
            get { return this.m_strReadTime; }
        }

        //FilePath=C:\HOSTC_IS\BACKUP(dd)\SECURITY.DAT
        //FilePath=C:\VNX\(yyyy)(mm)(dd)_VNSML.DAT
        public string FilePath
        {
            get { return this.m_strFilePath; }
            set
            {
                this.m_strFilePath = value;
                this.m_strFilePath = this.m_strFilePath
                    .Replace("(dd)", /*DateTime.Now.ToString("dd")*/"28")
                    .Replace("(MM)", /*DateTime.Now.ToString("MM")*/"07")
                    .Replace("(yyyy)", /*DateTime.Now.ToString("yyyy")*/"2023")
                    ;
            }
        }
        public string FileName
        {
            get { return this.m_strFileName; }
            set { this.m_strFileName = value; }
        }
        public int ReadFileTotal
        {
            get { return this.m_intReadFileTotal; }
        }
        public int ReadErrorTotal
        {
            get { return this.m_intReadErrorTotal; }
        }
        public int ReadDuration
        {
            get { return Convert.ToInt32(this.m_dblDuration); }
        }

        public int RowCountDone
        {
            get { return this.m_intRowCountDone; }
        }

        // ========================== REDIS ===============================        
        public string RedisPublishChannel
        {
            get { return this.m_strRedisPublishChannel; }
            set { this.m_strRedisPublishChannel = value; }
        }
        public string RedisTempateKeyFull
        {
            get { return this.m_strRedisTempateKeyFull; }
            set { this.m_strRedisTempateKeyFull = value; }
        }
        public string RedisTempateKeySnapshot
        {
            get { return this.m_strRedisTempateKeySnapshot; }
            set { this.m_strRedisTempateKeySnapshot = value; }
        }
        public int RedisFullDurationInMinutes
        {
            get { return this.m_intRedisFullDurationInMinutes; }
            set { this.m_intRedisFullDurationInMinutes = value; }
        }
        public int RedisSnapshotDurationInMinutes
        {
            get { return this.m_intRedisSnapshotDurationInMinutes; }
            set { this.m_intRedisSnapshotDurationInMinutes = value; }
        }
        public int SnapshotAutoIncrease
        {
            get { return this.m_intSnapshotAutoIncrease; }
            set { this.m_intSnapshotAutoIncrease = value; }
        }
        public string RedisKeyListCode
        {
            get { return this.m_strRedisKeyListCode; }
            set { this.m_strRedisKeyListCode = value; }
        }
        public string RedisKeyListCodeID
        {
            get { return this.m_strRedisKeyListCodeID; }
            set { this.m_strRedisKeyListCodeID = value; }
        }

        public bool HasDataUpdate
        {
            get { return (this.m_arrstrUpdateJSON != null); }
        }

        public int ThreadID
        {
            get { return this.m_intThreadID; }
            set { this.m_intThreadID = value; }
        }


        public bool InProces
        {
            get { return this.m_blnInProces; }
        }

        public T[] DataOld
        {
            get { return this.m_arrsttOldData; }
            set { this.m_arrsttOldData = value; }
        }
        public T[] DataNew
        {
            get { return this.m_arrsttNewData; }
            set { this.m_arrsttNewData = value; }
        }
        public T[] DataUpdate
        {
            get { return this.m_arrsttUpdateData; }
            set { this.m_arrsttUpdateData = value; }
        }

        public long TickCount
        {
            get { return this.m_lngTickCount; }
        }

        public string MonitorListenerURL
        {
            get { return this.m_strMonitorListenerURL; }
            set { this.m_strMonitorListenerURL = value; }
        }



        /// false = class
        /// </summary>
        /// <returns></returns>
        public bool IsStructType()
        {
            Type type = typeof(T);

            bool isStruct = type.IsValueType && !type.IsPrimitive;
            bool isClass = type.IsClass;

            return (isStruct == true);
        }
    }
}
