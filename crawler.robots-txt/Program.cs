﻿using System.Diagnostics;
using crawler.robots_txt;

string path = "./example/example-robots.txt";
FileStream fs = File.OpenRead(path);

//Uri url = new Uri("http://www.contoso.com/title/index.htm");
Uri url = new Uri("http://www.contoso.com/w/load.php?something=2");
Debug.WriteLine($"Absolute Path: {url.AbsoluteUri}");
string userAgent = "IsraBot";
Matcher matcher = new Matcher(url, userAgent);
bool allowed = matcher.AllowedByRobots(fs);
Debug.WriteLine($"{url.AbsoluteUri} is allowed? : {allowed}");