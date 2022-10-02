ARG VARIANT="3.7"
FROM mcr.microsoft.com/vscode/devcontainers/python:0-${VARIANT}

RUN pip install -U pip
RUN sudo apt-get update && sudo apt-get -y install gunicorn3
RUN sudo pip install opencv-python
RUN apt update && apt install -y libsm6 libxext6 libxrender1 libfontconfig1 ffmpeg libxrender-dev
