import { Injectable } from '@angular/core';
import { LocalStorageService, StorageKeys } from '../services/local-storage/local-storage.service';
import { Router } from '@angular/router';

@Injectable({
    providedIn: 'root'
})
export class LoggedGuardService {

    constructor(
		private storage: LocalStorageService,
		private router: Router
	) { }

    canActivate() {
		const token = this.storage.getStorage(StorageKeys.SESSION);
        if (!token) {
            this.router.navigate(['/login']);
            return false;
        }

        return true;
    }
}
