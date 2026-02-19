from transformers import AutoModelForCausalLM, AutoTokenizer
import os
import re
import random
import json
import requests

from huggingface_hub import login

hf_token = os.getenv("HF_TOKEN")
login(token=hf_token)

model_name = "Qwen/Qwen3-0.6B"

# load the tokenizer and the model
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(
    model_name,
    torch_dtype="auto",
    device_map="auto"
)

reload = True
mug_synonyms = ["mug"]
correctItems = 0

while reload:
    reload = False
    # prepare the model input
    prompt = "Provide 5 unique one-word objects with handles, no verbs, that could be used interchangeably with the word 'mug' for drinking. These objects should closely resemble a mug in both appearance and function, and each should be distinct with no repetition. Surround each list object with **."
    messages = [
        {"role": "user", "content": prompt}
    ]
    text = tokenizer.apply_chat_template(
        messages,
        tokenize=False,
        add_generation_prompt=True,
        enable_thinking=False  # Switches between thinking and non-thinking modes. Default is True.
    )
    model_inputs = tokenizer([text], return_tensors="pt").to(model.device)

    # conduct text completion
    generated_ids = model.generate(
        **model_inputs,
        max_new_tokens=32768
    )
    output_ids = generated_ids[0][len(model_inputs.input_ids[0]):].tolist()

    # parsing thinking content
    try:
        # rindex finding 151668 (</think>)
        index = len(output_ids) - output_ids[::-1].index(151668)
    except ValueError:
        index = 0

    content = tokenizer.decode(output_ids[index:], skip_special_tokens=True).strip("\n")
    print("content:", content)
    pattern = r'\*\*(.*?)\*\*'
    listItems = re.findall(pattern, content)

    for item in listItems:
        item = item.lower()

        if ' ' in item:
            reload = True
        elif item == "bowl" or item == "spoon" or item == "sack" or item == "tongs" or item == "serving" or item == "plate" or item == "tongue" or item == "tea" or item == "ulp" or item == "ladle" or item == "gourd" or item == "dagger" or item == "platter" or item == "pitcher" or item == "gallon" or item == "teapot":
            reload = True
        elif item in mug_synonyms:
            reload = True
        else:
            # print(item)
            mug_synonyms.append(item)
            correctItems += 1

        if correctItems == 5:
            reload = False
            break

word = random.choice(mug_synonyms)
print("Final Word:", word)

data = {
    "mugObject": word
}

json_data = json.dumps(data)

url = "http://localhost:5000/receive_data"

response = requests.post(url, json=data)

print(response.status_code)
print(response.text)
