from transformers import AutoModelForCausalLM, AutoTokenizer
import os
import re
import random
import json
import requests

from huggingface_hub import login


def object_call(word, prompt, incorrect_items):
    reload = True
    synonyms = [word]
    correct_items = 0

    while reload:
        reload = False
        # prepare the model input
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
                elif not item.isalnum():
                    reload = True
                else:
                    synonyms.append(item)
                    correct_items += 1

                if correct_items == 3:
                    reload = False
                    break

    final_word = random.choice(synonyms)
    print("Final Word:", final_word)
    return final_word


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
shiv_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'shiv' " \
              "which is a bladed weapon. These objects should closely resemble a shiv in both appearance and function, " \
              "and each should be distinct with no repetition. Surround each list object with **. "
shiv_incorrect = ["scythe", "sharp", "spear", "cut", "shovel", "mace", "hatsword", "hilt", "hatchet", "spike", "axe",
                  "pistol", "harpoon", "hoe", "hut", "hose", "hedge", "arrow", "hammer", "tome", "sharpen", "scimitar",
                  "staff", "whip", "crown", "hats", "spade", "cane", "sharer", "shill", "kniver", "hitch", "scissors",
                  "claw", "sharpened", "club", "shivblade", "shivstick", "shivpact", "shivrod", "shivspear", "handle", ]
# shiv_word = object_call("shiv", shiv_prompt, shiv_incorrect)

mug_prompt = "Provide 5 unique one-word objects with handles, no verbs, that could be used interchangeably with the " \
             "word 'mug' for drinking. These objects should closely resemble a mug in both appearance and function, " \
             "and each should be distinct with no repetition. Surround each list object with **. "
mug_incorrect = ["bowl", "spoon", "sack", "tongs", "serving", "plate", "tongue", "tea", "ulp", "ladle", "gourd",
                 "dagger", "platter", "pitcher", "gallon", "teapot", "ulpit"]
# mug_word = object_call("mug", mug_prompt, mug_incorrect)

pitcher_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word " \
                 "'pitcher' for pouring liquids. Each object should have a similar shape (narrow neck, handle, " \
                 "and a spout) and be suitable for pouring liquids in a manner resembling the use of a coffee pitcher " \
                 "(e.g., a container designed for hot or cold beverages). Each should be distinct with no " \
                 "repetition. Surround each list object with **. "
pitcher_incorrect = ["cup", "mug", "spout", "dish", "tin", "bowl", "cans", "glass", "junk", "cannula", "pint", "jerk",
                     "spoon", "syringe", "sack", "bottle", "caddy", "cone", "pail", "tinpot", "tumbler", "can",
                     "beaker", "casserole", "crate", "tins", "cask", "bucket", "gallon", "serving", "jacket", "cups",
                     "clerk", "dagger"]
# pitcher_word = object_call("pitcher", pitcher_prompt, pitcher_incorrect)

TV_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'TV' which " \
            "is a electric screen used to watch something. These objects should closely resemble a TV in both " \
            "appearance (large screen, display, etc.) and function (used for viewing media), and each should be " \
            "distinct with no repetition. Surround each list object with **. "
TV_incorrect = ["set"]
# TV_word = object_call("television", TV_prompt, TV_incorrect)

board_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word " \
               "'noticeboard' for displaying announcements. These objects should closely resemble a noticeboard in " \
               "both appearance and function and each should be distinct with no repetition. Surround each list object " \
               "with **. "
board_incorrect = ["boardcase", "boardspare", "box", "post", "note", "tag", "postcard", "boardroom", "signpost",
                   "poster", "sheet", "notice", "ann", "boarding"]
# board_word = object_call("noticeboard", board_prompt, board_incorrect)

chips_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word 'chips' " \
               "(bagged food for snacking). These objects should closely resemble chips in both " \
               "appearance and function and each should be distinct with no repetition. Surround each list object with " \
               "**. "
chips_incorrect = ["crunch", "dust", "grain", "coke", "pill", "chip", "packs", "cup", "cups", "crispy", "crisp",
                   "cereal", "fried", "popped", "crunchy", "bags", "popsicle", "boxes", "tins", "grains", "slices",
                   "soda", "fry", "fries", "sliced", "granola", "cone", "bites", "cakes", "doughnuts", "doughnut",
                   "pasta", "fruit", "fruits", "crispener", "grilled", "tender", "dough", "muffin", "dried", "grainy",
                   "cocoa", "cotton", "crack", "dishes", "shiny", "soft", "baked", "hard", "tiny", "sweet", "puffed",
                   "biscuit", "containers", "melted", "dusted", "pickle", "pozole", "burgers", "pancakes", "crackle",
                   "bag", "container", "shelf", "tin", "can", "cans", "bread", "salad", "tuna", "pillows", "sacks",
                   "sack", "crispie", "toppings", "desserts", "cereals", "chipsticks", "chippe", "chipmunk", "salty",
                   "tacos", "packed", "casseroles", "casserole", "crispers", "crisper", "waffles", "sodas", "stuffed",
                   "slate", "breadsticks", "crispiness", "crispies", "chiplets", "chiplet", "muffins", "snacky",
                   "snacktime", "grits", "grit", "peanut", "sizzled", "sauté", "grill", "pots", "bowl", "plates",
                   "plate", "dairy", "meat", "pudding", "nut", "pale", "snackies", "milk", "pops", "snackbox",
                   "snackpile", "mors"]
# chips_word = object_call("chips", chips_prompt, chips_incorrect)

crate_prompt = "Provide 5 unique one-word objects, no verbs, that could be used interchangeably with the word " \
               "'crate' for holding large objects. These objects should closely resemble a crate in " \
               "both appearance and function and each should be distinct with no repetition. Surround each list object " \
               "with **. "
crate_incorrect = ["cask", "cradle", "bowl", "cup", "cabinet", "satchel", "caddy", "pallet", "tinsel", "cub",
                   "cratecase", "cratehold", "cratebox", "crafteroom", "cratecontainer", "locker", "herd", "cupboard",
                   "trek", "cushion", "sack", "cubby", "shelf", "basket"]
# crate_word = object_call("crate", crate_prompt, crate_incorrect)

vial_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to a beaker, " \
              "a small glass vessel for storing chemicals. Each object should be distinct and not repeat. Surround " \
              "each list object with **. "
vial_incorrect = ["tub", "shaker", "cup", "bottle", "jars", "jar", "tumbler", "cylinder", "glasspot", "cask", "chamber",
                  "colander", "bowl", "bead"]
# vial_word = object_call("beaker", vial_prompt, vial_incorrect)

vitals_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function " \
                "to a vitals monitor, a device that measures health indicators. Each object should be distinct and " \
                "not repeat. Surround each list object with **. "
vitals_incorrect = ["measuring", "readers", "watch", "spectrometer", "temperature", "cardiovascular", "heartbeat",
                    "heart", "blood", "pulse", "lab", "oxygen", "respiratory", "sphygmomanometer", "echocardiogram",
                    "electrocardiogram", "wrist", "actuators", "breath", "health", "read", "stick", "well", "life",
                    "systolic", "diastolic", "thermoscope", "oscilloscope", "sphygrometer", "meter", "smartwatch",
                    "glove", "slate", "sight", "view", "cardiogram", "breathing", "eyes", "systole", "diastole",
                    "cardio", "body", "system", "sphygmatoscope", "atriumcope", "thromboprost", "vitalimeter", "soul",
                    "brain", "O2", "spectrum", "wound", "sphygmat"]
# vitals_word = object_call("vitals", vitals_prompt, vitals_incorrect)

computer_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to a " \
                  "computer, an electronic device for processing data, running software, and interacting with digital " \
                  "networks. Each object should be distinct and not repeat. Surround each list object with **. "
computer_incorrect = ["router", "camera", "printer", "hub", "browser", "server", "cloud", "clock", "smartwatch",
                      "smartphone", "tablet", "scanner", "speaker", "switch", "phone", "table", "modem", "mirror",
                      "pda", "network", "encoder", "touchpad", "ram"]
# computer_word = object_call("computer", computer_prompt, computer_incorrect)

circuit_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to a " \
                 "circuit box, a device that contains wires and controls the electricity in the building. Each " \
                 "object should be distinct and not repeat. Surround each list object with **. "
circuit_incorrect = ["light", "relay", "fuse", "timer", "switchgear", "contacts", "fuses", "hub", "radiator",
                     "contactor", "door", "switch", "connector", "socket", "generator", "lamp", "power", "electroniser",
                     "wireframe", "powerline", "outlet", "furnace", "doorbell", "rack", "lightswitch", "electrogrid",
                     "controlhub", "energycore", "module", "thermostat", "clock", "button", "battery", "mainboard"]
# circuit_word = object_call("circuit", circuit_prompt, circuit_incorrect)

tools_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to a " \
               "toolbox, a case for storing tools. Each object should be distinct and not repeat. Surround each list " \
               "object with **. "
tools_incorrect = ["cabinet", "trolley", "crate", "stall", "tote", "stylish", "caddy", "holding", "stable", "clot",
                   "key", "tool", "dagger", "sturdy", "stowaway", "shelf", "rack", "sprocket", "stitcher", "stool",
                   "toolshelf", "basket", "sleeper", "pouch", "sticker", "carton", "ladle", "drawer", "sling", "tin",
                   "bin", "cart"]
# tools_word = object_call("toolbox", tools_prompt, tools_incorrect)

screws_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to screws, " \
                "small metal fasteners used to hold a materials together by threading into them. Each object should " \
                "be distinct and not repeat. Surround each list object with **. "
screws_incorrect = ["hinge", "hinges", "clamp", "tackle", "clamps", "gears", "gear", "tapes", "tape", "gaskets",
                    "gasket", "cables", "cable", "bushings", "bushing", "buckles", "buckle", "crimps", "crimp", "hacks",
                    "hack", "wrenches", "wrench", "key", "thread", "tubes", "tube", "pucks", "puck", "hubs", "hub",
                    "socket", "sockets", "slots", "slot", "claws", "claw", "washer", "washers", "hanger", "hangers",
                    "bar", "bars", "shims", "shim", "hammers", "hammer", "locks", "lock", "nap", "naps", "plug",
                    "plugs", "screwdrivers", "screwdriver", "rope", "ropes", "pliers", "plier", "nugget", "nuggets",
                    "hacksaws", "hacksaw", "wires", "wire", "cement", "cements", "rings", "ring", "clinks", "clink",
                    "crank", "cranks", "keyholes", "keyhole", "rods", "rod", "handle", "handles", "housings", "housing",
                    "sticks", "stick", "rubes", "rube", "gates"]
# screws_word = object_call("screws", screws_prompt, screws_incorrect)

liquid_prompt = "Provide 5 unique one-word objects (no verbs) that are similar in appearance and function to " \
                "liquid chemicals, used to power a ship. Each object should be distinct and not repeat. Surround each " \
                "list object with **. "
liquid_incorrect = ["oxy", "energ", "mot", "energetic", "light", "lamp", "energium", "vitalite", "oxygen", "lumina",
                    "thermos", "motivation", "thermoelectric", "battery", "electro", "lumen", "chem", "solar", "hydro",
                    "wind", "pump", "generator", "engine", "laser", "electron", "spark", "atom", "ion", "vaporizer",
                    "charmant", "thermal", "boil", "brew", "heat", "furnace", "burn", "mines", "gas", "charm", "jet",
                    "chimney", "boiler", "catalyst", "oxidizer", "molten", "distiller", "refrigerator", "reactor",
                    "steam", "vapor", "gaseous", "mist", "fueled", "batter", "chill", "bloom", "flame", "luminous",
                    "syringe", "cylinder", "liquef", "oxyd", "volumetric", "vodka", "soda", "liquor", "syrup", "melt",
                    "run", "cone", "cup", "thermometer", "spatula", "acetate", "spill", "magnet", "chrom", "liqueur",
                    "matter", "pour", "droplet", "hydrogen", "source", "chiller", "refinery", "chim", "liqu", "phenol"]
liquid_word = object_call("chemicals", liquid_prompt, liquid_incorrect)

data = {
    "key": "jail",
    # "shivObject": shiv_word,
    # "mugObject": mug_word,
    # "pitcherObject": pitcher_word,
    # "TVObject": TV_word,
    # "boardObject": board_word,
    # "chipsObject": chips_word,
    # "crateObject": crate_word
    # "vialObject": vial_word
    # "vitalsObject": vitals_word
    # "computerObject": computer_word
    # "circuitObject": circuit_word
    # "toolsObject": tools_word
    # "screwsObject": screws_word
    "liquidObject": liquid_word

}

# The URL of the FastAPI server endpoint
url = "http://localhost:8000/receive_data"

# Send POST request with JSON data
response = requests.post(url, json=data)

print(response.status_code)
print(response.text)
