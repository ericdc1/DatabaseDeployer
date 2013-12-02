# required parameters :
# 	$databaseName

properties {
	$projectName = "DatabaseDeployer" 
    $version = "2.0.0"

    $version = $version + "." + (get-date -format "MMdd")  
	$projectConfig = "Release"
	$base_dir = resolve-path .
	$source_dir = "$base_dir\source"
    $unitTestAssembly = "$projectName.UnitTests.dll"
    $nunitPath = "$source_dir\packages\NUnit.Runners.2.6.3\tools\"
	$build_dir = "$base_dir\build\build"
	$test_dir = "$build_dir\test"
	$testCopyIgnorePath = "_ReSharper"
	$package_dir = "$build_dir\package"	
	$package_file = "$build_dir\latestVersion\" + $projectName +"_Package.zip"
	$databaseServer = ".\sqlexpress"
	$databaseScripts = "$source_dir\Database"
	$databaseName = "Demo"
}

task default -depends Init, UpdateAssemblyInfo, Compile, Test
task ci -depends Init, UpdateAssemblyInfo, Compile, Test, Package, NugetPack

task Init {
    delete_file $package_file
    delete_directory $build_dir
    create_directory $test_dir
	create_directory $build_dir
}

task Compile -depends Init {
    msbuild /t:clean /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln /p:VisualStudioVersion=12.0

    delete_file $error_dir
    msbuild /t:build /v:q /nologo /p:Configuration=$projectConfig $source_dir\$projectName.sln /p:VisualStudioVersion=12.0
}

task Test {
    if (Test-Path  ("$nunitPath\nunit-console-x86.exe")){
        copy_all_assemblies_for_test $test_dir
        if (Test-Path  ("$test_dir\$unitTestAssembly")){
            write-host "Testing $unitTestAssembly"
	        exec {
		        & $nunitPath\nunit-console-x86.exe $test_dir\$unitTestAssembly /xml $build_dir\UnitTestResult.xml    
	        }
        }
        else
        {
            write-host "Cannot run unit tests as $nunitPath\$unitTestAssembly is MISSING"
        }
    }
    else{
      write-host "Cannot run tests as $nunitPath\nunit.console.clr4.exe is MISSING"
    }
}

task UpdateAssemblyInfo {
    Update-AssemblyInfoFiles $version
}

task RebuildDatabase {
    exec { & $base_dir\build\DatabaseDeployer\DatabaseDeployer.exe Rebuild $databaseServer "$databaseName" "$databaseScripts\Scripts"}
}

task UpdateDatabase {
    try{
        exec { & $base_dir\build\DatabaseDeployer\DatabaseDeployer.exe Update $databaseServer "$databaseName" "$databaseScripts\Scripts"}
    }
    catch{
        write-host "Database does not exist - running rebuild"
        exec { & $base_dir\build\DatabaseDeployer\DatabaseDeployer.exe Rebuild $databaseServer "$databaseName" "$databaseScripts\Scripts"}
    }
}

task SeedDatabase { 
    exec { & $base_dir\build\DatabaseDeployer\DatabaseDeployer.exe Seed $databaseServer " $databaseName" "$databaseScripts\Scripts"}
}

task Package -depends Compile {
    write-host "Clean package directory"
    delete_directory $package_dir
   
    write-host "Copy newly compiled version of Database Deployer"
	copy_files "$base_dir\source\DatabaseDeployer.Console\Bin\Release" "$package_dir\DatabaseDeployer" 

    write-host "Copy in database scripts"
    copy_files "$databaseScripts\scripts" "$package_dir\database\"
    write-host "Copy DatabaseDeployer tool so scripts can be ran"
    copy_files "$base_dir\build\DatabaseDeployer"  "$package_dir\database\"  "$package_dir\database\"
    write-host "Create batch file to run db updates"
    create-dbdeployscript("$package_dir\database\_Update-Database.bat")
 
    write-host "Zip it up"
	zip_directory $package_dir $package_file 
}
 
 task NugetPack -depends Package {
  exec {
    & $base_dir\build\ilmerge.exe /target:exe /lib:C:\Windows\Microsoft.NET\Framework\v4.0.30319 /targetplatform:v4 /out:$package_dir\DatabaseDeployer\DatabaseDeployer.exe $package_dir\DatabaseDeployer\DatabaseDeployer.console.exe $package_dir\DatabaseDeployer\DatabaseDeployer.core.dll  
    }
 exec {
    & $source_dir\.nuget\nuget.exe pack -Version $version -outputdirectory $build_dir $base_dir\DatabaseDeployer.nuspec
    }
}

function global:zip_directory($directory,$file) {
    write-host "Zipping folder: " $test_assembly
    delete_file $file
    cd $directory
    & "$base_dir\build\7zip\7za.exe" a -mx=9 -r $file
    cd $base_dir
}

function global:copy_files($source,$destination,$exclude=@()){    
    create_directory $destination
    Get-ChildItem $source -Recurse -Exclude $exclude -ErrorAction SilentlyContinue | Copy-Item -ErrorAction SilentlyContinue -Destination {Join-Path $destination $_.FullName.Substring($source.length)} 
}

function global:Copy_and_flatten ($source,$filter,$dest) {
  ls $source -filter $filter  -r | Where-Object{!$_.FullName.Contains("$testCopyIgnorePath") -and !$_.FullName.Contains("packages") -and !$_.FullName.Contains("build") }| cp -dest $dest -force
}

function global:copy_all_assemblies_for_test($destination){
  create_directory $destination
  Copy_and_flatten $source_dir *.exe $destination
  Copy_and_flatten $source_dir *.dll $destination
  Copy_and_flatten $source_dir *.config $destination
  Copy_and_flatten $source_dir *.xml $destination
  Copy_and_flatten $source_dir *.pdb $destination
}

function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name)
{
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:delete_files_in_dir($dir)
{
	get-childitem $dir -recurse | foreach ($_) {remove-item $_.fullname}
}

function global:create_directory($directory_name)
{
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}



function create-dbdeployscript($filename)
{ 
"@echo off
set /p serverName=""DB Server: "" %=%
DatabaseDeployer.exe Update %serverName% $databaseName . 
pause"  | out-file $filename -encoding "ASCII"       
}


function Update-AssemblyInfoFiles ([string] $version, [System.Array] $excludes = $null) {
 
#-------------------------------------------------------------------------------
# Update version numbers of AssemblyInfo.cs
# adapted from: http://www.luisrocha.net/2009/11/setting-assembly-version-with-windows.html
#-------------------------------------------------------------------------------
 
	if ($version -notmatch "[0-9]+(\.([0-9]+|\*)){1,3}") {
		Write-Error "Version number incorrect format: $version"
	}
	
	$versionPattern = 'AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssembly = 'AssemblyVersion("' + $version + '")';
	$versionFilePattern = 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)'
	$versionAssemblyFile = 'AssemblyFileVersion("' + $version + '")';
 
	Get-ChildItem -r -filter AssemblyInfo.* | % {
		$filename = $_.fullname
		
		$update_assembly_and_file = $true
		
		# set an exclude flag where only AssemblyFileVersion is set
		if ($excludes -ne $null)
			{ $excludes | % { if ($filename -match $_) { $update_assembly_and_file = $false	} } }
		
		# see http://stackoverflow.com/questions/3057673/powershell-locking-file
		# I am getting really funky locking issues. 
		# The code block below should be:
		#     (get-content $filename) | % {$_ -replace $versionPattern, $version } | set-content $filename
 
		$tmp = ($file + ".tmp")
		if (test-path ($tmp)) { remove-item $tmp }
 
		if ($update_assembly_and_file) {
			(get-content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } | % {$_ -replace $versionPattern, $versionAssembly }  > $tmp
			write-host Updating file AssemblyInfo and AssemblyFileInfo: $filename --> $versionAssembly / $versionAssemblyFile
		} else {
			(get-content $filename) | % {$_ -replace $versionFilePattern, $versionAssemblyFile } > $tmp
			write-host Updating file AssemblyInfo only: $filename --> $versionAssemblyFile
		}
 
		if (test-path ($filename)) { remove-item $filename }
		move-item $tmp $filename -force		
 
	}
}

