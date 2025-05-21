import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutModule } from './layouts/main-layout/main-layout.module';
import { LoginModule } from './pages/auth/login/login.module';
import { MainLayoutComponent } from './layouts/main-layout/main-layout.component';
import { LoginComponent } from './pages/auth/login/login.component';
import { ListadoTicketsComponent } from './pages/tickets/listado-tickets/listado-tickets.component';
import { ListadoTicketsModule } from './pages/tickets/listado-tickets/listado-tickets.module';
import { VerTicketModule } from './pages/tickets/ver-ticket/ver-ticket.module';
import { VerTicketComponent } from './pages/tickets/ver-ticket/ver-ticket.component';
import { LoggedGuardService } from './shared/guards/logged-guard.service';

const routes: Routes = [
	{
		path: '', component: MainLayoutComponent, children: [
			{ path: '', redirectTo: 'tickets', pathMatch: 'full' },
			{ path: 'tickets', component: ListadoTicketsComponent, },
			{ path: 'tickets/:id', component: VerTicketComponent, },
		], canActivate: [LoggedGuardService]
	},
	{ path: 'login', component: LoginComponent, }
];

@NgModule({
	imports: [RouterModule.forRoot(routes)],
	exports: [
		RouterModule,
		MainLayoutModule,
		LoginModule,
		ListadoTicketsModule,
		VerTicketModule,
	],
})
export class AppRoutingModule { }
