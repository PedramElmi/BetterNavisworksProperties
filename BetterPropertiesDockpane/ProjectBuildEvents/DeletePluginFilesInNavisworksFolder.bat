set addinPath=C:\Program Files\Autodesk\Navisworks Manage 2022\Plugins\BetterPropertiesDockpane

if exist "%addinPath%" ( del "%addinPath%\*.*" /q /s )

pause