@echo off

@set t=%time: =0%
@set D=%date:-=%
@set T=%t:~0,2%%t:~3,2%

@set SPA_VER=GBM_FRONT
@set SPA=spa
@set SPA_ENCODE=%SPA%_encode
@set SPA_ORIGIN=%SPA%_origin_%SPA_VER%_%D%_%T%

@set ROOT=.\dist

@set N_SRC=%ROOT%\%SPA%
@set C_SRC=%ROOT%\%SPA%_
@set O_SRC=%ROOT%\%SPA_ORIGIN%
@set E_SRC=.%ROOT%\%SPA_ENCODE%

@set BUILD=%ROOT%\%SPA%_build_%SPA_VER%_%D%_%T%

if exist %O_SRC% (@rmdir /S /Q %O_SRC% > NULL)
if exist %E_SRC% (@rmdir /S /Q %E_SRC% > NULL)
if exist %BUILD% (@rmdir /S /Q %BUILD% > NULL)

for /d %%G in ("%C_SRC%*") do rmdir /S /Q "%%~G"

@echo ---------------------------------------------------------------------
@echo [ Web Builder ]
@echo ---------------------------------------------------------------------
@call javascript-obfuscator %N_SRC% --output %E_SRC% --config javascript-obfuscator.json

if %ERRORLEVEL% neq 0 goto _BUILD_FAIL
if not exist %E_SRC% goto _ERROR
goto _COPY

:_BUILD_FAIL
@echo ---------------------------------------------------------------------
@echo * ERROR: [javascript-obfuscator] failed.
@echo   npm install -g javascript-obfuscator
@echo ---------------------------------------------------------------------
goto _END

:_ERROR
@echo ---------------------------------------------------------------------
@echo * ERROR: [%E_SRC%] not found.
@echo ---------------------------------------------------------------------
goto _END

:_COPY
@echo ---------------------------------------------------------------------
@xcopy %N_SRC%\*.* %BUILD%\*.* /S /Q /R /Y
@xcopy %E_SRC%\*.* %BUILD%\*.* /S /Q /R /Y

@ren %N_SRC% %SPA_ORIGIN%
@xcopy %BUILD%\*.* %N_SRC%\*.* /S /Q /R /Y
if exist %E_SRC% (@rmdir /S /Q %E_SRC%)

@echo ---------------------------------------------------------------------
@echo * OK: [%BUILD%]
@echo ---------------------------------------------------------------------

:_END
if exist NULL ( @del /F /Q NULL )
