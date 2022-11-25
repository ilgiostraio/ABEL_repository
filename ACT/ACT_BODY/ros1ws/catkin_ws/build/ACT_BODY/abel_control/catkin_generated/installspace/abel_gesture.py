#!/usr/bin/env python3

import rospy
from sensor_msgs.msg import JointState
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint
from dynamixel_workbench_msgs.msg import DynamixelState, DynamixelStateList
from dynamixel_workbench_msgs.srv import DynamixelCommand, DynamixelCommandRequest
from std_msgs.msg import Header
from std_msgs.msg import String

from .abel_move import AbelMove



class AbelGesture(object):
    def __init__(self):
        #Create objects
        self.abel = AbelMove()

        # Topic for gestures
        self.gesture_topic_name = '/abel_control/arms/gesture'

        # We start the Publisher for the topic created
       # self.gesture_pub = rospy.Publisher(self.gesture_topic_name, String, queue_size=10)

    def gesture_arms_list(self):
        rospy.loginfo("Gesture available: [neutral, neutral_high, zero, mano, hug, hello, sequence_test]")

        ### joints = [shoulder_l1, shoulder_l2, biceps_l, elbow_l, forearm_l, shoulder_r1, shoulder_r2, biceps_r, elbow_r, forearm_r ] ####

    def neutral(self, duration):
        """ Neutrale"""
        self.joint_position_home = JointTrajectoryPoint()
        self.joint_position_home.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_home.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_home)

    def D6_up(self, duration):
        """ Neutrale"""
        self.joint_position_home = JointTrajectoryPoint()
        self.joint_position_home.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_home.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_home)

    def neutral_high(self, duration):
        """ Neutrale, braccia alte per configurazione bicipite senza collisioni """

        rospy.loginfo("Start gesture neutral_high...")
        self.joint_position_home = JointTrajectoryPoint()
        self.joint_position_home.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_home.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_home)
        rospy.loginfo("End gesture neutral_high...")

    def zero(self, duration):
        """Zero joints"""
        self.joint_position_zero = JointTrajectoryPoint()
        self.joint_position_zero.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_zero.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_zero)

    def mano(self, duration):
        """ Stretta di mano """
        rospy.loginfo("Start gesture mano...")
        self.joint_position_hand = JointTrajectoryPoint()
        self.joint_position_hand.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_hand.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_hand)
        rospy.loginfo("End gesture mano...")


    def hug(self, duration):
        """Abbraccio"""
        self.joint_position_hug = JointTrajectoryPoint()
        self.joint_position_hug.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_hug.velocities = [ 0, 0 ]

        self.joint_position_hug.accelerations = [0, 0]

        self.joint_position_hug.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_hug)
    

    def hello(self, duration):
        """Saluto"""
        rospy.loginfo("Start gesture hello...")
        self.joint_position_hello = JointTrajectoryPoint()
        self.joint_position_hello.positions = [-0.003067961661145091, 0.15186409652233124]

        self.joint_position_hello.time_from_start = rospy.Duration(duration)
        self.abel.move_all_joints(self.joint_position_hello)
        rospy.loginfo("End gesture hello...")



    def sequence_test(self, time):
        """Perform a simple sequence of movement for testing and debugging"""    
        self.neutral_high(time)
        rospy.sleep(time)

        self.neutral(time)
        rospy.sleep(time)

        self.mano(time)
        rospy.sleep(time)

        self.hug(time)
        rospy.sleep(time)

        self.hello(time)
        rospy.sleep(time)

        self.neutral(time)
        rospy.sleep(time)




