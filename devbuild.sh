#!/bin/bash

# Function to start npm and dotnet in background
start_processes() {
	# Start processes and redirect output
	sudo docker compose -f docker-compose.dev.yml up -d 
	npm --prefix Frontend run start & 
	cd Backend/src && dotnet run . &
}

# Trap signals to clean up child processes
trap "sudo docker compose -f ./docker-compose.dev.yml down" INT TERM ERR
trap "kill 0" EXIT

start_processes | sed 's/\[?25[hl]//' &> /dev/stdout

# Wait for the background processes to finish
wait