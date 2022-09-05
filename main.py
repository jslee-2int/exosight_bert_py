import numpy as np
import ctypes
'''
* MUTE control

:OUTC:CH0:MUTE 0 , :OUTC:CH0:MUTE 1;
:OUTC:CH1:MUTE 0 , :OUTC:CH1:MUTE 1;
:OUTC:CH2:MUTE 0 , :OUTC:CH2:MUTE 1;
:OUTC:CH3:MUTE 0 , :OUTC:CH3:MUTE 1;

* Deemphasis control

0-means 0dB, 1-means 0.3dB, 2-means 0.7dB, 3-means 1.1dB,
4-means 1.5dB, 5-means 1.9dB, 6-means 2.4dB, 7-means 2.8dB,
8-means 3.5dB, 9-means 4.0dB, 10-means 4.6dB, 11-means 5.2dB,
12-means 5.9dB, 13-means 6.6dB, 14-means 7.4dB, 15-means 8.2dB,

:OUTC:CH0:DE XX , :OUTC:CH1:DE XX, :OUTC:CH2:DE XX, :OUTC:CH3:DE XX 

'''

class Exosight_cmd:
    def __init__(self):
        self.dll = ctypes.windll.LoadLibrary('HzDevApi64.dll')
        self.dll.DevApi_UbertInitDevDll(ctypes.c_int(3), ctypes.c_int(0), ctypes.c_int(0))
        print('Init Dev Dll ..')
        pyarr = [0, 0, 0, 0, 0, 0, 0, 0]
        arr = (ctypes.c_int * len(pyarr))(*pyarr)
        # print(type(arr))
        self.dll.DevApi_UbertGetDevList.restype = ctypes.c_int32
        self.dll.DevApi_UbertGetDevList.restype = ctypes.c_int32
        bert_get_list = self.dll.DevApi_UbertGetDevList(arr)
        # print(type(bert_get_list))
        print('BERT Get Dev List : {}'.format(bert_get_list))
        self.dll.DevApi_UbertGetDevNumIdAccordProductId.restype = ctypes.c_int32
        bert_get_num_id = self.dll.DevApi_UbertGetDevProductIdAccordNumId(ctypes.c_int32(0))
        print('BERT Get Dev NumId : {}'.format(bert_get_num_id))
        self.dll.DevApi_UbertGetDevNumIdAccordProductId.restype = ctypes.c_int32
        bert_get_state = self.dll.DevApi_UbertGetDevConnecttedStatus(ctypes.c_int32(1))
        print('BERT Get Dev State : {}'.format(bert_get_state))

        self.de_list = ['0dB', '0.3dB', '0.7dB', '1.1dB', '1.5dB', '1.9dB', '2.4dB', '2.8dB', '3.5dB', '4.0dB',
                        '4.6dB', '5.2dB', '5.9dB', '6.6dB', '7.4dB', '8.2dB']

    def cmd(self, cmd):
        cmd = str(cmd)
        if ':OUTC'in cmd:
            cmd = cmd.replace(':OUTC','')
            if ':CH0' in cmd:
                cmd = cmd.replace(':CH0', '')
                if ':MUTE ' in cmd:
                    cmd = cmd.replace(':MUTE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamTxMute(ctypes.c_int32(1), ctypes.c_int32(0), ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para TxMute : Mute {}'.format(cmd))
                elif ':DE ' in cmd:
                    cmd = cmd.replace(':DE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamDeemphasis.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamDeemphasis(ctypes.c_int32(1), ctypes.c_int32(0),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Deemphasis : {}'.format(self.de_list[cmd]))
                elif ':AM ' in cmd:
                    cmd = cmd.replace(':AM ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamAmplitude.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamAmplitude(ctypes.c_int32(1), ctypes.c_int32(0),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Amplitude : {}'.format(cmd))
            elif ':CH1' in cmd:
                cmd = cmd.replace(':CH1', '')
                if ':MUTE ' in cmd:
                    cmd = cmd.replace(':MUTE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamTxMute(ctypes.c_int32(1), ctypes.c_int32(1), ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para TxMute : Mute {}'.format(cmd))
                elif ':DE ' in cmd:
                    cmd = cmd.replace(':DE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamDeemphasis.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamDeemphasis(ctypes.c_int32(1), ctypes.c_int32(1),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Deemphasis : {}'.format(self.de_list[cmd]))
                elif ':AM ' in cmd:
                    cmd = cmd.replace(':AM ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamAmplitude.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamAmplitude(ctypes.c_int32(1), ctypes.c_int32(1),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Amplitude :{}'.format(cmd))
            elif ':CH2' in cmd:
                cmd = cmd.replace(':CH2', '')
                if ':MUTE ' in cmd:
                    cmd = cmd.replace(':MUTE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamTxMute(ctypes.c_int32(1), ctypes.c_int32(2), ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para TxMute : Mute {}'.format(cmd))
                elif ':DE ' in cmd:
                    cmd = cmd.replace(':DE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamDeemphasis(ctypes.c_int32(1), ctypes.c_int32(2),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Deemphasis : {}'.format(self.de_list[cmd]))
                elif ':AM ' in cmd:
                    cmd = cmd.replace(':AM ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamAmplitude.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamAmplitude(ctypes.c_int32(1), ctypes.c_int32(2),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Amplitude : {}'.format(cmd))
            elif ':CH3' in cmd:
                cmd = cmd.replace(':CH3', '')
                if ':MUTE ' in cmd:
                    cmd = cmd.replace(':MUTE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamTxMute(ctypes.c_int32(1), ctypes.c_int32(3), ctypes.c_int32(cmd))
                    msX = np.ndarray((3, ), 'f', bert_get_state, order='C')
                    print('BERT Set Para TxMute : Mute {}'.format(cmd))
                elif ':DE ' in cmd:
                    cmd = cmd.replace(':DE ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamTxMute.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamDeemphasis(ctypes.c_int32(1), ctypes.c_int32(3),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para Deemphasis : {}'.format(self.de_list[cmd]))
                elif ':AM ' in cmd:
                    cmd = cmd.replace(':AM ', '')
                    cmd = int(cmd)
                    self.dll.DevApi_UbertSetParamAmplitude.restype = ctypes.c_int32 * 3
                    bert_get_state = self.dll.DevApi_UbertSetParamAmplitude(ctypes.c_int32(1), ctypes.c_int32(3),
                                                                         ctypes.c_int32(cmd))
                    msX = np.ndarray((3,), 'f', bert_get_state, order='C')
                    print('BERT Set Para TxMute : {}'.format(cmd))
        elif ':JDAP ' in cmd:
            cmd = cmd.replace(':JDAP ', '')
            cmd = int(cmd)
            self.dll.DevApi_UbertSetParamDataRateAndPrbs.restype = ctypes.c_int32 * 2
            bert_get_state = self.dll.DevApi_UbertSetParamDataRateAndPrbs(ctypes.c_int32(1), ctypes.c_int32(cmd))
            msX = np.ndarray((3,), 'f', bert_get_state, order='C')
            print('BERT Set Para TxMute : {}'.format(msX))
            # DevApi_UbertSetParamDataRateAndPrbs

if __name__ == "__main__":
    exosight = Exosight_cmd()
    while True:
        cmd = input("CMD (consol) >> ")
        exosight.cmd(cmd)