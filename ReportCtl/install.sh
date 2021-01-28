if [-z $PREFIX];
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
install -Dvm755 AutoReport.service $PREFIX/lib/systemd/system
install -Dvm600 config.yml $PREFIX/etc/AutoReport
ln -s $PREFIX/usr/bin/autoreport $PREFIX/usr/share/AutoReport/AutoReport
ln -s $PREFIX/usr/bin/reportctl $PREFIX/usr/share/AutoReport/ReportCtl

echo Install Completed