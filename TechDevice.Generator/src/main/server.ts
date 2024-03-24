import webSocket from 'ws';

namespace SocketService {
    export interface IServerController<TMessage> {    
        sendMessage(message: TMessage): Promise<void>;
    }
    export class ServerController<TMessage> implements IServerController<TMessage> {
        private readonly socketServer: webSocket.Server;
    
        public constructor(server: webSocket.Server) {
            this.socketServer = server;
        }
        public initialize(): void {
            this.socketServer.on('connection', (ws: webSocket.WebSocket) => {
                ws.on('message', this.onMessage);
                ws.on('close', this.onClose);
            });
        }
        public async sendMessage(message: TMessage): Promise<void> {
            this.socketServer.clients.forEach((client) => {
                client.send(JSON.stringify(message));
            });
        }
        public onMessage(message: string): void {
            console.log(`Client: ${message}`);
        }
        public onClose() : void {
            console.log('Connection was closed')
        }
    }
}
export default SocketService;