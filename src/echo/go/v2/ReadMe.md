# Dockerize

**docker build -t bnaya/go-simple-server:v1 . **

docker run -it --rm --name bnaya-go-simple-server -p 8090:80 -e GO_SERVER_PORT="80" bnaya/go-simple-server:v1
