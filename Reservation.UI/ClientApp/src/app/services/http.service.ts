import { Injectable } from '@angular/core';
import { NotificationsService } from 'angular2-notifications';
import { throwError } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  baseUrl: string;

  constructor(private http: HttpClient, private notificationService: NotificationsService) {
    this.baseUrl = 'https://localhost:44340/api/';
  }

  formatUrl(path, options){
    if (options) {
      const keys = Object.keys(options);
      if(keys.length > 0) path += '?';
      for (let i = 0; i < keys.length; i++) {
        path += `${keys[i]}=${options[keys[i]]}&`;
      }
    }
    return path;
  }

  uploadFile(url: string, model: any) {
    const options = this.addHeaders('POST');
    options.headers.delete('content-type');
    options.headers.delete('Accept');
    return this.http.post(this.baseUrl + url, model, options)
      .pipe(catchError(err => this.handleError(err)));
  }

  getUserId() {
    let userId = localStorage.getItem('userId');
    if(!userId) {
      userId = new Date().getTime().toString();
      localStorage.setItem('userId', userId)
    }
    return userId;
  }

  get(url: string, options?: any) {
    return this.http.get(this.baseUrl + url, this.addHeaders('GET', options))
      .pipe(catchError(err => this.handleError(err)));
  }

  post(url: string, model: any) {
    return this.http.post(this.baseUrl + url, model, this.addHeaders('POST'))
      .pipe(catchError(err => this.handleError(err)));
  }

  update(url: string, model: any) {
    return this.http.put(this.baseUrl + url, model, this.addHeaders('PUT'))
      .pipe(catchError(err => this.handleError(err)));
  }

  delete(url: string) {
    return this.http.delete(this.baseUrl + url, this.addHeaders('DELETE'))
      .pipe(catchError(err => this.handleError(err)));
  }

  private addHeaders(method: string, options?: any) {
    options = options || {};
    options.method = method;
    options.headers = options.headers || new HttpHeaders();
    options.headers = options.headers.set('content-type', 'application/json ');
    options.headers = options.headers.set('Accept', 'application/json');
    return options;
  }

  private handleError(response: any) {
    console.log(response);
    if (response && response._body) {
      try {
        const jsonError = JSON.parse(response._body);
        if (jsonError && jsonError.error) {
          this.notificationService.error('Error', jsonError.error);
        } else if (jsonError && jsonError.message) {
          this.notificationService.error('Error', jsonError.message);
        }
      } catch (e) {
        console.log(e);
        this.notificationService.error('Error', '');
      }
    }

    if(response.error){
      if(response.error.errors) {
        let keys = Object.keys(response.error.errors);
        let message = '';
        for(var i = 0; i < keys.length; i++) {
          message = response.error.errors[keys[i]].join(', ')
        }
        this.notificationService.error('Error', message)
        console.log(message)
      }
    }

    switch (response.status) {
      case 401:
        console.log('unauthorize')
        break;
      case 500:
        this.notificationService.error('Error Interno');
        break
      default:
        break;
    }
    return throwError(response._body);
  }
}
