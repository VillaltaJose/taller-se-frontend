import { Component } from '@angular/core';
import { TicketsService } from 'src/app/shared/services/api/tickets.service';

@Component({
	selector: 'app-listado-tickets',
	templateUrl: './listado-tickets.component.html',
	styleUrls: ['./listado-tickets.component.scss']
})
export class ListadoTicketsComponent {
	
	tickets: any[] = [];
	loading = false;
	
	constructor(
		private _ticketsService: TicketsService,
	) {
		this.loadTickets();
	}

	loadTickets() {
		this.loading = true;
		this._ticketsService.getTickets()
			.subscribe(api => {
				this.tickets = api.value;
				this.loading = false;
			}, err => {
				console.error(err);
				this.loading = false;
			});
	}
}
