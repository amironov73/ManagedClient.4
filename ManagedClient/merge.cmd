@echo off
cd bin\Debug
..\..\..\ILMerge\ILMerge.exe ManagedClient.dll Antlr4.Runtime.net35.dll Newtonsoft.Json.dll System.Threading.dll /out:ManagedClientMerged.dll /keyfile:..\..\ArsMagna.private.snk
cd ..\..
rem cd bin\Release
rem ..\..\..\ILMerge\ILMerge.exe ManagedClient.dll Antlr4.Runtime.net35.dll /out:ManagedClientMerged.dll /keyfile:..\..\ArsMagna.private.snk
rem cd ..\..