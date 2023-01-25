# AppSettingsManager

## Context
This proposal is focused on making application management more efficient for both the software engineers
managing applications directly and the companies that depend on those applications. Of course, large changes
to an application's functionality should be rigorously reviewed and tested before being deployed and possibly
causing issues in your business's day-to-day operations. However, there are also important but less complicated
changes to applications that could be as simple as adding a new employee's name to an access list or changing
how frequently the application refreshes its data. The catch is that, if that setting variable is held in the
application's main code base, then that change will have to flow through the same pipeline that the complicated
changes pass through; this means it might be delayed until more complicated changes are tested, approved,
and deployed.  This can cause headaches for your company's developers and significantly slow down your business.
If these simple settings lived in some other place and were retrieved when needed, these changes could be
implemented instantly.