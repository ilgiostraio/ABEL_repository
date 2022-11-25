===== ACE Library =====

1) Uncompress the ACE distribution into a directory, where it will create a ACE_wrappers directory containing the distribution. 
It is also a good idea to set an environment variable, for example ACE_SVN to hold the path to ACE_wrappers. In the following steps the ACE_wrappers directory will be referred to as ACE_SVN -- so ACE_SVN\ace would be your-path\ACE_wrappers\ace if you uncompressed into the root directory.

2) Create a file called config.h in the ACE_SVN\ace directory that contains: 

#include "ace/config-win32.h"

3) Now load the solution file for ACE (ACE_SVN/ACE.sln).

Make sure you are building the configuration (i.e, Debug/Release) the one you'll use. 
If you use the dynamic libraries, make sure you include ACE_SVN\lib in your PATH whenever you run programs that uses ACE. Otherwise you may experience problems finding ace.dll or aced.dll.


===== YARP =====

1) Uncompress the YARP distribution into a directory, where it will create a yarp directory containing the distribution. 
The yarp directory will be referred to as YARP_SVN in the following steps -- so YARP_SVN\src would be your-path\yarp\src if you uncompressed into the root directory.

2) Use CMake to generate a Visual Studio solution of yarp distribution. If ACE is well configured, CMake will find the right path to ACE.
Set the CMAKE_INSTALL_PREFIX to the directory of yarp.

3) Configure and Generate!

4) Open the generated Visual Studio solution and compile it both in Release and Debug mode.

