import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LocalStorageService, StorageKeys } from '../local-storage/local-storage.service';
import { AuthService } from '../api/auth.service';
import { catchError, Observable, shareReplay, tap, throwError } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
	providedIn: 'root'
})
export class AuthInterceptorsService implements HttpInterceptor {

    constructor(
		private storage: LocalStorageService,
		private authService: AuthService,
	) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let token = this.storage.getStorage(StorageKeys.SESSION)?.jwt;
        const prefix = environment.apiUrl;

        if (!req.headers.has('Content-Type')) {
            if(!req.headers.has('Auto-header')) {
				req = req.clone({
					headers: req.headers.set('Content-Type', 'application/json'),
				});
			}
        }

		let separator = ''
		if(req.url.charAt(0) != '/') separator = '/'

        let request = req.clone({
            url: `${prefix}${separator}${req.url}`,
			withCredentials: true,
        });

        if (token) {
            request = this.addToken(request);
		}

        return next.handle(request).pipe(
			tap((data: any) => {
				if (!data.body) return;

				if (!data.body.success) {
					throw data.body;
				}
			}),
            shareReplay(),
            catchError((err: any) => {
                if (err instanceof HttpErrorResponse && err.status === 401) {
                    return this.handle401Error(request, next)
				} else {
					return throwError({
						success: false,
						messages: err['messages'] ?? [{ text: err['message'].toString() }],
					});
				}
            })
        );
    }

    private addToken(request: HttpRequest<any>) {
		let token = this.storage.getStorage(StorageKeys.SESSION).jwt ?? ''

        return request.clone({
            setHeaders: { 'Authorization': `Bearer ${token}` }
        });
    }

    private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
		/*return this.authService.refreshToken()
		.pipe(
			switchMap(token => {
				this.storage.setStorage(StorageKeys.SESSION, token.value);
				return next.handle(this.addToken(request))
			}),
			catchError(() => {
				this.authService.logOut();
				return throwError("Your session has expired");
			})
		)*/
		this.authService.logOut();
		return throwError("Your session has expired");
    }
}
