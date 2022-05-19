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


def calc_attention(sample):
    return -10000



def main():
    mac_address = "00:55:da:b3:d2:69"
    inlet = connect_muse(mac_address)
    # we'll get 256 samples per second

    sample_matrix = []
    # create matrix of 256 samples representing a second
    for i in range(256):
        sample_matrix.append(get_sample(inlet))


    np_matrix = np.array(sample_matrix)
    # goal is to get np_matrix and get clean beta and theta
    # fft raw
    # butter/filter noisey waves
    # do fft again
    # calc power of relevant frequency ranges
    # fft_matrix = np.fft.fft(np_matrix)

    # print(np_matrix)
    # print(fft_matrix)
    inlet.close_stream()


if __name__ == '__main__':
    main()
