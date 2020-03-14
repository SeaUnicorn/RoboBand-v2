using UnityEngine;

/**
 * <summary> a class contains common methods</summary>
 */
public class Helper : MonoBehaviour {
/**
 * <value> InvertMode - a method to change mode</value>
 * <param name="previous_mode"> previous way of playing</param>
 * <returns string> new </returns>
 */
public static char InvertMode(char previous_mode)
    {
        if (previous_mode == 'U') return 'D';
        else return 'U';
    }

/**
 * <value> Distance - a method to calculate distance between the points</value>
 * <param name="point1"> start point</param>
 * <param name="point2"> finish point</param>
 * <returns float> distance between points</returns>
 */
public static float Distance(PointMode.point point1, PointMode.point point2)
    {
        return Mathf.Sqrt((point1.X - point2.X) * (point1.X - point2.X) + (point1.Y - point2.Y) * (point1.Y - point2.Y) + (point1.Z - point2.Z) * (point1.Z - point2.Z));
    }
}
