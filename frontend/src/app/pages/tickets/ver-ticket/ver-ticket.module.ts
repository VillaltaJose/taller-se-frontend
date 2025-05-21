import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VerTicketComponent } from './ver-ticket.component';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterModule } from '@angular/router';

@NgModule({
	declarations: [VerTicketComponent],
	imports: [
		CommonModule,
		RouterModule,
		NzButtonModule,
		NzInputModule,
		NzSelectModule,
		NzIconModule,
	],
})
export class VerTicketModule {}
