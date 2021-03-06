# 上报 全新版

**本程序适用于想用电脑进行上报，但是电脑无法使用定位服务的同学。**

**本程序仅提供上报功能，请确认自己填写位置经纬度的正确性，否则后果自负。**

**请熟知：**

> - 全国中高风险地区
> - 假期期间做好个人防护
> - 个人及共同居住人有疫情相关情况的，及时向学校报告

## 安装

- 与之前一样，请安装`chromium-driver`或者`chrome-driver`，其他的倒不用了，Windows就[下载](https://chromedriver.chromium.org/downloads)之后放在文件夹里就可以了。

- 下载最新的Release

- 解压

- 打开解压的文件夹并运行安装脚本（仅适用于Linux）

  ``` shell
  sudo ./install.sh
  ```

  额外地，如果您使用基于Debian的发行版，可以使用dpkg安装

  ``` shell
  sudo dpkg -i hit-autoreport-amd64.deb
  ```

- 启用服务（仅适用于Linux）

  ``` shell 
  sudo systemctl enable AutoReport.service --now
  ```

  如果使用包管理安装，则无需执行此步骤。

- 启动主程序（仅适用于Windows）

  Windows还未作做成服务形式，暂时请直接双击启动。

- 获取你的位置

  如果你使用iOS，你可以打开指南针，就能看到你所在位置的经纬度。如果你使用Android或者鸿蒙OS，请下载GPS或北斗定位系统相关工具，也能看到你的经纬度。

- 添加账户信息

  ``` shell
  sudo reportctl --add --name 你的账号
  ```

  *Windows同理，但无需输入sudo（下同）*

  然后按照提示输入经纬度和密码，你的密码**没有**被明文储存（不知道比之前的高到哪里去了）。注意经纬度请按照度/分/秒的格式输入，由于我国在东北半球，就没有负数的输入。

- 测试上报

  ``` shell
  sudo reportctl --report --name 你的账号
  ```

  运行这条命令会进行一次一次性上报，以测试你的配置是否正确。

然后就大功告成了，如果你要删除账号，请运行

``` shell
sudo reportctl --remove --name 你的账号
```

这样就不会自动运行。

默认上报时间是九点（**系统时间，不一定是北京时间**），如果要修改，请修改`/etc/AutoReport/config.yml`。

## 卸载

使用dpkg卸载，或者看着`install.sh`里的删。

## 编译

除了基本的东西，请安装`Powershell Core`。

## 感谢

[GeoNames](http://www.geonames.org/) ~~这样大家就不会因为不会英语输反经纬度了呢~~

[xrervip](https://github.com/xrervip) 初版逻辑

