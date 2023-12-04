using UnityEngine;

namespace _Workspace.CodeBase.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 AddZ(this Vector3 vector, float value)
            => new(vector.x, vector.y, vector.z + value);

        public static Vector3 AddY(this Vector3 vector, float value)
            => new(vector.x, vector.y + value, vector.z);

        public static Vector3 AddX(this Vector3 vector, float value)
            => new(vector.x + value, vector.y, vector.z);

        public static Vector3 WithX(this Vector3 vector, float value)
            => new(value, vector.y, vector.z);

        public static Vector3 WithY(this Vector3 vector, float value)
            => new(vector.x, value, vector.z);

        public static Vector3 WithZ(this Vector3 vector, float value)
            => new(vector.x, vector.y, value);
    }
}