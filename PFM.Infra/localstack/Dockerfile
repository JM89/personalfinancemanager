FROM localstack/localstack:3

COPY ./setup/ /etc/localstack/init/ready.d/

RUN chmod -R 744 /etc/localstack/init/ready.d/

ENTRYPOINT ["docker-entrypoint.sh"]