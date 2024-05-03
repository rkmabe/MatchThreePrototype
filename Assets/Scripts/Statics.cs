using UnityEngine;

namespace MatchThreePrototype
{

    public class Statics
    {


        public static float ALPHA_ON = 1;
        public static float ALPHA_OFF = 0;

        //private static float BLOCK_ALPHA_ON = .65f;

        public static float Interpolate(float x, float x1, float x2, float y1, float y2)
        {
            return y1 + ((x - x1) / (x2 - x1) * (y2 - y1));
        }

        public static bool IsCloseEnough(Vector2 positionDestination, Vector2 positionMoving)
        {
            float tolerance = .01f;

            float manhattanDistance = Mathf.Abs(positionDestination.x - positionMoving.x) + Mathf.Abs(positionDestination.y - positionMoving.y); //+ Mathf.Abs(positionDestination.z - positionMoving.z);

            if (manhattanDistance < tolerance)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static Vector3 Vector3Zero()
        {
            return StaticVector3Zero;
        }
        private static Vector3 StaticVector3Zero = new Vector3(0, 0, 0);

        public static Vector2 Vector2Zero()
        {
            return StaticVector2Zero;
        }
        private static Vector2 StaticVector2Zero = new Vector2(0, 0);


    }

}
