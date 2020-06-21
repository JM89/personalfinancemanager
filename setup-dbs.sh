msbuild_path="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"

"$msbuild_path" ../pfm-authentication-api/db/PFM.Authentication.Db/PFM.Authentication.Db.sqlproj -t:SqlPublish -p:SqlPublishProfilePath=PFM.Authentication.Db.publish.local.xml
"$msbuild_path" ../pfm-bank-account-api/db/PFM.BankAccount.Db/PFM.BankAccount.Db.sqlproj -t:SqlPublish -p:SqlPublishProfilePath=PFM.BankAccount.Db.publish.local.xml
