using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;

namespace DemoCSharpApp
{
    public partial class Form1 : Form
    {
        HzSdkLib ClsHzSdkLib = new HzSdkLib();

        public Form1()
        {
            InitializeComponent();

            //user
            UInitialize();

        }

        public void UInitialize()
        {
            Int32 rc = 0;

            if (HzSdkLib.TXRX_CH_NUM == 4)
            {
                /*just only init once*/
                rc = HzSdkLib.DevApi_UbertInitDevDll((Int32)HzSdkLib._DEVICE_TYPE.DEVICE_UBERT_4CH_10G25G, 0, 0);
            }
            else
            {
                /*just only init once*/
                rc = HzSdkLib.DevApi_UbertInitDevDll((Int32)HzSdkLib._DEVICE_TYPE.DEVICE_UBERT_2CH_10G25G, 0, 0);
            }

            if (rc != HzSdkLib.RETURN_TRUE)
            {
                MessageBox.Show("initialize dll fail!");
                System.Environment.Exit(0);
            }
        }

        public void DemoOfHzDevApiDllFor_4Ch10G25G()
        {
            Int32 rc = 0;
            Int32 i = 0;

            UInt32[] DevArray = new UInt32[HzSdkLib.DEVICE_NUM_MAX];
            Int32 GotDevNum = 0;
            UInt32 DevNumID = 0;

            byte[] AllParm = new byte[1 * 128 + HzSdkLib.TXRX_CH_NUM * 128];

            //UInt32 Ch = 0;
            UInt32 iCh = 0;

            string[] DevProductIdStrArrar = new string[]{"00000000000000010001010500000000"
                                                        ,"00000000000000010001010500000001"
                                                        ,"00000000000000010001010500000002"
                                                        ,"00000000000000010001010500000003"};

            while (true)
            {
                GotDevNum = ClsHzSdkLib.CSharp_UbertGetHidDevList(ref DevArray);
                if (GotDevNum <= 0)
                    //return;
                    continue;

                for (i = 0; i < DevProductIdStrArrar.Count(); i++)
                {
                    DevNumID = HzSdkLib.DevApi_UbertGetDevNumIdAccordProductId(DevProductIdStrArrar[i]);
                    if (DevNumID == 0xFFFFFFFF)
                        continue;

                    rc = HzSdkLib.DevApi_UbertGetDevConnecttedStatus(DevNumID);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;

                    rc = ClsHzSdkLib.CSharp_UbertGetAllParameters(DevNumID, ref AllParm);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    ClsHzSdkLib.ParseAllParams(AllParm);

                    if ((ClsHzSdkLib.SynParams.DataRate >= 25000000000) && (ClsHzSdkLib.SynParams.DataRate <= 28000000000))
                        ClsHzSdkLib.SynParams.DataRate += 1000000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate >= 1800000000) && (ClsHzSdkLib.SynParams.DataRate <= 13500000000))
                        ClsHzSdkLib.SynParams.DataRate += 1000000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate > 28000000000))
                        ClsHzSdkLib.SynParams.DataRate = 1800000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate > 13500000000))
                        ClsHzSdkLib.SynParams.DataRate = 25000000000;
                    ClsHzSdkLib.SynParams.Prbs = (UInt32)(ClsHzSdkLib.SynParams.Prbs == 1 ? 0 : 1);
                    ClsHzSdkLib.SynParams.TxJitter = (UInt16)(ClsHzSdkLib.SynParams.TxJitter == 0 ? 8 : 0);
                    rc = HzSdkLib.DevApi_UbertSetParamDataRateAndPrbsAndJitter(DevNumID, ClsHzSdkLib.SynParams.DataRate, ClsHzSdkLib.SynParams.Prbs, ClsHzSdkLib.SynParams.PrbsCus, ClsHzSdkLib.SynParams.TxJitter);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    Thread.Sleep(5000);

                    rc = ClsHzSdkLib.CSharp_UbertGetAllParameters(DevNumID, ref AllParm);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    ClsHzSdkLib.ParseAllParams(AllParm);

                    ClsHzSdkLib.SynParams.TriggerOfCdrDiv = (UInt32)(ClsHzSdkLib.SynParams.TriggerOfCdrDiv == 1 ? 0 : 1);
                    rc = HzSdkLib.DevApi_UbertSetParamTriggerFrequencyDiv(DevNumID, 0, ClsHzSdkLib.SynParams.TriggerOfCdrDiv);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    Thread.Sleep(200);

                    if (ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude > 400)
                        ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude -= 1 * 100;
                    rc = HzSdkLib.DevApi_UbertSetParamTriggerAmplitude(DevNumID, 0, ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;

                    for (iCh = 0; iCh < (int)HzSdkLib.TXRX_CH_NUM; iCh++)
                    {
                        if (ClsHzSdkLib.TxRxChParams[iCh].Amplitude > 800)
                            ClsHzSdkLib.TxRxChParams[iCh].Amplitude -= iCh * 100;
                        rc = HzSdkLib.DevApi_UbertSetParamAmplitude(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].Amplitude);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].RxPol = (byte)(ClsHzSdkLib.TxRxChParams[iCh].RxPol == 1 ? 0 : 1);
                        rc = HzSdkLib.DevApi_UbertSetParamRxPolarity(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].RxPol);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis = (byte)(ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis == iCh ? 5 : iCh);
                        rc = HzSdkLib.DevApi_UbertSetParamDeemphasis(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].TxMute = (byte)(ClsHzSdkLib.TxRxChParams[iCh].TxMute == 0 ? 1 : 0);
                        rc = HzSdkLib.DevApi_UbertSetParamTxMute(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].TxMute);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].RxEqualization = (byte)(ClsHzSdkLib.TxRxChParams[iCh].RxEqualization == 0 ? 3 : 0);
                        rc = HzSdkLib.DevApi_UbertSetParamRxEqualization(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].RxEqualization);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;
                    }

                    //string DevPath = "HID\\VID_0Ff0&PID_0017&REV_0200";
                    //rc = HzSdkLib.DevApi_UbertVerifyIfValidDevAccordDevicePathAndType(DevPath, 0);
                    //if (rc != HzSdkLib.RETURN_TRUE)
                    //    continue;
                }


            }
            //return true;
        }
        public void DemoOfHzDevApiDllFor_1Ch2Ch10G25G()
        {
            Int32 rc = 0;
            Int32 i = 0;

            UInt32[] DevArray = new UInt32[HzSdkLib.DEVICE_NUM_MAX];
            Int32 GotDevNum = 0;
            UInt32 DevNumID = 0;

            byte[] AllParm = new byte[1 * 128 + HzSdkLib.TXRX_CH_NUM * 128];

            //UInt32 Ch = 0;
            UInt32 iCh = 0;

            string[] DevProductIdStrArrar = new string[]{"00000000000000010001010500000000"
                                                        ,"00000000000000010001010500000001"
                                                        ,"00000000000000010001010500000002"
                                                        ,"00000000000000010001010500000003"};

            byte[] GotDevProductId = new byte[32];

            while (true)
            {
                GotDevNum = ClsHzSdkLib.CSharp_UbertGetHidDevList(ref DevArray);
                if (GotDevNum <= 0)
                    continue;

                Array.Clear(GotDevProductId, 0, GotDevProductId.Length);
                rc = ClsHzSdkLib.CSharp_UbertGetDevProductIdAccordNumId(DevArray[0], ref GotDevProductId);
                if (rc != HzSdkLib.RETURN_TRUE)
                    continue;
                string GotDevProductIdStr = System.Text.Encoding.ASCII.GetString(GotDevProductId).Replace('\0', ' ').Trim();

                for (i = 0; i < DevProductIdStrArrar.Count(); i++)
                {


                    DevNumID = HzSdkLib.DevApi_UbertGetDevNumIdAccordProductId(DevProductIdStrArrar[i]);
                    if (DevNumID == 0xFFFFFFFF)
                        continue;

                    rc = HzSdkLib.DevApi_UbertGetDevConnecttedStatus(DevNumID);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;

                    rc = ClsHzSdkLib.CSharp_UbertGetAllParameters(DevNumID, ref AllParm);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    ClsHzSdkLib.ParseAllParams(AllParm);

                    if ((ClsHzSdkLib.SynParams.DataRate >= 25000000000) && (ClsHzSdkLib.SynParams.DataRate <= 28000000000))
                        ClsHzSdkLib.SynParams.DataRate += 1000000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate >= 1800000000) && (ClsHzSdkLib.SynParams.DataRate <= 13500000000))
                        ClsHzSdkLib.SynParams.DataRate += 1000000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate > 28000000000))
                        ClsHzSdkLib.SynParams.DataRate = 1800000000;
                    else if ((ClsHzSdkLib.SynParams.DataRate > 13500000000))
                        ClsHzSdkLib.SynParams.DataRate = 25000000000;

                    ClsHzSdkLib.SynParams.Prbs = (UInt32)(ClsHzSdkLib.SynParams.Prbs == 1 ? 0 : 1);
                    /*call this function will clear the rx pol*/
                    rc = HzSdkLib.DevApi_UbertSetParamDataRateAndPrbs(DevNumID, ClsHzSdkLib.SynParams.DataRate, ClsHzSdkLib.SynParams.Prbs, ClsHzSdkLib.SynParams.PrbsCus);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    Thread.Sleep(3000);

                    rc = ClsHzSdkLib.CSharp_UbertGetAllParameters(DevNumID, ref AllParm);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    ClsHzSdkLib.ParseAllParams(AllParm);


                    ClsHzSdkLib.SynParams.TriggerOfCdrDiv = (UInt32)(ClsHzSdkLib.SynParams.TriggerOfCdrDiv == 1 ? 0 : 1);
                    rc = HzSdkLib.DevApi_UbertSetParamTriggerFrequencyDiv(DevNumID, 0, ClsHzSdkLib.SynParams.TriggerOfCdrDiv);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;
                    Thread.Sleep(200);

                    if (ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude > 300)
                        ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude -= 1 * 100;
                    rc = HzSdkLib.DevApi_UbertSetParamTriggerAmplitude(DevNumID, 0, ClsHzSdkLib.SynParams.TriggerOfCdrAmplitude);
                    if (rc != HzSdkLib.RETURN_TRUE)
                        continue;

                    for (iCh = 0; iCh < (int)HzSdkLib.TXRX_CH_NUM; iCh++)
                    {
                        if (ClsHzSdkLib.TxRxChParams[iCh].Amplitude > 500)
                            ClsHzSdkLib.TxRxChParams[iCh].Amplitude -= iCh * 100;
                        rc = HzSdkLib.DevApi_UbertSetParamAmplitude(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].Amplitude);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].RxPol = (byte)(ClsHzSdkLib.TxRxChParams[iCh].RxPol == 1 ? 0 : 1);
                        rc = HzSdkLib.DevApi_UbertSetParamRxPolarity(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].RxPol);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis = (byte)(ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis == iCh ? 5 : iCh);
                        rc = HzSdkLib.DevApi_UbertSetParamDeemphasis(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].TxDeemphasis);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        ClsHzSdkLib.TxRxChParams[iCh].TxMute = (byte)(ClsHzSdkLib.TxRxChParams[iCh].TxMute == 0 ? 1 : 0);
                        rc = HzSdkLib.DevApi_UbertSetParamTxMute(DevNumID, iCh, ClsHzSdkLib.TxRxChParams[iCh].TxMute);
                        if (rc != HzSdkLib.RETURN_TRUE)
                            continue;

                        Thread.Sleep(200);
                    }

                    //string DevPath = "HID\\VID_0Ff0&PID_0017&REV_0200";
                    //rc = HzSdkLib.DevApi_UbertVerifyIfValidDevAccordDevicePathAndType(DevPath, 0);
                    //if (rc != HzSdkLib.RETURN_TRUE)
                    //    continue;
                }


            }
            //return true;
        }
        private void TestDemo_Click(object sender, EventArgs e)
        {
            if (HzSdkLib.TXRX_CH_NUM==4)
                DemoOfHzDevApiDllFor_4Ch10G25G();
            else
                DemoOfHzDevApiDllFor_1Ch2Ch10G25G();
        }


    }
}
