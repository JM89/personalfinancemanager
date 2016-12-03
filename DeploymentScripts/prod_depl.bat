copy C:\inetpub\wwwroot\PersonalFinanceManager\Resources\bank_icons C:\inetpub\wwwroot\BackupResources

"c:\program files (x86)\msbuild\14.0\bin\msbuild.exe" "..\personalfinancemanager\personalfinancemanager.csproj" /t:package /p:configuration=release /p:outputpath="../deploymentscripts/releasefolder"

"c:\program files\iis\microsoft web deploy v3\msdeploy.exe" -verb:sync -source:package="releasefolder/_publishedwebsites/personalfinancemanager_package/personalfinancemanager.zip" -dest:auto -setparam:name="iis web application name",value="personalfinancemanager"

copy C:\inetpub\wwwroot\BackupResources C:\inetpub\wwwroot\PersonalFinanceManager\Resources\bank_icons