import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarComponent } from './sidebar.component';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { RouterModule } from '@angular/router';

@NgModule({
	declarations: [SidebarComponent],
	exports: [SidebarComponent],
	imports: [
		CommonModule,
		RouterModule,
		NzIconModule,
	],
})
export class SidebarModule {}
