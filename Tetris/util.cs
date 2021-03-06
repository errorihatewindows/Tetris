﻿using System;
using System.Windows.Forms;

namespace Tetris
{
    static class util
    {
        //contains all possible colors
        public static char[] colors = { 'g', 'y', 'r', 'l', 'b', 'p', 'o' };
        //Contains points for amount of lines cleared 
        public static int[] points = { 40, 100, 300, 1200 };
        
        public static void wait(int milliseconds)
        {
            Timer timer1 = new Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();
            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
            };
            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
 
}