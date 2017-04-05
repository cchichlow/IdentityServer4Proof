$source = "https://github.com/IdentityServer/AuthServer/archive/release.zip"
Invoke-WebRequest $source -OutFile ui.zip

Expand-Archive ui.zip

if (!(Test-Path -Path Quickstart))  { mkdir Quickstart }
if (!(Test-Path -Path Views))       { mkdir Views }
if (!(Test-Path -Path wwwroot))     { mkdir wwwroot }

copy .\ui\AuthServer-release\Quickstart\* Quickstart -recurse -force
copy .\ui\AuthServer-release\Views\* Views -recurse -force
copy .\ui\AuthServer-release\wwwroot\* wwwroot -recurse -force

del ui.zip
del ui -recurse
