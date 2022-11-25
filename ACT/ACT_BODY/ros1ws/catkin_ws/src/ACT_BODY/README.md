# ACT_BODY
- [Introduction](#introduction)
- [Dependencies](#dependencies)
- [Static workbench configuration](#config)
- [Commands](#commands)
- [Topics and messages](#topics)
- [Authors and copyright](#authors)

# Introduction
Welcome to the ACT_BODY package for Abel! This is a ROS package, then you should already know how to create a catkin workspace and setup it. For additional details: http://wiki.ros.org/catkin/Tutorials/create_a_workspace


Once you have your workspace clone this git repository in your catkin_ws/src folder:

```
cd catkin_ws/src
```

and then:

```
git clone https://git.biolab.iet.unipi.it/FACETeam/ACT_BODY -b new-organization
```

# Dependencies
In order to interface with dynamixels you will need to install the dynamixel workbench controller package:

```
cd catkin_ws/src
```

Main packages for dynamixels:

```
git clone https://github.com/ROBOTIS-GIT/dynamixel-workbench.git
git clone https://github.com/ROBOTIS-GIT/dynamixel-workbench-msgs.git
```

Dependent packages

```
git clone https://github.com/ROBOTIS-GIT/DynamixelSDK.git
```

And an additional package needed for dynamixel_state msgs definitions:

```
git clone https://github.com/ROBOTIS-GIT/open_manipulator_msgs
```

# Static configuration (config.yaml)
If not present, you should put a configuration file for Abel joints inside the abel_bringup/config folder. Choose between the yaml file inside the repository or create yours, but don't use too high profile velocity and accelerations values! (vel = 60 and acc = 5 as standards).

# Commands
Start the controller for the dynamixel:

```
roslaunch abel_bringup abel_dxl_controllers.launch
```

Launch the main scripts for Abel movement control:

```
roslaunch abel_control abel_control.launch
```

Open rqt in order to manage the different functions and send other gestures:

```
rqt
```

If you want to test the inverse kinematics you must open the moveit package for Abel, in order to simulate the model:

```
roslaunch abel_moveit_config demo.launch
```

Now Abel will listen to the joints values of the virtual model, trying to replicate those.
Be aware that Abel's inverse kinematics is still under development, so the solution may be approximate.

**Note**: Inside the file "**/abel_moveit_config/launch/demo.launch**" a parameter can be changed to manually move the joints of the virtual model:


```
<arg name="use_gui" default="true" />
```

A window will open along with MoveIt and Abel will try to replicate the movement of the joints based on the values set.

**!!! WARNING !!!
This ability to set the joint values manually does not take into account possible collisions, so NEVER PRESS randomize because dangerous configurations could occur and could damage the robot.**

For collision management refer to the MoveIt planner and the standard procedure for planning a trajectory (e.g. "random valid" in goal position).

# Topics
To interface with Abel you can use different communication channels and messages, but the most user friendly method is to use the GUI "rqt" and send very specific commands to Abel. Then, once you have started the controller for Abel and the main abel_control script, open rqt by typing in a new terminal:

```
rqt
```
A new window will open and you need to open the "Topics Publisher" plugin. When abel_bringup is launched several topics will be created, which will be detailed below.

/abel_dxl/joint_trajectory --> Receives the trajectory to be executed for dynamixel motors (msg: JointTrajectory)

/abel_dxl/joint_states     --> Feedback on current joint status (msg: JointState)

/abel_dxl/dynamixel_state  --> Feedback on current motor status (msg: DynamixelStateList)

/abel_control/neck/lookat  --> Topics on which to publish the pair of coordinates for Abel's gaze (msg: Float64MultiArray)

/abel_control/neck/gesture --> Topic on which to post the neck gesture for Abel to perform (msg: String)

/abel_control/neck/capture --> Send "1" to this topic to start the process of acquiring a new neck pose (msg: Int64)

/abel_control/arms/gesture --> Topic on which to post the arms gesture for Abel to perform (msg: String)

/abel_control/neck/capture --> Send "1" to this topic to start the process of acquiring a new arms pose (msg: Int64)


# Authors
This package for the realization of the movement for Abel has been developed during the master thesis project of the student Gabriele Sisinna, together with the collaboration of the research team of the E.Piaggio Center of the University of Pisa, and in particular with the technical support of the engineer Roberto Garofalo.

Thanks!
