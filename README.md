
SAS_Xproto
==========

A prototype for a tool I am making. The code is in C# and it seems to work well.



This is my prototype if it inspires you please feel free to play around with it here is how:
> create\edit the "cmd.xml" and run "SAS_Xproto.exe".... from the command line.


:[[ Prerequisites ]]
> .NET
> Putty

:.NET framework is installed by default on any modern version of Windows... if you do not have it go here.
:===================================================================================================
http://www.microsoft.com/net/downloads
:===================================================================================================


:Please install Putty with all the binaries and setup putty in your PATH:
:===================================================================================================
:#Use PUTTY
:http://www.chiark.greenend.org.uk/~sgtatham/putty/download.html

set PATH /p
set PATH=%PATH%;%programfiles(x86)%\PuTTY	:set path now
:setx PATH "%PATH%;%programfiles(x86)%\PuTTY"	:and forever; remove starting ":" (64-bit System)
:setx PATH "%PATH%;%programfiles%\PuTTY"	:(32-bit System)
set PATH /p

:#OR use something that includes putty binaries; like, Gow (Gnu On Windows) a lightweight alternative to Cygwin.
:https://github.com/bmatzelle/gow/wiki

C:\>which plink
C:\Program Files (x86)\Gow\bin\plink.EXE

C:\>
:===================================================================================================

:Modify or create the control file: 
:===================================================================================================
#You will likley only need to change these things.
<exec name='plink' args='USER_NAME@SERVER_NAME_OR_IP' />
<host name='chewy' ip='14.0.0.126'/>
<cmd regex='login' write='root'/>
<cmd regex='password' write='root'/>
:===================================================================================================



:[[ Edit/Create cmd.xml ]]
cd %USERPROFILE%\Downloads\SAS_Xproto
notepad cmd.xml
:===================================================================================================
<exec>
<exec name='plink' args='root@14.0.0.126' />
<host>
<host name='chewy' ip='14.0.0.126'/>
	<cmd>
			<cmd regex='login' write='root'/>
			<cmd regex='password' write='root'/>
			<cmd prompt='' write='export PS1='/>
			<cmd prompt='' write='uname -a'/>
			<cmd prompt='' write='df -h'/>
			<cmd prompt='' write='ifconfig -a'/>
			<cmd prompt='' write='cat /etc/resolv.conf'/>
			<cmd prompt='' write='who -r'/>
			<cmd prompt='' write='ps -ef'/>
			<cmd prompt='' write='last'/>
			<cmd prompt='' write='uptime'/>
			<cmd prompt='' write='ps -ef | grep -i "nginx"'/>
			<cmd prompt='' write='echo y | pacman -Syu'/>
			<cmd prompt='' write='@done'/>
	</cmd>

</host>
</exec>
:===================================================================================================


:[[ BUILD AND RUN THE "Executable" ]]

:Compile and RUN at the command line.
:===================================================================================================
:NO need for VISUAL STUDIO as any .NET framework has a compiler for c#

:#NOTE: USE ANY ONE OF THESE .NET VERSIONS just ADD/REMOVE ":" FROM FRAMEWORK YOU WISH TO USE

set PATH /p
set PATH=%SYSTEMROOT%\Microsoft.NET\Framework\v2.0.50727\;%PATH%
:set PATH=%SYSTEMROOT%\Microsoft.NET\Framework\v3.0\;%PATH%
:set PATH=%SYSTEMROOT%\Microsoft.NET\Framework\v3.5\;%PATH%
:set PATH=%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\;%PATH%
set PATH /p

:#NOTE: I tested this PROTOTYPE with .NET 2.0 to .NET 4.... all versions worked for me.

:#NOTE: Here you can "permanently" set path by remove starting ":" from the command line below
:setx PATH "%SYSTEMROOT%\Microsoft.NET\Framework\v4.0.30319\;%PATH%"

:#NOTE: You can still use the other command lines (above) to reset the path
:===================================================================================================


:#COMPILE/RUN
cd %USERPROFILE%\Downloads\SAS_Xproto
csc.exe SAS_Xproto.cs && SAS_Xproto.exe

:#QUICK REPL TOOL
cd %USERPROFILE%\Downloads\SAS_Xproto
for /F "tokens=1-3 delims=: " %i in ('time /t') do notepad SAS_Xproto.cs && pause && csc.exe SAS_Xproto.cs && cp SAS_Xproto.cs SAS_Xproto.cs.%i%j%k && SAS_Xproto.exe


:[[ ENJOY! ]]


