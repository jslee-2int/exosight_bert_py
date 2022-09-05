#include "stdafx.h"
#include "pubfun.h"

/*****************************************************************
**Name	  :   PubFun_ByteToUInt16ForBigEndian()
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
PUB_uInt16 PubFun_ByteToUInt16ForBigEndian(PUB_uInt8* data, PUB_uInt32 index)
{
    PUB_Int32 i=0;
	PUB_uInt32 Value=0;
	while(i<2)
	{
		Value =Value<<8;
		Value |=data[index + i];
		i++;
	}
    return Value;
}
/*****************************************************************
**Name	  :   PubFun_ByteToUInt16ForLittleEndian()
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
PUB_uInt16 PubFun_ByteToUInt16ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index)
{
 	PUB_Int32 i=0;
	PUB_uInt16 Value=0;
	while(i<2)
	{
		Value =Value<<8;
		Value |=data[index + 1-i];
		i++;
	}
    return Value;
}

/*****************************************************************
**Name	  :   PubFun_ByteToUInt32ForBigEndian()
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
PUB_uInt32 PubFun_ByteToUInt32ForBigEndian(PUB_uInt8* data, PUB_uInt32 index)
{
    PUB_Int32 i=0;
	PUB_uInt32 Value=0;
	while(i<4)
	{
		Value =Value<<8;
		Value |=data[index + i];
		i++;
	}
    return Value;
}
/*****************************************************************
**Name	  :   PubFun_ByteToUInt32ForBigEndian()
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
PUB_uInt32 PubFun_ByteToUInt32ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index)
{
 	PUB_Int32 i=0;
	PUB_uInt32 Value=0;
	while(i<4)
	{
		Value =Value<<8;
		Value |=data[index + 3-i];
		i++;
	}
    return Value;
}
/*****************************************************************
**Name	  :   PubFun_ByteToUInt64ForBigEndian()
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
PUB_uInt64 PubFun_ByteToUInt64ForBigEndian(PUB_uInt8* data, PUB_uInt32 index)
{
	PUB_Int32 i=0;
	PUB_uInt64 Value=0;
	while(i<8)
	{
		Value =Value<<8;
		Value |=data[index + i];
		i++;
	}
    return Value;
}
/*****************************************************************
**Name	  :   PubFun_ByteToUInt64ForLittleEndian()
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
PUB_uInt64 PubFun_ByteToUInt64ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index)
{
	PUB_Int32 i=0;
	PUB_uInt64 Value=0;
	while(i<8)
	{
		Value =Value<<8;
		Value |=data[index + 7-i];
		i++;
	}
    return Value;
}
/*****************************************************************
**Name	  :   PubFun_UInt32ToByteBigEndian()
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
void PubFun_UInt32ToByteBigEndian(PUB_uInt8* data, PUB_uInt32 Value)
{
	int i=0;
	for(i=0;i<4;i++)
	{
		data[3-i]=Value&0xFF;
		Value = Value>>8;
	}
}
/*****************************************************************
**Name	  :   PubFun_UInt32ToByteLittleEndian()
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
void PubFun_UInt32ToByteLittleEndian(PUB_uInt8* data, PUB_uInt32 Value)
{
	int i=0;
	for(i=0;i<4;i++)
	{
		data[i]=Value&0xFF;
		Value = Value>>8;
	}
}
/*****************************************************************
**Name	  :   PubFun_UInt32ToByteBigEndian()
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
void PubFun_UInt64ToByteBigEndian(PUB_uInt8* data, PUB_uInt64 Value)
{
	int i=0;
	for(i=0;i<8;i++)
	{
		data[7-i]=Value&0xFF;
		Value = Value>>8;
	}
}
/*****************************************************************
**Name	  :   PubFun_UInt32ToByteLittleEndian()
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
void PubFun_UInt64ToByteLittleEndian(PUB_uInt8* data, PUB_uInt64 Value)
{
	int i=0;
	for(i=0;i<8;i++)
	{
		data[i]=Value&0xFF;
		Value = Value>>8;
	}
}