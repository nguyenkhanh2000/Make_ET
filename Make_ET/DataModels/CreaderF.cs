using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CreaderF<T>:CReaderBase<T>
    {
        public CreaderF(string strListenterURL) { }
        public void ResizeArray<TItem>(ref TItem[] array)
        {
            if (array == null)
            {
                array = new TItem[1];
            }
            else
            {
                Array.Resize(ref array, array.Length + 1);
            }
        }
        public bool ReadFile()
        {
            // init
            int objSize = 0;
            IntPtr ptrObj = IntPtr.Zero;
            byte[] readBytes = null;// this.GetBytes(strFullPath);
            DateTime dtBegin = DateTime.Now; // duration
            
            try
            {
                // file not found
                if (!File.Exists(this.m_strFilePath))
                    return false;

                // init
                readBytes = File.ReadAllBytes(this.m_strFilePath);
                this.m_lngTickCount++;

                // increase ReadFileTotal
                this.m_intReadFileTotal++;

                // array old: luu cac data lan doc file truoc do vao arrsttOld
                //arrsttOld = this.m_arrsttOldData; // ko dung cach nay dc vi sau do gan new data vao this.m_arrsttOldData thi arrsttOld cung nhan theo >> sai logic
                if (this.m_arrsttNewData != null)// lan dau tien doc file thi se out arrsttOld = null vi this.m_arrsttOldData chua co data
                {
                    this.m_arrsttOldData = this.m_arrsttNewData;
                }

                // reset
                this.m_intRowCountDone = 0;
                this.m_arrsttUpdateData = null;
                this.m_arrsttNewData = null;
                this.m_arrstrUpdateSQL = null;  // 2017-02-16 15:55:46 ngocta2 ko reset ve null thi no cu cho vao queue lien tuc, thread SQL cu lay ra dc data lien tuc de exec, mac du ko co thay doi

                //------------------------------

                // size of object
                objSize = Marshal.SizeOf(typeof(T));

                //current file (file status)
                this.m_intNewLength = (int)readBytes.Length;
                this.m_intNewTotalRecord = (int)readBytes.Length / objSize;

                // pointer
                ptrObj = Marshal.AllocHGlobal(objSize);
                this.m_arrsttNewData = new T[this.m_intNewTotalRecord];
                // tu mang bytes, tach ra cac object gan vao temp array
                for (int i = 0; i < this.m_intNewTotalRecord; i++)
                {
                    Marshal.Copy(readBytes, i * objSize, ptrObj, objSize);

                    // lay duoc record data
                    //T obj = (T)Marshal.PtrToStructure(ptrObj, typeof(T));
                    T obj = Marshal.PtrToStructure<T>(ptrObj);

                    // E1VFVN30 co Ceiling=2140575744 => bo qua ????

                    // gan luon vao array NEW
                    // 2018-06-29 10:11:29 ngocta2 fix dup VPI, so HO teo
                    // khi nao so HO ngon thi phai xoa dieu kien if
                    //if(Convert.ToInt32(this.GetValueByPropertyName(obj, "StockNo")) != 38)
                    //{
                    /*this.ResizeArray<T>(ref this.m_arrsttNewData);*/ // tang size array NEW
                    /*this.m_arrsttNewData[this.m_arrsttNewData.Length - 1] = obj;*/ // day vao array NEW
                                                                                     //}
                    //this.ResizeArray<T>(ref this.m_arrsttNewData);
                    //this.m_arrsttNewData[this.m_arrsttNewData.Length - 1] = obj;
                    
                    this.m_arrsttNewData[i] = obj;

                    // tang so row da doc xong
                    ++this.m_intRowCountDone;
                }


                this.m_arrsttUpdateData = this.m_arrsttNewData;

                // send monitor
                //this.SendMonitor(CBase.GetCaller(2), "RowCountDone=" + this.m_intRowCountDone.ToString());

                // luu cac file status cu de check data moi cho lan sau
                this.m_intPreReadLength = this.m_intNewLength;
                this.m_intPreReadTotalRecord = this.m_intNewTotalRecord;

                // store read time to var
                this.m_strReadTime = DateTime.Now.ToString(FORMAT_DATETIME_1);

                // duration
                this.m_dblDuration = DateTime.Now.Subtract(dtBegin).TotalMilliseconds; // duration

                // change first status
                if (this.m_blnFirst) // lan dau tien doc file thi bat buoc doc tat, du param truyen vao la chi doc new data
                    this.m_blnFirst = !this.m_blnFirst;

                // return success
                return true;
            }
            catch (Exception ex)
            {
                //CLog.LogError(CBase.GetDeepCaller(), CBase.GetDetailError(ex));
                //this.m_intReadErrorTotal++;
                //return false; // return failed
                throw ex;
            }
            finally
            {
                // free memory
                Marshal.FreeHGlobal(ptrObj);
            }
        }

    }
}
