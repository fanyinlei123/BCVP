color 3

dotnet new -i .template.config\BCVP.Webapi.Template.1.11.30.nupkg

set /p OP=Please set your project name(for example:Baidu.Api):

md .1YourProject

cd .1YourProject

dotnet new blogcoretpl -n %OP%

cd ../


echo "Create Successfully!!!! ^ please see the folder .1YourProject"

dotnet new -u BCVP.Webapi.Template


echo "Delete Template Successfully"

pause