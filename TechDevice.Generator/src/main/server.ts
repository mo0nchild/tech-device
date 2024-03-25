/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable @typescript-eslint/no-namespace */
/* eslint-disable prettier/prettier */
import webSocket from 'ws';

namespace SocketService {
    export interface IServerController<TMessage> {    
        sendMessage(message: TMessage): Promise<void>;
        initialize(server: webSocket.Server): Promise<void>
        close: () => Promise<void>
    }
    export class ServerController<TMessage> implements IServerController<TMessage> {
        private socketServer: webSocket.Server | null = null;
        private lastMessage: TMessage | null = null;
    
        public constructor() { }
        public close: () => Promise<void> = async () => {
            if(this.socketServer !== null) {
                this.socketServer.clients.forEach((client) => client.close())
                this.socketServer.close();
                this.socketServer = null;
            }
        };
        public async initialize(server: webSocket.Server): Promise<void> {
            (this.socketServer = server).on('connection', (ws: webSocket.WebSocket) => {
                ws.on('message', (data) => this.onMessage(data.toString(), ws));
                ws.on('close', this.onClose);
                if(this.lastMessage != null) {
                    ws.send(JSON.stringify(this.lastMessage));
                }
            });
        }
        public async sendMessage(message: TMessage): Promise<void> {
            this.lastMessage = message;
            if(this.socketServer == null) return;

            this.socketServer.clients.forEach((client) => {
                client.send(JSON.stringify(message));
            });
        }
        public onMessage(message: string, ws: webSocket.WebSocket): void {
            console.log(`Client: ${message}`);
            if(this.lastMessage != null) ws.send(JSON.stringify(this.lastMessage));
        }
        public onClose() : void {
            console.log('Connection was closed')
        }
    }
}
export default SocketService;