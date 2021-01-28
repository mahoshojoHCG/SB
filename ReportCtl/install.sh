if [ ! "$PREFIX"];
then
PREFIX="/"
fi

install -Dvdm755 . $PREFIX/usr/share/AutoReport
install -Dvm655 *.dll $PREFIX/usr/share/AutoReport
install -Dvm655 *.a $PREFIX/usr/share/AutoReport
install -Dvm655 *.so $PREFIX/usr/share/AutoReport
install -Dvm655 *.json $PREFIX/usr/share/AutoReport
install -Dvm655 CN.txt $PREFIX/usr/share/AutoReport
install -Dvm755 AutoReport $PREFIX/usr/share/AutoReport
install -Dvm755 ReportCtl $PREFIX/usr/share/AutoReport
install -Dvm755 createdump $PREFIX/usr/share/AutoReport
install -Dvm755 AutoReport.service $PREFIX/lib/systemd/system
install -Dvm600 config.yml $PREFIX/etc/AutoReport
ln -s $PREFIX/usr/share/AutoReport/AutoReport $PREFIX/usr/bin/autoreport
ln -s $PREFIX/usr/share/AutoReport/ReportCtl $PREFIX/usr/bin/reportctl

echo Install Completed