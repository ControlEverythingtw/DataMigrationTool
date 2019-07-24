@echo on
for /r %%f in (..\*.pdb) do del %%f
for /r %%f in (..\*.vshost.exe) do del %%f
for /r %%f in (..\*.vshost.exe.config) do del %%f
for /r %%f in (..\*.exe.manifest) do del %%f
for /r %%f in (..\logfiles) do rmdir /s /q %%f
echo clear succesful
pause