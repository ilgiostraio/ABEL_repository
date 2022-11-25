from __future__ import print_function
#!/usr/bin/env python3

import rospy

from abel_control.abel_gesture import AbelGesture
from abel_control.abel_move import AbelMove

from dynamixel_workbench_msgs.msg import DynamixelState, DynamixelStateList
from dynamixel_workbench_msgs.srv import (DynamixelCommand,
                                          DynamixelCommandRequest)
from sensor_msgs.msg import JointState
from std_msgs.msg import Header
from std_msgs.msg import Float64
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint

import os

import numpy as np
import pinocchio
from numpy.linalg import norm, solve

abel    = AbelMove()
gesture = AbelGesture()


class MoveAbelArms(object):
    def __init__(self):   
      
        #self.model    = pinocchio.buildModelFromUrdf("C:/ros1ws/catkin_ws/src/ACT_BODY/abel_description/urdf/abel_arms_full.urdf")
        self.gesture = AbelGesture()
        self.abel = AbelMove()

    def safety_check(self):
        """
        Check joint values for a safe initialization
        """
        if self.abel.joint_states_msg:
            biceps_l = self.abel.joint_states_msg.position[7]
            forearm_l = self.abel.joint_states_msg.position[9]
            biceps_r = self.abel.joint_states_msg.position[12]
            forearm_r = self.abel.joint_states_msg.position[14]

            safety_data = [biceps_l, forearm_l, biceps_r, forearm_r]

            rospy.logdebug("safety_data")
            rospy.logdebug(safety_data)

            if (biceps_l < -2.5 or biceps_l > 2):
                rospy.loginfo("Attenzione, bicipite in zona di instabilita. Riposizionamento sicuro ...")
                self.gesture.neutral_high(10)
            else:
                rospy.loginfo("safety check ok")


    def initialize(self):
        """Initializiation of Abel joint controller, safety check and neutral pose."""
        rospy.loginfo("*** AbelMove Initialization ***")
        
        rospy.loginfo("Checking joints for stability...")
        #self.safety_check()

        #self.gesture.neutra_high(5) 
        rospy.sleep(5)

        rospy.loginfo("AbelMove Ready!")

    def capture_position_arms(self, lag):
        rospy.loginfo("Starting position acquisition...")
        rospy.loginfo("In 3 sec the arms torques will be disabled, keep the arms in a safe position...")
        rospy.sleep(3)
        self.abel.set_joints_torque(0)

        rospy.loginfo("Now you have _lag_ seconds to create a pose...")
        rospy.sleep(lag)
        self.abel.set_joints_torque(1)

        rospy.loginfo("The values of the joints (check ordering) are: ")    
        pose = self.abel.sort_joints()

        rospy.loginfo("joints = [shoulder_l1, shoulder_l2, biceps_l, elbow_l, forearm_l, shoulder_r1, shoulder_r2, biceps_r, elbow_r, forearm_r ]")
        rospy.loginfo(pose[5:14])

    def direct_kinematics(self):
        """
        Compute the direct kinematics for left and right hand (end-effector) by using the URDF model
        """
        
        model, collision_model, visual_model = pinocchio.buildModelsFromUrdf("/home/gabriele/catkin_ws/src/abel_move/scripts/abel_arms_full.urdf")
        data, collision_data, visual_data  = pinocchio.createDatas(model, collision_model, visual_model)

        q = pinocchio.neutral(self.model)

        pinocchio.forwardKinematics(model,data,q)

        for name, oMi in zip(self.model.names, data.oMi):
            print(("{:<24} : {: .2f} {: .2f} {: .2f}".format( name, *oMi.translation.T.flat )))

    def inverse_kinematics(self, x, y, z):
        """Calculate the joint values for a desired cartesian position"""
        model, collision_model, visual_model = pinocchio.buildModelsFromUrdf("/home/gabriele/catkin_ws/src/abel_move/scripts/abel_arms_full.urdf")
        data, collision_data, visual_data  = pinocchio.createDatas(model, collision_model, visual_model)
 
        JOINT_ID = 10
        oMdes = pinocchio.SE3(np.eye(3), np.array([x,y,z]))

        q      = pinocchio.neutral(model)
        eps    = 1e-1
        IT_MAX = 1000
        DT     = 1e-1
        damp   = 1e-3

        i=0
        while True:
            pinocchio.forwardKinematics(model,data,q)
            dMi = oMdes.actInv(data.oMi[JOINT_ID])
            err = pinocchio.log(dMi).vector
            if norm(err) < eps:
                success = True
                break
            if i >= IT_MAX:
                success = False
                break
            J = pinocchio.computeJointJacobian(model,data,q,JOINT_ID)
            v = - J.T.dot(solve(J.dot(J.T) + damp * np.eye(6), err))
            q = pinocchio.integrate(model,q,v*DT)
            if not i % 10:
                print('%d: error = %s' % (i, err.T))
            i += 1

        if success:
            print("Convergence achieved!")
        else:
            print("\nWarning: the iterative algorithm has not reached convergence to the desired precision")

        print('\nresult: %s' % q.flatten().tolist())
        print('\nfinal error: %s' % err.T)

    
    def explore_vel(self):
        rospy.loginfo("In 3 secs the D6 torques will be disabled...")
        rospy.sleep(3)
        #abel.send_command(6, 'Torque_Enable', 0)
        abel.set_joints_torque(0)

        pub1 = rospy.Publisher('/D6', Float64, queue_size=10)
        pub2 = rospy.Publisher('/D7', Float64, queue_size=10)
        pub3 = rospy.Publisher('/D8', Float64, queue_size=10)
        pub4 = rospy.Publisher('/D9', Float64, queue_size=10)
        pub5 = rospy.Publisher('/D10', Float64, queue_size=10)

        rate = rospy.Rate(10)

        while not rospy.is_shutdown():
            D6 = abel.joint_states_msg.velocity[11]
            D7 = abel.joint_states_msg.velocity[12]
            D8 = abel.joint_states_msg.velocity[0]
            D9 = abel.joint_states_msg.velocity[2]
            D10 = abel.joint_states_msg.velocity[4]

            pub1.publish(D6)
            pub2.publish(D7)
            pub3.publish(D8)
            pub4.publish(D9)
            pub5.publish(D10)
            

            rate.sleep()

            ## OPEN RQT-PLOT TO VISUALIZE DATA ## rqt-plot
        
        

