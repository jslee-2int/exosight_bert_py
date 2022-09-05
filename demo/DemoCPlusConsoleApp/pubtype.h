//#pragma once 


#ifndef _INC_PUBTYPE_H
#define _INC_PUBTYPE_H

#ifdef __cplusplus
//extern "C" {
extern "C" {
#endif

typedef char PUB_Int8;
typedef unsigned char PUB_uInt8;

typedef short PUB_Int16;
typedef unsigned short PUB_uInt16;

typedef int PUB_Int32;
typedef unsigned int PUB_uInt32;
typedef int PUB_Bool;

//typedef __int64 PUB_uInt64;
//typedef __int64 PUB_Int64;

typedef long long PUB_Int64;
typedef unsigned long long PUB_uInt64;

typedef float PUB_Float;
typedef double PUB_Double;
//typedef unsigned long DWORD;

#define PUB_TRUE			(1)		
#define PUB_FALSE			(0)

#ifndef true
#define true PUB_TRUE
#endif
#ifndef false
#define false PUB_FALSE

#endif
#ifdef __cplusplus
}
#endif

#endif


