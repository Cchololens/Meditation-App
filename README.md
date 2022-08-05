# Away-sis

![Preview](Images/preview.png)

A VR meditation application designed to assist the Christiana Care Oasis rooms in helping staff relax. Away-sis utilizes biofeedback and cognitive load information, to help users better understand how their meditation session went.

# Design

### Home Space

The space is created as a floating zen garden consisting of a temple, a personal tree, a spirit guide, aesthetic sand patterns, and a collection of floating bubbles on top of a lake.

To avoid overwhelming the user, this environment is designed as a simplistic starting point.

- The space serves as an area to bide time as the HP headset calibrates.
- The user can select the type of meditation by choosing from various floating bubbles on the right.
- It is also intended as an area to check feedback in the form of "Growth." To avoid assigning numbers to their quality of meditation, an abstract yet meaningful way to see their progress is created in the form of a tree which grows with the increase in the number of seconds spent under the "goldilocks zone". This feature is still a work in progress.

### Meditation Environment

To reduce interruption and to boost the immersive experience, the application doesn't use human voiceover, rather it uses the sound of nature and reverbs of bells to create a relaxing experience. The environment uses a combination of calming blue colors and slowly moving elements (waterfall, trees, lake, wind, fireflies, etc.) to create a soothing, yet interesting oasis environment.

The meditation experience cycles through a day-to-night-to-day cycle with a period of Deep Focus in between. The idea is to appeal to the user's internal clock to ease waking.

During the state of Deep Focus, the color from the island fades away, creating a sense of "nothingness." The only colored item is the particle system for breathing in and out, which prompts the user to slow down and focus on their breathing. Hence, the project targets 3(vision, breathing, and sound) to enable a state of deep meditation, which helps lower the cognitive Load.

# How To Use

The project uses Unity 2020.3.36f1 and is intended for the HP Reverb Omnicept G2 Headset. The application can run on any Windows Mixed Reality headset, but biofeedback data will only work with the specific HP headset.

To fully utilize the biofeedback data, download HP Omnicept Tray and HP Omnicept Overlay. https://hpomnicept.zendesk.com/hc/en-us/articles/1500001463241-Use-Omnicept-Overlay-with-your-VR-Apps

Plug in your HP headset...

# Technical Details
The timing of the meditation and breathing can be set in the MeditationManager object.
![](Images/medManager.png)
