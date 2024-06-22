import { HttpClient, HttpErrorResponse, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class FileUploadService {

  private baseUrl = 'https://localhost:7072/api/Script';

  constructor(private http: HttpClient) { }

  // uploadFile(file: File): Observable<any> {
  //   const formData: FormData = new FormData();
  //   formData.append('file', file, file.name);

  //   return this.http.post(`${this.baseUrl}/uploadAwsfile`, formData,  { observe: 'events', reportProgress: true, responseType:'text' }).pipe(
  //     map(event => {
  //       console.log("uploading to backend");
  //       if (event.type === HttpEventType.UploadProgress) {
  //         const percentDone = Math.round(100 * event.loaded / event.total!);
  //         console.log(`File is ${percentDone}% uploaded.`);
  //         return { progress: percentDone };  // Ensure that a value is returned
  //       } else if (event.type === HttpEventType.Response) {
  //         console.log('File is completely uploaded!', event.body);
  //         return event.body;  // Ensure that a value is returned
  //       }
  //       return null;  // Return a default value
  //     }),
  //     catchError(this.handleError)
  //   );
  // }

  // private handleError(error: HttpErrorResponse) {
  //   let errorMessage = 'An unknown error occurred!';
  //   if (error.error instanceof ErrorEvent) {
  //     errorMessage = `A client-side or network error occurred: ${error.error.message}`;
  //   } else {
  //     errorMessage = `Backend returned code ${error.status}, body was: ${error.error}`;
  //   }
  //   console.error(errorMessage);
  //   return throwError(() => new Error(errorMessage));
  // }


  // uploadFile(file: File){
  //   const formData: FormData = new FormData();
  //   formData.append('file', file, file.name);

  //   return this.http.post(this.baseUrl+"/uploadAwsfile",JSON.stringify({
  //     formData: formData
  //   }),
  //   {
  //     headers:{
  //       "content-type":'application/json',
  //     }
  //   }
  // )
  // }



  uploadAwsFile(file: File, email: any) {
    const formData = new FormData();
    console.log(file.name)
    console.log("comming in the aws file upload method");
    formData.append('file', file, file.name);
    return this.http.post(`https://localhost:7072/api/Script/uploadAwsfile/${email}`, formData);
  }

  uploadAzureFile(file: File, email: any) {
    const formData = new FormData();
    console.log(file.name)
    console.log("comming in the azure file upload method");
    formData.append('file', file, file.name);

    return this.http.post(`https://localhost:7072/api/Script/uploadAzurefile/${email}`, formData);
  }
}
