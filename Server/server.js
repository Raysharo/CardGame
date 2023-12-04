const express = require('express');
const https = require('https');
const fs = require('fs'); // Module File System pour lire le certificat
const cors = require('cors');
const WebSocket = require('ws');


const app = express();

// Chargement des certificats SSL (remplacez les fichiers par les vôtres)
const privateKey = fs.readFileSync('private-key.pem', 'utf8');
const certificate = fs.readFileSync('certificate.pem', 'utf8');
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
    ws.on('message', (message) => {
        console.log(`Reçu du client ${clientId}: ${message}`);

        // Envoyer le message avec l'identifiant du client au client spécifique
        //const messageWithClientId = `Client ${clientId}: ${message}`;
        //ws.send(`Vous avez envoyé: ${message}`);

        // Stocker le dernier message du client
        // lastMessages.set(clientId, messageWithClientId);

        // Exemple : Diffuser le message à tous les clients connectés

        if (clientId == 0) {
            //broadcastMessage(messageWithClientId);
            //broadcastMessage(message);
            console.log(`${message}`);
            clients.get(1).send(`${message}`);
        } else {
            //clients.get(0).send(messageWithClientId);
            clients.get(0).send(message);
            
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