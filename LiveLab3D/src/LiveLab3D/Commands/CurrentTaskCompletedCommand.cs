namespace LiveLab3D.Commands
{
    using Microsoft.Xna.Framework;

    public class CurrentTaskCompletedCommand : VehicleCommandBase
    {
        public int TaskType { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public Vector3 Position
        {
            get { return new Vector3(X, Y, Z); }
        }
        public Vector2 XYPosition
        {
            get { return new Vector2(X, Y); }
        }
        public float Heading { get; set; }
        public float DontCare { get; set; }
        public float DontCare2 { get; set; }
        public float DontCare3 { get; set; }
    }
}
