# Alarm
Simple alternative to the built-in windows alarm/timer


Built-in windows timer/alarm, Alarms & Clock, is a bit unwieldy, so I made a simple console alternative.
Problems addressed:
-(At time of this program being made) only allowed for 1 timer
-Timers and alarms are designed to be repeated, which isn't ideal for one-off use-cases
-Takes a while to startup and has odd lag to it

To address these:
-Each instance of my implementation can have a single timer/alarm
-There is no saving involved
-Uses the .Net Framework console for fast startup and no noticable lag

Minor issues with my implementation:
-No naming of timers means that if the user forgets what they mean there can be issues
-If 'highlighting' text on the alarm it may fail to 'ring' until the user presses escape
-Can't schedule for the next day?
