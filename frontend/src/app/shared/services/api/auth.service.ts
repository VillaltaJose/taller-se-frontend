import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalStorageService } from '../local-storage/local-storage.service';
import { Router } from '@angular/router';
import { Result } from 'src/app/models/result';

@Injectable({
	providedIn: 'root'
})
export class AuthService {
	constructor(
		private _http: HttpClient,
		private _storage: LocalStorageService,
		private _route: Router,
	) {}

	login(data: any) {
		return this._http.post<Result<any>>('auth', data);
	}

	logOut() {
		this._storage.clearAll();
		this._route.navigate(['/login']);
	}
}
