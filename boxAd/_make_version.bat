@echo off

@set t=%time: =0%
@set D=%date:-=%
@set T=%t:~0,2%%t:~3,2%

@set VERSION=%D%.%T%
echo {"version":"%VERSION%"} > version.json
