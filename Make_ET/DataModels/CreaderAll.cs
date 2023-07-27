using Make_ET.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CreaderAll<T>:CReaderBase<T>
    {
        public CreaderAll(string strListenerURL) { }
        /// <summary>
        /// danh cho QuoteFeederHOSE
        /// danh cho doc LS + LE 
        /// LS => 35k recs luc cuoi gio , neu restart app co the mat 2 phut de read full LS  
        /// </summary>
        public bool ReadFileBig(bool blnGetDistict = true)
        {
            //init
            int objSize = 0;
            IntPtr ptrObj = IntPtr.Zero;
            byte[] readBytes = null;//this.GetBytes(strFullPath)
            DateTime dtBegin = DateTime.Now;
            bool blnReadFul = false;
            //string filename= $"ReadFileBig-{this.m_strFileName}-{System.Threading.Thread.CurrentThread.ManagedThreadId}";
            try
            {
                //file not found
                if (!File.Exists(this.m_strFilePath))
                    return false;
                readBytes = this.GetBytes(this.m_strFilePath);
                this.m_lngTickCount++;

                if(this.m_blnFirst) //lan dau tien doc file thi bat buoc doc tat, du param truyen vao la chi doc new data
                    blnReadFul = true;
                //increase ReadFileTotal
                this.m_intReadFileTotal++;
                //size of obj
                objSize = Marshal.SizeOf(typeof(T));
                //current file(file status)
                this.m_intNewLength = (int)readBytes.Length;
                this.m_intNewTotalRecord = m_intNewLength / objSize;

                //pointer
                ptrObj = Marshal.AllocHGlobal(objSize);
                //reset
                this.m_intRowCountDone = 0;
                this.m_arrsttUpdateData = null;
                //Luu array NEW >> OLD
                if (this.m_arrsttNewData != null && this.m_intNewTotalRecord != this.m_arrsttNewData.Length)
                    this.m_arrsttOldData = this.m_arrsttNewData;
                if (blnReadFul)
                {
                    Array.Resize(ref this.m_arrsttUpdateData, this.m_intNewTotalRecord);
                    //Từ mảng bytes, tách ra các obj gắn vào temp array
                    for(int i = 0;  i < this.m_intNewTotalRecord; i++)
                    {
                        Marshal.Copy(readBytes, i * objSize, ptrObj, objSize);
                        //Lay dc record data
                        T obj = (T)Marshal.PtrToStructure(ptrObj,typeof(T));
                        //Gan luon vao array NEW
                        this.m_arrsttUpdateData[i] = obj;
                        //tang so row da doc xong
                        ++this.m_intRowCountDone;
                    }
                }
                else //READ UPDATE ONLY
                {
                    //ResizeArray 1 lan vao day thoi, eleCount = m_intNewTotalRecord - m_intPreReadTotalRecord
                    Array.Resize(ref this.m_arrsttUpdateData,this.m_intNewTotalRecord-m_intPreReadTotalRecord);
                    for(int i = this.m_intPreReadTotalRecord;i < this.m_intNewTotalRecord; i++)
                    {
                        Marshal.Copy(readBytes, i * objSize, ptrObj, objSize);
                        //lay dc record data
                        T obj = (T)Marshal.PtrToStructure(ptrObj, typeof(T));
                        //gan luon vao array UPDATE
                        this.m_arrsttUpdateData[i - this.m_intPreReadTotalRecord] = obj;
                        ++this.m_intRowCountDone;
                    }
                }
                if(blnReadFul) //READFULL
                {
                    this.m_arrsttNewData = this.m_arrsttUpdateData;
                    this.m_arrsttOldData = this.m_arrsttUpdateData;
                }
                //distinct
                //if(this.m_arrsttUpdateData != null)
                //{
                //    if (this.m_strFileName == FILE_PRS_LS) blnGetDistict = false;
                //    if(blnGetDistict)
                //    {
                //        this.m_arrsttUpdateData = this.GetDistinctRecords(this.m_arrsttUpdateData);
                //    }
                //}

                //luu cac file status cu de check data moi cho lan sau
                this.m_intPreReadLength = this.m_intNewLength;
                this.m_intPreReadTotalRecord = this.m_intNewTotalRecord;
                //store read time to var(DONE TIME)
                this.m_strReadTime = DateTime.Now.ToString(FORMAT_DATETIME_1);
                //duration
                this.m_dblDuration = DateTime.Now.Subtract(dtBegin).TotalMinutes;
                if(this.m_blnFirst) //lan dau tien doc file thi bat buoc doc tat, du param truyen vao la chi doc new data
                    this.m_blnFirst =  !this.m_blnFirst;
                return true;
            }
            catch (Exception ex) 
            {                
                Logger.LogError("An error occurred: " + ex.Message);
                this.m_intReadErrorTotal++;
                return false;
            }
            finally
            {
                Marshal.FreeHGlobal(ptrObj);
            }
        }
    }
}
