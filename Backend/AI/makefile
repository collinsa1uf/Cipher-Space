server: test.py .venv/Scripts/activate
	.venv/Scripts/python.exe -m PyInstaller --onefile --add-data "words.txt;." --add-data "Models;Models" --name=server test.py

clean:
	- rmdir /s /q build
	- del /f server.spec

all: server clean