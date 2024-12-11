FROM terraform-aws-cli:local

COPY --chown=nonroot ./tf-plan-apply.sh ./
COPY --chown=nonroot ./entrypoint.sh ./

COPY --chown=nonroot ./infrastructure/ ./infrastructure/

ENTRYPOINT [ "bash", "entrypoint.sh" ]