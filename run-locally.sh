docker container prune -f
docker volume prune -f
docker network create -d bridge local-network
docker-compose -f ./PFM.Infra/docker-compose-infra.yml up --build -d
cd ./PFM.Api
docker-compose -f docker-compose-api-init.yml up --build -d
cd ..