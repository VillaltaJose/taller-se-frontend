import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './login.component';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({
	declarations: [LoginComponent],
	imports: [
		CommonModule,
		NzInputModule,
		NzButtonModule,
		FormsModule,
		ReactiveFormsModule,
	],
})
export class LoginModule {}
