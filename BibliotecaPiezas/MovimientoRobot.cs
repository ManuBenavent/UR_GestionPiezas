﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    public static class MovimientoRobot
    {
        private static bool MOVE_BLOCKING = true;

        public static void MoverBase(RoboDK.Item ROBOT, double grados)
        {
            double[] joints = ROBOT.Joints();
            joints[0] += grados;

            try
            {
                ROBOT.MoveJ(joints, MOVE_BLOCKING);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception("Grados base: " + grados.ToString());
            }
        }

        public static void MovimientoHorizontal (RoboDK.Item ROBOT, double x, double y)
        {
            Mat robot_pose = ROBOT.Pose();
            double[] rob_xyzwpr = robot_pose.ToTxyzRxyz();
            double[] move_xyzwpr = new double[6] { x, y, rob_xyzwpr[2], rob_xyzwpr[3], rob_xyzwpr[4], rob_xyzwpr[5] };
            Mat movement_pose = Mat.FromTxyzRxyz(move_xyzwpr);

            try
            {
                ROBOT.MoveL(movement_pose, MOVE_BLOCKING);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception(movement_pose.ToString());
            }
        }

        public static void MovimientoVertical (RoboDK.Item ROBOT, double altura)
        {
            Mat robot_pose = ROBOT.Pose();
            double[] rob_xyzwpr = robot_pose.ToTxyzRxyz();
            double[] move_xyzwpr = new double[6] { rob_xyzwpr[0], rob_xyzwpr[1], altura, rob_xyzwpr[3], rob_xyzwpr[4], rob_xyzwpr[5] };
            Mat movement_pose = Mat.FromTxyzRxyz(move_xyzwpr);

            try
            {
                ROBOT.MoveL(movement_pose, MOVE_BLOCKING);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception(movement_pose.ToString());
            }
        }
    }
}