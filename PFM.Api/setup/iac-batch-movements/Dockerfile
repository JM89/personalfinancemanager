FROM zenika/terraform-aws-cli:release-7.0_terraform-1.3.9_awscli-2.12.5

COPY --chown=nonroot ./tf-plan-apply.sh ./
COPY --chown=nonroot ./entrypoint.sh ./

COPY --chown=nonroot ./infrastructure/ ./infrastructure/

ENTRYPOINT [ "bash", "entrypoint.sh" ]