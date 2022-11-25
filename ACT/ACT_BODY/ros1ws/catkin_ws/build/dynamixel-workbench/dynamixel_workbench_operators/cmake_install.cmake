# Install script for directory: C:/ros1ws/catkin_ws/src/dynamixel-workbench/dynamixel_workbench_operators

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
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/pkgconfig" TYPE FILE FILES "C:/ros1ws/catkin_ws/build/dynamixel-workbench/dynamixel_workbench_operators/catkin_generated/installspace/dynamixel_workbench_operators.pc")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/dynamixel_workbench_operators/cmake" TYPE FILE FILES
    "C:/ros1ws/catkin_ws/build/dynamixel-workbench/dynamixel_workbench_operators/catkin_generated/installspace/dynamixel_workbench_operatorsConfig.cmake"
    "C:/ros1ws/catkin_ws/build/dynamixel-workbench/dynamixel_workbench_operators/catkin_generated/installspace/dynamixel_workbench_operatorsConfig-version.cmake"
    )
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/dynamixel_workbench_operators" TYPE FILE FILES "C:/ros1ws/catkin_ws/src/dynamixel-workbench/dynamixel_workbench_operators/package.xml")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/dynamixel_workbench_operators" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/devel/lib/dynamixel_workbench_operators/joint_operator.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/lib/dynamixel_workbench_operators" TYPE EXECUTABLE FILES "C:/ros1ws/catkin_ws/devel/lib/dynamixel_workbench_operators/wheel_operator.exe")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/include/dynamixel_workbench_operators" TYPE DIRECTORY FILES "C:/ros1ws/catkin_ws/src/dynamixel-workbench/dynamixel_workbench_operators/include/dynamixel_workbench_operators/")
endif()

if("x${CMAKE_INSTALL_COMPONENT}x" STREQUAL "xUnspecifiedx" OR NOT CMAKE_INSTALL_COMPONENT)
  file(INSTALL DESTINATION "${CMAKE_INSTALL_PREFIX}/share/dynamixel_workbench_operators" TYPE DIRECTORY FILES
    "C:/ros1ws/catkin_ws/src/dynamixel-workbench/dynamixel_workbench_operators/launch"
    "C:/ros1ws/catkin_ws/src/dynamixel-workbench/dynamixel_workbench_operators/config"
    )
endif()

