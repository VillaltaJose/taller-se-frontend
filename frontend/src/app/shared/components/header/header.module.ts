import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { NzPopoverModule } from 'ng-zorro-antd/popover';

@NgModule({
	declarations: [HeaderComponent],
	exports: [HeaderComponent],
	imports: [
		CommonModule,
		NzPopoverModule,
	],
})
export class HeaderModule {}
