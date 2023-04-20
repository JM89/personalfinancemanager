FROM localstack/localstack-pro:2.0.2

RUN apt-get update && apt-get install wget

COPY ./setup/ /etc/localstack/init/ready.d/

RUN chmod -R 744 /etc/localstack/init/ready.d/

ENTRYPOINT ["docker-entrypoint.sh"]