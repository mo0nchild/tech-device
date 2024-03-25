/* eslint-disable prettier/prettier */
import { app, shell, BrowserWindow } from 'electron'
import { join } from 'path'
import { electronApp, optimizer, is } from '@electron-toolkit/utils'
import icon from '../../resources/icon.png?asset'

import socket from 'ws'
import SocketService from './server'
import { createIpcMain } from 'electron-typescript-ipc';
import { Api, ApiData } from './../preload/index'

function createWindow(): void {
	const mainWindow = new BrowserWindow({
		width: 720,
		height: 480,
		resizable: false,
		show: false,
		title: 'Генератор',
		autoHideMenuBar: true,
		...(process.platform === 'linux' ? { icon } : {}),
		webPreferences: {
			preload: join(__dirname, '../preload/index.js'),
			sandbox: false
		}
	})
	mainWindow.on('ready-to-show', () => {
		mainWindow.show()
	})
	mainWindow.webContents.setWindowOpenHandler((details) => {
		shell.openExternal(details.url)
		return { action: 'deny' }
	})
	if (is.dev && process.env['ELECTRON_RENDERER_URL']) {
		mainWindow.loadURL(process.env['ELECTRON_RENDERER_URL'])
	} else {
		mainWindow.loadFile(join(__dirname, '../renderer/index.html'))
	}
}

app.whenReady().then(() => {
	electronApp.setAppUserModelId('com.electron');
	const controller = new SocketService.ServerController<ApiData>();

	app.on('browser-window-created', (_, window) => {
		optimizer.watchWindowShortcuts(window)
	})
	const ipcMain = createIpcMain<Api>();
	ipcMain.handle('getData', async (_event, key) => {
		return await controller.sendMessage(key);
	});
	ipcMain.handle('serverUp', async () => {
		controller.initialize(new socket.Server({ port: 8000 }));
	});
	ipcMain.handle('serverDown', async () => {
		return await controller.close();
	});
	createWindow()
	app.on('activate', function () {
		if (BrowserWindow.getAllWindows().length === 0) createWindow()
	})
})

app.on('window-all-closed', () => {
	if (process.platform !== 'darwin') {
		app.quit()
	}
})	
