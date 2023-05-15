find . -iname "*.sh" -exec dos2unix {} \;
docker-compose -f ./PFM.Infra/docker-compose-infra.yml down
docker container prune -f
docker volume prune -f
docker network create -d bridge local-network
docker-compose -f ./PFM.Infra/docker-compose-infra.yml up --build -d
cd ./PFM.Auth.Api
docker-compose -f docker-compose-api-init.yml up --build -d
cd ..
cd ./PFM.Api
docker-compose -f docker-compose-api-init.yml --env-file ../PFM.Infra/configs/.env up --build -d
cd ..
cd ./PFM.BankAccountUpdater
docker-compose -f docker-compose-init.yml  up --build -d
cd ..
cd ./PFM.Bank.Api
docker-compose -f docker-compose-api-init.yml up --build -d
cd ..
cd ./PFM.Website
docker-compose -f docker-compose-init.yml --env-file ../PFM.Infra/configs/.env  up --build -d
cd ..