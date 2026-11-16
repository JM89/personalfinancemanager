load('./tools/tilt_extensions/load_module_file/Tiltfile', 'load_config')

config.define_bool('offline', usage='Run only the offline React website')
settings = config.parse()

if settings.get('offline', False):
  local_resource(
    'pfm-frontend-offline',
    serve_cmd='dotnet run --project PFM.Website.Reboot/src/PFM.Website/PFM.Website.csproj --launch-profile frontend-only',
    readiness_probe=probe(http_get=http_get_action(host='127.0.0.1', port=5110, path='/health/frontend')),
    links=['http://localhost:5110'],
  )
else:
  configs = read_yaml("Tiltfile.Module.yaml")
  load_config(configs)
