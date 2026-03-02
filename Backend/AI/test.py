from fastapi import FastAPI
from fastapi.responses import JSONResponse
from typing import Dict, Any
import uvicorn
import os
import re
import random
import torch
from transformers import AutoModelForCausalLM, AutoTokenizer

# --- FASTAPI SETUP ---
app = FastAPI()
data_store: Dict[str, Any] = {}  # In-memory storage for generated objects

# --- HUGGINGFACE MODEL SETUP ---
#hf_token = os.getenv("HF_TOKEN")

model_name = "Qwen/Qwen3-0.6B"

# Load tokenizer and model
tokenizer = AutoTokenizer.from_pretrained(model_name)
model = AutoModelForCausalLM.from_pretrained(
    model_name,
    torch_dtype="auto",
    device_map="auto"
)

# --- HELPER FUNCTION ---
def object_call(word, prompt, incorrect_items):
    reload = True
    synonyms = [word]
    correct_items = 0

    while reload:
        reload = False
        messages = [{"role": "user", "content": prompt}]
        text = tokenizer.apply_chat_template(
            messages,
            tokenize=False,
            add_generation_prompt=True,
            enable_thinking=False
        )
        model_inputs = tokenizer([text], return_tensors="pt").to(model.device)

        generated_ids = model.generate(
            **model_inputs,
            max_new_tokens=32768
        )
        output_ids = generated_ids[0][len(model_inputs.input_ids[0]):].tolist()

        try:
            index = len(output_ids) - output_ids[::-1].index(151668)
        except ValueError:
            index = 0

        content = tokenizer.decode(output_ids[index:], skip_special_tokens=True).strip("\n")
        pattern = r'\*\*(.*?)\*\*'
        list_items = re.findall(pattern, content)
        if len(list_items) == 0:
            reload = True
        else:
            for item in list_items:
                item = item.lower()
                if ' ' in item:
                    reload = True
                elif item in incorrect_items:
                    reload = True
                elif item in synonyms:
                    reload = True
                else:
                    synonyms.append(item)
                    correct_items += 1
                if correct_items == 3:
                    reload = False
                    break

    final_word = random.choice(synonyms)
    return final_word

# --- PROMPTS AND INCORRECT LISTS (UNCHANGED) ---
shiv_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'shiv' " \
              "which is a bladed weapon. These objects should closely resemble a shiv in both appearance and function, " \
              "and each should be distinct with no repetition. Surround each list object with **. "
shiv_incorrect = ["scythe", "sharp", "spear", "cut", "shovel", "mace", "hatsword", "hilt", "hatchet", "spike", "axe",
                  "pistol", "harpoon", "hoe", "hut", "hose", "hedge", "arrow", "hammer", "tome", "sharpen", "scimitar",
                  "staff", "whip", "crown", "hats", "spade", "cane", "sharer", "shill", "kniver", "hitch", "scissors",
                  "claw", "sharpened", "club", "shivblade", "shivstick", "shivpact", "shivrod", "shivspear", "handle", ]

mug_prompt = "Provide 5 unique one-word objects with handles, no verbs, that could be used interchangeably with the " \
             "word 'mug' for drinking. These objects should closely resemble a mug in both appearance and function, " \
             "and each should be distinct with no repetition. Surround each list object with **. "
mug_incorrect = ["bowl", "spoon", "sack", "tongs", "serving", "plate", "tongue", "tea", "ulp", "ladle", "gourd",
                 "dagger", "platter", "pitcher", "gallon", "teapot", "ulpit"]

pitcher_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word " \
                 "'pitcher' for pouring liquids. Each object should have a similar shape (narrow neck, handle, " \
                 "and a spout) and be suitable for pouring liquids in a manner resembling the use of a coffee pitcher " \
                 "(e.g., a container designed for hot or cold beverages). Each should be distinct with no " \
                 "repetition. Surround each list object with **. "
pitcher_incorrect = ["cup", "mug", "spout", "dish", "tin", "bowl", "cans", "glass", "junk", "cannula", "pint", "jerk",
                     "spoon", "syringe", "sack", "bottle", "caddy", "cone", "pail", "tinpot", "tumbler", "can",
                     "beaker", "casserole", "crate", "tins", "cask", "bucket", "gallon", "serving", "jacket", "cups",
                     "clerk"]

TV_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'TV' which " \
            "is a electric screen used to watch something. These objects should closely resemble a TV in both " \
            "appearance (large screen, display, etc.) and function (used for viewing media), and each should be " \
            "distinct with no repetition. Surround each list object with **. "
TV_incorrect = ["set"]

board_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word " \
               "'noticeboard' for displaying announcements. These objects should closely resemble a noticeboard in " \
               "both appearance and function and each should be distinct with no repetition. Surround each list object " \
               "with **. "
board_incorrect = ["boardcase", "boardspare", "box", "post", "note", "tag", "postcard", "boardroom", "signpost", "poster", "sheet", "notice", "ann"]

chips_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'chips' " \
               "(bagged food for snacking). These objects should closely resemble chips in both " \
               "appearance and function and each should be distinct with no repetition. Surround each list object with " \
               "**. "
chips_incorrect = ["crunch", "dust", "grain", "coke", "pill", "chip", "packs", "cup", "cups", "crispy", "crisp", "cereal", "fried", "popped", "crunchy", "bags", "popsicle", "boxes", "tins", "grains", "slices", "soda", "fry", "fries", "sliced", "granola", "cone", "bites", "cakes", "doughnuts", "doughnut", "pasta", "fruit", "fruits", "crispener", "grilled", "tender", "dough", "muffin", "dried", "grainy", "cocoa", "cotton", "crack", "dishes", "shiny", "soft", "baked", "hard", "tiny", "sweet", "puffed", "biscuit", "containers", "melted", "dusted", "pickle", "pozole", "burgers", "pancakes", "crackle", "bag", "container", "shelf", "tin", "can", "cans", "bread", "salad", "tuna", "pillows", "sacks", "sack", "crispie", "toppings", "desserts", "cereals", "chipsticks", "chippe", "chipmunk", "salty", "tacos", "packed", "casseroles", "casserole", "crispers", "crisper", "waffles", "sodas", "stuffed", "slate", "breadsticks", "crispiness", "crispies", "chiplets", "chiplet", "muffins", "snacky", "snacktime", "grits", "grit", "peanut", "sizzled", "sauté", "grill", "pots", "bowl", "plates", "plate", "dairy", "meat", "pudding", "nut", "pale", "snackies", "milk", "pops", "snackbox", "snackpile", "mors"]

# --- FASTAPI ENDPOINT ---
@app.post("/generate_objects")
async def generate_objects():
    # Generate objects using existing prompts
    shiv_word = object_call("shiv", shiv_prompt, shiv_incorrect)
    mug_word = object_call("mug", mug_prompt, mug_incorrect)
    pitcher_word = object_call("pitcher", pitcher_prompt, pitcher_incorrect)
    TV_word = object_call("television", TV_prompt, TV_incorrect)
    board_word = object_call("noticeboard", board_prompt, board_incorrect)
    chips_word = object_call("chips", chips_prompt, chips_incorrect)

    # Save to data_store
    data = {
        "key": "jail",
        "shivObject": shiv_word,
        "mugObject": mug_word,
        "pitcherObject": pitcher_word,
        "TVObject": TV_word,
        "boardObject": board_word,
        "chipsObject": chips_word
    }
    data_store[data["key"]] = data

    return {"status": "success", "data": data}

# --- GET DATA ENDPOINT ---
@app.get("/get_data/{key}")
async def get_data(key: str):
    if key in data_store:
        return JSONResponse(content={"status": "success", "data": data_store[key]}, status_code=200)
    else:
        return JSONResponse(content={"status": "error", "message": "Key not found"}, status_code=404)

# --- RUN SERVER ---
if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)