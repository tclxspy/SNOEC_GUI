# SNOEC_GUI #

Debug tooling for optical module. In the current, it supports QSFP28 SR4 product. Please see [USBtoI2C](https://github.com/tclxspy/USB_I2C_73) repository for the firmware code. 

## Summarize ##

Each tab is a part function operation. See below:

- Ch ON/Off --> enable/disable Tx and Rx
- DMI/ADC --> data monitor
- Alarm/Warning --> data monitor interrupt flag
- I2C Read/Write --> read and write register

![](https://i.imgur.com/TlEhZHQ.jpg)


## Calculate ##

ADC function, DEC to IEEE753/Int16, calculate DMI.    

	formula: Y=X*Slope/2^Shift+Offset.

![](https://i.imgur.com/mVGu31l.jpg)

## Read monitor interrupt flag ##

The interrupt shows module's status: Tx/Rx loss, high/low VCC, etc. 

- high/low alarm
- high/low warning

![](https://i.imgur.com/qrIqpQz.jpg)

## Read and write register ##

Fist to set device address 0xA0 and then choose register address.

![](https://i.imgur.com/KotfF4J.jpg)

## I2C Test ##

Test the reliability of I2C communication.

![](https://i.imgur.com/V1HdWFe.jpg)

## Note: ##

1. USB协议说全速通讯BULK最大包是64Byte。所以USB虚拟串口一次性只能穿不超过64个字节的数据，加上前面8个字节为格式字节，所以一次性传输数据为不超过56个字节。当然可以分包传送更多字节，由上位机实现，如SNOEC_GUI。