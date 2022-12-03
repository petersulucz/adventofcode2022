[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][string]$Path
)

$priorities = "_abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"

function processLine([string]$line){
    $one = $line.Substring(0, $line.Length / 2).ToCharArray()
    $two = $line.Substring($one.Length).ToCharArray()

    $intersection = $one | ? { $two -ccontains $_ } | select -First 1
    return $intersection
}

#
#   One
#
#$sum = 0
#foreach ($line in Get-Content $Path) {
#    $item = processLine $line
#    $sum += $priorities.IndexOf($item)
#}
#

function processMultiLine([string[]]$lines) {
    $intersection = $lines[0].ToCharArray() | ? { $lines[1].ToCharArray() -ccontains $_ }
    for($i = 2; $i -lt $lines.Length; $i++) {
        $intersection = $lines[$i].ToCharArray() | ? { $intersection -ccontains $_ }
    }
    return $intersection | select -First 1
}

#
#   Two
#
$sum = 0
$lines=@()
foreach ($line in Get-Content $Path) {
    $lines += $line
    if ($lines.Length -lt 3) {
        continue
    }

    $item = processMultiLine $lines
    $sum += $priorities.IndexOf($item)
    $lines = @()
}
$sum