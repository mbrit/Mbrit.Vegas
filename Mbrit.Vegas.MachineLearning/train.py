import pandas as pd
import numpy as np
import os
from sklearn.model_selection import train_test_split
from sklearn.metrics import classification_report, roc_auc_score
from xgboost import XGBClassifier
import joblib

LABELS = [0, 1, 2, 3]  # Bust, Evens, Minor, Spike
VECTOR_RANGE = range(10, 25)  # inclusive 10 through 20

for label in LABELS:
    label_name = ["Bust", "Evens", "Minor", "Spike"][label]

    for count in VECTOR_RANGE:
        print(f"\n=== Training model for label={label} ({label_name}) with count={count} ===")

        # Load prestructured binary dataset
        path = f"c:\\Mbrit\\Casino\\vectors--20250704-135336--5000000--{label_name}.csv"
        print(f"Loading: {path}")
        df = pd.read_csv(path)

        # Filter rows with sufficient vectors
        df_filtered = df[df["count"] >= count].copy()
        if df_filtered.empty:
            print("Skipped (no data for this count)")
            continue

        feature_cols = [f"r{i}" for i in range(count)]
        X = df_filtered[feature_cols]
        y = df_filtered["label"]  # Already binary: 1 if match, 0 if not

        if y.nunique() < 2 or y.value_counts().min() < 2:
            print("Skipped (not enough positive/negative samples to train)")
            continue

        X_train, X_test, y_train, y_test = train_test_split(
            X, y, test_size=0.2, stratify=y, random_state=42
        )

        model = XGBClassifier(
            objective="binary:logistic",
            eval_metric="logloss",
            use_label_encoder=False,
            n_jobs=-1
        )
        model.fit(X_train, y_train)

        y_pred = model.predict(X_test)
        y_proba = model.predict_proba(X_test)[:, 1]

        print(classification_report(y_test, y_pred))
        try:
            auc = roc_auc_score(y_test, y_proba)
            print("ROC AUC:", auc)
        except Exception as e:
            print("AUC error:", e)

        filename = f"model_{label_name}_{count}.pkl"
        joblib.dump(model, filename)
        print(f"Saved model to {filename}")
