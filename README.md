Just run the C# web server, then execute this code in the terminal of a Nerdle puzzle page!
```javascript
var script = document.createElement('script');
script.type = 'module';
script.src = 'http://localhost:5209/SolveNerdle.js'; // Specify the path to your JavaScript file
document.head.appendChild(script);
```
