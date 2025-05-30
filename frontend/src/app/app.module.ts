import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { es_ES } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import es from '@angular/common/locales/es';
import { FormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptorsService } from './shared/services/interceptors/auth-interceptors.service';

registerLocaleData(es);

@NgModule({
	declarations: [AppComponent],
	imports: [
		BrowserModule,
		AppRoutingModule,
		FormsModule,
		HttpClientModule,
		BrowserAnimationsModule,
	],
	providers: [
		{ provide: NZ_I18N, 
			useValue: es_ES 
		},
		{
			provide: HTTP_INTERCEPTORS,
			useClass: AuthInterceptorsService,
			multi: true,
		},
	],
	bootstrap: [AppComponent],
})
export class AppModule {}
