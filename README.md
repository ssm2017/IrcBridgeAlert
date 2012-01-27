This is an OpenSimulator region module that is sending an alert message when an avatar is entering a region that is using IrcBridge.
This module was made with the help of : http://bluewallvirtual.com/example_region_module

# How to use ?

 * Copy the files in a folder under "addon-modules"
 * Compile it (The compiler will create a file named IrcBridgeAlert.dll inside your OpenSim bin folder.)
 * If the IrcBridge module is enabled, the user will receive an alert message when entering any region hosted by the simulator. (you have nothing to do or configure, just add or remove the dll)

# Optional configuration
You have the ability to change the pre and post messages from OpenSim.ini under the [IRC] section.

Parameters names and default values are :

 * alert_msg_pre = "This region is linked to irc."
 * alert_msg_post = "Everything you say in public chat can be listened. See http://opensimulator.org/wiki/IRCBridgeModule for more informations."
