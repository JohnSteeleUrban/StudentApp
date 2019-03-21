// import { Injectable } from '@angular/core';
// import { HttpHeaders, HttpClient } from '@angular/common/http';

// import { environment } from './../../environments/environment';
// //import { PaginationService } from './pagination.service';

// @Injectable({
//   providedIn: 'root'
// })
// export class HttpBaseService {

//   private headers = new HttpHeaders();
//   private urlAddress = environment.urlAddress;

//   constructor(
//       private httpClient: HttpClient,
//       private paginationService: PaginationService
//       ) {

//       this.headers = this.headers.set('Content-Type', 'application/json');
//       this.headers = this.headers.set('Accept', 'application/json');
//   }

//   getAll<T>() {
//       const mergedUrl = `${this.urlAddress}` +
//           `?page=${this.paginationService.page}&pageCount=${this.paginationService.pageCount}`;

//       return this.httpClient.get<T>(mergedUrl, { observe: 'response' });
//   }

//   getSingle<T>(id: number) {
//       return this.httpClient.get<T>(`${this.urlAddress}${id}`);
//   }

//   add<T>(toAdd: T) {
//       return this.httpClient.post<T>(this.urlAddress, toAdd, { headers: this.headers });
//   }

//   update<T>(url: string, toUpdate: T) {
//       return this.httpClient.put<T>(url,
//           toUpdate,
//           { headers: this.headers });
//   }

//   delete(url: string) {
//       return this.httpClient.delete(url);
//   }
// }