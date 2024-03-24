import * as Ipc from 'electron-typescript-ipc';

export type ApiData = {
	readonly voltage: string;
	readonly amperage: string; 
}

export type Api = Ipc.GetApiType<{
	getData: (data: ApiData) => Promise<void>
}, {}>;

const ipcRenderer = Ipc.createIpcRenderer<Api>();
const api: Api = {
	invoke: {
		getData: async (data: ApiData): Promise<void> => {
			return await ipcRenderer.invoke('getData', data);
		}
	},
	on: { },
};
Ipc.contextBridge.exposeInMainWorld('api', api);
