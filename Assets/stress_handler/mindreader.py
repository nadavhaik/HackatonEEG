import time

from pylsl import StreamInlet, resolve_stream
from multiprocessing import Process
from muselsl import stream, record, record_direct, list_muses
import numpy as np


def connect_muse(mac_address):
    muses = list_muses()
    muses_macs = map(lambda x: x["address"].lower(), muses)
    if mac_address in muses_macs:
        t = Process(target=stream, args=(mac_address.upper(),))
        t.start()
        time.sleep(10)
        streams = resolve_stream('type', 'EEG')
        # create a new inlet to read from the stream
        return StreamInlet(streams[0])
    else:
        raise Exception("Requested Muse device is not connected")


def get_sample(inlet):
    sample, timestamp = inlet.pull_sample()
    return sample


def get_electroid_arrays(sample_matrix):
    l = []
    for i in range(5):
        subl = []
        for m in sample_matrix:
            subl.append(m[i])
        l.append(subl)
    return l

def calc_attention_ratio(electroid_arrays):
    return 0

def generate_attention_score(attention_ratios):
    return 0

def main():
    mac_address = "00:55:da:b3:d2:69"
    inlet = connect_muse(mac_address)
    # we'll get 256 samples per second

    sample_matrix = []
    # create matrix of 256 samples representing a second
    for i in range(256):
        sample_matrix.append(get_sample(inlet))

    # list of 5 arrays based on coloum
    electroid_arrays = get_electroid_arrays(sample_matrix)

    # for each electroid calculate attention ratio (beta/theta)
    attention_ratios = []
    for i in range(len(electroid_arrays)):
        attention_ratios.append(calc_attention_ratio(electroid_arrays[i]))

    # geneate attention score that will be sent through api
    attention_score = generate_attention_score(attention_ratios)
    inlet.close_stream()


if __name__ == '__main__':
    main()
