import rospy
from sensor_msgs.msg import JointState
from trajectory_msgs.msg import JointTrajectory, JointTrajectoryPoint
from std_msgs.msg import Header
from std_msgs.msg import Duration

def jointStateCallback(data):
    pub = rospy.Publisher('/dynamixel_workbench/joint_trajectory', JointTrajectory, queue_size=10)
    
    #TEST CON POSITION CONTROLLER
    joints_str = JointTrajectory()
    joints_str.header = Header()
    joints_str.header.stamp = rospy.Time.now()
    joints_str.joint_names = ['pan', 'tilt']

    point = JointTrajectoryPoint()
    point.positions = [0, 0]
    point.velocities = [0.0, 0.0]
    point.time_from_start = rospy.Duration(3)

    joints_str.points.append(point)

    pub.publish(joints_str)
    rospy.loginfo("COMANDO INVIATO: %s", joints_str)

def listener():
    rospy.init_node('listener', anonymous=True)
    rospy.Subscriber('/dynamixel_workbench/joint_states', JointState, jointStateCallback)
    

if __name__ == '__main__':
    listener()
    rospy.spin()