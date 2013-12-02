powershell.exe -NoProfile -ExecutionPolicy unrestricted -Command "& { Import-Module '.\build\psakev4\psake.psm1'; Invoke-psake .\default.ps1; }" 
pause