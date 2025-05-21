import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainLayoutComponent } from './main-layout.component';
import { RouterModule } from '@angular/router';
import { HeaderModule } from 'src/app/shared/components/header/header.module';
import { SidebarModule } from 'src/app/shared/components/sidebar/sidebar.module';

@NgModule({
	declarations: [MainLayoutComponent],
	imports: [
		CommonModule,
		RouterModule.forChild([{ path: '', component: MainLayoutComponent }]),
		HeaderModule,
		SidebarModule,
	],
})
export class MainLayoutModule {}
