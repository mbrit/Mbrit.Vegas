from waitress import serve
from predict import app  # assuming your Flask app is in predict.py

serve(app, host='0.0.0.0', port=5000, threads=8)
