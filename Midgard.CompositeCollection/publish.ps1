$build = $env:windir+'\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe'
&$build .\Midgard.CompositeCollection.csproj /p:Configuration=Release|AnyCPU 
NuGet.exe update -self
NuGet.exe pack .\Midgard.CompositeCollection.csproj -Symbols -Prop Configuration=Release
NuGet.exe push .\Midgard.CompositeCollection.1.0.0.0.nupkg
NuGet.exe push .\Midgard.CompositeCollection.1.0.0.0.symbols.nupkg