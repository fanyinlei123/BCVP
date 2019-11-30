color B

del  .PublishFiles\*.*   /s /q

dotnet restore

dotnet build

cd BCVP

dotnet publish -o ..\BCVP\bin\Debug\netcoreapp3.0\

md ..\.PublishFiles

xcopy ..\BCVP\bin\Debug\netcoreapp3.0\*.* ..\.PublishFiles\ /s /e 

echo "Successfully!!!! ^ please see the file .PublishFiles"

cmd