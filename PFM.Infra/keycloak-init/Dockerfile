FROM terraform-aws-cli:local

COPY entrypoint.sh ./
COPY tf-plan-apply.sh ./
# COPY ./core-modules/ ./core-modules
COPY ./access-control/ ./access-control

ENTRYPOINT [ "bash", "./entrypoint.sh" ]