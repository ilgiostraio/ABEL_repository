@echo off

if DEFINED DESTDIR (
  echo "Destdir.............%DESTDIR%"
  set DESTDIR_ARG="--root=%DESTDIR%"
)

cd "C:/ros1ws/catkin_ws/src/DynamixelSDK/ros/dynamixel_sdk"

if NOT EXIST "C:\ros1ws\catkin_ws\install\lib/site-packages\" (
  mkdir "C:\ros1ws\catkin_ws\install\lib/site-packages"
)

set "PYTHONPATH=C:\ros1ws\catkin_ws\install\lib/site-packages;C:/ros1ws/catkin_ws/build\lib/site-packages"
set "CATKIN_BINARY_DIR=C:/ros1ws/catkin_ws/build"
for /f "usebackq tokens=*" %%a in ('C:\ros1ws\catkin_ws\install') do (
  set _SETUPTOOLS_INSTALL_PATH=%%~pna
  set _SETUPTOOLS_INSTALL_ROOT=%%~da
)

"C:/opt/ros/noetic/x64/python.exe" ^
    "C:/ros1ws/catkin_ws/src/DynamixelSDK/ros/dynamixel_sdk\setup.py" ^
    build --build-base "C:/ros1ws/catkin_ws/build/DynamixelSDK/ros/dynamixel_sdk" ^
    install %DESTDIR_ARG%  ^
    --prefix="%_SETUPTOOLS_INSTALL_PATH%" ^
    --install-scripts="%_SETUPTOOLS_INSTALL_PATH%\bin" ^
    --root=%_SETUPTOOLS_INSTALL_ROOT%\
