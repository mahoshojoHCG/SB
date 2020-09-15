#!/bin/sh

install -m 755 sb.py -D /var/sb/sb.py
install -m 755 sb.service -D /lib/systemd/system/sb.service
install -m 755 sb.timer -D /lib/systemd/system/sb.timer