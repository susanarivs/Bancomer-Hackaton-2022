ARG VARIANT="3.7"
FROM mcr.microsoft.com/vscode/devcontainers/python:0-${VARIANT}
COPY requirements.txt /
EXPOSE 80

RUN sudo pip install -U pip
RUN sudo apt-get update && sudo apt-get -y install libsm6 libxext6 libxrender1 libfontconfig1 ffmpeg libxrender-dev
RUN pip3 install -r /requirements.txt

COPY . /app
WORKDIR /app
ENTRYPOINT ["./gunicorns.sh"]