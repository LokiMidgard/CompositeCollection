$build = "C:\Program Files (x86)\MSBuild\12.0\Bin\msbuild.exe"
&$build .\Midgard.CompositeCollection.csproj /p:Configuration=Release 
NuGet.exe update -self
NuGet.exe pack .\Midgard.CompositeCollection.csproj -Symbols -Prop Configuration=Release
NuGet.exe push .\Midgard.CompositeCollection.*.nupkg
