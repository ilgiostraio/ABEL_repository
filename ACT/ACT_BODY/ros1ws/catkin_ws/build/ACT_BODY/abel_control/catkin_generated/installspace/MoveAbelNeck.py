#!/usr/bin/env python3

from logging import shutdown
import numpy as np
import rospy
from rospy.core import logdebug

from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint

from std_msgs.msg import Header
from std_msgs.msg import Float64

from abel_control.AbelMove import AbelMove


class MoveAbelNeck(object):
    def __init__(self):
        #Create the object for Abel
        self.abel = AbelMove()

        #self.neck_gesture_pub = rospy.Publisher('/abel_control/neck/gesture', String, queue_size=1)
        #self.lookat_pub = rospy.Publisher('/abel_control/neck/lookat', Float64MultiArray, queue_size=1)

        self._joint_names = ['neck_1', 'neck_2', 'neck_3', 'neck_4', 'neck_5']
        
        self._dict_posture= {
            "neutral" :         [0.003067961661145091, -0.39576706290245056, 0.2929903268814087, -0.43411657214164734, 0.8590292930603027],
            "up" :              [-0.015339808538556099, 0.8283496499061584, -1.1443496942520142, -1.1198060512542725, 1.4879614114761353],
            "down" :            [-0.026077674701809883, -0.452524334192276, 0.4801360070705414, 0.3267379105091095, -0.02607767470180988],
            "zero" :            [0, 0, 0, 0, 0],
            "rotate_right" :    [-0.6166602969169617, -0.1733398288488388, -0.5491651296615601, -0.48166999220848083, 0.2316311001777649],
            "rotate_left" :      [0.630466103553772, -0.010737866163253784, 0.1702718734741211, -0.1672039031982422, 0.8283496499061584],
            
        }

    def __del__(self):
        rospy.loginfo("I'm being automatically destroyed MoveAbelNeck. Goodbye!")

    def initialize(self):
        self.run_posture("neutral", 2)

        rospy.sleep(2)
        rospy.loginfo("Neck is ready!")

    def shutdown(self):

        

        joint_position = JointTrajectoryPoint()
        joint_position.positions = [ 0.0, 0.4724660813808441, -0.6273981332778931, -1.2164467573165894, 1.5707963705062866]

        joint_position.time_from_start = rospy.Duration(1)

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()
        joints_str.joint_names = ['neck_1', 'neck_2', 'neck_3', 'neck_4', 'neck_5']

        joints_str.points.append(joint_position)

        self.abel.goal_dynamixel_position_publisher.publish(joints_str)




    def move_joints(self, point):

        rospy.loginfo("move joints neck")

        joints_str = JointTrajectory()
        joints_str.header = Header()
        joints_str.header.stamp = rospy.Time.now()

        joints_str.joint_names =  self._joint_names
    
        joints_str.points.append(point)
        self.abel.goal_dynamixel_position_publisher.publish(joints_str)

    def look_xy(self, x, y, duration=2):
        """ Experimental inverse kinematics for neck joints """

        self.lookat_point = JointTrajectoryPoint()
        
        #conversione point 0 1 a -1 1

        x = (x-0.5)*2
        y = (y-0.5)*2

        self.lookat_point.positions = [0.6*x, -0.4 + 0.7*y, 0.4 - 0.7*y, -0.4, 0.85]

        self.lookat_point.time_from_start = rospy.Duration(duration)
        self.move_joints(self.lookat_point)

        pose = self.abel.sort_joints()

        rospy.logdebug("joints = [ neck_1, neck_2, neck_3, neck_4, neck_5]")
        rospy.logdebug(pose[0:5])
        rospy.logdebug(self.lookat_point.positions)

    def gesture_neck_list(self):
        rospy.loginfo("Gesture available: [zero, neutral, up, down, hand, rotate_right, rotate_left, sequence_test, lookat_sequence_test]")

    def run_posture(self,posture,duration):

        if posture in self._dict_posture:
            
            joint_position = JointTrajectoryPoint()
            joint_position.positions = self._dict_posture[posture]
            joint_position.time_from_start = rospy.Duration(duration)

            self.move_joints(joint_position)
           
            rospy.loginfo("Running %s position for the  neck...", posture)
        else:
            rospy.logwarn("The posture %s not found", posture)

    def capture_position(self, lag):
        rospy.loginfo("Starting position acquisition...")
        rospy.loginfo("In 3 sec the neck torques will be disabled, keep the neck in a safe position...")
        rospy.sleep(3)
        self.abel.set_neck_torque(0)

        rospy.loginfo("Now you have %d seconds to create a pose...", lag)

        for c in range(lag):
            rospy.loginfo(lag-c)
            rospy.sleep(1)

        self.abel.set_neck_torque(1)

        rospy.loginfo("The values of the joints (check ordering) are: ")    
        pose = self.abel.sort_joints()

        rospy.loginfo("joints = [ neck_1, neck_2, neck_3, neck_4, neck_5]")
        rospy.loginfo(pose[0:5])


    def sequence_test(self, time):
        """ Perform a simple sequence of movement for testing and debugging """

        for key, value in self._dict_posture.items():
            self.run_gesture("key",time)
            rospy.sleep(time)

        
        self.run_gesture("neutral",time)
        rospy.sleep(time)

    def explore(self):
        rospy.loginfo("In 3 secs the neck torques will be disabled...")
        rospy.sleep(3)
        self.abel.set_neck_torque(0)

        pub1 = rospy.Publisher('/neck_joint1', Float64, queue_size=10)
        pub2 = rospy.Publisher('/neck_joint2', Float64, queue_size=10)
        pub3 = rospy.Publisher('/neck_joint3', Float64, queue_size=10)
        pub4 = rospy.Publisher('/neck_joint4', Float64, queue_size=10)
        pub5 = rospy.Publisher('/neck_joint5', Float64, queue_size=10)

        rate = rospy.Rate(10)

        while not rospy.is_shutdown():
            neck1 = self.abel.joint_states_msg.position[6]
            neck2 = self.abel.joint_states_msg.position[7]
            neck3 = self.abel.joint_states_msg.position[8]
            neck4 = self.abel.joint_states_msg.position[9]
            neck5 = self.abel.joint_states_msg.position[10]

            pub1.publish(neck1)
            pub2.publish(neck2)
            pub3.publish(neck3)
            pub4.publish(neck4)
            pub5.publish(neck5)

            rate.sleep()

            ## OPEN RQT-PLOT TO VISUALIZE DATA ## rqt-plot