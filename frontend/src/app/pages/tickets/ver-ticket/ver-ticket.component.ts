import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TicketsService } from 'src/app/shared/services/api/tickets.service';

@Component({
	selector: 'app-ver-ticket',
	templateUrl: './ver-ticket.component.html',
	styleUrls: ['./ver-ticket.component.scss'],
})
export class VerTicketComponent {

	ticketId: number = 0;
	ticket: any;
	messages: any[] = [];
	loading = false;

	constructor(
		private route: ActivatedRoute,
		private _ticketsService: TicketsService,
	) {
		const ticketId = this.route.snapshot.paramMap.get('id') ?? '0';
		this.ticketId = parseInt(ticketId);

		this.getTicket();
		this.getMessages();
	}

	getTicket() {
		this.loading = true;		

		this._ticketsService.getTicket(this.ticketId)
			.subscribe(api => {
				this.ticket = api.value;
			}, err => {
				console.error(err);
				this.loading = false;
			});
	}

	getMessages() {
		this._ticketsService.getMessages(this.ticketId)
			.subscribe(api => {
				this.messages = api.value;
				this.loading = false;
			}, err => {
				console.error(err);
				this.loading = false;
			});
	}
}
