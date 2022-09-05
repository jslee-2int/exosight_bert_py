//#pragma once 


#ifndef _INC_PUBFUN_H
#define _INC_PUBFUN_H

#include "pubtype.h"
PUB_uInt16 PubFun_ByteToUInt16ForBigEndian(PUB_uInt8* data, PUB_uInt32 index);
PUB_uInt32 PubFun_ByteToUInt32ForBigEndian(PUB_uInt8* data, PUB_uInt32 index);
PUB_uInt64 PubFun_ByteToUInt64ForBigEndian(PUB_uInt8* data, PUB_uInt32 index);
PUB_uInt16 PubFun_ByteToUInt16ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index);
PUB_uInt32 PubFun_ByteToUInt32ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index);
PUB_uInt64 PubFun_ByteToUInt64ForLittleEndian(PUB_uInt8* data, PUB_uInt32 index);
void PubFun_UInt32ToByteBigEndian(PUB_uInt8* data, PUB_uInt32 Value);
void PubFun_UInt64ToByteBigEndian(PUB_uInt8* data, PUB_uInt64 Value);
void PubFun_UInt32ToByteLittleEndian(PUB_uInt8* data, PUB_uInt32 Value);
void PubFun_UInt64ToByteLittleEndian(PUB_uInt8* data, PUB_uInt64 Value);


#endif


