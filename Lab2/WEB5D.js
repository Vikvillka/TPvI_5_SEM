const http = require('http');
const fs = require("fs");

function generateRandomSeries(n) {
    const count = Math.floor(Math.random() * (10 - 5 + 1)) + 5;
    const series = [];
    for (let i = 0; i < count; i++) {
        const randomNum = Math.floor(Math.random() * (2 * n + 1)) - n;
        series.push(randomNum);
    }
    return series;
}

const server = http.createServer((req, res) => {
  
    if (req.method === 'GET' && req.url === '/') {
        fs.readFile('indexD.html', 'utf-8', (err, data) => {
            if (err) {
                res.writeHead(500, { 'Content-Type': 'text/plain; charset=utf-8' });
                res.end("Ошибка сервера");
                return; 
            } else {
                res.writeHead(200, { 'Content-Type': 'text/html; charset=utf-8' });
                res.end(data);
                return;  
            }
        });
        return;
    }

    if (req.method === 'POST') {
        const x = req.headers['x-value-x'];
        const y = req.headers['x-value-y'];
        const nHeader = req.headers['x-rand-n'];

        if (x && y) {
            const xInt = parseInt(x);
            const yInt = parseInt(y);
            if (!isNaN(xInt) && !isNaN(yInt)) {
                const z = xInt + yInt;

                setTimeout(() => {
                    res.setHeader('X-Value-z', z);
                    res.writeHead(200);
                    res.end();
                }, 5000);
            } else {
                res.writeHead(400, { 'Content-Type': 'text/plain' });
                res.end('Invalid values for X or Y');
            }
        }
       
        else if (nHeader) {
            const n = parseInt(nHeader);
            if (!isNaN(n)) {
                const randomSeries = generateRandomSeries(n);

                setTimeout(() => {
                    res.writeHead(200, { 'Content-Type': 'application/json' });
                    res.end(JSON.stringify(randomSeries));
                }, 1000);
            } else {
                res.writeHead(400, { 'Content-Type': 'text/plain' });
                res.end('Invalid value for n');
            }
        } else {
            res.writeHead(400, { 'Content-Type': 'text/plain' });
            res.end('Bad Request: Missing required headers');
        }
    } else {
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Not Found');
    }
});

server.listen(3000, () => {
    console.log('Server is running on http://localhost:3000');
});
