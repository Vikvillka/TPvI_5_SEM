const http = require('http');
const fs = require("fs");

const server = http.createServer((req, res) => {
   
    if (req.method === 'GET' && req.url === '/') {
        fs.readFile('index.html', 'utf-8', (err, data) => {
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
        const x = parseInt(req.headers['x-value-x']);
        const y = parseInt(req.headers['x-value-y']);
        const z = x + y;

        res.setHeader('X-Value-z', z);
        res.writeHead(200);
        res.end();
        return;  
    }
    else{
        res.writeHead(404, { 'Content-Type': 'text/plain' });
        res.end('Not Found');
    }
});

server.listen(3000, () => {
    console.log('Server is running on http://localhost:3000');
});
