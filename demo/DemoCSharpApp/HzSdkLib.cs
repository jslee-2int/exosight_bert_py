using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace DemoCSharpApp
{
    public class SynParam
    {
        public UInt64 DataRate;
        public UInt32 Prbs;
        public UInt64 PrbsCus;
        public UInt32 TriggerOfCdrDiv;
        public UInt32 TriggerOfCdrAmplitude;
        public UInt16 TxJitter;

        public SynParam()
        {
            DataRate = 0;
            Prbs = 0;
            PrbsCus = 0;
            TriggerOfCdrDiv = 0;
            TriggerOfCdrAmplitude = 0;
            TxJitter = 0;  
        }
    }
    public class TxRxChParam
    {
        public Byte ConfigCompleted;
        public UInt32 Amplitude;
        public Byte RxPol;
        public UInt32 BerRunTime;
        public UInt32 BerLockTime;
        public UInt64 BerBitCount;
        public UInt64 BerErrorCount;
        public Byte BerLockStatus;
        public Byte BerErrorCountOverflow;
        public UInt32 Prbs;
        public UInt64 PrbsCus;
        public UInt32 TxDeemphasis;
        public Byte TxMute;
        public Byte RxEqualization;

        public TxRxChParam()
        { 
            ConfigCompleted=0;
            Amplitude = 0;
            RxPol = 0;
            BerRunTime = 0;
            BerLockTime = 0;
            BerBitCount = 0;
            BerErrorCount = 0;
            BerLockStatus = 0;
            BerErrorCountOverflow = 0;
            Prbs = 0;
            PrbsCus = 0;   
            TxDeemphasis = 0;
            TxMute = 0;
            RxEqualization = 0;
        }
    }
    class HzSdkLib
    {
        public const Int32 TXRX_CH_NUM = 2;

        public const Int32 DEVICE_NUM_MAX = 128;

        public const Int32 RETURN_TRUE = 1;
        public const Int32 RETURN_FALSE = 0;

        public enum _DEVICE_TYPE
        {
            DEVICE_UBERT_2CH_10G = 1,
            DEVICE_UBERT_2CH_10G25G,
            DEVICE_UBERT_4CH_10G25G,
            DEVICE_UBERT_4CH_10G,
            DEVICE_UGEN_1CH_10G,
            DEVICE_UGEN_2CH_10G,
            DEVICE_NULL,
        };

        public SynParam SynParams;
        public TxRxChParam[] TxRxChParams = new TxRxChParam[TXRX_CH_NUM];


        public HzSdkLib()
        {
            SynParams = new SynParam();

            for (int i = 0; i < TXRX_CH_NUM;i++ )
                TxRxChParams[i] = new TxRxChParam();
        }
        UInt32 HidDev_ByteToUInt32ForLittleEndian(byte[] data, UInt32 index)
        {
            Int32 i = 0;
            UInt32 Value = 0;
            while (i < 4)
            {
                Value = Value << 8;
                Value |= data[index + 3 - i];
                i++;
            }
            return Value;
        }
        UInt64 PubFun_ByteToUInt64ForBigEndian(byte[] data, Int32 index)
        {
            Int32 i = 0;
            UInt64 Value = 0;
            while (i < 8)
            {
                Value = Value << 8;
                Value |= data[index + i];
                i++;
            }
            return Value;
        }
        UInt32 PubFun_ByteToUInt32ForBigEndian(byte[] data, Int32 index)
        {
            Int32 i = 0;
            UInt32 Value = 0;
            while (i < 4)
            {
                Value = Value << 8;
                Value |= data[index + i];
                i++;
            }
            return Value;
        }
        UInt16 PubFun_ByteToUInt16ForBigEndian(byte[] data, Int32 index)
        {
            Int32 i = 0;
            UInt32 Value = 0;
            while (i < 2)
            {
                Value = Value << 8;
                Value |= data[index + i];
                i++;
            }
            return (UInt16)Value;
        }

        public void ParseAllParams(byte[] AllParams)
        {
            Int32 i =0;
	        Int32 Base = 0;
	        Int32 OffsetIndex = 0;

	        /*parse*/
	        Base =0;
	        OffsetIndex=0;
	        i=0;
	        /*DataRate*/
	        SynParams.DataRate=PubFun_ByteToUInt64ForBigEndian(AllParams,Base+OffsetIndex);
	        OffsetIndex+=sizeof(UInt64);

	        /*Prbs*/
            SynParams.Prbs = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
	        OffsetIndex+=sizeof(UInt32);

	        /*PrbsCus*/
            SynParams.PrbsCus = PubFun_ByteToUInt64ForBigEndian(AllParams, Base + OffsetIndex);
	        OffsetIndex+=sizeof(UInt64);

	        /*TriggerOfCdrDiv*/
            SynParams.TriggerOfCdrDiv = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
	        OffsetIndex+=sizeof(UInt32);

	        /*TriggerOfCdrAmplitude*/
            SynParams.TriggerOfCdrAmplitude = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
	        OffsetIndex+=sizeof(UInt32);

            /*reserved*/
            OffsetIndex += sizeof(UInt32);
            OffsetIndex += sizeof(UInt32);

            /*TxJitter*/
            SynParams.TxJitter = PubFun_ByteToUInt16ForBigEndian(AllParams, Base + OffsetIndex);
            OffsetIndex += sizeof(UInt16);

	        for(i=0;i<TXRX_CH_NUM;i++)
	        {
		        Base =i*128+128;
		        OffsetIndex=0;
		        /*ConfigCompleted*/
		        TxRxChParams[i].ConfigCompleted=AllParams[Base+OffsetIndex];
		        OffsetIndex+=sizeof(byte);
	
		        /*Amplitude*/
                TxRxChParams[i].Amplitude = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
		        OffsetIndex+=sizeof(UInt32);

		        /*RxPol*/
		        TxRxChParams[i].RxPol=AllParams[Base+OffsetIndex];
		        OffsetIndex+=sizeof(byte);

		        /*BerLockStatus*/
		        TxRxChParams[i].BerLockStatus=AllParams[Base+OffsetIndex];
		        OffsetIndex+=sizeof(byte);

		        /*BerErrorCountOverflow*/
		        TxRxChParams[i].BerErrorCountOverflow=AllParams[Base+OffsetIndex];
                OffsetIndex += sizeof(byte);

		        /*BerRunTime*/
                TxRxChParams[i].BerRunTime = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
		        OffsetIndex+=sizeof(UInt32);

		        /*BerLockTime*/
                TxRxChParams[i].BerLockTime = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
		        OffsetIndex+=sizeof(UInt32);

		        /*BerBitCount*/
                TxRxChParams[i].BerBitCount = PubFun_ByteToUInt64ForBigEndian(AllParams, Base + OffsetIndex);
		        OffsetIndex+=sizeof(UInt64);

		        /*BerErrorCount*/
                TxRxChParams[i].BerErrorCount = PubFun_ByteToUInt64ForBigEndian(AllParams, Base + OffsetIndex);
		        OffsetIndex+=sizeof(UInt64);

                /*Prbs*/
                TxRxChParams[i].Prbs = PubFun_ByteToUInt32ForBigEndian(AllParams, Base + OffsetIndex);
                OffsetIndex += sizeof(UInt32);

                /*PrbsCus*/
                TxRxChParams[i].PrbsCus = PubFun_ByteToUInt64ForBigEndian(AllParams, Base + OffsetIndex);
                OffsetIndex += sizeof(UInt64);

                /*TxDeemphasis*/
                TxRxChParams[i].TxDeemphasis = AllParams[Base + OffsetIndex];
                OffsetIndex += sizeof(byte);

                /*TxMute*/
                TxRxChParams[i].TxMute = AllParams[Base + OffsetIndex];
                OffsetIndex += sizeof(byte);

                /*RxEqualization*/
                TxRxChParams[i].RxEqualization = AllParams[Base + OffsetIndex];
                OffsetIndex += sizeof(byte);
	        }        
        
        }

        public Int32 CSharp_UbertGetHidDevList(ref UInt32[] DevArray)
        { 
            Int32 GotDevNum = 0;

            IntPtr NoManagedBuffer = IntPtr.Zero;
            NoManagedBuffer = Marshal.AllocHGlobal(DevArray.Length * sizeof(UInt32));
            if (NoManagedBuffer == IntPtr.Zero)
                return 0;

            byte[] ManagedBuffer = new byte[DevArray.Length * sizeof(UInt32)];

            GotDevNum = HzSdkLib.DevApi_UbertGetDevList(NoManagedBuffer);
            if (GotDevNum <= 0)
            {
                Marshal.FreeHGlobal(NoManagedBuffer);
                return 0;
            }
            Marshal.Copy(NoManagedBuffer, ManagedBuffer, 0, DevArray.Length);

            for (int i = 0; i < DevArray.Length; i++)
            {
                DevArray[i] = HidDev_ByteToUInt32ForLittleEndian(ManagedBuffer, (UInt32)(i * sizeof(UInt32)));
            }

            Marshal.FreeHGlobal(NoManagedBuffer);
            return GotDevNum; 
        }

        public Int32 CSharp_UbertGetDevProductIdAccordNumId(UInt32 DevNumId, ref byte[] DevProductIdStr)
        {
            Int32 GotDevNum = 0;

            IntPtr NoManagedBuffer = IntPtr.Zero;
            NoManagedBuffer = Marshal.AllocHGlobal(DevProductIdStr.Length * sizeof(byte));
            if (NoManagedBuffer == IntPtr.Zero)
                return 0;

            byte[] ManagedBuffer = new byte[DevProductIdStr.Length * sizeof(byte)];

            GotDevNum = HzSdkLib.DevApi_UbertGetDevProductIdAccordNumId(DevNumId, NoManagedBuffer);
            if (GotDevNum <= 0)
            {
                Marshal.FreeHGlobal(NoManagedBuffer);
                return 0;
            }
            Marshal.Copy(NoManagedBuffer, ManagedBuffer, 0, DevProductIdStr.Length);

            for (int i = 0; i < DevProductIdStr.Length; i++)
            {
                DevProductIdStr[i] = ManagedBuffer[i];
            }

            Marshal.FreeHGlobal(NoManagedBuffer);
            return GotDevNum;
        }

        public Int32 CSharp_UbertGetAllParameters(UInt32 DevNumId, ref byte[] AllParams)
        {
            Int32 rc = 0;

            IntPtr NoManagedBuffer = IntPtr.Zero;
            NoManagedBuffer = Marshal.AllocHGlobal(AllParams.Length * sizeof(byte));
            if (NoManagedBuffer == IntPtr.Zero)
                return 0;

            byte[] ManagedBuffer = new byte[AllParams.Length * sizeof(byte)];

            rc = HzSdkLib.DevApi_UbertGetAllParameters(DevNumId, NoManagedBuffer);
            if (rc <= 0)
            {
                Marshal.FreeHGlobal(NoManagedBuffer);
                return 0;
            }
            Marshal.Copy(NoManagedBuffer, ManagedBuffer, 0, AllParams.Length);

            for (int i = 0; i < AllParams.Length; i++)
            {
                AllParams[i] = ManagedBuffer[i];
            }

            Marshal.FreeHGlobal(NoManagedBuffer);
            return rc;

        }

#if HZDEV_X86
        public const string HZSDKDLLNAME = "HzDevApi32.dll";
#else
        public const string HZSDKDLLNAME = "HzDevApi64.dll";
#endif
        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertInitDevDll(Int32 DeviceType, Int32 DllAutoProDevHotplug, Int32 Reserved);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertGetDevList(IntPtr DevArray);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern UInt32 DevApi_UbertGetDevNumIdAccordProductId(string DevProductId);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertGetDevProductIdAccordNumId(UInt32 DevNumId, IntPtr DevProductId);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertGetDevConnecttedStatus(UInt32  DevNumId);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertGetAllParameters(UInt32 DevNumId, IntPtr AllParams);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertSetClearBer(UInt32  DevNumId,UInt32 Ch);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertSetParamDataRateAndPrbs(UInt32 DevNumId, UInt64 DataRate, UInt32 Prbs, UInt64 PrbsCus);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertSetParamDataRateAndPrbsAndJitter(UInt32 DevNumId, UInt64 DataRate, UInt32 Prbs, UInt64 PrbsCus, UInt16 TxJitter);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertSetParamAmplitude(UInt32  DevNumId,UInt32 Ch, UInt32  Amplitude);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertSetParamTxMute(UInt32 DevNumId, UInt32 Ch, UInt32 TxMute);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertSetParamDeemphasis(UInt32 DevNumId, UInt32 Ch, UInt32 TxDeemphasis);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertSetParamRxPolarity(UInt32  DevNumId,UInt32 Ch, UInt32  RxPol);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertSetParamRxEqualization(UInt32 DevNumId, UInt32 Ch, Byte RxEqualization);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertSetParamTriggerFrequencyDiv(UInt32  DevNumId,UInt32 TriggerNum,UInt32  DataRateDiv);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
	    public static extern Int32 DevApi_UbertSetParamTriggerAmplitude(UInt32  DevNumId,UInt32 TriggerNum,UInt32  Amplitude);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertAddAndDelDevInDllDevListAccordDevicePathAndType(Int32 AddOrDel, string DevPath, UInt32 DevType);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertAddAndDelDevInDllDevListAccordDevNumID(UInt32 DevNumID);

        [DllImport(HZSDKDLLNAME, CallingConvention = CallingConvention.Cdecl)]
        public static extern Int32 DevApi_UbertVerifyIfValidDevAccordDevicePathAndType(string DevPath, UInt32 DevType);
    }
}
