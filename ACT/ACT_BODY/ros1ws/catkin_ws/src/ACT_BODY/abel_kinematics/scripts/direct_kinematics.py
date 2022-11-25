import pinocchio
from pinocchio import build


def direct_kinematics(self):
        """
        Compute the direct kinematics for left and right hand (end-effector) by using the URDF model
        """
        
        model, collision_model, visual_model = pinocchio.buildModelsFromUrdf("")
        data, collision_data, visual_data  = pinocchio.createDatas(model, collision_model, visual_model)

        q = pinocchio.neutral(self.model)

        pinocchio.forwardKinematics(model,data,q)

        for name, oMi in zip(self.model.names, data.oMi):
            print(("{:<24} : {: .2f} {: .2f} {: .2f}".format( name, *oMi.translation.T.flat )))