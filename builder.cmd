@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------build-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild release
@echo ---------------------------------------------------------------------------
@echo ---------------reactor-----------------------------------------------------
@dotNET_Reactor -project builder\Paway.WPF.nrproj
@copy bin\Release\Paway.WPF.xml bin\Release\Paway.WPF_Secure\Paway.WPF.xml
@echo ---------------------------------------------------------------------------
@echo ---------------nugut------------------------------------------------------
@nuget pack builder\Paway.WPF.nuspec
@echo --------------------------------------------------------------------------- 
@IF "%1" == "" @PAUSE