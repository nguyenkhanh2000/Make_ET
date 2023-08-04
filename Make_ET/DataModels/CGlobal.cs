using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Make_ET.DataModels
{
    public class CGlobal
    {
        public static long g_lngTickCount = 0;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SECURITY
        {
            public short StockNo;		    // Mã chứng khoán dạng số
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] StockSymbol;		// Mã chứng khoán dạng chuỗi
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] StockType;		// Lọai chứng khóan: + S: Cổ phiếu + D: Trái phiếu + U: Chứng chỉ quỹ
            public int Ceiling;		        // Giá trần
            public int Floor;		        // Giá sàn
            public double BigLotValue;		// Bỏ qua 

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public char[] SecurityName;		// Tên đây đủ của chứng khoán
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] SectorNo;		    // Bỏ qua
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Designated;		// Bỏ qua
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] SUSPENSION;		// CK bị tạm ngưng giao dịch:  + Null: Giao dịch bình thường + S: Bị tạm ngưng
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Delist;           // CK bị hủy niêm yết: + Null: Giao dịch bình thường + D: Bị hủy niêm yết


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] HaltResumeFlag;   // CK bị ngưng hoặc giao dịch trơ lại trong phiên giao dịch
                                            //                              + Null: Giao dịch bình thường
                                            //                              + H: Bị ngưng giao dịch trong phiên
                                            //                              + A: Bị ngưng giao dịch khớp lệnh trong phiên
                                            //                              + P: Bị ngưng giao dịch thỏa thuận trong phiên

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] SPLIT;            // CK thực hiện tách cổ phiếu:  
                                            //                              + Null: Không thực hiện 
                                            //                              + S: Thực hiện
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Benefit;      // CK thực hiện quyền và chia cổ tức
                                        //							  + Null: Không thực hiện
                                        //                            + A: Phát hành thêm & Cổ tức
                                        //                            + D: Chia cổ tức
                                        //                            + R: Thực hiện quyền
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Meeting;      // TCNY tổ chức đại hội cổ đông: 
                                        //                              + Null: Không 
                                        //                              + M: Tổ chức đại hội cổ đông    


            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Notice;       // TCNY bị yêu cầu cung cấp thong tin quan trong trong phiên giao dịch
                                        //                             + Null: Không
                                        //                             + P: Chờ thong tin cần cung cấp
                                        //                             + R: Đã nhận thong tin cung cấp

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] ClientIDRequest;  // Bỏ qua

            public short CouponRate;        //  Bỏ qua  

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] IssueDate;        // Ngày phát hành
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] MatureDate;       // Bỏ qua
            public int AvrPrice;            // Giá bình quân gia quyền của các mức giá khớp
            public short ParValue;          // Mệnh giá phát hành

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] SDCFlag;          // Bỏ qua
            public int PriorClosePrice;     // Giá đóng cửa gần nhất

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] PriorCloseDate;   // Ngày giao dịch gần nhất
            public int ProjectOpen;         // Giá tạm khớp trong đợt KL định kỳ
            public int OpenPrice;           // Giá khớp mớ cửa
            public int Last;                // Giá khớp
            public int LastVol;             // Tổng khối lượng khớp
            public double LastVal;          // Tổng giá trị khớp
            public int Highest;             // Giá khớp cao nhất
            public int Lowest;              // Giá Khớp thấp nhất
            public double Totalshares;      // Bỏ qua
            public double TotalValue;       // Bỏ qua
            public short AccumulateDeal;    // Bỏ qua
            public short BigDeal;           // Bỏ qua
            public int BigVolume;           // Bỏ qua
            public double BigValue;         // Bỏ qua
            public short OddDeal;           // Bỏ qua
            public int OddVolume;           // Bỏ qua
            public double OddValue;         // Bỏ qua    
            public int Best1Bid;            // Giá đặt mua tốt nhất 1
            public int Best1BidVolume;      // Khối lượng tương ứng giá đặt mua 1
            public int Best2Bid;            // Giá đặt mua tốt nhất 2
            public int Best2BidVolume;      // Khối lượng tương ứng giá đặt mua 2
            public int Best3Bid;            // Giá đặt mua tốt nhất 3
            public int Best3BidVolume;      // Khối lượng tương ứng giá đặt mua 3
            public int Best1Offer;          // Giá đặt bán tốt nhất 1
            public int Best1OfferVolume;    // Khối lượng tương ứng giá đặt bán 1
            public int Best2Offer;          // Giá đặt bán tốt nhất 2
            public int Best2OfferVolume;    // Khối lượng tương ứng giá đặt bán 2
            public int Best3Offer;          // Giá đặt bán tốt nhất 3
            public int Best3OfferVolume;    // Khối lượng tương ứng giá đặt bán 3
            public short BoardLost;         // Bỏ qua

            // ---------------------- CW -----------------------------------
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] UnderlyingSymbol;         // Chứng khoán cơ sở (sử dụng cho CW)               String 8
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public char[] IssuerName;               // Tên tổ chức phát hành (sử dụng cho CW)           String 25
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] CoveredWarrantType;       // Loại chứng quyền (sử dụng cho CW)                String 1
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] MaturityDate;             // Ngày hết hạn (sử dụng cho CW)                    String 8
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] LastTradingDate;          // Ngày giao dịch cuối cùng (sử dụng cho CW)        String 8
            public int ExercisePrice;               // Giá thực hiện (sử dụng cho CW)                   Long 4
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public char[] ExerciseRatio;            // Tỷ lệ thực hiện. (sử dụng cho CW)                String 11
            public double ListedShare;              // Khối lượng CW niêm yết (sử dụng cho CW)          Double 8
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] OddLotFlag;               // Chứng khoán đang bị tạm dừng giao dịch hoặc giao dịch có được tiếp tục(đối với lô lẻ) string  1

            // ---------------------- /CW -----------------------------------

            // 2014-12-16 17:27:35 ngocta2
            // FAST-SPEED METHOD
            public override bool Equals(object obj)
            {
                //if (!(obj is STOCK_HCM))
                //  return false;

                var other = (SECURITY)obj;

                return StockNo == other.StockNo // short
                    && Ceiling == other.Ceiling // int
                    && Floor == other.Floor   // int

                    && ProjectOpen == other.ProjectOpen    // int
                    && OpenPrice == other.OpenPrice      // int
                    && Last == other.Last           // int
                    && LastVol == other.LastVol        // int
                                                       //&& LastVal      == other.LastVal        // double (ko tinh cai nay vi co the day la val PT)
                    && Highest == other.Highest        // int
                    && Lowest == other.Lowest         // int

                    && Best1Bid == other.Best1Bid             // int
                    && Best1BidVolume == other.Best1BidVolume       // int
                    && Best2Bid == other.Best2Bid             // int
                    && Best2BidVolume == other.Best2BidVolume       // int
                    && Best3Bid == other.Best3Bid             // int
                    && Best3BidVolume == other.Best3BidVolume       // int
                    && Best1Offer == other.Best1Offer             // int
                    && Best1OfferVolume == other.Best1OfferVolume       // int
                    && Best2Offer == other.Best2Offer             // int
                    && Best2OfferVolume == other.Best2OfferVolume       // int
                    && Best3Offer == other.Best3Offer             // int
                    && Best3OfferVolume == other.Best3OfferVolume       // int
                    ;
            }
        }
        /// <summary>
        ///  SECURITYOL.DAT - Chứa thông tin về giá, khối lượng giao dịch lô lẻ của chứng khoán
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct SECURITYOL
        {
            public short StockNo;		    // Mã chứng khoán dạng số
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] StockSymbol;		// Mã chứng khoán dạng chuỗi
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] StockType;		// Lọai chứng khóan: + S: Cổ phiếu + D: Trái phiếu + U: Chứng chỉ quỹ
            public int Ceiling;		        // Giá trần
            public int Floor;               // Giá sàn
            public int PriorClosePrice;		// Giá đóng cửa gần nhất

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 25)]
            public char[] SecurityName;     // Tên đây đủ của chứng khoán

            public int LastOL;		        // Giá khớp
            public int LastOLVol;               // Tổng khối lượng khớp

            public int Best1Bid;		    // Giá đặt mua tốt nhất 1
            public int Best1BidVolume;		// Khối lượng tương ứng giá đặt mua 1
            public int Best2Bid;		    // Giá đặt mua tốt nhất 2
            public int Best2BidVolume;		// Khối lượng tương ứng giá đặt mua 2
            public int Best3Bid;		    // Giá đặt mua tốt nhất 3
            public int Best3BidVolume;		// Khối lượng tương ứng giá đặt mua 3
            public int Best1Offer;		    // Giá đặt bán tốt nhất 1
            public int Best1OfferVolume;	// Khối lượng tương ứng giá đặt bán 1
            public int Best2Offer;		    // Giá đặt bán tốt nhất 2
            public int Best2OfferVolume;	// Khối lượng tương ứng giá đặt bán 2
            public int Best3Offer;		    // Giá đặt bán tốt nhất 3
            public int Best3OfferVolume;    // Khối lượng tương ứng giá đặt bán 3


            // 2014-12-16 17:27:35 ngocta2
            // FAST-SPEED METHOD
            public override bool Equals(object obj)
            {
                //if (!(obj is STOCK_HCM))
                //  return false;

                var other = (SECURITYOL)obj;

                return StockNo == other.StockNo // short
                    && Ceiling == other.Ceiling // int
                    && Floor == other.Floor   // int


                    && LastOL == other.LastOL           // int
                    && LastOLVol == other.LastOLVol        // int
                                                           //&& LastVal      == other.LastVal        // double (ko tinh cai nay vi co the day la val PT

                    && Best1Bid == other.Best1Bid             // int
                    && Best1BidVolume == other.Best1BidVolume       // int
                    && Best2Bid == other.Best2Bid             // int
                    && Best2BidVolume == other.Best2BidVolume       // int
                    && Best3Bid == other.Best3Bid             // int
                    && Best3BidVolume == other.Best3BidVolume       // int
                    && Best1Offer == other.Best1Offer             // int
                    && Best1OfferVolume == other.Best1OfferVolume       // int
                    && Best2Offer == other.Best2Offer             // int
                    && Best2OfferVolume == other.Best2OfferVolume       // int
                    && Best3Offer == other.Best3Offer             // int
                    && Best3OfferVolume == other.Best3OfferVolume       // int
                    ;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct MARKET_STAT
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] ControlCode;       //Trạng thái(string,1byt)
            public int Time;                 //Thời gian của máy chủ (long,4 bytes)

        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]

        /// LS.DAT - File thông tin Mã xác nhận lệnh khớp+ Mã chứng khoán dạng số +Khối lượng khớp+Giá khớp+Bên mua/bán
        public struct LS
        {
            public long ConfirmNo;          //Mã xác nhận lệnh khớp
            public int StockNo;             //Mã chứng khoán dạng số
            public double MatchedVol;       //Khối lượng khớp
            public int Price;               //Giá khớp
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Side;             //Bên mua bên bán
        }
        /// LO.DAT - File thông tin Mã xác nhận lệnh khớp+ Mã chứng khoán dạng số +Khối lượng khớp+Giá khớp+Bên mua/bán lô lẻ
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LO
        {
            public long ConfirmNo;          //Mã xác nhận lệnh khớp
            public int StockNo;             //Mã chứng khoán dạng số
            public double MatchedVol;       //Khối lượng khớp
            public int Price;               //Giá khớp
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Side;             //Bên mua bên bán
        }
        /// FROOM.DAT - File Room cua nha dau tu nuoc ngoai
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct FROOM
        {
            public int StockNo;
            public double TotalRoom;                //Tong room NDTNN duoc phep mua
            public double CurrentRoom;              //Room con lai NDTNN dc phep mua
            public double BuyVolume;                //Tong khoi luong nuoc ngoai mua
            public double SellVolume;               //Tong khoi luong nuoc ngoai ban
        }
        /// OS.DAT - Thông tin về Giá mở cửa của đợt khớp lệnh định kỳ
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct OS
        {
            public int StockNo;                     //ID CK(long,4)
            public int Price;                       //Gia(long,4)
        }
        /// LE.DAT - File thông tin giá khớp, khối lượng khớp, thời gian khớp trên MainBoard
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct LE
        {
            public int StockNo;                     //Ma chung khoan dang so
            public int Price;                       //Gia khop
            public int AccumulatedVol;              //Tong khoi luong
            public double AccumulatedVal;           //Tong gia tri
            public int Highest;                     //Gia khop cao nhat
            public int Lowest;                      //Gia khop thap nhat
            public int Time;                        //Thoi gian khop
        }
        /// <summary>
        /// PUT_AD.DAT - File thông tin về các quảng cáo giao dịch thỏa thuận của trái phiếu và cổ phiếu trên BigLotBoard.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PUT_AD
        {
            public short TradeID;		// 412           - Số hiệu giao dịch do mát chủ cấp (Integer, 2)
            public short StockNo;		// 3434 (TYA)    - Mã chứng khoán dạng số (Integer, 2)
            public int Vol;		        // 200000        - Khối lượng (Long, 4)
            public double Price;		// 10.2          - Giá (Double, 8)
            public int FirmNo;		    // 41            - Số hiệu Broker đăng quảng cáo (Long, 4) >> mã CTCK
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Side;		    // B             - Đăng mua/bán (String,1 )
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Board;		// B             - Bảng giao dịch  B: BigLotBoard.
            public int Time;		    // 103013        - Thời gian đăng quảng cáo (Long, 4)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Flag;		    // A             - Tình trạng của tin đăng quảng cáo: + A: Quảng cáo được đăng + C: Quảng cáo bị hủy
        }
        /// PUT_EXEC.DAT - File Thông tin về lệnh giao dịch thỏa thuận được khớp.
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PUT_EXEC
        {
            public long ConfirmNo;                  //Số hiệu giao dịch do máy chủ cấp
            public short StockNo;                   //Mã chứng khoán dạng số
            public int Vol;                         //Khối lượng(long,4) 
            public int Price;                       //Giá
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Board;                    //Bảng giao dịch  B:BigLotBoard
        }
        /// <summary>
        /// PUT_DC.DAT - Thông tin về lệnh khớp giao dịch thỏa thuận bị hủy. [chưa có trường hợp nào] 
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PUT_DC
        {
            public long ConfirmNo;		// Số hiệu giao dịch do máy chủ cấp (Long, 4)
            public short StockNo;		// Mã chứng khoán dạng số (Integer, 2)
            public int Vol;		        // Khối lượng (Long, 4)
            public int Price;		    // Giá (Long, 4)
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
            public char[] Board;		// Bảng giao dịch  B: BigLotBoard.
        }
        
        //FULL_ROW_QUOTE -- 4 file SECURITY,LS,OS,FROM
        public struct FULL_ROW_QUOTE
        {
            public string Co; //{get; set; }  // 00 - code
            public string Re; //{get; set; }  // 01 - ref
            public string Ce; //{get; set; }  // 02 - ceiling
            public string Fl; //{get; set; }  // 03 - floor

            public string BQ4; //{get; set; } // 04 - buy quantity 4 //BQ4 = KL mua 4+ = TotalBidQtty - BQ1 - BQ2 - BQ3 [tu tinh]
            public string BP3; //{get; set; } // 05 - buy price 3
            public string BQ3; //{get; set; } // 06 - buy quantity 3
            public string BP2; //{get; set; } // 07 - buy price 2
            public string BQ2; //{get; set; } // 08 - buy quantity 2
            public string BP1; //{get; set; } // 09 - buy price 1
            public string BQ1; //{get; set; } // 10 - buy quantity 1

            public string MP; //{get; set; }  // 11 - match price
            public string MQ; //{get; set; }  // 12 - match quantity
            public string MC; //{get; set; }  // 13 - match change

            public string SP1; //{get; set; } // 14 - sell price 1
            public string SQ1; //{get; set; } // 15 - sell quantity 1
            public string SP2; //{get; set; } // 16 - sell price 2
            public string SQ2; //{get; set; } // 17 - sell quantity 2
            public string SP3; //{get; set; } // 18 - sell price 3
            public string SQ3; //{get; set; } // 19 - sell quantity 3
            public string SQ4; //{get; set; } // 20 - sell quantity 4

            public string TQ; //{get; set; }  // 21 - total quantity (NM)
            public string Op; //{get; set; }  // 22 - open (NM)
            public string Hi; //{get; set; }  // 23 - highest (NM)
            public string Lo; //{get; set; }  // 24 - lowest (NM)
            public string Av; //{get; set; }  // 25 - average (NM)
            public string FB; //{get; set; }  // 26 - foreign buy (quantity)
            public string FS; //{get; set; }  // 27 - foreign sell (quantity)
            public string FR; //{get; set; }  // 28 - foreign room (current)
                              //-------------------
            /* public int SN;*/ //{get; set; }     // 29 - hidden - StockNo
            public string SN;
            public string ST; //{get; set; }  // 30 - hidden - StockType
            public string PO; //{get; set; }  // 31 - hidden - ProjectOpen
            public string Ri; //{get; set; }  // 32 - hidden - Rights 
            public string MPO; //{ get; set; } // 33 - hidden - MP old - truoc khi bi chuyen sang ProjectOpen
            public string MQO; //{ get; set; } // 34 - hidden - MQ old - truoc khi bi chuyen sang 0
            public string TQO; //{get; set; }  // 35 - hidden - total quantity old
            //==================
            //public void Reset()
            //{
            //    this.BP1 = "-1";
            //    this.SP1 = "-1";
            //}
        }
        public struct FULL_ROW_PT
        {
            public FULL_PUT_AD[] PUT_AD_BUY;            //Lệnh PT mua
            public FULL_PUT_AD[] PUT_AD_SELL;           //Lệnh PT bán
            public FULL_PUT_EXEC[] PUT_EXEC;            //Các lệnh PT đã khớp
            public FULL_PUT_EXEC[] PUT_DC;              //PUT_DC.DAT:Thông tin về lệnh khớp giao dịch thỏa thuận bị hủy
        }
        
        public struct FULL_PUT_AD
        {
            //MARKET_STAT
            public string Auto;
            public string Code;
            public string Price;
            public string Quantity;
            public string Time;                     //-Thời gian đăng quảng cáo (Long, 4), thoi gian cua server tai So HOSE
            public string TradeID;
        }
        
        public struct FULL_PUT_EXEC
        {
            //MARKET_STAT
            public string Auto;             //STT	
            public string Code;             //Mã CK
            public string Re;               //Trần
            public string Ce;               //Sàn
            public string Fl;               //TC
            public string PTPrice;          //Giao dịch thỏa thuận	- Giá
            public string PTQuantity;       //Giao dịch thỏa thuận	- KL    
            public string PTTotalQuantity;  //Giao dịch thỏa thuận	- Tổng KL
            public string PTTotalValue;     //Giao dịch thỏa thuận	- Tổng GT
            public string NMTotalQuantity;  //KL GD khớp lệnh
            public string NMPTTotalQuantity;//Tổng KL giao dịch
            public string ListingQuantity;  //Số lượng niêm yết
            public string Time;             // time cua app server
            public string ConfirmNo;        // hidden field , dung khi Cancel order PUT_DC
        }
        /// <summary>
        /// YYYYMMDD_VNX.DAT: Thông tin tổng hợp chỉ số VNX.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct VNX_MARKET_INDEX
        {
            public int IndexValue;          // 43092     - VNIndex (Long, 4bytes)
            public double TotalSharesAOM;     // 12278530  - Tổng khối lượng (Double, 8bytes)
            public double TotalValuesAOM;     // 436692    - Tổng giá trị (Double, 8bytes)
            public double TotalSharesPT;      // 12278530  - Tổng khối lượng (Double, 8bytes)
            public double TotalValuesPT;      // 436692    - Tổng giá trị (Double, 8bytes)
            public short Up;                // Short 2 Tổng số CK tăng giá …
            public short Down;              // Short 2 Tổng số CK giảm giá …
            public short NoChange;          // Short 2 Tổng số CK đứng giá …
            public short Ceiling;           // Short 2 Tổng số CK tăng trần
            public short Floor;             // Short 2 Tổng số CK giảm sàn 
            public int Time;                // Thời gian của máy chủ giao dịch (Long, 4bytes)
        }
        /// <summary>
        /// YYYYMMDD_VNX.DAT: Thông tin tổng hợp chỉ số VNX .
        /// </summary>        
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct VNX_MARKET_VNINDEX
        {
            public int Index;          // 43092     - VNIndex (Long, 4bytes)
            public int TotalTrade;     //  - số lượng giao dịch (long, 4bytes)
            public double TotalShares;      //   - Tổng khối lượng (Double, 8bytes)
            public double TotalValues;     //    - Tổng giá trị (Double, 8bytes)
            public double UpVolume;                //  Tồng KL của các CK tăng giá(Double, 8bytes) …
            public double DownVolume;              //  Tồng KL của các CK giảm giá(Double, 8bytes) …
            public double NoChangeVolume;          // Tổng KL của các CK đứng giá (Double, 8bytes)
            public short Up;                // Short 2 Tổng số CK tăng giá …
            public short Down;              // Short 2 Tổng số CK giảm giá …
            public short NoChange;          // Short 2 Tổng số CK đứng giá …
            public int Time;                // Thời gian của máy chủ giao dịch (Long, 4bytes)
        }
        /// <summary>
        /// YYYYMMDD_VNX.DAT: Thông tin tổng hợp chỉ số VNX .
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct VNX_MARKET_LIST
        {
            public short StockNo;           // Mã chứng khoán dạng số [2bytes]
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] StockSymbol;		// Mã chứng khoán dạng chuỗi [8bytes]
        }
        /// <summary>
        /// Thông tin tổng hợp iIndex
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct IINDEX
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] iIndexSymbol;		// Mã iIndex dạng chuỗi
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] ETFSymbol;		// Mã ETF dạng chuỗi
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
            public char[] IndexSymbol;		// Mã ETF dạng chuỗi
            public int iIndex;              // Chỉ số iIndex
            public int Time;                // Thời gian của máy chủ giao dịch (Long, 4bytes)
        }
        /// Thông tin tổng hợp iNAV
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct INAV
        {
            public short StockNo;           // Mã ETF dạng số
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] StockSymbol;		// Mã chứng khoán dạng chuỗi
            public int iNAV;                // Giá trị tài sản ròng tham chiếu
            public int Time;                // Thời gian của máy chủ giao dịch (Long, 4bytes)
        }
        public struct FULL_ROW_INDEX
        {
            //MARKET_STAT
            public string STAT_ControlCode;
            public string STAT_Time;
            public string STAT_Date;

            //VNINDEX 2021-05-28 hungtq2
            //public string VNINDEX_ChangePercent;
            //public string VNINDEX_Change;
            //public string VNINDEX_Index;          // 43092     - VNIndex (Long, 4bytes)
            //public string VNINDEX_TotalTrade;     //  - số lượng giao dịch (long, 4bytes)
            //public string VNINDEX_TotalShares;      //   - Tổng khối lượng (Double, 8bytes)
            //public string VNINDEX_TotalValues;     //    - Tổng giá trị (Double, 8bytes)
            //public string VNINDEX_UpVolume;                //  Tồng KL của các CK tăng giá(Double, 8bytes) …
            //public string VNINDEX_DownVolume;              //  Tồng KL của các CK giảm giá(Double, 8bytes) …
            //public string VNINDEX_NoChangeVolume;          // Tổng KL của các CK đứng giá (Double, 8bytes)
            //public string VNINDEX_Up;                // Short 2 Tổng số CK tăng giá …
            //public string VNINDEX_Down;              // Short 2 Tổng số CK giảm giá …
            //public string VNINDEX_NoChange;          // Short 2 Tổng số CK đứng giá …
            //public string VNINDEX_Time;                // Thời gian của máy chủ giao dịch (Long, 4bytes)

            //TOTALMKT, VNINDEX
            public string VNI_ChangePercent;
            public string VNI_Change;
            public string VNI_IndexValue;      // 1. IndexValue
            public string VNI_TotalTrade;
            public string VNI_TotalSharesAOM;  // 2. TotalSharesAOM
            public string VNI_TotalValuesAOM;  // 3. TotalValuesAOM
            public string VNI_UpVolume;
            public string VNI_NoChangeVolume;
            public string VNI_DownVolume;
            public string VNI_Up;     // 4. Up
            public string VNI_Down;     // 5. Down
            public string VNI_NoChange;
            public string VNI_Time;
            public string VNI_Ceiling;  // so ma tang tran (tu tinh)
            public string VNI_Floor;    // so ma giam san (tu tinh)
            public string VNI_TotalSharesOld; // luu so cu de tinh ra data chart (vol trong tung phut)

            //VN30
            public string VN30_ChangePercent;
            public string VN30_Change;
            public string VN30_IndexValue;
            public string VN30_TotalSharesAOM;
            public string VN30_TotalValuesAOM;
            public string VN30_TotalSharesPT;
            public string VN30_TotalValuesPT;
            public string VN30_Up;
            public string VN30_Down;
            public string VN30_NoChange;
            public string VN30_Ceiling;
            public string VN30_Floor;
            public string VN30_Time;
            //public string VN30_TotalSharesAOMOld;// luu so cu de tinh ra data chart (vol trong tung phut)

            //VN100
            public string VN100_ChangePercent;
            public string VN100_Change;
            public string VN100_IndexValue;
            public string VN100_TotalSharesAOM;
            public string VN100_TotalValuesAOM;
            public string VN100_TotalSharesPT;
            public string VN100_TotalValuesPT;
            public string VN100_Up;
            public string VN100_Down;
            public string VN100_NoChange;
            public string VN100_Ceiling;
            public string VN100_Floor;
            public string VN100_Time;
            //public string VN100_TotalSharesAOMOld;// luu so cu de tinh ra data chart (vol trong tung phut)

            //VNALL
            public string VNALL_ChangePercent;
            public string VNALL_Change;
            public string VNALL_IndexValue;
            public string VNALL_TotalSharesAOM;
            public string VNALL_TotalValuesAOM;
            public string VNALL_TotalSharesPT;
            public string VNALL_TotalValuesPT;
            public string VNALL_Up;
            public string VNALL_Down;
            public string VNALL_NoChange;
            public string VNALL_Ceiling;
            public string VNALL_Floor;
            public string VNALL_Time;

            //VNXALL            {2016-10-26 14:28:36 ngocta2}
            public string VNXALL_ChangePercent;
            public string VNXALL_Change;
            public string VNXALL_IndexValue;
            public string VNXALL_TotalSharesAOM;
            public string VNXALL_TotalValuesAOM;
            public string VNXALL_TotalSharesPT;
            public string VNXALL_TotalValuesPT;
            public string VNXALL_Up;
            public string VNXALL_Down;
            public string VNXALL_NoChange;
            public string VNXALL_Ceiling;
            public string VNXALL_Floor;
            public string VNXALL_Time;
            //public string VNALL_TotalSharesAOMOld;// luu so cu de tinh ra data chart (vol trong tung phut)

            //VNMID
            public string VNMID_ChangePercent;
            public string VNMID_Change;
            public string VNMID_IndexValue;
            public string VNMID_TotalSharesAOM;
            public string VNMID_TotalValuesAOM;
            public string VNMID_TotalSharesPT;
            public string VNMID_TotalValuesPT;
            public string VNMID_Up;
            public string VNMID_Down;
            public string VNMID_NoChange;
            public string VNMID_Ceiling;
            public string VNMID_Floor;
            public string VNMID_Time;
            //public string VNMID_TotalSharesAOMOld;// luu so cu de tinh ra data chart (vol trong tung phut)
            
            //VNSML
            public string VNSML_ChangePercent;
            public string VNSML_Change;
            public string VNSML_IndexValue;

            public string VNSML_TotalSharesAOM;
            public string VNSML_TotalValuesAOM;
            public string VNSML_TotalSharesPT;
            public string VNSML_TotalValuesPT;
            public string VNSML_Up;
            public string VNSML_Down;
            public string VNSML_NoChange;
            public string VNSML_Ceiling;
            public string VNSML_Floor;
            public string VNSML_Time;
            //public string VNSML_TotalSharesAOMOld;// luu so cu de tinh ra data chart (vol trong tung phut)

            //iNAV
            public string INAV_StockNo;
            public string INAV_StockSymbol;
            public string INAV_iNAV;
            public string INAV_Time;

            //iIndex
            public string IINDEX_iIndexSymbol;
            public string IINDEX_ETFSymbol;
            public string IINDEX_IndexSymbol;
            public string IINDEX_iIndex;
            public string IINDEX_Time;
        }
        public class LastIndexHO
        {
            public string Time { get; set; }
            public List<LastIndexHODetail> Data { get; set; }
        }
        public class LastIndexHODetail
        {
            public string TradingDate { get; set; }
            public double VNIndex { get; set; }
            public double VNSML { get; set; }
            public double VNMID { get; set; }
            public double VNALL { get; set; }
            public double VN30 { get; set; }
            public double VN100 { get; set; }
            public double VNXALL { get; set; }      // 2016-10-26 14:37:53 ngocta2
        }

        public const string MARKET_STAT_PRE = "J"; // truoc 09h00 => 62635 | End EOD Security Update Transmission
        public const string MARKET_STAT_ATO = "P"; // phien ATO 09h00-09h15
        public const string MARKET_STAT_ATC = "A"; // phien ATC 14h30-14h45
        public const string MARKET_STAT_CON = "O"; // phien LT  09h15-11h30 + 13h00-14h30 
        public const string MARKET_STAT_INT = "I"; // nghi trua 11h30-13h00
        public const string MARKET_STAT_INT_OL = "L"; // nghi trua 11h30-13h00 lô lẻ
        public const string MARKET_STAT_CLO = "C"; // dong cua mainboard [14h45-15h00] day la luc dang giao dich trai phieu        
        public const string MARKET_STAT_END = "K"; // ket thuc K => 150000	ghi data cuoi ngay vao REDIS (~ET file cua 4G)
        public const string MARKET_STAT_ATC_OL = "S"; // 14h30-14h45 lô lẻ
    }
}
