echo off

::���������� �����ϴ� ��� �Է��ϼ���
::ex) c:\....\excels
rem excelPath
set excelPath=xlsx

::cs������ ������ ���
::ex) c:\....\cs
rem csPath
set csPath=C:\Users\shlif\Documents\GitHub\MyDetectiveServer\GameServer\Table

::json������ ������ ���
::ex) c:\....\json
rem jsonPath
set jsonPath=C:\Users\shlif\Documents\GitHub\MyDetectiveServer\GameServer\bin\Debug\TableDatas

TB.exe %excelPath% %csPath% %jsonPath%

pause