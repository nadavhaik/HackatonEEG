from singleton import singleton
from datetime import datetime, timezone
MIN_STRESS_GAUGE = 0
MAX_STRESS_GAUGE = 100

class ProcessedSampleData:
    def __init__(self, stress_value: float):
        self.stress_value = stress_value
        self.sample_time = datetime.now(timezone.utc)

@singleton
class BrainData:
    def __init__(self):
        self.stress_gauge = 0
        self.last_update_time = None

    @property
    def is_connected(self):
        return self.last_update_time is not None and (datetime.now(timezone.utc) - self.last_update_time).seconds < 30

    def update(self, sample: ProcessedSampleData):
        if self.last_update_time is None:
            delta = 1 # sec
        elif sample.sample_time < self.last_update_time:
            return
        else:
            delta = (sample.sample_time - self.last_update_time).seconds

        new_stress_value = self.stress_gauge + delta*sample.stress_value
        if new_stress_value < MIN_STRESS_GAUGE:
            new_stress_value = MIN_STRESS_GAUGE
        elif new_stress_value > MAX_STRESS_GAUGE:
            new_stress_value = MAX_STRESS_GAUGE

        self.stress_gauge = new_stress_value
        self.last_update_time = sample.sample_time


