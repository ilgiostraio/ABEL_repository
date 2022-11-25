from __future__ import print_function
from logging import shutdown
from numpy.core.defchararray import count
#!/usr/bin/env python3

import rospy
import os

import numpy as np

from abel_control.AbelMove import AbelMove


from sensor_msgs.msg import JointState
from std_msgs.msg import Header
from std_msgs.msg import Float64
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint

import time

class MoveAbelArms(object):
    def __init__(self):   
        self.abel    = AbelMove()

        self._joint_names = ['shoulder_l1', 'shoulder_l2', 'biceps_l', 'elbow_l', 'forearm_l', 
                            'shoulder_r1', 'shoulder_r2', 'biceps_r', 'elbow_r', 'forearm_r']
        
        self._dict_gesture= {
            "neutral" :         [-0.5, -0.2, 0,  0.75, 0,  0.5,  0.2, 0,  -0.75, 0],
            "neutral_high" :    [0.5, 0.9,  0.25,  0,  0.25,  -0.5, -0.9, -0.25, -0.75, 0],
            "zero" :            [0, 0, 0,  0, 0, 0,  0, 0,  0, 0],
            "hand" : [-0.22549518942832947, -0.010737866163253784, -0.5, 0.9295923709869385, 1.274738073348999, 
                    -1.3543108320236206, 0.10737866163253784, 0.9449321627616882, 0.2684466540813446, -0.6563301706314087],
            "hug" : [1.9173012399673462, 0.8482913970947266, -0.3037281930446625, -0.07976700365543365, 0.5568350553512573, 
                        -1.418932318687439, -0.771592378616333, 0.35895150899887085, 0.5322913527488708, -0.5016117095947266],
            "hello" : [-0.4, -0.18254372477531433, 0.18561168015003204, 0.7501166462898254, 0.0, 
                        -1.8208352327346802, -1.9496896266937256, -0.2653786838054657, 0.7761942744255066, 0]
            
        }



 
        self.shutdown_position= [-0.21168935298919678, 0.12732040882110596, -1.365242899658203,0.7685244083404541, 0.0, 0.3160000443458557, 0.04141748324036598, 1.843844904931641, -1.0078253746032715, 0.003067961661145091]


    def shutdown(self):

        
        joint_position = JointTrajectoryPoint()
        joint_position.positions = [-1.365242899658203, 1.843844904931641, 0.7685244083404541, -1.0078253746032715, 
            0.0, 0.003067961661145091,-0.21168935298919678, 0.12732040882110596, 0.3160000443458557, 0.04141748324036598]

        joint_position.time_from_start = rospy.Duration(1)

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()
        joints_str.joint_names = ['biceps_l', 'biceps_r', 'elbow_l', 'elbow_r', 'forearm_l', 'forearm_r', 'shoulder_l1', 'shoulder_l2', 'shoulder_r1', 'shoulder_r2']

        joints_str.points.append(joint_position)

        self.abel.goal_dynamixel_position_publisher.publish(joints_str)



    def initialize(self):
        """Initializiation of Abel joint controller, safety check and neutral pose."""
        
        
        self.safety_check()
        rospy.sleep(1)
        self.run_gesture("neutral",2) 
        rospy.sleep(3)

        rospy.loginfo("Arms are Ready!")
    
    def safety_check(self):
        """
        Check joint values for a safe initialization
        """
        rospy.loginfo("--- Checking joints for stability ---")
        rospy.wait_for_message(self.abel.joint_states_topic_name, JointState, 2.0)

        if self.abel.joint_states_msg :
            biceps_l = self.abel.joint_states_msg.position[7]
            forearm_l = self.abel.joint_states_msg.position[9]
            biceps_r = self.abel.joint_states_msg.position[12]
            forearm_r = self.abel.joint_states_msg.position[14]

            safety_data = [biceps_l, forearm_l, biceps_r, forearm_r]

            if (biceps_l < -2.5 or biceps_l > 2) and (biceps_r < -2.5 or biceps_r > 2):
                rospy.logerr(safety_data)
                rospy.logerr("Warning, some joints are in dangerous position. Safe ripositioning...")
                raise Exception('Failed safety check')
            else:
                rospy.loginfo("All joints are in safety position.")
        else:
            rospy.logerr("Error, I don't know the position of the joints")
            raise Exception('Failed safety check')
        


    def move_joints(self, point):

        rospy.loginfo("move joints arms")

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()
        joints_str.joint_names = self._joint_names 
        joints_str.points.append(point)

        self.abel.goal_dynamixel_position_publisher.publish(joints_str)
    
    def move_all_joints_left_arm(self, point):

        rospy.loginfo("move all joints left arm")

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()

        joints_str.joint_names = ['shoulder_l1', 'shoulder_l2', 'biceps_l', 'elbow_l', 'forearm_l']
    
        joints_str.points.append(point)
        self.abel.goal_dynamixel_position_publisher.publish(joints_str)
    
    def move_all_joints_right_arm(self, point):

        rospy.loginfo("move joints right arm")

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()

        joints_str.joint_names = ['shoulder_r1', 'shoulder_r2', 'biceps_r', 'elbow_r', 'forearm_r']
    
        joints_str.points.append(point)
        self.abel.goal_dynamixel_position_publisher.publish(joints_str)

    def capture_position(self, type, lag):
        rospy.loginfo("Starting position acquisition...")
        rospy.loginfo("In 3 sec the arms torques will be disabled, keep the arms in a safe position...")
        rospy.sleep(3)
        
        self.abel.set_arms_joints_torque(type,0)

        rospy.loginfo("Now you have %d seconds to create a pose...", lag)

        for c in range(lag):
            rospy.loginfo(lag-c)
            rospy.sleep(1)

        self.abel.set_arms_joints_torque(type,1)

        rospy.loginfo("The values of the joints (check ordering) are: ")    
        pose = self.abel.sort_joints()

        rospy.loginfo("joints = [shoulder_l1, shoulder_l2, biceps_l, elbow_l, forearm_l, shoulder_r1, shoulder_r2, biceps_r, elbow_r, forearm_r ]")
        rospy.loginfo(pose[5:14])
    
    def explore_vel(self):
        rospy.loginfo("In 3 secs the D6 torques will be disabled...")
        rospy.sleep(3)
        self.abel.send_command(6, 'Torque_Enable', 0)
        self.abel.set_joints_torque(0)

        pub1 = rospy.Publisher('/D6', Float64, queue_size=10)
        pub2 = rospy.Publisher('/D7', Float64, queue_size=10)
        pub3 = rospy.Publisher('/D8', Float64, queue_size=10)
        pub4 = rospy.Publisher('/D9', Float64, queue_size=10)
        pub5 = rospy.Publisher('/D10', Float64, queue_size=10)

        rate = rospy.Rate(10)

        while not rospy.is_shutdown():
            D6 = self.abel.joint_states_msg.velocity[11]
            D7 = self.abel.joint_states_msg.velocity[12]
            D8 = self.abel.joint_states_msg.velocity[0]
            D9 = self.abel.joint_states_msg.velocity[2]
            D10 = self.abel.joint_states_msg.velocity[4]

            pub1.publish(D6)
            pub2.publish(D7)
            pub3.publish(D8)
            pub4.publish(D9)
            pub5.publish(D10)
            

            rate.sleep()

            ## OPEN RQT-PLOT TO VISUALIZE DATA ## rqt-plot

    def gesture_arms_list(self):
        rospy.loginfo("Gesture available: [neutral, neutral_high, zero, mano, hug, hello, sequence_test]")

        ### joints = [shoulder_l1, shoulder_l2, biceps_l, elbow_l, forearm_l, shoulder_r1, shoulder_r2, biceps_r, elbow_r, forearm_r ] ####

    def sequence_gesture_test(self, time):
        """Perform a simple sequence of movement for testing and debugging"""   

        for key, value in self._dict_gesture.items():
            self.run_gesture("key",time) #ATTENTION TO THIS "KEY" INSTEAD OF KEY
            rospy.sleep(time)

        self.run_gesture("neutral", time)
        rospy.sleep(time)

    def run_gesture(self,gesture,duration):

        #gesture1 = gesture.replace('\\', '')
        #gesture2 = gesture1[1:-1]

        if gesture in self._dict_gesture:
            
            joint_position = JointTrajectoryPoint()
            joint_position.positions = self._dict_gesture[gesture]
            joint_position.time_from_start = rospy.Duration(duration)

            self.move_joints(joint_position)

            rospy.loginfo("Running %s position for the arms...", gesture)

        else:
            rospy.logwarn("Position %s not found!", gesture)