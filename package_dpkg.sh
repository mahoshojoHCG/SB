rm *.deb
rm -rf package
mkdir -p package/dpkg/DEBIAN
cp PackageFile/debian/* package/dpkg/DEBIAN
cd ReportCtl
dotnet publish -c release -r linux-x64 --no-self-contained
cd ./bin/release/net5.0/linux-x64/publish
PREFIX=../../../../../../package/dpkg ./install.sh
cd ../../../../../..
cd AutoReport
dotnet publish -c release -r linux-x64 --no-self-contained
cp ./bin/release/net5.0/linux-x64/publish/*.json ../package/dpkg/usr/share/AutoReport
cd ..
dpkg-deb -b package/dpkg hit-autoreport-debian-amd64.deb