/* eslint-disable prettier/prettier */
import * as Ipc from 'electron-typescript-ipc';

export type ApiData = {
	readonly voltage: string;
	readonly amperage: string; 
}

export type Api = Ipc.GetApiType<{
	getData: (data: ApiData) => Promise<void>,
	serverUp: () => Promise<void>,
	serverDown: () => Promise<void>
// eslint-disable-next-line @typescript-eslint/ban-types
}, { }>;

const ipcRenderer = Ipc.createIpcRenderer<Api>();
const api: Api = {
	invoke: {
		getData: async (data: ApiData): Promise<void> => {
			return await ipcRenderer.invoke('getData', data);
		},
		serverUp: async (): Promise<void> => await ipcRenderer.invoke('serverUp'),
		serverDown: async (): Promise<void> => await ipcRenderer.invoke('serverDown')
	},
	on: { }
};
Ipc.contextBridge.exposeInMainWorld('api', api);
