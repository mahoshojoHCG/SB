rm *.tar.xz
rm -rf package
mkdir package
cd ReportCtl
dotnet publish -c release -r linux-x64
dotnet publish -c release -r win-x64
mv ./bin/release/net5.0/linux-x64/publish ../package/linux-x64
mv ./bin/release/net5.0/win-x64/publish ../package/win-x64
cd ../AutoReport
dotnet publish -c release -r win-x64
cp ./bin/release/net5.0/win-x64/publish/*.exe ../package/win-x64
cd ../package
tar -cJvf linux-x64.tar.xz linux-x64
tar -cJvf win-x64.tar.xz win-x64
mv *.tar.xz ..