What started as an exercise to count the number of valid Nerdle solutions (there are 17,719 by the way) turned into a Nerdle puzzle solver!

Just run the C# web server, then execute this code in the terminal of a Nerdle puzzle page!
```javascript
var script = document.createElement('script');
script.type = 'module';
script.src = 'http://localhost:5209/SolveNerdle.js';
document.head.appendChild(script);
```