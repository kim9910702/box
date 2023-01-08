@echo off
@call _make_version.bat
@call quasar build
@call _encode.bat
