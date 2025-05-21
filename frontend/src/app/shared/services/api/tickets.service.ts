import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Result } from 'src/app/models/result';

@Injectable({
	providedIn: 'root'
})
export class TicketsService {

	constructor(
		private _http: HttpClient
	) { }

	getTickets() {
		return this._http.get<Result<any>>('tickets');
	}

	getTicket(id: number) {
		return this._http.get<Result<any>>(`tickets/${id}`);
	}

	getMessages(id: number) {
		return this._http.get<Result<any>>(`tickets/${id}/messages`);
	}
}
