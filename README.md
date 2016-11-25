#Quiet - SSH Profile Manager for Windows, Linux, and Mac


Quiet is a CLI SSH profile manager that runs on .Net Core.
This is not an SSH client itself, but rather a wrapper around your already installed ssh client.

Windows users, see https://github.com/PowerShell/Win32-OpenSSH.

This allows for an easy way to manage SSH connections across operating systems. All profiles are stored in a JSON file in your home directory.

##Why?
There are several different ways to manage SSH connections on Linux and Mac.
On Windows, the default is [PuTTy](www.putty.org). There's nothing wrong with PuTTy - it's a great little program, in fact - but I wanted something that ran the same on all OS's.

I also wanted something that was simple to manage without any features that I never used.

Lastly, having this done in a CLI means that I could use whatever console I wanted in Windows, and not be stuck with the console window that PuTTy opens.

##How?

###List profiles
```
quiet list
```

###Connect to a profile
```
quiet connect -n <profileName>
```

###Add a profile
```
quiet add -n <profileName> -u <username> -h <hostname> -p <port> -g <profileGroup>
```

###Delete a profile
```
quiet delete -n <profileName>
```

###Update a profile
```
quiet update -n <profileName> -u <newUsername>
```

All CRUD options on the profiles can be done by manually editing the `profiles.json` folder as well. It's as simple a structure as you'd expect.


##Install

Coming soon...
