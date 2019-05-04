@echo off
If "%PROCESSOR_ARCHITECTURE%"=="x86" (
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /u C:\Release\AgentService\AgentService.exe
) Else (
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\InstallUtil.exe /u C:\Release\AgentService\AgentService.exe
)


net start "AgentService"


pause