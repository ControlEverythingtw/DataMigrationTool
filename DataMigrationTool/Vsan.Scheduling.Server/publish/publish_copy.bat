pause
echo 创建发布目录
rd /s /q D:\Gwy_Publish\services\Vsan.Scheduling.Server
md D:\Gwy_Publish\services\Vsan.Scheduling.Server

echo 开始发布
xcopy /e ..\*.* D:\Gwy_Publish\services\Vsan.Scheduling.Server
echo 发布成功
pause