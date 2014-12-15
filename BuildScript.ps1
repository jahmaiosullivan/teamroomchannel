properties {
    $baseDir = resolve-path .
    $configuration = "debug"

	# SQL Server Info
	$DefaultServer='(local)'
	$SqlServer = if ($SqlServer -ne $NULL) { $SqlServer } else { $DefaultServer }
	$SecondarySqlServer = if ($SecondarySqlServer -ne $NULL) { $SecondarySqlServer } else { $SqlServer }
	$ProjectName = 'Teamroom'
	$SqlDatabase = if ($SqlDatabase -ne $NULL) { $SqlDatabase } else { $ProjectName }	
}

task upgradedb -depends db-init-upgrade
task default -depends run-clean, run-build, run-unit-tests, run-js-tests
task debug -depends default
task release -depends default
task notests -depends run-clean, run-build
task tests -depends run-unit-tests, run-js-tests
task css -depends less

# Config Tasks
# Build Tasks
task run-clean {
    exec { msbuild "$ProjectName.sln" /t:Clean /v:quiet }
    clean $webCompileDir
}

task run-build -depends generate-config, less {
	exec { msbuild "$ProjectName.sln" /t:Build /v:Minimal /p:Configuration=$configuration }
}

task generate-config {
	exec { msbuild "$ProjectName.Web/$ProjectName.Web.csproj" /t:TransformWebConfig /p:Configuration=$configuration /v:Minimal }	
	$transformedConfigPath = "$ProjectName.Web\obj\$configuration\TransformWebConfig\transformed\web.config"
	if ((test-path $transformedConfigPath) -eq $TRUE) {		
        copy "$transformedConfigPath" "$ProjectName.Web\web.config"
		write-host "Copied transformed web.config from $transformedConfigPath" -foregroundColor 'cyan'
    }
	else 
	{
		write-host "Unable to find transformed web.config from $transformedConfigPath" -foregroundColor 'yellow'
	}
}

task run-unit-tests {
    exec { .\ExternalBinaries\xunit\xunit.console.clr4.x86.exe "$ProjectName.Tests\bin\$configuration\$ProjectName.Tests.dll" }
}

task run-unit-tests-release {
    exec { .\ExternalBinaries\xunit\xunit.console.clr4.x86.exe "$ProjectName.Tests\bin\Release\$ProjectName.Tests.dll" }
}

task run-js-tests {
    exec { .\ExternalBinaries\Chutzpah\chutzpah.console.exe "$ProjectName.Tests\Web\Scripts" }
}

task less {
	exec { .\ExternalBinaries\dotless\dotless.Compiler.exe -m "$ProjectName.Web\content\less\styles.less" "$ProjectName.Web\content\" }
}

# Database Tasks
task db-init-upgrade {
    exec { .\Database\Tools\DatabaseMigrationManager.exe upgrade /s $SqlServer /d $SqlDatabase /sp Database/Main }
}

# Functions
function global:clean([string[]]$paths) {
    foreach ($path in $paths) {
      remove-item -force -recurse $path -ErrorAction SilentlyContinue
    }
}

function global:create([string[]]$paths) { 
    foreach ($path in $paths) {
        if ((test-path $path) -eq $FALSE) {
            new-item -path $path -type directory | out-null
        }
    }
}

function global:roboexec([scriptblock]$cmd) {
    & $cmd | out-null
	if ($lastexitcode -gt 0) { throw "Received evil error code $lastexitcode for command: " + $cmd }
}

function global:robocopyexec([scriptblock]$cmd) {
    & $cmd | out-null
	if ($lastexitcode -eq 0) { throw "Received evil error code $lastexitcode for command: " + $cmd }
}

function global:robocopyfiltered($source, $dest) {
    robocopyexec { robocopy "$source" "$dest" /E /xf *.config.template *.cs *.csproj.* Microsoft.TeamFoundation*.dll *.cd _* /xd obj }
}

function global:execSql($script) {
    exec { osql -n -E -S $SqlServer -d $SqlDatabase -Q "`"$script`"" } "Error Executing $script"
}

function global:execSqlScript($scriptName, [string] $database = $SqlDatabase) {
    exec { osql -b -n -E -S $SqlServer -d $database -i "$baseDir\Database\$scriptName.sql" }
}

function global:regex-replace($filePath, $find, $replacement) {
    $regex = [regex] $find
    $content = [System.IO.File]::ReadAllText($filePath)
    
    Assert $regex.IsMatch($content) "Unable to find the regex '$find' to update the file '$filePath'"
    
    [System.IO.File]::WriteAllText($filePath, $regex.Replace($content, $replacement))
}

function global:build-config($configFile, [string[]]$copyFilePaths) {
    exec { &"$baseDir\Tools\Bin\ConfigFileMaker.exe" "$baseDir\Config\$configFile.template" "$baseDir\Config\$configFile" }
    foreach ($path in $copyFilePaths) {
      copy-item "$baseDir\Config\$configFile" "$baseDir\$path" -Force
    }
}









