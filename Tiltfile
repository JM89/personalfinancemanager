load('./tools/tilt_extensions/load_module_file/Tiltfile', 'load_config')
load('./tools/tilt_extensions/docker_build/Tiltfile', 'docker_build_local')

# Load the configuration and start dependencies.
config=read_yaml("Tiltfile.Module.yaml")
load_config(config)

docker_build_local("pfm.infra/terraform-aws-cli")