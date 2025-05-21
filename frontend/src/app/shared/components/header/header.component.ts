import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
	selector: 'app-header',
	templateUrl: './header.component.html',
	styleUrls: ['./header.component.scss'],
})
export class HeaderComponent {
	title: string = '';

	constructor(
		private _router: Router,
	) {
		this._router.events.subscribe(() => {
			this.title = this._router.routerState.snapshot.root.firstChild?.data['title'] || '';
		});
	}
}
