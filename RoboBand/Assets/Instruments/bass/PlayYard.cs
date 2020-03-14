using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/**
 * <summary> a class that formulates commandes</summary>
 */
public class PlayYard : MonoBehaviour {
    /**
     * <value> strings - information about strings position </value>
     */
    public Strings strings;

    /**
     * <summary> a class contains information about notes and methods to edit it</summary>
     */
    public class noteInf
    {
        /**
         * <value> duration - duration of note</value>
         * <value> str_ing - string number</value>
         * <value> note - a fret </value>
         */
        public float duration;
        public int str_ing;
        public int note;

        /**
         * <value> constructer</value>
         */
        public noteInf(float duration, int str_ing, int note)
        {
            this.duration = duration;
            this.str_ing = str_ing;
            this.note = note;
        }

        /**
         * <value> Edit - a method to edit duration</value>
         * <param name="duration"> new duration</param>
         */
        public void Edit(float duration)
        {
            this.duration = duration;
        }
        /**
         * <value> Edit - a method to edit string and fret</value>
         * <param name="note"> new fret</param>
         * <param name="str_ing"> new string</param>
         */
        public void Edit(int str_ing, int note)
        {
            this.str_ing = str_ing;
            this.note = note;
        }
    }

    /**
     * <value> Play_tabs_xml - a method to covert xml tabs to string commands</value>
     * <param name="g_f"> information about guitar</param>
     * <returns string> commands</returns>
     */
    public string Play_tabs_xml(G_CMD.guitar_Info g_f)
    {
        //stringbuilder used to optim. concat
        StringBuilder CMD = new StringBuilder();
        //first infoline
        CMD.AppendLine("M499 \nM498 \nG101 J0=90 J1=-47 J2=88 J4=-73 F9000  \nG18 \ndef plc_global path_synch REAL LadyOnline:Tseloe as duration \nduration = " + g_f.duration.ToString() + "\nM499 \nM498");
        char mode_last = 'D';
        char mode = 'D';
        int string_last = g_f.tabs[0].str_ing;
        bool starting = false;
        //M500 - reset timer; M999 - reset errors 
        CMD.Append(StartPoint(g_f) + "M500 \nM999\n");
        for (int i = 0; i < g_f.tabs.Count; i++)
        {
            

            if (i != 0 && g_f.tabs[i].note != 999 && g_f.mode != 'P')
            {
                //print duration of this
             //   CMD.AppendLine("M" + (500 + Convert.ToInt32(g_f.tabs[i].duration * 64)));

                if (string_last == g_f.tabs[i].str_ing)
                {

                    //if guitar plays on the same string
                    mode = Helper.InvertMode(mode_last);
                    mode_last = mode;
                }
                else
                {
                    
                    //if new string is above  previous -> mode 'D' down
                    //if new string is under  previous -> mode 'U' up
                    if (g_f.tabs[i].str_ing > string_last) mode = 'D';
                    if (g_f.tabs[i].str_ing < string_last) mode = 'U';
                    if (starting) CMD.Append(Crossing(mode_last, string_last, mode, g_f.tabs[i].str_ing, g_f));
                    else starting = true;
                    mode_last = mode;
                    string_last = g_f.tabs[i].str_ing;

                }
            }
            // tapping mode or rest note
            if (g_f.mode == 'P' || g_f.tabs[i].note == 999)
            {
                //print duration of this
               // CMD.AppendLine("M" + (500 + Convert.ToInt32(g_f.tabs[i].duration * 64)));
                if ( g_f.tabs[i].note == 0)
                {
                    CMD.Append(NULLTapping(g_f.tabs[i].str_ing, g_f));
                    mode_last = mode;
                    
                }
                mode = 'P';
                string_last = g_f.tabs[i].str_ing;
            }

            string crossing = null;
            if(g_f.tabs[i].str_ing < 5) crossing = GameObject.Find("PanelBass").GetComponent<G_CMD>().G_Maker(mode, g_f.angle_ZX, g_f.angle_ZY, strings.point[g_f.tabs[i].str_ing - 1], g_f.tabs[i].str_ing, g_f.tabs[i].note, g_f.tabs[i].duration);
            //if string is not empty
            if (!string.IsNullOrWhiteSpace(crossing)) CMD.AppendLine(crossing);
            
            
        }
        //at the end
        CMD.AppendLine("M532");
        CMD.AppendLine("M100 \nM200 \nM300 \nM400");
        CMD.AppendLine("M532");
        CMD.AppendLine("G101 J0=90 J1=-47 J2=88 J4=-73 ");
        return CMD.ToString();
    }


    private string NULLTapping(int st, G_CMD.guitar_Info g_f)
    {
        return GameObject.Find("PanelBass").GetComponent<G_CMD>().NullTapping(g_f.angle_ZX, g_f.angle_ZY, strings.point[st - 1], g_f.Velocity);
    }

    /**
     * <value> Crossing - a method to find out how to jump to the new position</value>
     * <param name="mode_last"> previous mode</param>
     * <param name="g_f"> information about guitar</param>
     * <param name="mode"> new mode</param>
     * <param name="st"> new string</param>
     * <param name="string_last"> previous string</param>
     * <returns string> a command</returns>
     */
    private string Crossing(char mode_last, int string_last, char mode, int st, G_CMD.guitar_Info g_f)
    {
        //if new string above previous
        int jump_mode = 1;
        //if new string under previous
        if (string_last > st) jump_mode = 2;
        //jump
        
        return GameObject.Find("PanelBass").GetComponent<G_CMD>().Jump(strings.point[string_last -1], mode_last, strings.point[st - 1], Helper.InvertMode(mode), g_f.angle_ZX, g_f.angle_ZY, jump_mode);
    }

    /**
     * <value> StartPoint - a method to calculate position about startpoint</value>
     * <param name="g_f"> contains information about guitar</param>
     * <returns> command</returns>
     */
    private string StartPoint(G_CMD.guitar_Info g_f)
    {
        string CMD = "";
        //calculates string position
        Load(g_f.angle_ZX, g_f.angle_ZY, g_f.first_point);
        
        for (int i = 0; i < g_f.tabs.Count; i++)
        {
            //in case of rest
            if (g_f.tabs[i].note != 999)
            {
                CMD = Convert.ToString(GameObject.Find("PanelBass").GetComponent<G_CMD>().StartPoint(g_f.angle_ZX, g_f.angle_ZY, strings.point[g_f.tabs[i].str_ing -1], g_f.Velocity));
                return CMD;
            }
        }
        return CMD;
    }

    

    /**
     * <value> Load - a method to calculate strings position</value>
     * <param name="angleZX"> guitar rotation</param>
     * <param name="angle_ZY"> guitar rotation</param>
     * <param name="first_point"> first string position</param>
     */
    private void Load(float angleZX, float angle_ZY, PointMode.point first_point)
    {
        PointMode.point firstString = first_point;
        strings = new Strings(firstString, angleZX, angle_ZY);
    }

    /**
     * <summary> a class contains information about strings position</summary>
     */
    public class Strings
    {
        /**
         * <value> point - 4 strings position</value>
         */
        public List<PointMode.point> point;

        /**
         * <value> constructor</value>
         */
        public Strings(PointMode.point point_1, float angleZX, float angleZY)
        {
            point = new List<PointMode.point>
        {
            point_1 //first string was set
        };
            //shift 20 mm - (distance between strings)
            PointMode.pointShift shiftStrings = new PointMode.pointShift(-20, 0, 0);

            var point_2 = GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification( point_1, shiftStrings, angleZX, angleZY);
            point.Add(GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification( point_1, shiftStrings, angleZX, angleZY));
            var point_3 = GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification(point_2, shiftStrings, angleZX, angleZY);
            point.Add(GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification( point_2, shiftStrings, angleZX, angleZY));
            point.Add(GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification( point_3, shiftStrings, angleZX, angleZY));
        }
        /**
         * <value> constructor</value>
         */
        public Strings()
        {
            point = new List<PointMode.point>();
        }
        /**
         * <value> constructor</value>
         */
        public Strings(PointMode.point point_1, PointMode.point point_2, PointMode.point point_3, PointMode.point point_4)
        {
            point = new List<PointMode.point>();
            point.Add(point_1);
            point.Add(point_2);
            point.Add(point_3);
            point.Add(point_4);
        }
        /**
         * <value> EditString - a method to edit strings' position </value>
         * <param name="angleZX"> guitar rotation</param>
         * <param name="angleZY"> guitar rotation</param>
         * <param name="point_1"> first string position</param>
         * <returns Strings> a list of 4 points</returns>
         */
        public Strings EditString(PointMode.point point_1, float angleZX, float angleZY)
        {
            PointMode.point[] p = new PointMode.point[4];
            p[0] = point_1;
            PointMode.pointShift shiftStrings = new PointMode.pointShift(-20, 0, 0);
            p[1] = GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification(p[0], shiftStrings, angleZX, angleZY);
            p[2] = GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification(p[1], shiftStrings, angleZX, angleZY);
            p[3] = GameObject.Find("Load_Tabs").GetComponent<PointMode>().Modification(p[2], shiftStrings, angleZX, angleZY);
            return new Strings(p[0], p[1], p[2], p[3]);
        }

    }

}

