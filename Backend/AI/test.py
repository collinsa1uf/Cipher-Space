from flask import Flask, request, jsonify
import os
import re
import random
from transformers import AutoModelForCausalLM, AutoTokenizer

app = Flask(__name__)

# Load model
model_name = "Qwen/Qwen3-0.6B"
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(
    model_name,
    torch_dtype="auto",
    device_map="auto"
)

# Predefined filters
EXCLUDE_ITEMS = {"bowl", "spoon", "sack", "tongs", "serving", "plate",
                 "tongue", "tea", "ulp", "ladle", "gourd", "dagger",
                 "platter", "pitcher", "gallon", "teapot", "stapler"}

@app.route("/receive_data", methods=["POST"])
def receive_data():
    try:
        mug_synonyms = ["mug"]
        correct_items = 0
        reload = True

        while reload:
            reload = False

            # Prepare prompt
            prompt = ("Provide 5 unique one-word objects with handles, no verbs, "
                      "that could be used interchangeably with the word 'mug' for drinking. "
                      "These objects should closely resemble a mug in both appearance and function, "
                      "and each should be distinct with no repetition. Surround each list object with **.")

            messages = [{"role": "user", "content": prompt}]
            text = tokenizer.apply_chat_template(
                messages,
                tokenize=False,
                add_generation_prompt=True,
                enable_thinking=False
            )

            model_inputs = tokenizer([text], return_tensors="pt").to(model.device)

            # Generate output
            generated_ids = model.generate(
                **model_inputs,
                max_new_tokens=1024  # smaller than 32768 for practical server use
            )

            output_ids = generated_ids[0][len(model_inputs.input_ids[0]):].tolist()

            # Parse thinking content
            try:
                index = len(output_ids) - output_ids[::-1].index(151668)  # </think>
            except ValueError:
                index = 0

            content = tokenizer.decode(output_ids[index:], skip_special_tokens=True).strip("\n")
            pattern = r'\*\*(.*?)\*\*'
            list_items = re.findall(pattern, content)

            for item in list_items:
                item = item.lower()

                if " " in item:
                    reload = True
                elif item in EXCLUDE_ITEMS:
                    reload = True
                elif item in mug_synonyms:
                    reload = True
                else:
                    mug_synonyms.append(item)
                    correct_items += 1

                if correct_items == 5:
                    reload = False
                    break

        # Pick final word randomly
        word = random.choice(mug_synonyms)
        return jsonify({"mugObject": word})

    except Exception as e:
        return jsonify({"error": str(e)}), 500


if __name__ == "__main__":
    from waitress import serve
    serve(app, host="0.0.0.0", port=5055)