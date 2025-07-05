from flask import Flask, request, jsonify
import pandas as pd
import joblib
import traceback
import os

app = Flask(__name__)

LABEL_NAMES = {
    0: "Bust",
    1: "Evens",
    2: "Minor",
    3: "Spike"
}

VECTOR_RANGE = range(10, 24)  # Inclusive
MODELS = {}

# === Load all models once at startup ===
for label, label_name in LABEL_NAMES.items():
    for count in VECTOR_RANGE:
        model_path = f"model_{label_name}_{count}.pkl"
        if os.path.exists(model_path):
            MODELS[(label, count)] = joblib.load(model_path)
        else:
            print(f"Warning: Model missing: {model_path}")

@app.route('/predict', methods=['POST'])
def predict():
    try:
        data = request.get_json()
        vector = data.get("vector")
        label = data.get("label")

        if not isinstance(label, int) or label not in LABEL_NAMES:
            raise ValueError("Label must be one of 0 (Bust), 1 (Evens), 2 (Minor), 3 (Spike)")

        if not isinstance(vector, list):
            raise ValueError("Vector must be a list of binary values")

        count = len(vector)

        model_key = (label, count)
        model = MODELS.get(model_key)
        if model is None:
            raise FileNotFoundError(f"Model not loaded for label={LABEL_NAMES[label]} count={count}")

        input_df = pd.DataFrame([vector], columns=[f"r{i}" for i in range(count)])
        prediction = model.predict(input_df)[0]
        probabilities = model.predict_proba(input_df)[0].tolist()

        return jsonify({
            "model": f"model_{LABEL_NAMES[label]}_{count}.pkl",
            "prediction": int(prediction),
            "probabilities": probabilities
        })

    except Exception as e:
        print("Error during prediction:")
        traceback.print_exc()
        return jsonify({"error": str(e)}), 400

if __name__ == "__main__":
    app.run(debug=False)
