# Notes

[istio-routing-basics](https://medium.com/google-cloud/istio-routing-basics-14feab3c040e)
[canary-deployments](https://kublr.com/blog/hands-on-canary-deployments-with-istio-and-kubernetes/)

## NPM

- npm install -g express-generator
- npm i -s express cors node-fetch await-timeout

## Dockerize (from project root folder)

### cd to the folder  
**docker image build -t bnaya/ping-js:v1 .**  
**docker image build -t bnaya/pong-js:v1 .**  
**docker image build -t bnaya/echo-js:v1 .**  
**docker image build -t bnaya/echo-js:v2 .**  
**docker image build -t bnaya/echo-cs:v1 .**  
**docker image build -t bnaya/ping-cs:v2 .**  
**docker image build -t bnaya/echo-go:v2 .**  

run attached  
**docker run --rm -p 7777:80 bnaya/ping-js:v1**  
**docker run --rm -p 7777:80 bnaya/pong-js:v1**  
**docker run --rm -p 7777:80 bnaya/echo-js:v1**  
**docker run --rm -p 7777:80 bnaya/echo-cs:v1**  
**docker run --rm -p 7777:80 bnaya/echo-go:v2**  

run detached  
**docker run -d -p 7777:80 bnaya/ping-js:v1**

validate  
http://localhost:7777/echo/x/y?m=1&n=2  
http://localhost:7777/health  
http://localhost:7777/ping?count=3  

### Docker clean-up

[clean up](https://www.digitalocean.com/community/tutorials/how-to-remove-docker-images-containers-and-volumes) any resources — images, containers, volumes, and networks — that are dangling (not associated with a container)  
**docker system prune**

for all stopped container (not just dangling images)  
**docker system prune -a**

## HELM

[helm intro](https://docs.bitnami.com/kubernetes/how-to/create-your-first-helm-chart/)  
**helm create chart**  
**helm install --name ping-pong .\ping-pong-chart\\**  
**helm upgrade ping-pong .\ping-pong-canary-chart\\**  
**helm upgrade ping-pong .\ping-pong-ab-chart\\**  
**helm upgrade ping-pong .\ping-pong--go-chart\\**  
**helm history ping-pong**  
**helm rollback ping-pong 1**  
**helm delete ping-pong --purge**

### Diagnostic

check the status  
**istioctl authn tls-check**

### Gateway

kubectl get svc istio-ingressgateway -n istio-system  
istioctl get virtualservices  
istioctl get destinationrules  
kubectl get destinationrules -o yaml  

## Istio


### Istio Ingress Geteway:
kubectl get all  

Virtual Services  
**istioctl get virtualservices**  
**istioctl delete virtualservices ping-pong-virtual-service**  

Destination Rules  
**istioctl get destinationrules**  
**kubectl get destinationrules -o yaml**  

istioctl delete destinationrules destination-rule-echo ping-pong-destination-rule  

#### Check it under istio  
http://localhost/api/v1/echo/abc?x=1&y=2  
http://localhost/api/v1/ping-diag/health  
http://localhost/api/v1/ping?count=5  
http://localhost/api/v1/pong?count=5  