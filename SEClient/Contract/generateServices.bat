call "%VS110COMNTOOLS%vsvars32.bat"

@pushd %cd%

@rem %~dp0 will give you the full path to the batch file (fixed)
cd %~dp0

rem xsd.exe results_labx.xsd /classes /language:cs /n:MT.pHLab.SE.V1 
xsd.exe results_labxdirect.xsd /classes /n:MT.pHLab.SE.V1 


@popd

:Exit