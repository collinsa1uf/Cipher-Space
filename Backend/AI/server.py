from fastapi import FastAPI
from fastapi.responses import JSONResponse
from pydantic import BaseModel
from typing import Dict, Any
import uvicorn

app = FastAPI()

# recentData = None

data_store = {}


@app.post("/receive_data")
async def receive_data(data: Dict[str, Any]):
    key = data.get("key")

    if not key:
        return JSONResponse(content={"status": "error", "message": "Key is required"}, status_code=400)

    data_store[key] = data

    return JSONResponse(content={"status": "success", "received": data}, status_code=200)


@app.get("/get_data/{key}")
async def get_data(key: str):
    if key in data_store:
        return JSONResponse(content={"status": "success", "data": data_store[key]}, status_code=200)
    else:
        return JSONResponse(content={"status": "error", "message": "Key not found"}, status_code=404)


if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)
