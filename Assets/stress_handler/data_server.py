from datetime import timezone, datetime
from http.server import BaseHTTPRequestHandler, HTTPServer
import logging
import json
from threading import Thread
from brain_data import BrainData, ProcessedSampleData
from singleton import singleton
from mindreader import run_reader
PORT = 9400


class DataServer(BaseHTTPRequestHandler):
    def _set_response(self):
        self.send_response(200)
        self.send_header('Content-type', 'application/json')
        self.end_headers()

    def do_GET(self):
        logging.info("GET request,\nPath: %s\nHeaders:\n%s\n", str(self.path), str(self.headers))

        if self.path == "/get/braindata":
            if not BrainData().is_connected:
                res = "null"
            else:
                res = f"{BrainData().stress_gauge}|{datetime.now(timezone.utc).isoformat()}"
            self._set_response()
            self.wfile.write(res.encode())
        elif self.path == "/kill":
            self._set_response()
            self.wfile.write('"status": "ok"'.encode())
            Runner().kill_server()
        else:
            self.send_error(404, "Not supported")

    def do_POST(self):

        content_length = int(self.headers['Content-Length'])
        post_data = json.loads(self.rfile.read(content_length))
        if self.path == "/post/setgauge":
            self._set_response()
            sg = post_data["stress_gauge"]
            BrainData().update(ProcessedSampleData(sg))
            logging.info(f"Stress gauge set to {BrainData().stress_gauge}")
            self.wfile.write('"status": "ok"'.encode())
        else:
            self.send_error(404, "Not supported")


@singleton
class Runner:
    def __init__(self):
        self.httpd = None

    def run(self, server_class=HTTPServer, handler_class=DataServer, port=PORT):
        logging.basicConfig(level=logging.INFO)
        server_address = ('', port)
        self.httpd = server_class(server_address, handler_class)
        logging.info('Starting httpd...\n')
        try:
            self.httpd.serve_forever()
        except KeyboardInterrupt:
            pass
        self.httpd.server_close()
        logging.info('Stopping httpd...\n')

    def kill_server(self):
        self.httpd.server_close()

if __name__ == "__main__":
    t = Thread(target=run_reader)
    t.start()
    Runner().run()
    t.join()
