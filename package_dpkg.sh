rm -rf dpkg
mkdir -p dpkg/DEBIAN
cp PackageFile/control dpkg/DEBIAN
cd ReportCtl
dotnet publish -c release -r linux-x64 --no-self-contained
cd ./bin/release/net5.0/linux-x64/publish
PREFIX=../../../../../../dpkg ./install.sh
cd ../../../../../..
dpkg-deb -b dpkg hit-autoreport-amd64.deb