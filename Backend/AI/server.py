from flask import Flask, request, jsonify
import json
from waitress import serve

app = Flask(__name__)

@app.route('/receive_data', methods=['POST'])
def receive_data():
    data = request.get_json()  # Get the incoming JSON data
    print("Received Data:", data)

    # You can also return a response (optional)
    return jsonify({"status": "success", "received": data}), 200

if __name__ == "__main__":
    serve(app, host='0.0.0.0', port=5000)