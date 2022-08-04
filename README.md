# Away-sis

![Preview](Images/preview.png)

A VR meditation application designed to assist the Christiana Care Oasis rooms in helping staff relax. Away-sis utilizes biofeedback and cognitive load information, to help users better understand how their meditation session went.

# Design
<b>Home Space</b><br>
The space is created as a floating zen garden consisting of a temple, personal tree, a spirit guide, aesthetic sand patterns, and a collection of floating bubbles on top a lake.<br>
To avoid overwhelming the user this environment is a starting point for any user and provides 2 options to interact with the system. 
First, the user can select the type of meditation by choosing from a floating bubble on the right. The collection of floating bubble is the different types of experiences for the user.
Second, the user can check the their feedback in the form of "Growth" of the their tree on the left. To avoid assigning numbers to their quality of meditation, an abstract yet meaningful way to see their progress is created in the form of a tree which grows with the increase in the number of seconds spent under the "goldilocks zone".
<br><b>Medittation Environment</b><br>
To reduce interuption and to boost immersive experience, the application doesn't use any human voice rather the sound of nature and reverbs of bells are used to create a relaxing experience. The environment uses a combination of colors associated with calming or cool(blue spectrum) along with elements(waterfall, trees, lake, wasps, fireflies, etc.) which are related to a peaceful natural oasis setting. 
The meditation experience cycles through a day to night to day with a period of Deep Focus in between with the goal of using an internal clock mechanism to wake up the user.<br>
During the state of Deep Focus, the color from all objects fades away creating a sense of "nothingness" where the only colored item is the particle system for breathing in and out to make sure the user focusses on the breathing along with suitable background music to calm down. Hence, the project targets 3(vision, breathing, and sound) to enable a state of deep meditation, which helps lower the cognitive Load. 

# How To Use

Project uses Unity 2020.3.36f1 and is intended for the HP Reverb Omnicept G2 Headset. The application can run on any Windows Mixed Reality headset, but biofeedback data will only work with the specific HP headset.

To fully utilize the biofeedback data, download HP Omnicept Tray and HP Omnicept Overlay. https://hpomnicept.zendesk.com/hc/en-us/articles/1500001463241-Use-Omnicept-Overlay-with-your-VR-Apps

Plug in your HP headset...

# Technical Details
The timing of the meditation and breathing can be set in the MeditationManager object.
![](Images/medManager.png)
