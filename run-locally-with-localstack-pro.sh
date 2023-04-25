find . -iname "*.sh" -exec dos2unix {} \;
docker-compose -f ./PFM.Infra/docker-compose-localstack-pro.yml down
docker container prune -f
docker volume prune -f
docker network create -d bridge local-network
docker-compose -f ./PFM.Infra/docker-compose-localstack-pro.yml --env-file ./PFM.Infra/configs/.env up --build -d
cd ./PFM.Api
docker-compose -f ./PFM.Infra/docker-compose-iac.yml --env-file ./PFM.Infra/configs/.env up --build -d
cd ..

