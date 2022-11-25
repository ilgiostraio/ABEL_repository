#!/usr/bin/env python.exe
# license removed for brevity
import rospy
from std_msgs.msg import String
from std_msgs.msg import Float64, Float64MultiArray, Header,Int64

import random

lookat = Float64MultiArray()
lookat.data = []

neck_gest = ['up', 'down', 'rotate_right', 'rotate_left']
arms_gest =['neutral', 'neutral_high', 'hand', 'hug', 'hello']

def talker():
    pub_lookAt = rospy.Publisher('/abel_control/neck/lookat', Float64MultiArray, queue_size=10)
    pub_neckgest = rospy.Publisher('/abel_control/neck/gesture', String, queue_size=10)
    pub_capture_neck = rospy.Publisher("/abel_control/neck/capture", Int64, queue_size=10 )

    pub_gesture_arms = rospy.Publisher("/abel_control/arms/gesture", String,queue_size=10 )
    pub_capture_arms = rospy.Publisher("/abel_control/arms/capture", Int64, queue_size=10 )


    rospy.init_node('test_abel', anonymous=True)

    #rospy.loginfo("pub_capture_arms")
    #pub_capture_arms.publish(1)
    #rospy.sleep(20)

    #rospy.loginfo("pub_capture_neck")
    #pub_capture_neck.publish(1)
    #rospy.sleep(20)

    duration = 2 #sec
    rate = rospy.Rate(1/duration) # 3hz

    count=0

    while not rospy.is_shutdown():
        x = random.randint(25, 75)/100
        y = random.randint(25, 75)/100
        
        lookat.data = [x, y, duration-1]
        
        rospy.loginfo("lookAt %f %f %f",x,y,lookat.data[2] )
        pub_lookAt.publish(lookat)

        if count % 5 ==0 :
            # Sleep necessaria per terminare il movimento del collo, altrimenti il workbench
            # scarta la traiettoria (Dynamixel is moving)
            rospy.sleep(4)
            gest_arms = random.choice(arms_gest)
            rospy.loginfo("gest arms " + gest_arms)
            #pub_gesture_arms.publish(gest_arms)
            rospy.sleep(5)
            
        count = count +1

        rate.sleep()

if __name__ == '__main__':
    try:
        talker()
    except rospy.ROSInterruptException:
        pass