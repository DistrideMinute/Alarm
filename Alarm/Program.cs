using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Alarm
{
    class Program
    {
        public static Timer timer;
        static void Main(string[] args)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"Alarm01.wav");
            string input = " ";
            while (input != "")
            {
                Console.WriteLine("Begin a timer or alarm with the following syntax:\ntimer minutes\ntimer minutes seconds\ntimer hours minutes seconds\nalarm hour minutes");
                input = Console.ReadLine();
                string[] inputSplit = input.Split(' ');
                try
                {
                    if (inputSplit[0].ToLower() == "timer")
                    {
                        
                        int hour = DateTime.Now.Hour;
                        int minute = DateTime.Now.Minute;
                        int second = DateTime.Now.Second;
                        int[] inputSplitVals = new int[inputSplit.Length - 1];
                        for (int i = 1; i < inputSplit.Length; i++)
                        {
                            inputSplitVals[i - 1] = Convert.ToInt32(inputSplit[i]);
                        }
                        switch (inputSplitVals.Length)
                        {
                            case 1:
                                minute += inputSplitVals[0];
                                break;
                            case 2:
                                minute += inputSplitVals[0];
                                second += inputSplitVals[1];

                                break;
                            case 3:
                                hour += inputSplitVals[0];
                                minute += inputSplitVals[1];
                                second += inputSplitVals[2];

                                break;
                        }
                        if (second >= 60)
                        {
                            second -= 60;
                            minute += 1;
                        }
                        if (minute >=60)
                        {
                            minute -= 60;
                            hour += 1;
                        }
                        if (hour >= 24)
                        {
                            hour -= 24;
                        }
                        timer = new Timer(hour, minute, second);
                        input = "";
                    }
                    else if (inputSplit[0].ToLower() == "alarm")
                    {
                        int hour = Convert.ToInt32(inputSplit[1]);
                        int minutes = Convert.ToInt32(inputSplit[2]);
                        int second = 0;
                        timer = new Timer(hour, minutes, second);
                        input = "";
                    }
                }
                catch
                {
                    Console.WriteLine("Invalid input format");
                }
            }
            player.Load();
            if (player.IsLoadCompleted)
            {
                while (!timer.done)
                {
                    Thread.Sleep(500);
                }
                player.PlayLooping();
                Console.ReadLine();
                player.Stop();
            }
            
        }
        public static void UpdateTime(bool done = false)
        {
            Console.Clear();
            if (!done)
            {
                Console.WriteLine(timer.hourLeft + ":" + timer.minLeft + ":" + timer.secLeft + " remaining.");
                while (Console.KeyAvailable)
                    Console.ReadKey(false);
            }
            else
            {
                Console.WriteLine("Times up!");
            }
        }
    }

    /// <summary>
    /// Multithreaded to allow the console window to display the time remaining.
    /// </summary>
    class Timer
    {
        int targetHour;
        int targetMinute;
        int targetSecond;
        public int hourLeft;
        public int minLeft;
        public int secLeft;
        public bool done = false;
        public Timer(int hour, int minute, int second)
        {
            targetHour = hour;
            targetMinute = minute;
            targetSecond = second;
            hourLeft = -1;
            minLeft = -1;
            secLeft = -1;
            new Thread(new ThreadStart(BeginTimer)).Start();
            
        }
        private void BeginTimer()
        {
            int lCHour = DateTime.Now.Hour;
            int lCMinute = DateTime.Now.Minute;
            int lCSecond = DateTime.Now.Second;
            while (!done)
            {
                lCHour = DateTime.Now.Hour;
                lCMinute = DateTime.Now.Minute;
                lCSecond = DateTime.Now.Second;
                hourLeft = targetHour - lCHour;
                minLeft = targetMinute - lCMinute;
                secLeft = targetSecond - lCSecond;
                if (hourLeft <= 0 && minLeft <= 0 && secLeft <= 0)
                {
                    done = true;
                    Program.UpdateTime(true);
                }
                else
                {
                    //Calculate visual time left
                    if (secLeft < 0)
                    {
                        secLeft += 60;
                        minLeft--;
                    }
                    if (secLeft >= 60)
                    {
                        secLeft -= 60;
                        minLeft++;
                    }
                    if (minLeft < 0)
                    {
                        minLeft += 60;
                        hourLeft--;
                    }
                    if (minLeft >= 60)
                    {
                        minLeft -= 60;
                        hourLeft++;
                    }
                    if (hourLeft < 0)
                    {
                        hourLeft += 24;
                    }
                    if (hourLeft >= 24)
                    {
                        hourLeft -= 24;
                    }
                    Program.UpdateTime();
                    Thread.Sleep(500); //Ensures ~.5 second precision
                }
                

            }
        }
    }
}
