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

docker buildx build ./PFM.Infra/terraform-aws-cli -f ./PFM.Infra/terraform-aws-cli/Dockerfile -t terraform-aws-cli:local 

docker-compose -f ./PFM.Infra/docker-compose-infra.yml --profile $COMPOSE_PROFILE --env-file ./PFM.Infra/configs/.env up --build -d
docker-compose -f ./PFM.Api/docker-compose-api-init.yml --profile $COMPOSE_PROFILE --env-file ./PFM.Infra/configs/.env up --build -d
docker-compose -f ./PFM.BankAccountUpdater/docker-compose-init.yml --profile $COMPOSE_PROFILE up --build -d
docker-compose -f ./PFM.MovementAggregator/docker-compose-init.yml --profile $COMPOSE_PROFILE up --build -d
docker-compose -f ./PFM.Bank.Api/docker-compose-api-init.yml --profile $COMPOSE_PROFILE up --build -d
docker-compose -f ./PFM.Website.Reboot/docker-compose-init.yml --profile $COMPOSE_PROFILE --env-file ./PFM.Infra/configs/.env  up --build -d
