import os
import io
import time
import json
import scan
import base64
from flask import Flask, request, jsonify
from PIL import Image
from requests import get, post


app = Flask(__name__)

# Instancia de escaner docs
scanner = scan.DocScanner()


# Ruta predeterminada, muestra la versión de la implementación
@app.route('/')
def index():
    return app.send_static_file("index.html")


# Manejador de ruta que puede recibir
#     - archivo de imágen en octet-stream
#     - archivos como multipart/form-data en el parámetro imageData
@app.route('/clean', methods=['POST'])
def predict_image_handler():
    # Endpoint URL
    endpoint = r"https://frmreco.cognitiveservices.azure.com"
    # Subscription Key
    apim_key = "--aqui--"
    # Modelo ID
    model_id = "--aqui--"
    # API version
    API_version = "v2.1"

    post_url = endpoint + "/formrecognizer/%s/custom/models/%s/analyze" % (API_version, model_id)
    params = {
        "includeTextDetails": False
    }

    headers = {
        # tipos soportados: 'application/pdf', 'image/jpeg', 'image/png', 'image/tiff', 'image/bmp'
        'Content-Type': 'image/jpeg',
        'Ocp-Apim-Subscription-Key': apim_key,
    }
    try:
        image_data = None
        if 'imageData' in request.files:
            image_data = request.files['imageData']
        elif 'imageData' in request.form:
            image_data = request.form['imageData']
        else:
            image_data = io.BytesIO(request.get_data())

        filesaved = scanner.scan64(image_data)
        with open(filesaved, "rb") as f:
            data_bytes = f.read()

        os.remove(filesaved)
        resp = post(url=post_url, data=data_bytes, headers=headers, params=params)
        if resp.status_code != 202:
            return 'Analisis fallido 1', 500
        get_url = resp.headers["operation-location"]

        n_tries = 15
        n_try = 0
        wait_sec = 5
        max_wait_sec = 60
        while n_try < n_tries:
            resp = get(url=get_url, headers={"Ocp-Apim-Subscription-Key": apim_key})
            resp_json = resp.json()
            if resp.status_code != 200:
                return 'Analisis fallido 2', 500
            status = resp_json["status"]
            if status == "succeeded":
                return resp_json
            if status == "failed":
                return 'Analisis fallido 3', 500
            # Anaisis aún en ejecución, esperar y reintentar
            time.sleep(wait_sec)
            n_try += 1
            wait_sec = min(2*wait_sec, max_wait_sec)
    except Exception as e:
        print("Error interno:\n%s" % str(e))
        return 'Error al procesar la imágen', 500


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)
    #app.run(host='0.0.0.0', port=5000)
