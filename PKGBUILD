pkgname=sb
_pkgname=sb
pkgver=1.0.1
pkgrel=1
pkgdesc="Everyday upload"
arch=('any')
url="https://github.com/openwrt-dev/po2lmo.git"
license=('GPL3')
depends=('python-selenium' 'python')
makedepends=()
optdepends=('chrome-driver: use chrome driver'
            'chromium-driver: use chromium driver')
provides=("$_pkgname")
conflicts=("$_pkgname")
source=('sb.py'
        'sb.service'
        'sb.timer'
        'LICENSE')
sha512sums=('5a9b84bec4b623a71b0300479db31d736c23e374c211b00b99670678e44cd0856f4f542666529dbdccd99c8d1d0a70f760acb368368c8709a56851c96a18849f'
            '2a9c281bad4ba900fde81c6a66b3052a884815e765d93a89867308758a4ffdd3a0e2f6fad210bbdb137fbf6472524e376a779ad2fe0c9f0ab45d4f21d4e66e13'
            'befd610128580bf1c89eb671242d982501ded845a63ee5045369efc5963955e55d5518ab976a7f5f300a4b3a7ba33f816cb95367866d5a1f08c869e99e50e714'
            '54e8bab3fe92fc4d1bea1b6e7f40dfee34f80d2e4299483d62e5d0aea8257dc4a30eaae69a2b8acc7be5b0d5e9c4312072449ce963e2d864c26af4677b3af448')

package() {
    install -m 755 sb.py -D ${pkgdir}/var/sb/sb.py
    install -m 755 sb.service -D ${pkgdir}/lib/systemd/system/sb.service
    install -m 755 sb.timer -D ${pkgdir}/lib/systemd/system/sb.timer
    install -m 644 LICENSE -D ${pkgdir}/usr/share/licenses/sb/LICENSE

}
