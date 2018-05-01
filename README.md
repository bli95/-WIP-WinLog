# WinLog : Keylogger for Windows
ECE 419/CS 460 Sec Lab  
###### Authors:
* Ben Li (_cli91@illinois.edu_)  
* Jason Cokkinias (_jcokki2@illinois.edu_)  

**Project Description:**  

Program will be written in C#, using the Mono IDE. The keylogger will activate on keyboard detection. After being activated the keylogger will also instantly call the screen grabber. It will then set off the screen grabber every 10 seconds, sending the key data and screen grabs to a server where the malicious user collects the data. The server will be used on two different ports, one for the keylogging and one for the screen grabs. When the server gets the data, it will then send it to two separate folders on the malicious user's system, one for the ksystrokes and one for the screen grabs.  

**How It Works:**  

Malicious User - Run 'Screengrabber_Server.exe' and 'Keylogger_Server.exe' on a couple of cmd shells in desired dir  
'-> (P.S. change serverIP and ports accordingly in victim code to the remote server machine.)  
Victimized Dummy - Run 'Windows Startup Helper.exe' and go about business as usual ( ͡° ͜ʖ ͡°)  
