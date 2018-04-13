# Build Number Generator
이 프로그램은 .NET 응용프로그램의 File Version을 일괄적으로 수정하는 프로그램이다.
특정 경로 하위에 있는 모든 AssemblyInfo.cs 파일에서 AssemblyFileInfo 값을 수정한다.
기본적으로 버전의 Build 부분만 수정하게 되어 있으나, 추가적인 옵션을 이용하여,
Major, Minor, Revision 부분도 수정할 수 있다.

프로그램의 용도는 Jenkins와 같은 빌드 서버 뿐만 아니라, Visual Studio의 External Tool에 등록하여 사용할 수 있다.
CUI 기반이기 때문에, 외부 실행형으로 적절하게 활용이 가능하다.


USAGE ( by Command Line )

Arguments
   Type#1 :  all {solution Path} [mj={n}][mn={n}][bv={n}][rv={n}]
   Type#2 :  project {solution Path} {project name} [mj={n}][mn={n}][bv={n}][rv={n}]
   Type#3 :  get {solution Path} {project name} [mj|mn|bv|rv]
   Type#4 :  getpart {solution Path} {project name} [mj|mn|bv|rv]

   all : Update all assembly, fileversion update and publish version
   project : Update specific project settings that is assembly, fileversion update and publish version.
   get : Get version by level. ex) 1.2.3.4 :  mj -> 1. / mn -> 1.2. / bv -> 1.2.3. / rv -> 1.2.3.4
   getpart : Get part version by level. ex) 1.2.3.4 :  mj -> 1 / mn -> 2 / bv -> 3 / rv -> 4
   - {solution Path} : Solution folder full path name
   - {Project Name} : Project Name. .csproj Filename
   - mj : Major Version
   - mn : Minor Version
   - bv : Build Version
   - rv : Revision Version


Type#1 : BuildVersionGenerator.exe all "C:\Projects\TestProj" mj=1 mn=0 bv=16 rv =1
Type#2 : BuildVersionGenerator.exe project "C:\Projects\TestProj" mainapp mj=1 mn=0 bv=16 rv=1
Type#2 : BuildVersionGenerator.exe get "C:\Projects\TestProj" mainapp bv
Type#2 : BuildVersionGenerator.exe getpart "C:\Projects\TestProj" Test  mainapp mj


