while getopts p: flag
do
    case "${flag}" in
        p) profile=${OPTARG};;
    esac
done

COMPOSE_PROFILE=${profile:-'dev'}    

echo "Running startup script for profile: '$COMPOSE_PROFILE'";

find . -iname "*.sh" -exec dos2unix {} \;
docker-compose -f ./PFM.Infra/docker-compose-infra.yml --profile dev down 
docker-compose -f ./PFM.Infra/docker-compose-infra.yml --profile prod down 
docker-compose -f ./PFM.Infra/docker-compose-infra.yml --profile frontend-dev down
docker container prune -f
docker volume prune -f
docker network create -d bridge local-network
docker-compose -f ./PFM.Infra/docker-compose-infra.yml --profile $COMPOSE_PROFILE --env-file ./PFM.Infra/configs/.env up --build -d
cd ./PFM.Api
docker-compose -f docker-compose-api-init.yml --profile $COMPOSE_PROFILE --env-file ../PFM.Infra/configs/.env up --build -d
cd ..
cd ./PFM.BankAccountUpdater
docker-compose -f docker-compose-init.yml --profile $COMPOSE_PROFILE up --build -d
cd ..
cd ./PFM.MovementAggregator
docker-compose -f docker-compose-init.yml --profile $COMPOSE_PROFILE up --build -d
cd ..
cd ./PFM.Bank.Api
docker-compose -f docker-compose-api-init.yml --profile $COMPOSE_PROFILE up --build -d
cd ..
cd ./PFM.Website.Reboot
docker-compose -f docker-compose-init.yml --profile $COMPOSE_PROFILE --env-file ../PFM.Infra/configs/.env  up --build -d
cd ..