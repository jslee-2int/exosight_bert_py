// DemoWin32Console.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "pubtype.h"
#include "pubfun.h"
#include <malloc.h>
#include <windows.h>

#define DllAPI __declspec(dllimport)

#define HIDAPI_DLL_DECLARE __declspec(dllimport)
#ifdef HZDEV_X86
#pragma comment(lib,"HzDevApi32.lib")
#else
#pragma comment(lib,"HzDevApi64.lib")
#endif

#define TXRX_CH_NUM 2

struct SynParams_t
{
	PUB_uInt64 DataRate;
	PUB_uInt32 Prbs;
	PUB_uInt64 PrbsCus;
	PUB_uInt32 TriggerOfCdrDiv;
	PUB_uInt32 TriggerOfCdrAmplitude;
	PUB_uInt16 TxJitter;
};
struct TxRxChParams_t
{
	PUB_uInt8 ConfigCompleted;
	PUB_uInt32 Amplitude;
	PUB_uInt8 RxPol;
	PUB_uInt32 BerRunTime;
	PUB_uInt32 BerLockTime;
	PUB_uInt64 BerBitCount;
	PUB_uInt64 BerErrorCount;
	PUB_uInt8 BerLockStatus;
	PUB_uInt8 BerErrorCountOverflow;
	PUB_uInt32 Prbs;
	PUB_uInt64 PrbsCus;
	PUB_uInt8 TxDeemphasis;
	PUB_uInt8 TxMute;
	PUB_uInt8 RxEqualization;
};
struct ChParams_t
{
	struct SynParams_t SynParams;
	struct TxRxChParams_t TxRxChParams[TXRX_CH_NUM];
};
enum _DEVICE_TYPE
{
	DEVICE_UBERT_2CH_10G=1,
	DEVICE_UBERT_2CH_10G25G,
	DEVICE_UBERT_4CH_10G25G,	
	DEVICE_UBERT_4CH_10G,
	DEVICE_UGEN_1CH_10G,
	DEVICE_UGEN_2CH_10G,
	DEVICE_NULL,
};
extern "C"
{
	DllAPI void HidCom_FindDevice(PUB_uInt8* Status, PUB_uInt8* AllDevsStr,PUB_Int16 usbVid, PUB_Int16 usbPid);

	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertInitDevDll(PUB_Int32 DeviceType,PUB_Int32 DllAutoProDevHotplug,PUB_Int32 Reserved);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertGetDevList(PUB_uInt32 *DevArray);
	HIDAPI_DLL_DECLARE PUB_uInt32 DevApi_UbertGetDevNumIdAccordProductId(PUB_Int8 *DevProductId);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertGetDevProductIdAccordNumId(PUB_uInt32 DevNumId,PUB_Int8 *DevProductId);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertGetDevConnecttedStatus(PUB_uInt32 DevNumId);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertGetAllParameters(PUB_uInt32 DevNumId,PUB_uInt8 *AllParams);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetClearBer(PUB_uInt32 DevNumId,PUB_uInt32 Ch);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamDataRateAndPrbs(PUB_uInt32 DevNumId,PUB_uInt64 DataRate,PUB_uInt32 Prbs,PUB_uInt64 PrbsCus);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamDataRateAndPrbsAndJitter(PUB_uInt32 DevNumId,PUB_uInt64 DataRate,PUB_uInt32 Prbs,PUB_uInt64 PrbsCus,PUB_uInt16 TxJitter);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamTxMute(PUB_uInt32 DevNumId,PUB_uInt32 Ch, PUB_uInt32 TxMute);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamAmplitude(PUB_uInt32 DevNumId,PUB_uInt32 Ch, PUB_uInt32 Amplitude);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamDeemphasis(PUB_uInt32 DevNumId,PUB_uInt32 Ch, PUB_uInt32 Deemphasis);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamRxPolarity(PUB_uInt32 DevNumId,PUB_uInt32 Ch, PUB_uInt32 RxPol);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamRxEqualization(PUB_uInt32 DevNumId,PUB_uInt32 Ch, PUB_uInt8 RxEqualization);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamTriggerFrequencyDiv(PUB_uInt32 DevNumId,PUB_uInt32 TriggerNum,PUB_uInt32 DataRateDiv);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertSetParamTriggerAmplitude(PUB_uInt32 DevNumId,PUB_uInt32 TriggerNum,PUB_uInt32 Amplitude);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertAddAndDelDevInDllDevListAccordDevicePathAndType(PUB_Int32 AddOrDel,PUB_Int8 *DevPath,PUB_uInt32 DevType);
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertAddAndDelDevInDllDevListAccordDevNumID(PUB_Int32 AddOrDel,PUB_uInt32 DevNumID); 
	HIDAPI_DLL_DECLARE PUB_Int32 DevApi_UbertVerifyIfValidDevAccordDevicePathAndType(PUB_Int8 *DevPath,PUB_uInt32 DevType);

}
/*****************************************************************
**Name	  :   ParseAllParams()
**
**Function:
**
**Params :
**
**Return  :
**
**Input    :
**
**Output :
*****************************************************************/
void ParseAllParams(PUB_uInt8 *AllParams, struct ChParams_t *pChParams)
{
	PUB_Int32 i =0;
	PUB_Int32 Base = 0;
	PUB_Int32 OffsetIndex = 0;

	/*parse*/
	Base =0;
	OffsetIndex=0;
	i=0;
	/*DataRate*/
	pChParams->SynParams.DataRate=PubFun_ByteToUInt64ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.DataRate);

	/*Prbs*/
	pChParams->SynParams.Prbs=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.Prbs);

	/*PrbsCus*/
	pChParams->SynParams.PrbsCus=PubFun_ByteToUInt64ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.PrbsCus);

	/*TriggerOfCdrDiv*/
	pChParams->SynParams.TriggerOfCdrDiv=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.TriggerOfCdrDiv);

	/*TriggerOfCdrAmplitude*/
	pChParams->SynParams.TriggerOfCdrAmplitude=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.TriggerOfCdrAmplitude);

	/*reserved*/
	OffsetIndex+=sizeof(PUB_uInt32);
	OffsetIndex+=sizeof(PUB_uInt32);

	/*TxJitter*/
	pChParams->SynParams.TxJitter=PubFun_ByteToUInt16ForBigEndian(&AllParams[Base+OffsetIndex],0);
	OffsetIndex+=sizeof(pChParams->SynParams.TxJitter);

	for(i=0;i<TXRX_CH_NUM;i++)
	{
		Base =i*128+128;
		OffsetIndex=0;
		/*ConfigCompleted*/
		pChParams->TxRxChParams[i].ConfigCompleted=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].ConfigCompleted);
	
		/*Amplitude*/
		pChParams->TxRxChParams[i].Amplitude=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].Amplitude);

		/*RxPol*/
		pChParams->TxRxChParams[i].RxPol=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].RxPol);

		/*BerLockStatus*/
		pChParams->TxRxChParams[i].BerLockStatus=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerLockStatus);

		/*BerErrorCountOverflow*/
		pChParams->TxRxChParams[i].BerErrorCountOverflow=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerErrorCountOverflow);

		/*BerRunTime*/
		pChParams->TxRxChParams[i].BerRunTime=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerRunTime);

		/*BerLockTime*/
		pChParams->TxRxChParams[i].BerLockTime=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerLockTime);

		/*BerBitCount*/
		pChParams->TxRxChParams[i].BerBitCount=PubFun_ByteToUInt64ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerBitCount);

		/*BerErrorCount*/
		pChParams->TxRxChParams[i].BerErrorCount=PubFun_ByteToUInt64ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].BerErrorCount);

		/*Prbs*/
		pChParams->TxRxChParams[i].Prbs=PubFun_ByteToUInt32ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].Prbs);

		/*PrbsCus*/
		pChParams->TxRxChParams[i].PrbsCus=PubFun_ByteToUInt64ForBigEndian(&AllParams[Base+OffsetIndex],0);
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].PrbsCus);

		/*TxDemphasis*/
		pChParams->TxRxChParams[i].TxDeemphasis=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].TxDeemphasis);

		/*TxMute*/
		pChParams->TxRxChParams[i].TxMute=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].TxMute);

		/*RxEqualization*/
		pChParams->TxRxChParams[i].RxEqualization=AllParams[Base+OffsetIndex];
		OffsetIndex+=sizeof(pChParams->TxRxChParams[i].RxEqualization);
	}

}
/*****************************************************************
**Name	  :   DemoOfHzHidApiDllFor_4Ch10G25G()
**
**Function:
**
**Params :
**
**Return  :
**
**Input    :
**
**Output :
*****************************************************************/
int DemoOfHzHidApiDllFor_4Ch10G25G()
{
	volatile PUB_Int32 ByteCnt = 0;

	PUB_Int32 rc = 0;
	PUB_Int32 i = 0;

	PUB_uInt32 DevArray[128] = {0};
	PUB_Int32 GotDevNum = 0;
	PUB_uInt32 DevNumID = 0;

	PUB_uInt8 AllParm[1*128+TXRX_CH_NUM*128]={0};

	PUB_uInt8 Ch=0;
	PUB_uInt8 iCh=0;
	struct ChParams_t ChParams;

	ByteCnt = sizeof(long long);
	ByteCnt = sizeof(double);
	ByteCnt = sizeof(float);


	char DevProductIdStrArrar[][32+1] = {"00000000000000010001010500000000"
                                        ,"00000000000000010001010500000001"
                                        ,"00000000000000010001010500000002"
                                        ,"00000000000000010001010500000003"};

	char GotDevProductIdStr[32]={0};

	while(1)
	{
		GotDevNum = DevApi_UbertGetDevList(DevArray);
		if(GotDevNum<=0)
			continue;

		rc = DevApi_UbertGetDevProductIdAccordNumId(DevArray[0], GotDevProductIdStr);
		if (rc != true)
			continue;

        for (i = 0; i < sizeof(DevProductIdStrArrar)/sizeof(DevProductIdStrArrar[0]); i++)
        {
            DevNumID = DevApi_UbertGetDevNumIdAccordProductId(DevProductIdStrArrar[i]);
            if (DevNumID == 0xFFFFFFFF)
                continue;

			rc = DevApi_UbertGetDevConnecttedStatus(DevNumID);
			if(rc!=true)
				break;

			rc = DevApi_UbertGetAllParameters(DevNumID,AllParm);
			if(rc!=true)
				break;
			ParseAllParams(AllParm,&ChParams);

			if(ChParams.SynParams.DataRate<=28000000000)
				ChParams.SynParams.DataRate+=1000000000;
			ChParams.SynParams.Prbs = ChParams.SynParams.Prbs==1 ? 0 : 1;
			ChParams.SynParams.TxJitter = ChParams.SynParams.TxJitter == 0 ? 8 : 0;
			/*call this function will clear the rx pol*/
			rc = DevApi_UbertSetParamDataRateAndPrbsAndJitter(DevNumID,ChParams.SynParams.DataRate,ChParams.SynParams.Prbs,ChParams.SynParams.PrbsCus,ChParams.SynParams.TxJitter);
			if(rc!=true)
				break;	
			Sleep(5000);

			rc = DevApi_UbertGetAllParameters(DevNumID,AllParm);
			if(rc!=true)
				break;
			ParseAllParams(AllParm,&ChParams);

			ChParams.SynParams.TriggerOfCdrDiv = ChParams.SynParams.TriggerOfCdrDiv==1 ? 0 : 1;
			rc = DevApi_UbertSetParamTriggerFrequencyDiv(DevNumID,0,ChParams.SynParams.TriggerOfCdrDiv);
			if(rc!=true)
				break;
			Sleep(200);

			if(ChParams.SynParams.TriggerOfCdrAmplitude>400)
				ChParams.SynParams.TriggerOfCdrAmplitude -=1*100;
			rc = DevApi_UbertSetParamTriggerAmplitude(DevNumID,0,ChParams.SynParams.TriggerOfCdrAmplitude);
			if(rc!=true)
				break;	

			for(iCh=0;iCh<TXRX_CH_NUM;iCh++)
			{
				if(ChParams.TxRxChParams[iCh].Amplitude>800)
					ChParams.TxRxChParams[iCh].Amplitude -=iCh*100;
				rc = DevApi_UbertSetParamAmplitude(DevNumID,iCh,ChParams.TxRxChParams[iCh].Amplitude);
				if(rc!=true)
					break;	

				ChParams.TxRxChParams[iCh].RxPol =ChParams.TxRxChParams[iCh].RxPol==1 ? 0 : 1;
				rc = DevApi_UbertSetParamRxPolarity(DevNumID,iCh,ChParams.TxRxChParams[iCh].RxPol);
				if(rc!=true)
					break;

				ChParams.TxRxChParams[iCh].TxDeemphasis =ChParams.TxRxChParams[iCh].TxDeemphasis==iCh ? 5 : iCh;
				rc = DevApi_UbertSetParamDeemphasis(DevNumID,iCh,ChParams.TxRxChParams[iCh].TxDeemphasis);
				if(rc!=true)
					break;

				ChParams.TxRxChParams[iCh].TxMute =ChParams.TxRxChParams[iCh].TxMute==0 ? 1 : 0;
				rc = DevApi_UbertSetParamTxMute(DevNumID,iCh,ChParams.TxRxChParams[iCh].TxMute);
				if(rc!=true)
					break;

				ChParams.TxRxChParams[iCh].RxEqualization =ChParams.TxRxChParams[iCh].RxEqualization==0 ? 6 : 0;
				rc = DevApi_UbertSetParamRxEqualization(DevNumID,iCh,ChParams.TxRxChParams[iCh].RxEqualization);
				if(rc!=true)
					break;
			}
		}
	
	
	}

	return 0;
}
/*****************************************************************
**Name	  :   DemoOfHzHidApiDllFor_4Ch10G25G()
**
**Function:
**
**Params :
**
**Return  :
**
**Input    :
**
**Output :
*****************************************************************/
int DemoOfHzHidApiDllFor_1Ch2Ch10G25G()
{
	volatile PUB_Int32 ByteCnt = 0;

	PUB_Int32 rc = 0;
	PUB_Int32 i = 0;

	PUB_uInt32 DevArray[128] = {0};
	PUB_Int32 GotDevNum = 0;
	PUB_uInt32 DevNumID = 0;

	PUB_uInt8 AllParm[1*128+TXRX_CH_NUM*128]={0};

	PUB_uInt8 Ch=0;
	PUB_uInt8 iCh=0;
	struct ChParams_t ChParams;

	ByteCnt = sizeof(long long);
	ByteCnt = sizeof(double);
	ByteCnt = sizeof(float);

	char DevProductIdStrArrar[][32+1] = {"00000000000000010001010500000000"
                                        ,"00000000000000010001010500000001"
                                        ,"00000000000000010001010500000002"
                                        ,"00000000000000010001010500000003"};

	char GotDevProductIdStr[33]={0};

	while(1)
	{
		GotDevNum = DevApi_UbertGetDevList(DevArray);
		if(GotDevNum<=0)
			continue;

		rc = DevApi_UbertGetDevProductIdAccordNumId(DevArray[0], GotDevProductIdStr);
		if (rc != true)
			continue;

        for (i = 0; i < sizeof(DevProductIdStrArrar)/sizeof(DevProductIdStrArrar[0]); i++)
        {
            DevNumID = DevApi_UbertGetDevNumIdAccordProductId(DevProductIdStrArrar[i]);
            if (DevNumID == 0xFFFFFFFF)
                continue;

			rc = DevApi_UbertGetDevConnecttedStatus(DevNumID);
			if(rc!=true)
				break;

			rc = DevApi_UbertGetAllParameters(DevNumID,AllParm);
			if(rc!=true)
				break;
			ParseAllParams(AllParm,&ChParams);

            if ((ChParams.SynParams.DataRate >= 25000000000) && (ChParams.SynParams.DataRate <= 28000000000))
                ChParams.SynParams.DataRate += 1000000000;
            else if ((ChParams.SynParams.DataRate >= 1800000000) && (ChParams.SynParams.DataRate <= 13500000000))
                ChParams.SynParams.DataRate += 1000000000;
            else if ((ChParams.SynParams.DataRate > 28000000000))
                ChParams.SynParams.DataRate = 1800000000;
            else if ((ChParams.SynParams.DataRate > 13500000000))
                ChParams.SynParams.DataRate = 25000000000;

			ChParams.SynParams.Prbs = ChParams.SynParams.Prbs==1 ? 0 : 1;
			/*call this function will clear the rx pol*/
			rc = DevApi_UbertSetParamDataRateAndPrbs(DevNumID,ChParams.SynParams.DataRate,ChParams.SynParams.Prbs,ChParams.SynParams.PrbsCus);
			if(rc!=true)
				break;	
			Sleep(3000);

			rc = DevApi_UbertGetAllParameters(DevNumID,AllParm);
			if(rc!=true)
				break;
			ParseAllParams(AllParm,&ChParams);

			rc = DevApi_UbertGetAllParameters(DevNumID,AllParm);
			if(rc!=true)
				break;
			ParseAllParams(AllParm,&ChParams);

			ChParams.SynParams.TriggerOfCdrDiv = ChParams.SynParams.TriggerOfCdrDiv==1 ? 0 : 1;
			rc = DevApi_UbertSetParamTriggerFrequencyDiv(DevNumID,0,ChParams.SynParams.TriggerOfCdrDiv);
			if(rc!=true)
				break;
			Sleep(200);

			if(ChParams.SynParams.TriggerOfCdrAmplitude>400)
				ChParams.SynParams.TriggerOfCdrAmplitude -=1*100;
			rc = DevApi_UbertSetParamTriggerAmplitude(DevNumID,0,ChParams.SynParams.TriggerOfCdrAmplitude);
			if(rc!=true)
				break;	

			for(iCh=0;iCh<TXRX_CH_NUM;iCh++)
			{
				if(ChParams.TxRxChParams[iCh].Amplitude>400)
					ChParams.TxRxChParams[iCh].Amplitude -=iCh*100;
				rc = DevApi_UbertSetParamAmplitude(DevNumID,iCh,ChParams.TxRxChParams[iCh].Amplitude);
				if(rc!=true)
					break;	

				ChParams.TxRxChParams[iCh].RxPol =ChParams.TxRxChParams[iCh].RxPol==1 ? 0 : 1;
				rc = DevApi_UbertSetParamRxPolarity(DevNumID,iCh,ChParams.TxRxChParams[iCh].RxPol);
				if(rc!=true)
					break;		

				ChParams.TxRxChParams[iCh].TxDeemphasis =ChParams.TxRxChParams[iCh].TxDeemphasis==iCh ? 5 : 1;
				rc = DevApi_UbertSetParamDeemphasis(DevNumID,iCh,ChParams.TxRxChParams[iCh].TxDeemphasis);
				if(rc!=true)
					break;

				ChParams.TxRxChParams[iCh].TxMute =ChParams.TxRxChParams[iCh].TxMute==0 ? 1 : 0;
				rc = DevApi_UbertSetParamTxMute(DevNumID,iCh,ChParams.TxRxChParams[iCh].TxMute);
				if(rc!=true)
					break;

				Sleep(200);
			}
		}
	
	
	}

	return 0;
}
/*************************************************
**Name	  :   _tmain()
**
**Function:
**
**Params :
**
**Return  :
**
**Input    :
**
**Output :
*****************************************************************/
int _tmain(int argc, _TCHAR* argv[])
{
	PUB_Int32 rc = 0;

	if(TXRX_CH_NUM==4)
	{
		rc = DevApi_UbertInitDevDll(DEVICE_UBERT_4CH_10G25G,0,0);
		if(rc!=1)
			return false;

		DemoOfHzHidApiDllFor_4Ch10G25G();
	}	
	else
	{
		rc = DevApi_UbertInitDevDll(DEVICE_UBERT_2CH_10G25G,0,0);
		if(rc!=1)
			return false;
		DemoOfHzHidApiDllFor_1Ch2Ch10G25G();
	}
	return 0;
}

