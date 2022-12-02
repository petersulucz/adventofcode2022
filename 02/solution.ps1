[CmdletBinding()]
param (
    [Parameter(Mandatory=$true)][string]$Path
)

function compwin($a, $b) {
    if ($a -eq $b) {
        return 0
    }
    #Write-Host "'$a' '$b'"
    if ($a -eq "ROCK") {
        if ($b -eq "PAPER") {
            return -1
        }
        return 1;
    }
    if ($a -eq "PAPER") {
        if ($b -eq "SCISSORS") {
            return -1
        }
        return 1;
    }
    if ($a -eq "SCISSORS") {
        if ($b -eq "ROCK") {
            return -1
        }
        return 1;
    }
    throw "ughhhh? $a $b"
}

$OpponentMap = @{
    "A" = "ROCK"
    "B" = "PAPER"
    "C" = "SCISSORS"
}

$scoreKey = @{
    "ROCK" = 1
    "PAPER" = 2
    "SCISSORS" = 3
}

$MyMap = @{
    "X" = "ROCK"
    "Y" = "PAPER"
    "Z" = "SCISSORS"
}

$MyMap2 = @{
    "X" = -1
    "Y" = 0
    "Z" = 1
}

$RPS = @("ROCK", "PAPER", "SCISSORS")

## First
#$results = @()
#foreach ($line in Get-Content $Path) {
#    $values = $line -split ' '
#    $you = $OpponentMap[$values[0]]
#    $me = $MyMap[$values[1]]
#
#    $score = compwin $me $you
#    $score = ($score + 1) * 3 + $scoreKey[$me]
#    $results += $score
#}
#$results | Measure-Object -Sum

## Second
$results = @()
foreach ($line in Get-Content $Path) {
    $values = $line -split ' '
    $you = $OpponentMap[$values[0]]
    $myResult = $myMap2[$values[1]]
    $me = $RPS | Where-Object { (compwin $_ $you) -eq $myResult  }

    $score = $myResult
    $score = ($score + 1) * 3 + $scoreKey[$me]
    $results += $score
    $score
}
$results | Measure-Object -Sum