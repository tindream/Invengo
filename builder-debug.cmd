@title builder
@echo ---------------version-----------------------------------------------------
@GitVersion.exe
@echo ---------------debug-------------------------------------------------------
@devenv Paway.WPF.sln /Rebuild debug
@echo --------------------------------------------------------------------------- 
@PAUSE