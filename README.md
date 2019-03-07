# PixelTracker
A simple pixel tracker I created in my free time

## How to include a pixel
Configure and publish the solution to a web site. Then you can embed images of the form
```
  <img src="https://example.com/websitepath/pixel.jpg?param={{guid}}" />
```
Where `{{guid}}` is a globally unique identifier. 
 
## The pixel tracker will log
Timestamp, param, Referring URL, and Referring Hostname
one per line into a `.csv` file specified in `Properties.Settings.Default.TrackerLogFile`
 
## Limitations
There are many limitations to this piece of code. For example, 
* it would be nice to plug into windows authentication and log a username in an Active Directory domain. 
* it would be nice to parse more headers and metadata from the request.
* it would be nice to log to a database rather than to a text file.

These are all enhancements you can build on top of this. 
This piece of code is intended as a barebones PixelTracker to demonstrate
how one can leverage an ASP.NET module to do this.

Cheers!
