import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable()
export class AppService {
  constructor(private http: HttpClient) {}

  private baseUrl = window.location.href;
  private pathApi = (this.baseUrl.charAt(this.baseUrl.length - 1) == '/' ? this.baseUrl.substr(0, this.baseUrl.length - 1) : this.baseUrl) + '/api/';
  public sendMail(id: number, comments: string): Observable<any> {
    return this.http.post(this.pathApi + 'SendMail', { toEmails: id, comments: comments},{
        responseType: 'text'
    }).pipe(map(response => response));
  }
}
