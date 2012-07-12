copy emptyfantasyjob.sdf fantasyjob.sdf
del startup\* /q

for /D %%j in (jobs\*) do RD /S /Q "%%j"



