# AutoNET - AutoCAD .NET Extensions Library

A simple Libray that is used to extend AutoCAD with commands and functions that I think it is lacking.

This Library currently only targets the AutoCAD 2018 .NET API

## How to Install
Currently there is no releases for this library so the only way for you to install it is to clone this repo, build the 
solution and then use the `NETLOAD` command to reference the `AutoNet.dll` file within AutoCAD.

## Functions
- **AutoLength**: A command that will return the total length of all lines and polylines that are selected