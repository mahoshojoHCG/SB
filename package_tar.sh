rm *.tar.xz
rm -rf package
mkdir package

#Build self-contained
cd ReportCtl
dotnet publish -c release -r linux-x64
dotnet publish -c release -r win-x64
mv ./bin/release/net5.0/linux-x64/publish ../package/linux-x64-self-contained
mv ./bin/release/net5.0/win-x64/publish ../package/win-x64-self-contained
cd ../AutoReport
dotnet publish -c release -r win-x64
cp ./bin/release/net5.0/win-x64/publish/*.exe ../package/win-x64-self-contained

#Build no-self-contained
cd ../ReportCtl
dotnet publish -c release -r linux-x64 --no-self-contained
dotnet publish -c release -r win-x64 --no-self-contained
mv ./bin/release/net5.0/linux-x64/publish ../package/linux-x64-no-self-contained
mv ./bin/release/net5.0/win-x64/publish ../package/win-x64-no-self-contained
cd ../AutoReport
dotnet publish -c release -r win-x64
cp ./bin/release/net5.0/win-x64/publish/*.exe ../package/win-x64-no-self-contained

cd ../package
tar -cJvf hit-autoreport-linux-x64-self-contained.tar.xz linux-x64-self-contained
tar -cJvf hit-autoreport-win-x64-self-contained.tar.xz win-x64-self-contained
tar -cJvf hit-autoreport-linux-x64-self-no-contained.tar.xz linux-x64-no-self-contained
tar -cJvf hit-autoreport-win-x64-self-no-contained.tar.xz win-x64-no-self-contained
mv *.tar.xz ..