import { HttpClient, HttpEvent, HttpParams, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ApplicantResult } from '../models/aplicant-result.model';
import { Applicant } from '../models/applicant.model';



@Injectable({
  providedIn: 'root'
})
export class ApplicantService {

  constructor(private http: HttpClient) { }
  

  createNewApplicant(applicant:Applicant, cvFile: any, coverLetter: any):Observable<boolean>{

    let requestUrl = environment.serverURL.concat("applicants");
    const formData = new FormData();
    formData.append('name', applicant.name);
    formData.append('surname', applicant.surname);
    formData.append('email', applicant.email);
    formData.append('education', applicant.education.toString());
    formData.append('city', applicant.city);
    formData.append('street', applicant.street);
    formData.append('country', applicant.country);
    formData.append('description', applicant.description);
    formData.append('cvFile', cvFile);
    formData.append('coverLetterFile', coverLetter);


    return this.http.post<boolean>(requestUrl, formData);
 
  }

  uploadAttachment(file: File, applicantId: string, isCV: boolean): Observable<HttpEvent<any>> {
    let requestUrl = environment.serverURL.concat(`applicants/attachments/${applicantId}`);
    const formData: FormData = new FormData();
    formData.append('file', file);
    console.log("usao")
    console.log(requestUrl);
    const request = new HttpRequest('POST', requestUrl, formData, {
      reportProgress: true,
      responseType: 'json'
    });

    return this.http.request(request);
  }

 

}
