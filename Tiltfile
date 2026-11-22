load('./tools/tilt_extensions/load_module_file/Tiltfile', 'load_config')
load('./tools/tilt_extensions/offline/Tiltfile', 'run_offline_frontend')

config.define_bool('offline', usage='Run only the offline React website')
settings = config.parse()

if settings.get('offline', False):
  run_offline_frontend()
else:
  configs = read_yaml("Tiltfile.Module.yaml")
  load_config(configs)
