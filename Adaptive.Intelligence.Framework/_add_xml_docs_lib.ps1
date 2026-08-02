$projectRoot = '.\Adaptive.Intelligence.Framework'
$files = Get-ChildItem $projectRoot -Recurse -Filter *.cs | Where-Object { $_.FullName -notmatch '\\(bin|obj)\\' }

function Get-MemberName([string]$line){
	$trim = $line.Trim()
	if($trim -match '\b(class|struct|interface|enum|record)\s+([A-Za-z_][A-Za-z0-9_]*)'){ return $matches[2] }
	if($trim -match '\b([A-Za-z_][A-Za-z0-9_]*)\s*\('){ return $matches[1] }
	if($trim -match '\b([A-Za-z_][A-Za-z0-9_]*)\s*\{'){ return $matches[1] }
	if($trim -match '\b([A-Za-z_][A-Za-z0-9_]*)\s*(=|;)'){ return $matches[1] }
	return 'member'
}

$declPatterns = @(
	'^\s*(public|private|internal|protected)\s+((sealed|static|abstract|partial|readonly|unsafe|new)\s+)*(class|struct|interface|enum|record)\s+\w+',
	'^\s*(public|private|internal|protected)\s+((static|virtual|override|abstract|sealed|readonly|unsafe|extern|new|partial|async)\s+)*[\w<>,\?\[\]\s\.]+\s+\w+\s*\(',
	'^\s*(public|private|internal|protected)\s+((static|virtual|override|abstract|sealed|new|required|init)\s+)*[\w<>,\?\[\]\s\.]+\s+\w+\s*\{',
	'^\s*(public|private|internal|protected)\s+((const|static|readonly)\s+)*[\w<>,\?\[\]\s\.]+\s+\w+\s*(=|;)'
)

$changedFiles = 0
$insertedCount = 0

foreach($file in $files){
	$lines = [System.Collections.Generic.List[string]]::new()
	$lines.AddRange([string[]](Get-Content $file.FullName))
	$output = [System.Collections.Generic.List[string]]::new()

	for($i=0; $i -lt $lines.Count; $i++){
		$line = $lines[$i]
		$isDecl = $false
		foreach($rx in $declPatterns){
			if($line -match $rx){ $isDecl = $true; break }
		}

		if($isDecl){
			$trim = $line.Trim()
			if($trim -match '^(public|private|internal|protected)\s+(if|for|foreach|while|switch|return|throw)\b'){
				$isDecl = $false
			}
		}

		if($isDecl){
			$j = $output.Count - 1
			while($j -ge 0 -and [string]::IsNullOrWhiteSpace($output[$j])){ $j-- }
			$hasXml = $false
			if($j -ge 0 -and $output[$j].TrimStart().StartsWith('///')){ $hasXml = $true }

			if(-not $hasXml){
				$indent = ($line -replace '^(\s*).*$','$1')
				$name = Get-MemberName $line
				$output.Add("$indent/// <summary>")
				$output.Add("$indent/// Gets the definition for $name.")
				$output.Add("$indent/// </summary>")
				$insertedCount++
			}
		}

		$output.Add($line)
	}

	$newContent = ($output -join [Environment]::NewLine)
	$origContent = Get-Content $file.FullName -Raw
	if($newContent -ne $origContent){
		Set-Content -Path $file.FullName -Value $newContent -NoNewline
		$changedFiles++
	}
}

"Library files changed: $changedFiles"
"XML summary blocks inserted: $insertedCount"
