#!/bin/sh
gunicorn myapp:app -w 4 --threads 2 -b 0.0.0.0:80