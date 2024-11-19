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
        fs.readFile('indexB.html', 'utf-8', (err, data) => {
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
        const nHeader = req.headers['x-rand-n'];

        if (nHeader && !isNaN(parseInt(nHeader))) {
            const n = parseInt(nHeader);

            const randomSeries = generateRandomSeries(n); 
            console.log("Generated series:", randomSeries); 

            res.writeHead(200, { 'Content-Type': 'application/json' });
            res.end(JSON.stringify(randomSeries)); 
            return;
        } 
    } else {
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Not Found');
    }
});

server.listen(3000, () => {
    console.log('Server is running on http://localhost:3000');
});
