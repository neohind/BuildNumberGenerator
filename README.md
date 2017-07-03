# Build Number Generator
이 프로그램은 .NET 응용프로그램의 File Version을 일괄적으로 수정하는 프로그램이다.
특정 경로 하위에 있는 모든 AssemblyInfo.cs 파일에서 AssemblyFileInfo 값을 수정한다.
기본적으로 버전의 Build 부분만 수정하게 되어 있으나, 추가적인 옵션을 이용하여,
Major, Minor, Revision 부분도 수정할 수 있다.

프로그램의 용도는 Jenkins와 같은 빌드 서버 뿐만 아니라, Visual Studio의 External Tool에 등록하여 사용할 수 있다.
CUI 기반이기 때문에, 외부 실행형으로 적절하게 활용이 가능하다.


USAGE ( by Command Line )

BuildVersionGenerator.exe "{Solution Full Path Name}" {build_number} [[rv=1] [mj=1] [mn=0]]

- {Solution Full Path Name} : Full path name of solution path. ex) D:\Works\Projects\TestApp
- {build_number} : In Jenkins, The build has any number of build number. The valus is representated by "%BUILD_NUMBER%"
- mj : [Optional] Major Number. File Version composited Major.Minor.Revision. This value is replace Major version.
- mn : [Optional] Minor Number. File Version composited Major.Minor.Revision. This value is replace Minor version.
- rv : [Optional] Revision Number. File Version composited Major.Minor.Revision. This value is replace revision version.

In Visual Studio External Tool Option
 - Title : Build Version Generator
 - Command : {applocaltion path : BuildVersionGenerator 파일 위치}\BuildVersionGenerator.exe
 - Aguments : "$(SolutionDir)" 0 mj=1 mn=0 rv=0
 - Inital Directory : $(SolutionDir)
 - Check to Prompt arguments and Close on exit 



