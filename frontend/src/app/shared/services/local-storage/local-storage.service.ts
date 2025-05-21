import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root'
})
export class LocalStorageService {

	constructor() {	}

	getStorage(key: StorageKeys) {
		try {
			let data = window.localStorage.getItem(key);

			if (data) {
				return JSON.parse(data);
			}
		} catch(ex) {
			return null
		}
	}

	setStorage(key: StorageKeys, data: any): void {
		window.localStorage.setItem(key, JSON.stringify(data));
	}

	clear(key: StorageKeys) {
		window.localStorage.removeItem(key);
	}

	clearAll() {
		window.localStorage.clear();
	}

}

export enum StorageKeys {
	SESSION = 'session',
}