# SNOEC_GUI #

Debug tooling for optical module. In the current, it supports QSFP28 SR4 product. Please see [USBtoI2C](https://github.com/tclxspy/USB_I2C_73) repository for the firmware code. 

## Summarize ##

Each tab is a part function operation. See below:

- Ch ON/Off --> enable/disable Tx and Rx
- DMI/ADC --> data monitor
- Alarm/Warning --> data monitor interrupt flag
- I2C Read/Write --> read and write register

![](http://i.imgur.com/ReyyWa2.jpg)

## Read monitor interrupt flag ##

The interrupt shows module's status: Tx/Rx loss, high/low VCC, etc. 

- high/low alarm
- high/low warning

![](http://i.imgur.com/LAdKeSy.jpg)

## Read and write register ##

Fist to set device address 0xA0 and then choose register address.

![](http://i.imgur.com/6dJSbiW.jpg)

## Note: ##

1. 由于USB虚拟串口一次性只能穿不超过64个字节的数据，加上前面8个字节为格式字节，所以一次性传输数据为不超过56个字节。当然可以分包传送更多字节，由上位机实现，如SNOEC_GUI。