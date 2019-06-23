FROM golang:1.11.5

# Create app directory
WORKDIR /usr/go/server

# Install mux
RUN go get github.com/gorilla/mux
Copy ./main.go .
# Expose the application on port 8080
EXPOSE 80

Run ls

CMD ["go", "run", "main.go"]