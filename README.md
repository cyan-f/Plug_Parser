# Plug_Parser

An attempt to control a Lovense Hush via FF14's Advanced Combat Tracker (ACT). 

Currently in a "Proof-of-Concept" stage. 
The code is messy and uncommented, but is functional for testing to see if it works at all.

There are many UX issues, and clear remnants of example code I copied. 
I am aware of this, and do not intend to leave them in past proof-of-concept.
Thank you for your patience and understanding.

# WARNING
A known bug is that FF14 may freeze while using this plugin.
If this happens, restart ACT.

# Installation

 * [Optional] Download, install, and run the Buttplug.io server, make sure SSL/TLS is un-ticked, and set it running with everything as default.
 * Download and latest release from this repository.
 * Add all DLLs provided to the same ACT folder.
 * Launch ACT, and add `Hush_14_Plugin.dll` as a new plugin.

 
# Use
 
 * Once the plugin is activated, in ACT's `Plugins` tab, visit the `Hush_14_Plugin.dll` tab.
 * Turn on your device and have it ready to pair.
 * Once it has been successfully found and paired, you will see `Device [your device name] connected`
 * Press The Any Key to finish scanning.
    + You should see "Buzz Complete!" at the end of the log, and your toy should briefly vibrate.
 * Play the game, and your device should vibrate once for every attack you cast.
