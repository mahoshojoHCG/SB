#!/bin/sh

install -m 755 sb.py -D /var/sb
install -m 755 sb.service -D /lib/systemd/system
install -m 755 sb.timer -D /lib/systemd/system