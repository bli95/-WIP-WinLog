# WinLog : Keylogger for Windows
ECE 419/CS 460 Sec Lab  
###### Authors:
* Ben Li (_cli91@illinois.edu_)  
* Jason Cokkinias (_jcokki2@illinois.edu_)  

**Project Description:**  
Program will be written in C#, using the Mono IDE. The keylogger will activate on keyboard detection. The keylogger will fill up a buffer of some X key strokes, once that buffer is full, it will call a function to send data to our remote server programs. When that function is called, the screen grabber will also activate and take a screenshot of the screen sending that over as well. We have two different programs for server side: one for the keylogging and one for the screen grabbing. When the keylogger server gets the data, it will append the new data to the keylogging file. When the screengrabbing server gets the data, it will add the screengrabbed image to a folder in the server's directory, with the line number that corresponds to the keylogging txt file. Note there are currently no limit to the size of the log file or the folder.  

**How It Works:**  

_Malicious User_ - Run 'Screengrabber_Server.exe' and 'Keylogger_Server.exe' on a couple of cmd shells in desired dir  
'-> (P.S. change serverIP and ports accordingly in victim code to the remote server machine.)  
_Victimized Dummy_ - Run 'Windows Startup Helper.exe' and go about business as usual ( ͡° ͜ʖ ͡°)  
