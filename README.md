# Build Number Generator
�� ���α׷��� .NET �������α׷��� File Version�� �ϰ������� �����ϴ� ���α׷��̴�.
Ư�� ��� ������ �ִ� ��� AssemblyInfo.cs ���Ͽ��� AssemblyFileInfo ���� �����Ѵ�.
�⺻������ ������ Build �κи� �����ϰ� �Ǿ� ������, �߰����� �ɼ��� �̿��Ͽ�,
Major, Minor, Revision �κе� ������ �� �ִ�.

���α׷��� �뵵�� Jenkins�� ���� ���� ���� �Ӹ� �ƴ϶�, Visual Studio�� External Tool�� ����Ͽ� ����� �� �ִ�.
CUI ����̱� ������, �ܺ� ���������� �����ϰ� Ȱ���� �����ϴ�.


USAGE ( by Command Line )

BuildVersionGenerator.exe "{Solution Full Path Name}" {build_number} [[rv=1] [mj=1] [mn=0]]

- {Solution Full Path Name} : Full path name of solution path. ex) D:\Works\Projects\TestApp
- {build_number} : In Jenkins, The build has any number of build number. The valus is representated by "%BUILD_NUMBER%"
- mj : [Optional] Major Number. File Version composited Major.Minor.Revision. This value is replace Major version.
- mn : [Optional] Minor Number. File Version composited Major.Minor.Revision. This value is replace Minor version.
- rv : [Optional] Revision Number. File Version composited Major.Minor.Revision. This value is replace revision version.

In Visual Studio External Tool Option
 - Title : Build Version Generator
 - Command : {applocaltion path : BuildVersionGenerator ���� ��ġ}\BuildVersionGenerator.exe
 - Aguments : "$(SolutionDir)" 0 mj=1 mn=0 rv=0
 - Inital Directory : $(SolutionDir)
 - Check to Prompt arguments and Close on exit 



