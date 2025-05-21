import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ListadoTicketsComponent } from './listado-tickets.component';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { RouterModule } from '@angular/router';

@NgModule({
	declarations: [ListadoTicketsComponent],
	imports: [
		CommonModule,
		RouterModule,
		NzTableModule,
		NzButtonModule,
	],
})
export class ListadoTicketsModule {}
