# Install script for directory: C:/ros1ws/catkin_ws/src/ACT_BODY/abel_control

# Set the install prefix
if(NOT DEFINED CMAKE_INSTALL_PREFIX)
  set(CMAKE_INSTALL_PREFIX "C:/ros1ws/catkin_ws/install")
endif()
string(REGEX REPLACE "/$" "" CMAKE_INSTALL_PREFIX "${CMAKE_INSTALL_PREFIX}")

# Set the install configuration name.
if(NOT DEFINED CMAKE_INSTALL_CONFIG_NAME)
  if(BUILD_TYPE)
    string(REGEX REPLACE "^[^A-Za-z0-9_]+" ""
           CMAKE_INSTALL_CONFIG_NAME "${BUILD_TYPE}")
  else()
    set(CMAKE_INSTALL_CONFIG_NAME "RelWithDebInfo")
  endif()
  message(STATUS "Install configuration: \"${CMAKE_INSTALL_CONFIG_NAME}\"")
endif()

# Set the component getting installed.
if(NOT CMAKE_INSTALL_COMPONENT)
  if(COMPONENT)
    message(STATUS "Install component: \"${COMPONENT}\"")
    set(CMAKE_INSTALL_COMPONENT "${COMPONENT}")
  else()
    set(CMAKE_INSTALL_COMPONENT)
  endif()
endif()

# Is this installation the result of a crosscompile?
if(NOT DEFINED CMAKE_CROSSCOMPILING)
  set(CMAKE_CROSSCOMPILING "FALSE")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  include("C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/safe_execute_install.cmake")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/pkgconfig" TYPE FILE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/abel_control.pc")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/abel_control/cmake" TYPE FILE FILES
    "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/abel_controlConfig.cmake"
    "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/abel_controlConfig-version.cmake"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/abel_control" TYPE FILE FILES "C:/ros1ws/catkin_ws/src/ACT_BODY/abel_control/package.xml")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE PROGRAM FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/AbelMove.py")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_AbelMove.py_exec_install_python/AbelMove.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  list(APPEND CMAKE_ABSOLUTE_DESTINATION_FILES
   "C:/ros1ws/catkin_ws/devel/lib/abel_control/AbelMove.exe")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
file(INSTALL DESTINATION "C:/ros1ws/catkin_ws/devel/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_AbelMove.py_exec_cip_devel_python/AbelMove.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE PROGRAM FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/main.py")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_main.py_exec_install_python/main.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  list(APPEND CMAKE_ABSOLUTE_DESTINATION_FILES
   "C:/ros1ws/catkin_ws/devel/lib/abel_control/main.exe")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
file(INSTALL DESTINATION "C:/ros1ws/catkin_ws/devel/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_main.py_exec_cip_devel_python/main.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE PROGRAM FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/MoveAbelArms.py")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_MoveAbelArms.py_exec_install_python/MoveAbelArms.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  list(APPEND CMAKE_ABSOLUTE_DESTINATION_FILES
   "C:/ros1ws/catkin_ws/devel/lib/abel_control/MoveAbelArms.exe")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
file(INSTALL DESTINATION "C:/ros1ws/catkin_ws/devel/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_MoveAbelArms.py_exec_cip_devel_python/MoveAbelArms.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE PROGRAM FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/installspace/MoveAbelNeck.py")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_MoveAbelNeck.py_exec_install_python/MoveAbelNeck.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  list(APPEND CMAKE_ABSOLUTE_DESTINATION_FILES
   "C:/ros1ws/catkin_ws/devel/lib/abel_control/MoveAbelNeck.exe")
  if(CMAKE_WARN_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(WARNING "ABSOLUTE path INSTALL DESTINATION : ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
  if(CMAKE_ERROR_ON_ABSOLUTE_INSTALL_DESTINATION)
    message(FATAL_ERROR "ABSOLUTE path INSTALL DESTINATION forbidden (by caller): ${CMAKE_ABSOLUTE_DESTINATION_FILES}")
  endif()
file(INSTALL DESTINATION "C:/ros1ws/catkin_ws/devel/lib/abel_control" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/build/ACT_BODY/abel_control/catkin_generated/windows_wrappers/abel_control_MoveAbelNeck.py_exec_cip_devel_python/MoveAbelNeck.exe")
endif()

