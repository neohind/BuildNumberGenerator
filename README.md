# BuildNumberGenerator
This application is designed for Automatic building .NET Project on. JENKINS CI Build Server.
Automatic update "File Version" by Jenkis Build Number


USAGE ( by Command Line )

BuildVersionGenerator.exe "{Solution Full Path Name}" {build_number} [[rv=1] [mj=1] [mn=0]]

- {Solution Full Path Name} : Full path name of solution path. ex) D:\Works\Projects\TestApp
- {build_number} : In Jenkins, The build has any number of build number. The valus is representated by "%BUILD_NUMBER%"
- mj : [Optional] Major Number. File Version composited Major.Minor.Revision. This value is replace Major version.
- mn : [Optional] Minor Number. File Version composited Major.Minor.Revision. This value is replace Minor version.
- rv : [Optional] Revision Number. File Version composited Major.Minor.Revision. This value is replace revision version.

