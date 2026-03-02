main: main.py .venv/Scripts/activate
	.venv/Scripts/python.exe -m PyInstaller --onefile main.py

server: server.py .venv/Scripts/activate
	.venv/Scripts/python.exe -m PyInstaller --onefile server.py

clean:
	rm -rf dist build spec __pycache__ .venv *.spec

all: main server