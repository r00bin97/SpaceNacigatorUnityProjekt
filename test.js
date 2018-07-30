let http = require('http');

const port = 8080;
const server = http.createServer();
server.on('request', function (req, res) {
    let count = fs.readFileSync('cnt.txt').toString();
    count = parseInt(count);
    count++;

    fs.writeFile('cnt.txt', count, function () {
        res.writeHead(200, {'count-Type':'text/html'});
        res.write('alles mögliche geschrieben')
        res.end();
    });


});

server.listen(port);
