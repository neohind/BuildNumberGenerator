# Build Number Generator
�� ���α׷��� .NET �������α׷��� File Version�� �ϰ������� �����ϴ� ���α׷��̴�.
Ư�� ��� ������ �ִ� ��� AssemblyInfo.cs ���Ͽ��� AssemblyFileInfo ���� �����Ѵ�.
�⺻������ ������ Build �κи� �����ϰ� �Ǿ� ������, �߰����� �ɼ��� �̿��Ͽ�,
Major, Minor, Revision �κе� ������ �� �ִ�.

���α׷��� �뵵�� Jenkins�� ���� ���� ���� �Ӹ� �ƴ϶�, Visual Studio�� External Tool�� ����Ͽ� ����� �� �ִ�.
CUI ����̱� ������, �ܺ� ���������� �����ϰ� Ȱ���� �����ϴ�.


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


