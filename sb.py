#!/usr/bin/env python3
# -*- coding:utf-8 -*-
#@Time  : 2020/2/18 18:42
#@Author: f
#@File  : main.py

# Copyright (c) 2020 Qiqi

# Permission is hereby granted, free of charge, to any person obtaining a copy
# of this software and associated documentation files (the "Software"), to deal
# in the Software without restriction, including without limitation the rights
# to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
# copies of the Software, and to permit persons to whom the Software is
# furnished to do so, subject to the following conditions:

# The above copyright notice and this permission notice shall be included in all
# copies or substantial portions of the Software.

# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
# SOFTWARE.


from selenium import webdriver
import time

from selenium.common.exceptions import UnexpectedAlertPresentException, NoSuchElementException
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.chrome.options import Options
import sys

class Report(object):
    def __init__(self):

        self.log("开始上报")
        self.base_url = 'http://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xs/yqxx'
        chrome_options = Options()
        chrome_options.add_argument('--headless')  # 16年之后，chrome给出的解决办法，抢了PhantomJS饭碗
        chrome_options.add_argument('--disable-gpu')
        chrome_options.add_argument('--no-sandbox')  # root用户不加这条会无法运行
        self.driver = webdriver.Chrome(chrome_options=chrome_options)
        self.log("调用chrome")
        while 'https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xsHome' != self.driver.current_url:
            self.log("尝试登陆")
            try:
                self.login()
            except Exception as e:
                print(str(e))
                Report.log(str(e))
                self.driver.refresh()
            time.sleep(0.5)
        self.Report()

    def login(self):
        self.load_url('http://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/shsj/loginChange')
        time.sleep(0.5)
        if 'http://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xsHome' == self.driver.current_url:
            return
        #self.driver.execute_script("tongyishenfen()")
        self.driver.find_element_by_xpath("/html/body/div/div[2]/button[1]").click()
        time.sleep(0.5)
        self.driver.find_element_by_xpath("/html/body/div[2]/div[2]/div[2]/div/div[3]/div/form/p[1]/input").send_keys(id)
        self.driver.find_element_by_xpath("/html/body/div[2]/div[2]/div[2]/div/div[3]/div/form/p[2]/input[1]").send_keys(Password)
        self.driver.find_element_by_xpath("/html/body/div[2]/div[2]/div[2]/div/div[3]/div/form/p[2]/input[1]"). send_keys(Keys.ENTER)
        time.sleep(0.5)
        if 'https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xsHome' == self.driver.current_url:
            self.log("登陆成功")
        else:
            self.log("登陆失败")
        return


    def Report(self):
        self.load_url('https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xs/yqxx')
        self.log("尝试首次载入上报界面")
        while 'https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xs/yqxx' != self.driver.current_url:
            self.load_url('https://xg.hit.edu.cn/zhxy-xgzs/xg_mobile/xs/yqxx')
            self.log("再次尝试载入上报界面")
            time.sleep(1)
        self.driver.execute_script("add()")
        self.log("添加新的上报")
        while True:
                time.sleep(0.5)
                print("检测checkbox勾选状况")
                try:
                    if self.driver.find_element_by_id("txfscheckbox"):
                        self.driver.find_element_by_id("txfscheckbox").click()
                        self.log("勾选checkbox")
                        break
                except UnexpectedAlertPresentException:
                    self.log("检测到今日已生成疫情上报")
                    self.log("------------")
                    sys.exit(0)

                except NoSuchElementException:
                    self.log("检测到今日已生成疫情上报")
                    self.log("------------")
                    sys.exit(0)


        self.driver.execute_script("save()")
        self.log("上报成功")
        self.log("------------")
        return

    def load_url(self, url):
        self.driver.get(url)
        self.driver.implicitly_wait(5)

    def log(self,msg):
        print(id+" "+msg)
        f.write(id)
        f.write(time.strftime(" %Y-%m-%d %H:%M:%S ", time.localtime(time.time())))
        f.write(msg+"\n")



if __name__ == '__main__':
    id=""
    Password=""
    f = open('log', 'a')
    if len(sys.argv)==3:
        id=sys.argv[1]
        Password=sys.argv[2]
        r = Report()
        sys.exit(0)
    try:
        file=open('./账户密码.txt', 'r')
        id = file.readline()
        Password = file.readline()
    except FileNotFoundError:
        print("未检测到缓存账户密码，您可以尝试在可执行文件同目录下创建 账户密码.txt 在本地保存您的账户密码以实现自动化登陆\n"
              "尝试手动登陆统一身份认证")
    except PermissionError:
        print("权限不足，尝试使用管理员权限进行访问")

    try:
        r=Report()
    except Exception as e:
        print(str(e))
        Report.log(str(e))
