#!/bin/bash
set -e  # зупиняє скрипт при помилці


cd "WStore(Backend)"
docker build -t wstore-backend --platform linux/amd64,linux/arm64 .
docker tag wstore-backend:latest v1dkos/wstore-backend:latest
docker push v1dkos/wstore-backend:latest

echo "Done ---api---!"

read -p "Press any key to exit..."
