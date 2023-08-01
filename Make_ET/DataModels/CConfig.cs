using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CConfig
    {
        public const string LOG_FILE_PATH = "D:\\FPTS Job\\log.txt";
        public const string KEY_ET_QUOTE = "S5G__ET_QUOTE";
        public const string KEY_ET_PT = "S5G__ET_PT";
        public const string KEY_ET_Index = "S5G__ET_INDEX";

        public const string directoryPath = /*@"D:\FPTS Job\FPT_HOSTC_IS\BACKUP22\"; */       @"D:\FPTS_Test\BACKUP28\";
        public const string directoryPath_Index = /*@"D:\FPTS Job\VNX\"; */          @"D:\FPTS_Test\FPT_VNX\"; 

        public const string INI_SECTION_SECURITY = "SECURITY";
        public const string INI_SECTION_SECURITYOL = "SECURITYOL";
        public const string INI_SECTION_TOTALMKT = "TOTALMKT";
        public const string INI_SECTION_OS = "OS";
        public const string INI_SECTION_PUT_AD = "PUT_AD";
        public const string INI_SECTION_PUT_EXEC = "PUT_EXEC";
        public const string INI_SECTION_PUT_DC = "PUT_DC";
        public const string INI_SECTION_DELIST = "DELIST";
        public const string INI_SECTION_NEWLIST = "NEWLIST";
        public const string INI_SECTION_FROOM = "FROOM";
        public const string INI_SECTION_MARKET_STAT = "MARKET_STAT";
        public const string INI_SECTION_LS = "LS";
        public const string INI_SECTION_LO = "LO";
        public const string INI_SECTION_LE = "LE";
        public const string INI_SECTION_REDIS = "REDIS";
        public const string INI_SECTION_MARKET_STATUS = "MARKET_STATUS";
        public const string INI_SECTION_CS_VN30 = "CS_VN30";
        public const string INI_SECTION_CS_VNINDEX = "CS_VNINDEX";
        public const string INI_SECTION_CS_VN100 = "CS_VN100";
        public const string INI_SECTION_CS_VNALL = "CS_VNALL";
        public const string INI_SECTION_CS_VNMID = "CS_VNMID";
        public const string INI_SECTION_CS_VNSML = "CS_VNSML";
        public const string INI_SECTION_CS_VNXALL = "CS_VNXALL";            
        public const string INI_SECTION_INDEX_VN30 = "INDEX_VN30";
        public const string INI_SECTION_INDEX_VN100 = "INDEX_VN100";
        public const string INI_SECTION_INDEX_VNALL = "INDEX_VNALL";
        public const string INI_SECTION_INDEX_VNMID = "INDEX_VNMID";
        public const string INI_SECTION_INDEX_VNSML = "INDEX_VNSML";
        public const string INI_SECTION_INDEX_INAV = "INDEX_INAV";
        public const string INI_SECTION_INDEX_IINDEX = "INDEX_IINDEX";
        public const string INI_SECTION_INDEX_VNXALL = "INDEX_VNXALL";      

        public const string INI_KEY_INTERVALQUOTE = "IntervalQUOTE";
        public const string INI_KEY_INTERVALINDEX = "IntervalINDEX";
        public const string INI_KEY_INTERVALPT = "IntervalPT";
        public const string INI_KEY_INTERVALSQL = "IntervalSQL";
        public const string INI_KEY_INTERVALREDIS = "IntervalRedis";
        public const string INI_KEY_FILENAME = "FileName";
        public const string INI_KEY_REDISKEYLISTCODE = "RedisKeyListCode";
        public const string INI_KEY_REDISKEYLISTCODEID = "RedisKeyListCodeID";
        public const string INI_KEY_FILEPATH = "FilePath";
        public const string INI_KEY_SKIPPROPERTYNAME = "SkipPropertyName";
        public const string INI_KEY_SKIPVALUEBIGGER = "SkipValueBigger";
        public const string INI_KEY_COLUMNLISTJSON = "ColumnListJSON";
        public const string INI_KEY_COLUMNLISTSQL = "ColumnListSQL";
        public const string INI_KEY_ISBUILDUPDATEJSON = "IsBuildUpdateJSON";
        public const string INI_KEY_ISBUILDUPDATESQL = "IsBuildUpdateSQL";
        public const string INI_KEY_PROPERTYNAMEOFSYMBOLFIELD = "PropertyNameOfSymbolField";
        public const string INI_KEY_PROPERTYNAMEOFSYMBOLIDFIELD = "PropertyNameOfSymbolIDField";
        public const string INI_KEY_IDPROPERTYNAME = "IDPropertyName";
        public const string INI_KEY_TEMPLATEJSONFULL = "TemplateJsonFull";
        public const string INI_KEY_TEMPLATEJSONELEMENT = "TemplateJsonElement";
        public const string INI_KEY_TEMPLATESQLFULL = "TemplateSqlFull";
        public const string INI_KEY_TEMPLATESQLELEMENT = "TemplateSqlElement";
        public const string INI_KEY_JSONFILTERPROPERTYNAME = "JSONFilterPropertyName";
        public const string INI_KEY_JSONFILTERVALIDVALUE = "JSONFilterValidValue";
        public const string INI_KEY_REDISFULLDURATIONINMINUTES = "RedisFullDurationInMinutes";
        public const string INI_KEY_REDISSNAPSHOTDURATIONINMINUTES = "RedisSnapshotDurationInMinutes";
        public const string INI_KEY_REDISTEMPATEKEYFULL = "RedisTempateKeyFull";
        public const string INI_KEY_REDISTEMPATEKEYSNAPSHOT = "RedisTempateKeySnapshot";
        public const string INI_KEY_REDISPUBLISHCHANNEL = "RedisPublishChannel";
        public const string INI_KEY_THREADID = "ThreadID";
        public const string INI_KEY_CHANNELGROUPQUOTE = "ChannelGroupQUOTE";
        public const string INI_KEY_CHANNELGROUPINDEX = "ChannelGroupINDEX";
        public const string INI_KEY_CHANNELGROUPPT = "ChannelGroupPT";
        public const string INI_KEY_CHANNELGROUPOTHER = "ChannelGroupOTHER";
        public const string INI_KEY_ENDCONTROLCODES = "EndControlCodes";
        public const string INI_KEY_BREAKHOUR = "BreakHour";
        public const string INI_KEY_REDISKEYFULLROW = "RedisKeyFullRow";      //        RedisKeyFullRow=S5G__FULL_ROW_QUOTE
        public const string INI_KEY_REDISKEYET = "RedisKeyET";//"RedisKeyET";
        public const string INI_KEY_SCOREFORMAT = "ScoreFormat";      //        
        public const string INI_KEY_KEYFROOMFIRST = "KeyFroomFirst";//"S5G_FROOM_FIRST";
        public const string INI_KEY_KEYLASTINDEXHO = "KeyLastIndexHO";//S5G_LAST_INDEX_HO";
        public const string INI_KEY_KEYCOMPANYNAME = "KeyCompanyName";// "S5G_COMPANY_NAME";
        public const string INI_KEY_KEYMINISTRY = "KeyMinistry";//"S5G_MINISTRY";
        public const string INI_KEY_KEYVNXLIST = "KeyVNXList";//"S5G_VNX_LIST";     
        public const string INI_KEY_CHART_REDISKEYVNI = "RedisKeyVNI";//=S5G__CHART_VNI
        public const string INI_KEY_CHART_TEMPLATEJSON = "TemplateJson";//TemplateJson={"Time":"(Time)","Data":{"TimeJS":(TimeJS),"Index":(Index),"Vol":(Vol)}}
        public const string INI_KEY_CHART_REDISKEYVN30 = "RedisKeyVN30";//=S5G__CHART_VN30
        public const string INI_KEY_CHART_REDISKEYVN100 = "RedisKeyVN100";//=S5G__CHART_VN100
        public const string INI_KEY_CHART_REDISKEYVNALL = "RedisKeyVNALL";//=S5G__CHART_VNALL
        public const string INI_KEY_CHART_REDISKEYVNMID = "RedisKeyVNMID";//=S5G__CHART_VNMID
        public const string INI_KEY_CHART_REDISKEYVNSML = "RedisKeyVNSML";//=S5G__CHART_VNSML
        public const string INI_KEY_REDISKEYLOG = "RedisKeyLog";//RedisKeyLog=S5G__LOG_INDEX
        public const string INI_KEY_DIRBACKUPFROOM = "DirBackupFROOM";
        public const string INI_KEY_4GINDEX = "Key4GIndex";//Key4GIndex=S4G_INDEX

        public const string FORMAT_INDEX_VALUE = "#,##0.##";
        public const string FORMAT_INDEX_CHANGE = "#,##0.00";
        public const string FORMAT_INDEX_VALUE_REDIS = "####.##"; // luu tren REDIS 
        public const string FORMAT_INDEX_QTTY = "#,##0"; // neu "#,###" thi >> ""

        public static string Char2String(object objValue)
        {
            try
            {
                if (objValue == null)
                    return "null";
                string strValue = "";
                if (objValue.GetType().ToString() == "System.Char[]")
                {
                    char[] arrchrData = (char[])objValue;
                    string strTemp = new string(arrchrData);
                    strValue = strTemp.Trim();
                }
                else
                    strValue = objValue.ToString();
                return strValue;
            }
            catch(Exception ex) 
            {
                throw ex; 
            }
        }
        public const string SEP1 = "|";
        public const string SEP2 = ":";
        public const string KEY_SU = "SU";
        public const string KEY_DE = "DE";
        public const string KEY_HA = "HA";
        public const string KEY_SP = "SP";
        public const string KEY_BE = "BE";
        public const string KEY_ME = "ME";
        public const string KEY_NO = "NO";
    }
}
