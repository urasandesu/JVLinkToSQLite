# JVLinkToSQLite は、JRA-VAN データラボが提供する競馬データを SQLite データベースに変換するツールです。
# 
# Copyright (C) 2023 Akira Sugiura
# 
# This program is free software: you can redistribute it and/or modify
# it under the terms of the GNU General Public License as published by
# the Free Software Foundation, either version 3 of the License, or
# (at your option) any later version.
# 
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
# 
# You should have received a copy of the GNU General Public License
# along with this program.  If not, see <https://www.gnu.org/licenses/>.
# 
# Additional permission under GNU GPL version 3 section 7
# 
# If you modify this Program, or any covered work, by linking or combining it with 
# ObscUra (or a modified version of that library), containing parts covered 
# by the terms of ObscUra's license, the licensors of this Program grant you 
# additional permission to convey the resulting work.

[CmdletBinding(DefaultParametersetName = 'Package')]
param (
    [Parameter(ParameterSetName = 'Package')]
    [Switch]
    $Package, 

    [ValidateSet("", "Clean", "Rebuild")] 
    [string]
    $BuildTarget, 

    [string]
    $AuthorId, 

    [string]
    $SoftwareId,

    [string]
    $PublicKey
)

trap {
    Write-Error ($Error[0] | Out-String)
    exit -1
}

try {
    msbuild /ver | Out-Null
} catch [System.Management.Automation.CommandNotFoundException] {
    Write-Error "このビルド スクリプトは、Developer Command Prompt for VS 2022 を管理者権限で実行し、その上で動作させる必要があります。"
    exit -767922108
}


try {
    nuget | Out-Null
} catch [System.Management.Automation.CommandNotFoundException] {
    Write-Error "このビルド スクリプトの実行には、予め NuGet コマンド ライン インターフェイス (CLI) をインストールしておく必要があります。"
    exit 1452479952
}


try {
    7z | Out-Null
} catch [System.Management.Automation.CommandNotFoundException] {
    Write-Error "このビルド スクリプトの実行には、予め 7-Zip をインストールしておく必要があります。"
    exit 594712101
}


if (![string]::IsNullOrEmpty($BuildTarget)) {
    $buildTarget_ = ":" + $BuildTarget
}

if (![string]::IsNullOrEmpty($AuthorId) -and ![string]::IsNullOrEmpty($SoftwareId) -and ![string]::IsNullOrEmpty($PublicKey)) {
    $env:AuthorId = $AuthorId
    $env:SoftwareId = $SoftwareId
    $env:PublicKey = $PublicKey
} elseif (![string]::IsNullOrEmpty($AuthorId) -or ![string]::IsNullOrEmpty($SoftwareId) -or ![string]::IsNullOrEmpty($PublicKey)) {
    Write-Error "AuthorId, SoftwareId, PublicKey の指定をする場合、いずれかだけでなく、全ての値を設定する必要があります。"
    exit 1602304247
} else {
    $env:AuthorId = $null
    $env:SoftwareId = $null
    $env:PublicKey = $null
}

switch ($PsCmdlet.ParameterSetName) {
    'Package' { 
        $solution = "JVLinkToSQLite.sln"
        nuget restore $solution

        $target = "/t:JVLinkToSQLite$buildTarget_;Urasandesu_JVLinkToSQLite$buildTarget_;Test_Urasandesu_JVLinkToSQLite$buildTarget_;ObfuscatedResources$buildTarget_;Urasandesu_JVLinkToSQLite_Basis$buildTarget_;Test_Urasandesu_JVLinkToSQLite_Basis$buildTarget_;Test_Urasandesu_JVLinkToSQLite_JVData$buildTarget_"
        $configurations = "/p:Configuration=Release"
        foreach ($configuration in $configurations) {
            Write-Verbose ("Solution: {0}" -f $solution)
            Write-Verbose ("Configuration: {0}" -f $configuration)
            msbuild $solution $target $configuration /m
            if ($LASTEXITCODE -ne 0) {
                exit $LASTEXITCODE
            }
        }

        $artifact = ".\work\JVLinkToSQLiteArtifact.exe"
        if ([System.IO.File]::Exists($artifact)) {
            Remove-Item $artifact -Force
        }
        if ('Clean' -ne $BuildTarget) {
            7z a "-r" "-x!*.pdb" "-mmt" "-mx5" "-sfx" $artifact .\JVLinkToSQLite\bin\Release
            7z rn $artifact Release JVLinkToSQLiteArtifact
        }
    }
}
