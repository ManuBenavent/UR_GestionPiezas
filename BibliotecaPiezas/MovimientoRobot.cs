using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaPiezas
{
    public static class MovimientoRobot
    {
        private static bool MOVE_BLOCKING = true;

        /// <summary>
        /// Mueve la base del robot los grados indicados
        /// </summary>
        /// <param name="ROBOT">Referencia RoboDK del robot</param>
        /// <param name="grados">Grados a girar</param>
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

        /// <summary>
        /// Realiza un movimiento horizontal sobre un plano hasta la posición. 
        /// Si no consigue realizar el movimiento de una sola vez realiza un movimiento articular hasta 
        /// una posición central hasta que consigue colocarse en la posición deseada
        /// </summary>
        /// <param name="ROBOT">Referencia RoboDK del robot</param>
        /// <param name="x">posición x</param>
        /// <param name="y">posición y</param>
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
                //throw new Exception(movement_pose.ToString());
                ROBOT.MoveJ(new double[] { 90, -90, -90, -90, 90, 0 });
                MovimientoHorizontal(ROBOT, x, y);
            }
        }

        /// <summary>
        /// Reraliza un movimiento vertical hasta la altura indicada
        /// </summary>
        /// <param name="ROBOT">Referencia del robot a mover</param>
        /// <param name="altura">Altura a la que nos queremos mover</param>
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

        /// <summary>
        /// Realiza un giro de la herramienta
        /// </summary>
        /// <param name="ROBOT">Robot a girar</param>
        /// <param name="grados">Grados a girar</param>
        /// <param name="reverse">Indica si se realiza en sentido contrario, por defecto no</param>
        public static void OrientarVentosa (RoboDK.Item ROBOT, double grados, bool reverse = false)
        {
            double[] joints = ROBOT.Joints();
            if (reverse)
                joints[5] += grados;
            else
                joints[5] -= grados;

            try
            {
                ROBOT.MoveJ(joints, MOVE_BLOCKING);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception("Grados base: " + grados.ToString());
            }
        }

        /// <summary>
        /// Coloca el robot en el modo de trabajo para la zona amarilla
        /// </summary>
        /// <param name="ROBOT">robot a mover</param>
        public static void ZonaAmarilla(RoboDK.Item ROBOT)
        {
            try
            {
                ROBOT.MoveJ(new double[] { 90, -90, -90, -90, 90, 0 });
                double[] jo = ROBOT.Joints();
                jo[3] = 90;
                ROBOT.MoveJ(jo);
                jo[4] = -90;
                ROBOT.MoveJ(jo);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception("No se pudo colocar en orientacion zona amarilla");
            }
        }

        /// <summary>
        /// Coloca el robot en el modo de trabajo en la zona verde
        /// </summary>
        /// <param name="ROBOT">robot a mover</param>
        public static void ZonaVerde(RoboDK.Item ROBOT)
        {
            try
            {
                double[] jo = ROBOT.Joints();
                jo[4] = 90;
                ROBOT.MoveJ(jo);
                jo[3] = -90;
                ROBOT.MoveJ(jo);
            }
            catch (RoboDK.RDKException)
            {
                throw new Exception("No se pudo colocar en orientacion zona amarilla");
            }
        }
    }
}
