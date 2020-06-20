msbuild_path="C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe"

"$msbuild_path" ../pfm-bank-account-api/db/PFM.BankAccount.Db/PFM.BankAccount.Db.sqlproj -t:SqlPublish -p:SqlPublishProfilePath=PFM.BankAccount.Db.publish.local.xml
