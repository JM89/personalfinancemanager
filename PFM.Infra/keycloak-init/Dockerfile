# Setup build arguments
ARG AWS_CLI_VERSION=linux-aarch64-2.19.4
ARG TERRAFORM_VERSION=1.5.2
ARG DEBIAN_VERSION=bookworm-20230612-slim
ARG DEBIAN_FRONTEND=noninteractive

# Download Terraform binary
FROM debian:${DEBIAN_VERSION} as terraform
ARG TARGETARCH
ARG TERRAFORM_VERSION
RUN apt-get update
# RUN apt-get install --no-install-recommends -y libcurl4=7.74.0-1.3+deb11u7
RUN apt-get install --no-install-recommends -y ca-certificates
RUN apt-get install --no-install-recommends -y curl
RUN apt-get install --no-install-recommends -y gnupg
RUN apt-get install --no-install-recommends -y unzip
WORKDIR /workspace
RUN curl --silent --show-error --fail --remote-name https://releases.hashicorp.com/terraform/${TERRAFORM_VERSION}/terraform_${TERRAFORM_VERSION}_linux_${TARGETARCH}.zip
COPY security/hashicorp.asc ./
COPY security/terraform_${TERRAFORM_VERSION}** ./
RUN gpg --import hashicorp.asc
RUN gpg --verify terraform_${TERRAFORM_VERSION}_SHA256SUMS.sig terraform_${TERRAFORM_VERSION}_SHA256SUMS
RUN sha256sum --check --strict --ignore-missing terraform_${TERRAFORM_VERSION}_SHA256SUMS
RUN unzip -j terraform_${TERRAFORM_VERSION}_linux_${TARGETARCH}.zip

# Install AWS CLI version 2
FROM debian:${DEBIAN_VERSION} as aws-cli
ARG AWS_CLI_VERSION
RUN apt-get update
RUN apt-get install -y --no-install-recommends ca-certificates
RUN apt-get install -y --no-install-recommends curl
RUN apt-get install -y --no-install-recommends gnupg
RUN apt-get install -y --no-install-recommends unzip
RUN apt-get install -y --no-install-recommends git
RUN apt-get install -y --no-install-recommends jq
WORKDIR /workspace
RUN curl --show-error --fail --output "awscliv2.zip" --remote-name "https://awscli.amazonaws.com/awscli-exe-${AWS_CLI_VERSION}.zip"
COPY security/awscliv2.asc ./
COPY security/awscliv2-${AWS_CLI_VERSION}.sig ./awscliv2.sig
RUN gpg --import awscliv2.asc
RUN gpg --verify awscliv2.sig awscliv2.zip
RUN unzip -u awscliv2.zip
RUN ./aws/install --install-dir /usr/local/aws-cli --bin-dir /usr/local/bin

# Build final image
FROM debian:${DEBIAN_VERSION} as build

RUN apt-get update \
  && apt-get install -y --no-install-recommends \
    ca-certificates \
    git \
    jq \
    curl \
    openssh-client \
  && apt-get clean \
  && rm -rf /var/lib/apt/lists/*
WORKDIR /workspace
COPY --from=terraform /workspace/terraform /usr/local/bin/terraform
COPY --from=aws-cli /usr/local/bin/ /usr/local/bin/
COPY --from=aws-cli /usr/local/aws-cli /usr/local/aws-cli

COPY entrypoint.sh ./
COPY tf-plan-apply.sh ./
# COPY ./core-modules/ ./core-modules
COPY ./access-control/ ./access-control

ENTRYPOINT [ "bash", "./entrypoint.sh" ]