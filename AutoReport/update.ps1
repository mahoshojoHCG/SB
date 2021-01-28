if(!(Test-Path "CN.txt"))
{
    $Response = Invoke-WebRequest -Uri http://download.geonames.org/export/dump/CN.zip 
    $Stream = [System.IO.File]::OpenWrite("CN.zip")
    $Stream.Write($Response.Content, 0, $Response.Content.Length)
    $Stream.Flush()
    $Stream.Close()
    Expand-Archive -Path "CN.zip" -DestinationPath (Get-Location)
}
