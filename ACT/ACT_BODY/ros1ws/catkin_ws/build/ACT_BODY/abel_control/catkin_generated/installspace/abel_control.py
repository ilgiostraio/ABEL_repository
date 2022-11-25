#!/usr/bin/env python3

import numpy as np
import rospy
import pinocchio

from abel_control.abel_gesture import AbelGesture
from abel_control.abel_move import AbelMove
from abel_control.move_abel_arms import MoveAbelArms
from abel_control.move_abel_neck import MoveAbelNeck


from dynamixel_workbench_msgs.msg import DynamixelState, DynamixelStateList
from dynamixel_workbench_msgs.srv import (DynamixelCommand,
                                          DynamixelCommandRequest)

from moveit_msgs.msg import DisplayTrajectory
from sensor_msgs.msg import JointState
from std_msgs.msg import Float64, Float64MultiArray, Header
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint
from std_msgs.msg import *

global abel, arms, neck, gesture, D6_publisher


#global D6_publisher

def lookat_callback(data):
        rospy.loginfo(rospy.get_caller_id() + "Sto guardando a %s", data.data)
        if (abs(data.data[0])<=1) and (abs(data.data[1])<=1):
                x = data.data[0]
                y = data.data[1]
                neck.look_xy(x, y, 5)
        else:
                rospy.WARN("Inserire dei valori compresi fra -1 ed 1...")

def inv_kin_callback(data):
        rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)
        arms.inverse_kinematics(data.data[0], data.data[1], data.data[2])

def gesture_arms_callback(data):
        gesture.gesture_arms_list()
        rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)
        class_method = getattr(gesture, data.data)
        class_method(5)

def gesture_neck_callback(data):
        neck.gesture_neck_list()
        rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)
        class_method = getattr(neck, data.data)
        class_method(5)

def capture_arms_callback(data):
        rospy.loginfo(rospy.get_caller_id() + "I heard %s", data.data)
        if data == 0:
                rospy.loginfo("Send 1 if you want to start arms gesture acquisition...")
        else:
                arms.capture_position_arms(10)
                
def capture_neck_callback(data):
        if data == 0:
                rospy.loginfo("Send 1 if you want to start neck gesture acquisition...")
        else:
                neck.capture_position_neck(10)

def moveit_callback(data):
        pos = data.position
        rospy.loginfo(pos)
        point = JointTrajectoryPoint()

        point.positions = [0.003067961661145091, -0.39576706290245056, 0.2929903268814087, -0.43411657214164734, 0.8590292930603027, 
                           pos[0], pos[1], pos[2], pos[3], pos[4], 
                           pos[5], pos[6], pos[7], pos[8], pos[9] ]

        point.time_from_start = rospy.Duration(2)

        abel.move_all_joints(point)

def D6_callback(data):
    pos = data.velocity[1]
    D6_publisher.publish(pos)

if __name__ == "__main__":

    rospy.init_node('abel_control', log_level=rospy.DEBUG)
    rate = rospy.Rate(10)


    
    abel    = AbelMove()
    arms    = MoveAbelArms()
    neck    = MoveAbelNeck()
    gesture = AbelGesture()
    
    lookat_subscriber = rospy.Subscriber("/abel_control/neck/lookat", Float64MultiArray, lookat_callback )
    inv_kin_subscriber = rospy.Subscriber("/abel_control/arms/inverse_kinematics", Float64MultiArray, inv_kin_callback )

    gesture_arms_subscriber = rospy.Subscriber("/abel_control/arms/gesture", String, gesture_arms_callback )
    gesture_neck_subscriber = rospy.Subscriber("/abel_control/neck/gesture", String, gesture_neck_callback )

    capture_arms_subscriber = rospy.Subscriber("/abel_control/arms/capture", Int64, capture_arms_callback )
    capture_neck_subscriber = rospy.Subscriber("/abel_control/neck/capture", Int64, capture_neck_callback )

    D6_subscriber = rospy.Subscriber("/dynamixel_workbench/joint_states", JointState, D6_callback)
    D6_publisher = rospy.Publisher("/abel_control/D6_data", Float64, queue_size=10)


    ## -- MOVEIT TEST FOR JOINTS VALUES -- ##
    #moveit_subscriber = rospy.Subscriber("/joint_states", JointState, moveit_callback, queue_size=100 )


    ## -- INIT -- ##
    arms.initialize()
    #neck.initialize()
    
    #gesture.neutral(5)
    gesture.mano(5)
    rospy.sleep(3)

    
    ## -- MOVEMENT SEQUENCE -- ##
    #gesture.sequence_test(5)
    #neck.sequence_test(5)
    #rospy.sleep(5)


    ## -- ESPLORAZIONE MOTORI COLLO -- ##
    #neck.explore()
    
    ## -- TEST LOOK_XY -- ##
    #neck.lookat_sequence_test()

    ## -- TEST CINEMATICA DIRETTA ED INVERSA -- ##
    #arms.direct_kinematics()

    ##TEST CAPTURE POSITION##
    #arms.capture_position_arms(5)
    #neck.capture_position_neck(10)
    
    ##TEST ESPLORAZIONE MOTORE SPALLA SX##
    #arms.explore_vel()


    rospy.spin()
