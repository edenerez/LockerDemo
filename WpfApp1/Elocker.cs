using DcdzDriverBoardLib;
using DcdzMsCommLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Elocker
    {
        private readonly DriverBoardClass _driverboard = new DriverBoardClass();


        //打开柜子串口
        public bool open(string comName)
        {
            try
            {
                uint funcret = _driverboard.Open(comName);
                if (funcret == 0)
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
           
        }

        //打开箱门 （driverID=0,boxID=1:打开1号柜子的1号门）
        public bool openBox(int driverID, int boxID)
        {
            try
            {
                byte ret;
                int funcret = _driverboard.OpenBox((byte)driverID, (byte)boxID, out ret);
                if (funcret == 0)
                    return true;
                return false;
            }
            catch (Exception)
            {

                return false;
            }
          

        }

        //获取1号副柜的单个门状态  
        public void QueryBoxStatus(int boxId,out int OpenStatus, out int Article)
        {
            try
            {
                byte[,] ret = (byte[,])NeedState(0, true, true, false, false);
                OpenStatus = ret[1, boxId-1]==0?1:0;
                Article = ret[0, boxId-1];
            }
            catch (Exception)
            {
                OpenStatus = 0;
                Article = 0;
                throw;
            }
          
        }

        //获取1号副柜的所有门状态
        public byte[,] queryAllBoxStatus(int driverID)
        {
            return (byte[,])NeedState(driverID, true, true, false, false);

        }
        private Array NeedState(int drivernum, bool openflag, bool articleflag, bool warnflag, bool powerflag)
        {
            try
            {
                int ret;
                Array doorstate;

                ret = _driverboard.GetNeedState((byte)drivernum, articleflag, openflag, warnflag, powerflag, out doorstate);

                if (ret == 0)
                    return doorstate;
                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
    }

    public class Scanner
    {
        protected readonly DcdzMsComm DcdzMsComm = new DcdzMsCommClass();
        private OnReceived _onReceived=null ;
        public void startScanenr(OnReceived onReceived)
        {
            _onReceived += onReceived;
        }
        public bool open(string comName)
        {
            try
            {
                if (DcdzMsComm.Open(comName) == 0)
                {
                    DcdzMsComm.SetCommState(9600, 8, 0, 0);
                    DcdzMsComm.SetCommTimeouts(500, 50, 3000, 500, 500);
                    DcdzMsComm.SetReadWriteQueue(100, 100);
                    IMsCommEvent imsComEvent = DcdzMsComm.GetCommEvent();
                    imsComEvent.COMM_EV_RXCHAR = true;
                    imsComEvent.COMM_EV_ERR = true;
                    DcdzMsComm.OpenCommEventNotification(imsComEvent);
                    DcdzMsComm.CommEvent += DcdzMsComm_CommEvent;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                return false;
            }
           
         
           
        }

        private string _datastr = "";
        void DcdzMsComm_CommEvent(uint uEventMark)
        {
            try
            {
                if (uEventMark == 0)
                {
                    int datalen = (int)DcdzMsComm.GetInQueue();
                    Array databuffer = new byte[datalen];
                    DcdzMsComm.Read(ref databuffer, datalen);

                    #region 结果处理


                    #endregion

                    foreach (byte item in (byte[])databuffer)
                    {
                        if (item == 0x0D && !string.IsNullOrEmpty(_datastr))
                        {
                            //扫描枪扫描出现乱码的问题
                            if ("html".Equals(_datastr.ToLower().Trim()))
                            {
                                _datastr = "";
                                break;
                            }

                            _onReceived(_datastr);
                              _datastr = "";
                            break;
                        }

                        _datastr += Convert.ToChar(item);
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
    public delegate void OnReceived(string dataInfo);


}
