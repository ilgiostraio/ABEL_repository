execute_process(COMMAND "C:/ros1ws/catkin_ws/build/DynamixelSDK/ros/dynamixel_sdk/catkin_generated/python_distutils_install.bat" RESULT_VARIABLE res)

if(NOT res EQUAL 0)
  message(FATAL_ERROR "execute_process(C:/ros1ws/catkin_ws/build/DynamixelSDK/ros/dynamixel_sdk/catkin_generated/python_distutils_install.bat) returned error code ")
endif()
