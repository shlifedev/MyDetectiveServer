echo off

::���������� �����ϴ� ��� �Է��ϼ���
::ex) c:\....\excels
rem excelPath
set excelPath=xlsx

::cs������ ������ ���
::ex) c:\....\cs
rem csPath
set csPath=..\PIDL\Table

::json������ ������ ���
::ex) c:\....\json
rem jsonPath
set jsonPath=..\GameServer\bin\Debug\TableDatas
TB.exe %excelPath% %csPath% %jsonPath%

echo off
call AutoCopy_ToUnity.bat
call AutoCopy_ServerSync.bat
pause