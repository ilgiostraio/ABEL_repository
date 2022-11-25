#!/usr/bin/env python3

import rospy
from trajectory_msgs.msg import  JointTrajectoryPoint
from std_msgs.msg import String

from abel_control.MoveAbelArms import MoveAbelArms

class AbelGesture(object):
    def __init__(self):
        #Create objects
        self.arms = MoveAbelArms()

        # Topic for gestures
        self.gesture_topic_name = '/abel_control/arms/gesture'

        # We start the Publisher for the topic created
        self.gesture_pub = rospy.Publisher(self.gesture_topic_name, String, queue_size=10)

        self._dict_gesture= {
            "neutral" :         [-0.5, -0.2, 0,  0.75, 0,  0.5,  0.2, 0,  -0.75, 0],
            "neutral_high" :    [0.5, 0.9,  0.25,  0,  0.25,  -0.5, -0.9, -0.25, -0.75, 0],
            "zero" :            [0, 0, 0,  0, 0, 0,  0, 0,  0, 0],
            "hand" : [-0.22549518942832947, -0.010737866163253784, -0.5, 0.9295923709869385, 1.274738073348999, 
                    -1.3543108320236206, 0.10737866163253784, 0.9449321627616882, 0.2684466540813446, -0.6563301706314087],
            "hug" : [1.9173012399673462, 0.8482913970947266, -0.3037281930446625, -0.07976700365543365, 0.5568350553512573, 
                     -1.418932318687439, -0.771592378616333, 0.35895150899887085, 0.5322913527488708, -0.5016117095947266],
            "hello" : [-0.4, -0.18254372477531433, 0.18561168015003204, 0.7501166462898254, 0.0, 
                        -1.8208352327346802, -1.9496896266937256, -0.2653786838054657, 0.7761942744255066],

        }

    def gesture_arms_list(self):
        rospy.loginfo("Gesture available: [neutral, neutral_high, zero, mano, hug, hello, sequence_test]")

        ### joints = [shoulder_l1, shoulder_l2, biceps_l, elbow_l, forearm_l, shoulder_r1, shoulder_r2, biceps_r, elbow_r, forearm_r ] ####

    def _send_position(self, points, duration):
        joint_position = JointTrajectoryPoint()
        joint_position.positions = points
        joint_position.time_from_start = rospy.Duration(duration)

        self.arms.move_joints(joint_position)

    def neutral(self, duration):
        """ Neutrale"""

        self._send_position(self._dict_gesture["neutral"], duration)
       

    def neutral_high(self, duration):
        """ Neutrale, braccia alte per configurazione bicipite senza collisioni """
        self._send_position(self._dict_gesture["neutral_high"], duration)

    def zero(self, duration):
        """Zero joints"""
        self._send_position(self._dict_gesture["zero"], duration)


    def hand(self, duration):
        """ Hand """
        self._send_position(self._dict_gesture["hand"], duration)

    def hug(self, duration):
        """Hug"""
        self._send_position(self._dict_gesture["hug"], duration)

    

    def hello(self, duration):
        """Saluto"""
        self._send_position(self._dict_gesture["hello"], duration)


    def sequence_test(self, time):
        """Perform a simple sequence of movement for testing and debugging"""    
        self.neutral_high(time)
        rospy.sleep(time)

        self.neutral(time)
        rospy.sleep(time)

        self.hand(time)
        rospy.sleep(time)

        self.hug(time)
        rospy.sleep(time)

        self.hello(time)
        rospy.sleep(time)

        self.neutral(time)
        rospy.sleep(time)