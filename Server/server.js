const express = require('express');
const https = require('https');
const fs = require('fs'); // Module File System pour lire le certificat
const cors = require('cors');
const WebSocket = require('ws');


const app = express();

// Chargement des certificats SSL (remplacez les fichiers par les vôtres)
const privateKey = fs.readFileSync('Server/private-key.pem', 'utf8');
const certificate = fs.readFileSync('Server/certificate.pem', 'utf8');
const credentials = { key: privateKey, cert: certificate };

// Création du serveur HTTPS
const server = https.createServer(credentials, app);
const wss = new WebSocket.Server({ server });

// Enable CORS for all routes
app.use(cors());

// Stockage des connexions clients avec des identifiants uniques
const clients = new Map();
const lastMessages = new Map();

let nextClientId = 0;

wss.on('connection', (ws, req) => {
    console.log('Nouvelle connexion WebSocket');

    // Récupérer l'ID du client depuis les paramètres de l'URL
    const clientId = parseInt(req.url.split('/')[1]); // Extrait l'ID de l'URL
    clients.set(clientId, ws);
    lastMessages.set(clientId, null);
    // Écoute des messages du client
    ws.on('message', (data) => {
        // console.log(`Reçu du client ${clientId}: ${data}`);

        // // Vérifier si le message est une chaîne de caractères
        // if (typeof data === 'string') {
        //     console.log(`Reçu du client ${clientId}: ${data}`);

        //     // Gérer le message de déplacement de carte
        //     if (data.startsWith("move card to player")) {
        //         // Traiter le message comme un déplacement de carte à la table
        //         let cardId = data.split(" ")[5]; // Assurez-vous que cet index est correct
        //         broadcastToTable(`card ${cardId} moved to table by player ${clientId}`);
        //     } else {
        //         // Gérer les autres types de messages
        //         // ...
        //     }
        // } else {
        //     // Si ce n'est pas une chaîne de caractères, vous devrez convertir les données binaires en chaîne de caractères
        //     console.log('Message reçu, mais ce n\'est pas une chaîne de caractères.');
        //     // Vous pouvez convertir l'ArrayBuffer en chaîne si nécessaire ici
        // }

        let message;
        try {
            // Tentez de parser les données JSON reçues
            message = JSON.parse(data);
            console.log('Message parsé en tant qu\'objet:', message); // Log pour voir l'objet JSON
        } catch (e) {
            // Si le parse échoue, traiter les données comme une chaîne
            console.log('Les données ne sont pas au format JSON. Erreur:', e);
            message = data;
        }
        // Maintenant, utilisez la variable `message`
        if (typeof message === 'string') {
            console.log('Le message est une chaîne:', message);
            // Vérifiez si la chaîne commence par le bon texte
            if (message.startsWith("move card to player")) {
                let cardId = message.split(" ")[5]; // Assurez-vous que l'index est correct
                broadcastToTable(`card ${cardId} moved to table by player ${clientId}`);
            }
        } else if (typeof message === 'object' && message.type === "move card to player") {
            console.log('Le message est un objet et de type "move card to player".');
            let cardId = message.cardId;
            broadcastToTable(`card ${cardId} moved to table by player ${clientId}`);
        } else {
            console.log('Format de message inconnu ou non pris en charge.', message);
        }
    });

    // Envoi d'un message au client connecté
    //ws.send(`Bienvenue sur le serveur WebSocket, client ${clientId}`);
});

function broadcastMessage(message) {
    // Exemple : Diffuser le message à tous les clients connectés, sauf à la table
    clients.forEach((client, clientId) => {
        if (client.readyState === WebSocket.OPEN && clientId !== 0) {
            lastMessages.set(clientId, message);
            client.send(message);
        }
    });
}

function broadcastToTable(message) {
    // Diffuser le message seulement à la table
    // Dans cet exemple, supposons que l'ID du client de la table est 0
    let tableClientId = 0;
    if (clients.has(tableClientId)) {
        let tableClient = clients.get(tableClientId);
        if (tableClient.readyState === WebSocket.OPEN) {
            tableClient.send(message);
        }
    }
}

// HTTP server
app.get('/latest-message/:clientId', (req, res) => {
    const clientId = parseInt(req.params.clientId);
    const lastMessage = lastMessages.get(clientId);
    console.log("demande pour recuperer les cartes d'un jouer")
    res.json({ message: lastMessage });
});


// Démarrer le serveur
const PORT = process.env.PORT || 3000;
server.listen(PORT, () => {
    console.log(`Serveur WebSocket en cours d'exécution sur le port ${PORT}`);
});