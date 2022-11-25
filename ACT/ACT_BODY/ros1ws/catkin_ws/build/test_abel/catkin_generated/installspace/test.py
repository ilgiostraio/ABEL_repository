#!/usr/bin/env python.exe
# license removed for brevity
import rospy
from std_msgs.msg import String
from std_msgs.msg import Float64, Float64MultiArray, Header


lookat = Float64MultiArray()
lookat.data = []

def talker():
    pub = rospy.Publisher('/abel_control/neck/lookat', Float64MultiArray, queue_size=10)
    rospy.init_node('test_abel', anonymous=True)
    rate = rospy.Rate(3) # 3hz
    while not rospy.is_shutdown():
        lookat.data = [380, 399]
        
        rospy.loginfo(lookat)
        pub.publish(lookat)
        rate.sleep()

if __name__ == '__main__':
    try:
        talker()
    except rospy.ROSInterruptException:
        pass