[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][string]$Path
)

$sums = @();
$rolling = 0
foreach ($line in Get-Content $Path) {
    if ($line) {
        $rolling += [int]::Parse($line)
    } else {
        $sums += $rolling
        $rolling = 0
    }
}
$sums += $rolling

#First
$sums | Sort-Object -Descending | select -First 1
#Second
$sums | Sort-Object -Descending | select -First 3 | Measure-Object -Sum