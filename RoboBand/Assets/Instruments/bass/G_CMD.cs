using System;
using System.Collections.Generic;
using UnityEngine;

/**
 * <summary> a class containes methods to formulate command to manipulator</summary>
 */
public class G_CMD : MonoBehaviour {

    /**
     * <summary> a class that contains information about guitar</summary>
     */
    public class guitar_Info {
        /**
         * <value> angle_ZX, angle_ZY - rotation of the guitar</value>
         * <value> first_point - coordinates of the firststring</value>
         * <value> duration - duration of the whole note</value>
         * <value> mode - the way of playing</value>
         * <value> tabs - parsed tabs</value>
         * <value> Velocity - Velocity of manipulator (in AS unites)</value>
         */
        public float angle_ZX, angle_ZY;
        public PointMode.point first_point;
        public int duration;
        public char mode;
        public List<PlayYard.noteInf> tabs = new List<PlayYard.noteInf>();
        public int Velocity = 9000;

        /**
         * constructor with parameters
         */
        public guitar_Info(List<PlayYard.noteInf> tabs, float angle_ZX, float angle_ZY,  char mode, PointMode.point first_point, int duration)
        {
            this.tabs = tabs;
            this.angle_ZX = angle_ZX;
            this.angle_ZY = angle_ZY;
            this.mode = mode;
            this.first_point = first_point;
            this.duration = duration;
        }
    }

    /**
     * <value> StartPoint  - a method calculates to points for manipulator: above the first string and near the first string</value>
     * <param name="angleZX"> rotation of the guitar</param>
     * <param name="angleZY"> rotation of the guitar</param>
     * <param name="point"> first point coordinates</param>
     * <param name="velocity"> the value of manipulator velocity</param>
     * <returns string> commands</returns>
     */
    public string StartPoint(float angleZX, float angleZY, PointMode.point point, int velocity)
    {   //copies object
        PointMode.pointShift pointShift = new PointMode.pointShift(0, 0, 0); 
        point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
        string CMD = Printer(gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY), velocity);
        pointShift.Edit(10, 0, 0);
        CMD += Printer(gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY), velocity);
        return CMD;
    }


    public string NullTapping(float angleZX, float angleZY, PointMode.point point, int velocity)
    {
        //copies object
        PointMode.pointShift pointShift = new PointMode.pointShift(0, 0, 10);
        point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
        string CMD = Printer(gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY), velocity);
        pointShift.Edit(10, 0, -10);
        CMD += Printer(gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY), velocity);
        return CMD;
    }
    /**
     * <value> Printer - a method to formulate a command for manipulator (set to point)</value>
     * <param name="point"> point to set</param>
     * <param name="velocity"> velocity of manipulator in AS unites</param>
     * <returns string> commands</returns>
     */
    public static string Printer(PointMode.point point, int velocity)
    { // G_string[] former
        
    return ("G01  X=" + point.X.ToString("F3") + " Y=" + point.Y.ToString("F3") + " Z=" + point.Z.ToString("F3") +"\n");
 //' A=' + str(point.A) + ' B=' + str(point.B) +
 //' C=' + str(point.C) +
    }

    /**
     * <value> Jump - a method to cross string without playing</value>
     * <param name="point"> a new point</param>
     * <param name="point_last"> a previous point</param>
     * <param name="angleZY"> guitar rotation</param>
     * <param name="angleZX"> guitar rotation</param>
     * <param name="jump_mode"> reflects if previous string is above or under</param>
     * <param name="mode"> mode of playing this note</param>
     * <param name="mode_last">mode of playing previous note</param>
     * 
     * <returns string> a command</returns>
     */
    public string Jump(PointMode.point point_last, int mode_last, PointMode.point point, int mode, float angleZX, float angleZY, int jump_mode)
    {
        string CMD = "";
        PointMode.pointShift pointShift = new PointMode.pointShift(0, 0, 0);
        point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
        point_last = gameObject.GetComponentInChildren<PointMode>().Modification(point_last, pointShift, angleZX, angleZY);
        //calculates previous point
        switch (mode_last)
        {
            case 'D':
                pointShift.Edit(-10, 0, 0);
                point_last = gameObject.GetComponentInChildren<PointMode>().Modification(point_last, pointShift, angleZX, angleZY);
                break;
            case 'U':
                pointShift.Edit(10, 0, 0);
                point_last = gameObject.GetComponentInChildren<PointMode>().Modification(point_last, pointShift, angleZX, angleZY);
                break;
        }
        //calculates target point
        switch(mode)
        {
            case 'D':
                pointShift.Edit(-10, 0, 0);
                point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
                break;
            case 'U':
                pointShift.Edit(10, 0, 0);
                point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
                break;
        }
        //calculates distance between points
        if(Helper.Distance(point, point_last)>10)
        {
            //if distance more then 10 mm
            //manipulator should cross the distance with circus trajectories
            //radius of crossing:
            var radius = Mathf.Round(Helper.Distance(point, point_last) * 30 / 20);
            if (radius < 11) radius = 11;
            CMD = CMD + "G0" + (jump_mode + 1) + printerRadius(point, radius) + "\n";
        }
        return CMD;
    }

    /**
     * <value> printerRadius - a method to formulate crossing radius</value>
     * <param name="point"> target point</param>
     * <param name="radius"> calculated radius</param>
     * <returns string> command</returns>
     */
    public string printerRadius(PointMode.point point, float radius) {
        return (" G161 X" + point.X + " Z" + point.Z + " R=" + radius);
    }

    
    /**
     * <value> G_Maker - a method to formulate command</value>
     * 
     * <param name="angleZX"> guitar rotation</param>
     * <param name="angleZY"> guitar rotation</param>
     * <param name="note"> a fret</param>
     * <param name="mode"> a mode of playing</param>
     * <param name="point"> target point</param>
     * <param name="str"> string number</param>
     * 
     * <returns string> command</returns>
     */
    public string G_Maker(char mode, float angleZX, float angleZY, PointMode.point point, int str, int note, float duration)
    {
        PointMode.pointShift pointShift = new PointMode.pointShift(0, 0, 0);
        point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
        string CMD = "";
        //print duration of this
        CMD +=("M" + (500 + Convert.ToInt32(duration * 64)) + "\n");
        switch (mode)
        {
            case 'D':
                pointShift.Edit(-10, 0, 0);
                point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
               // if (note != 0)
                CMD += ( "M" + (str * 100 + note) + "\n");
                CMD += "G03" + printerRadius(point, 50);
                break;
            case 'U':
                pointShift.Edit(10, 0, 0);
                point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
                //if (note != 0)
                CMD += ( "M" + (str * 100 + note) + "\n");
                CMD += "G02" + printerRadius(point, 50);
                break;
            case 'P':

                //if (note != 0 && note != 999) CMD += ("M" + (str * 100 + note) + "\n");
                if ( note != 999) CMD += ("M" + (str * 100 + note) + "\n");
                if (note == 0)
                {
                    pointShift.Edit(-10, 0, 0);
                    point = gameObject.GetComponentInChildren<PointMode>().Modification(point, pointShift, angleZX, angleZY);
                    CMD += "G03" + printerRadius(point, 50);
                }

                break;
        }
        return CMD;
    }


}
