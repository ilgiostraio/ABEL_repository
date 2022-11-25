import pinocchio

import numpy as np
from numpy import norm, solve

def inverse_kinematics(self, x, y, z):
        """Calculate the joint values for a desired cartesian position"""
        model, collision_model, visual_model = pinocchio.buildModelsFromUrdf("")
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