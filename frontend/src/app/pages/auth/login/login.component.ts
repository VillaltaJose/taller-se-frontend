import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { Message, Result } from 'src/app/models/result';
import { AuthService } from 'src/app/shared/services/api/auth.service';
import { LocalStorageService, StorageKeys } from 'src/app/shared/services/local-storage/local-storage.service';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.scss']
})
export class LoginComponent {
    loading = false;
    formLogin: FormGroup;
    errors: Message[] = [];

    constructor(
        private _authService: AuthService,
        private _router: Router,
        private _storageService: LocalStorageService,
    ) {
        this.formLogin = new FormGroup({
            email: new FormControl('', [Validators.required]),
            password: new FormControl('', [Validators.required]),
        });
    }

    login() {
        if (this.formLogin.invalid) {
            this.formLogin.markAllAsTouched();
            return;
        }

        this.errors = [];

        this.loading = true;

        const data = this.formLogin.getRawValue();

        this._authService.login(data)
            .pipe(finalize(() => this.loading = false))
            .subscribe(api => {
                console.log(api);
                this._storageService.setStorage(StorageKeys.SESSION, api.value.jwt);
                this._router.navigate(['/tickets']);
            }, (err: Result<void>) => {
                this.errors = err.messages;
            })

    }
}
