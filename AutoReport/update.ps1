function TrimInput 
{
    $Reader = [System.IO.StreamReader]::new("CN.txt", [System.Text.Encoding]::UTF8)
    $Writer = [System.IO.StreamWriter]::new([System.IO.File]::OpenWrite("CN.new.txt"), [System.Text.Encoding]::UTF8)
    $Line = $Reader.ReadLine()
    for ($Line = $Reader.ReadLine(); -not [System.String]::IsNullOrWhiteSpace($Line); $Line = $Reader.ReadLine()) {
        $Content = $Line.Split("`t")
        if($Content[6] -ne "A")
        {
            continue
        }
        $Writer.WriteLine($Line)
    }
    $Writer.Flush()
    $Writer.Close()
    $Reader.Close()
    Remove-Item CN.txt
    Move-Item CN.new.txt CN.txt
}

if(!(Test-Path "CN.txt"))
{
    $Response = Invoke-WebRequest -Uri http://download.geonames.org/export/dump/CN.zip 
    $Stream = [System.IO.File]::OpenWrite("CN.zip")
    $Stream.Write($Response.Content, 0, $Response.Content.Length)
    $Stream.Flush()
    $Stream.Close()
    Expand-Archive -Path "CN.zip" -DestinationPath (Get-Location)
    Remove-Item CN.zip
    Remove-Item readme.txt
    TrimInput
}