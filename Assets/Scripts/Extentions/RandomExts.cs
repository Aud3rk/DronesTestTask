using UnityEngine;
using Random = System.Random;

namespace Extentions
{
    public static class RandomExts
    {
        public static double NextDouble (this Random @this, double min, double max)
        {
            return @this.NextDouble() * (max - min) + min;
        }

        public static float NextFloat (this Random @this, float min, float max)
        {
            return (float)@this.NextDouble(min, max);
        }
        public static Vector3 GeneratePoint(Vector3 lowPoint, Vector3 highPoint)
        {
            Vector3 position = new Vector3();
            Random random = new Random();
            position.x = random.NextFloat(lowPoint.x, highPoint.x);
            position.y = random.NextFloat(lowPoint.y, highPoint.y);
            position.z = random.NextFloat(lowPoint.z, highPoint.z);
            return position;
        }
    }
}