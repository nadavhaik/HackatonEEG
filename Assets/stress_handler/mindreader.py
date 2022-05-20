import time

from pylsl import StreamInlet, resolve_stream
from multiprocessing import Process
from pylsl import *
import numpy as np
from data_server import Runner
from brain_data import *
from scipy import signal
import scipy

def disconnect_all_muses():
    streams = resolve_stream('type', 'EEG')
    for s in streams:
        StreamInlet(s).close_stream()

def connect_muse(mac_address):
    streams = resolve_stream('type', 'EEG')
    if len(streams) > 0:
        return StreamInlet(streams[0])
    # # muses = list_muses()
    # muses_macs = map(lambda x: x["address"].lower(), muses)
    # if mac_address in muses_macs:
    #     t = Process(target=stream, args=(mac_address.upper(),))
    #     t.start()
    #     time.sleep(1)
    #     streams = resolve_stream('type', 'EEG')
    #     # create a new inlet to read from the stream
    #     return StreamInlet(streams[0])
    else:
        raise Exception("Requested Muse device is not connected")


def get_sample(inlet):
    sample, timestamp = inlet.pull_sample()
    return sample


def get_electroid_arrays(sample_matrix):
    l = []
    for i in range(4):
        subl = []
        for m in sample_matrix:
            subl.append(m[i])
        l.append(subl)
    return l


def calc_attention_ratio(electroid_arrays):
    attention_ratio = []
    for arr in electroid_arrays:
        attention_ratio.append(calc_attention_ratio_array(arr))
    return attention_ratio


def bandpower(x, fs, fmin, fmax):
    f, Pxx = signal.periodogram(x, fs=fs)
    ind_min = scipy.argmax(f > fmin) - 1
    ind_max = scipy.argmax(f > fmax) - 1
    return scipy.trapz(Pxx[ind_min: ind_max], f[ind_min: ind_max])


def calc_attention_ratio_array(sample_array):
    np_matrix = np.array(sample_array)
    sos = signal.butter(1, 40, 'hp', fs=256, output='sos')
    filtered = signal.sosfilt(sos, np_matrix)
    theta = bandpower(filtered, 256, 4, 7)
    beta = bandpower(filtered, 256, 12, 30)
    return beta / max(theta,0.0000001)


def generate_attention_ratio(attention_ratios):
    return attention_ratios[0]*0.1+attention_ratios[1]*.4+attention_ratios[2]*0.4+attention_ratios[3]*0.1



def main():
    Runner().run()
    #mac_address = "00:55:da:b3:d2:69"
    mac_address = "00:55:DA:B9:49:FF".lower()
    inlet = connect_muse(mac_address)
    # we'll get 256 samples per second
    focused_ratio = -1000
    not_focused_ratio = -1000
    start_time = datetime.now()

    try:
        while True:
            sample_matrix = []
            attention_score = 0.5
            #data needed for sampling
            # both values have been calculated
            if focused_ratio<0 or not_focused_ratio<0:
                sample_matrix.append(get_sample(inlet))
                # process focused
                if (datetime.now()-start_time).seconds<10 and focused_ratio<0:
                    electroid_arrays = get_electroid_arrays(sample_matrix)
                    attention_ratios = []
                    for i in range(len(electroid_arrays)):
                        attention_ratios.append(calc_attention_ratio(electroid_arrays))
                    focused_ratio = generate_attention_ratio(attention_ratios)
                    print("focused ratio is"+ focused_ratio)
                # process unfocused
                elif (datetime.now()-start_time).seconds<20:
                    electroid_arrays = get_electroid_arrays(sample_matrix)
                    attention_ratios = []
                    for i in range(len(electroid_arrays)):
                        attention_ratios.append(calc_attention_ratio(electroid_arrays))
                    not_focused_ratio = generate_attention_ratio(attention_ratios)
                    print("not focused ratio is" + focused_ratio)
            # focused/unfocused calculated
            else:
                # gather samples for 1 second
                gather_time = datetime.now()
                while (datetime.now() - gather_time).seconds < 1:
                    sample_matrix.append(get_sample(inlet))
                    electroid_arrays = get_electroid_arrays(sample_matrix)
                    attention_ratios = []
                    for i in range(len(electroid_arrays)):
                        attention_ratios.append(calc_attention_ratio(electroid_arrays))
                    attention_ratio = generate_attention_ratio(attention_ratios)
                    zero_one_scale = 0.5
                    focus_scale_size = abs(not_focused_ratio - focused_ratio)
                    if not_focused_ratio>focused_ratio:
                        zero_one_scale = max(min(1, (attention_ratio-focused_ratio)/focus_scale_size),0)

                    else:
                        zero_one_scale = 1 - max(min(1,(attention_ratio-not_focused_ratio)/focus_scale_size),0)

                    attention_score = (zero_one_scale-0.5)*2
                    BrainData().update(ProcessedSampleData(attention_score))
                    print("normal attention score is "+attention_score)



    except:
        pass

    inlet.close_stream()
    Runner().kill_server()


if __name__ == '__main__':
    main()
